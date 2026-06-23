using System;
using Spacats.Input;
using Spacats.Utils;
using UnityEngine;

namespace Spacats.CharacterController
{
    /// <summary>
    /// Moves the character via UnityEngine.CharacterController (collide-and-slide).
    /// Driven from Update for frame-rate smooth motion. Handles grounded locomotion,
    /// air/gravity and collision-aware flying.
    /// </summary>
    public class CharacterMovementController : MonoBehaviour
    {
        [SerializeField] private MovementRuntimeData _runtimeData;
        [SerializeField] private MovementSettings _settings;
        private CharacterInputRuntimeData _inputData;
        private AnimatorToMovementData _atomData;
        private MovementToAnimatorData _mtoaData;

        // Small constant downward velocity kept while grounded so the controller stays
        // glued to slopes/steps when walking downhill (prevents the descent jitter).
        private const float GroundedStickVelocity = -2f;
        public void Init(CharacterInputRuntimeData inputData, AnimatorToMovementData atomData, MovementToAnimatorData mtoaData)
        {
            _inputData = inputData;
            _atomData = atomData;
            _mtoaData = mtoaData;
            _runtimeData.CurrentFlying = false;
            _runtimeData.PreviousFlying = false;
            _runtimeData.VerticalVelocity = 0f;
            _runtimeData.HorizontalVelocity = Vector3.zero;
        }

        public void CallUpdate()
        {
            if (PauseController.IsPaused)
            {
                return;
            }
            
            DispositionRotateParent();
        }

        /// <summary>
        /// Drives the character through UnityEngine.CharacterController (collide-and-slide).
        /// Called every frame from Update for frame-rate smooth movement (no FixedUpdate stepping).
        /// </summary>
        public void TryMove()
        {
            if (_settings.Controller == null) return;

            _mtoaData.MainAnimationType = MainAnimationTypes.Idle;

            if (PauseController.IsPaused)
            {
                _runtimeData.HorizontalVelocity = Vector3.zero;
                _runtimeData.VerticalVelocity = 0f;
                _runtimeData.WasPaused = true;
                return;
            }

            if (_runtimeData.WasPaused)
            {
                _runtimeData.WasPaused = false;
            }

            _runtimeData.MoveDirection = _inputData.MoveDirectionVector;

            UpdateGroundedState();

            _runtimeData.CurrentFlying = _inputData.Flying;
            if (_runtimeData.CurrentFlying != _runtimeData.PreviousFlying)
            {
                if (_runtimeData.CurrentFlying) OnFlyEnter();
                else OnFlyExit();
            }

            _runtimeData.PreviousFlying = _runtimeData.CurrentFlying;

            if (_runtimeData.CurrentFlying)
            {
                ProcessFlying();
                RefreshCurrentSpeed();
                return;
            }

            switch (_runtimeData.State)
            {
                case SpaceStates.InAir: ProcessInAir(); break;
                case SpaceStates.OnGround: ProcessOnGround(); break;
                case SpaceStates.InWater: break;
            }

            RefreshCurrentSpeed();
        }

        private void RefreshCurrentSpeed()
        {
            Vector3 horizontalVelocity = _settings.Controller.velocity;
            horizontalVelocity.y = 0f;
            _runtimeData.HorizontalSpeed = horizontalVelocity.magnitude;
            _runtimeData.RigidBodyVelocity = _settings.Controller.velocity;
            _runtimeData.RigidBodySpeed = _runtimeData.RigidBodyVelocity.magnitude;
        }

        /// <summary>
        /// Determines whether the character is grounded using the CharacterController's own
        /// grounded flag combined with a small sphere check for robustness on slopes/steps.
        /// </summary>
        private void UpdateGroundedState()
        {
            Vector3 position = _settings.Controller.transform.position;
            Vector3 spherePosition = new Vector3(position.x, position.y - _settings.GroundedOffset, position.z);
            bool grounded = _settings.Controller.isGrounded ||
                            Physics.CheckSphere(spherePosition, _settings.GroundedRadius, _settings.GroundLayers, QueryTriggerInteraction.Ignore);

            _runtimeData.Grounded = grounded;
            _runtimeData.State = grounded ? SpaceStates.OnGround : SpaceStates.InAir;
        }

        private void ProcessOnGround()
        {
            float speed = ProcessOnGroundSpeed();

            // Keep the controller pinned to the ground/slope so descending stays smooth.
            if (_runtimeData.VerticalVelocity < 0f)
            {
                _runtimeData.VerticalVelocity = GroundedStickVelocity;
            }

            Vector3 targetHorizontal = _runtimeData.MoveDirection * speed;
            _runtimeData.HorizontalVelocity = Vector3.Lerp(_runtimeData.HorizontalVelocity,
                targetHorizontal, Time.deltaTime * _settings.SmoothSpeedChange);

            Vector3 motion = _runtimeData.HorizontalVelocity + Vector3.up * _runtimeData.VerticalVelocity;
            _runtimeData.RuntimeVelocity = motion;
            _settings.Controller.Move(motion * Time.deltaTime);

            if (_inputData.MoveDirection == MoveDirections.Idle)
            {
                _mtoaData.MainAnimationType = MainAnimationTypes.Idle;

                if (_inputData.MoveType == MoveInputTypes.Crouch)   _mtoaData.MainAnimationType = MainAnimationTypes.CrouchIdle;
            }
        }

        private float ProcessOnGroundSpeed()
        {
            float speed = _settings.RunSpeed.x;
            _mtoaData.MainAnimationType = MainAnimationTypes.RunForward;
            
            if (_inputData.MoveType == MoveInputTypes.Sprint)
            {
                speed = _settings.SprintSpeed.x;
                _mtoaData.MainAnimationType = MainAnimationTypes.SprintForward;
                if (_inputData.IsMovingBack())
                {
                    speed = _settings.SprintSpeed.y;
                    _mtoaData.MainAnimationType = MainAnimationTypes.SprintBackward;
                }
                
                return speed;
            }
            
            if (_inputData.MoveType == MoveInputTypes.Crouch)
            {
                speed = _settings.CrouchSpeed.x;
                _mtoaData.MainAnimationType = MainAnimationTypes.CrouchForward;
                if (_inputData.IsMovingBack())
                {
                    speed = _settings.CrouchSpeed.y;
                    _mtoaData.MainAnimationType = MainAnimationTypes.CrouchBackward;
                }
                
                return speed;
            }
            
            if (_inputData.MoveType == MoveInputTypes.Walk)
            {
                speed = _settings.WalkSpeed.x;
                _mtoaData.MainAnimationType = MainAnimationTypes.WalkForward;
                if (_inputData.IsMovingBack())
                {
                    speed = _settings.WalkSpeed.y;
                    _mtoaData.MainAnimationType = MainAnimationTypes.WalkBackward;
                }
                
                return speed;
            }
            
            if (_inputData.IsMovingBack())
            {
                speed = _settings.RunSpeed.y;
                _mtoaData.MainAnimationType = MainAnimationTypes.RunBackward;
            }
            
            return speed;
        }

        private void ProcessInAir()
        {
            // Accumulate gravity (clamped to terminal velocity) for a natural fall.
            _runtimeData.VerticalVelocity += _settings.Gravity * Time.deltaTime;
            if (_runtimeData.VerticalVelocity < -_settings.TerminalVelocity)
            {
                _runtimeData.VerticalVelocity = -_settings.TerminalVelocity;
            }

            // Preserve horizontal momentum from the last grounded frame.
            Vector3 motion = _runtimeData.HorizontalVelocity + Vector3.up * _runtimeData.VerticalVelocity;
            _runtimeData.RuntimeVelocity = motion;
            _settings.Controller.Move(motion * Time.deltaTime);
        }
        
        private void ProcessFlying()
        {
            float speed = _settings.FlySpeed;
            // Flying ignores gravity, but movement still goes through CharacterController.Move,
            // so collisions (walls/floor/ceiling) are resolved and the character cannot clip through them.
            _runtimeData.VerticalVelocity = 0f;
            Vector3 targetVelocity = _inputData.FlyDirectionVector * speed;
            _runtimeData.HorizontalVelocity = Vector3.Lerp(_runtimeData.HorizontalVelocity,
                targetVelocity, Time.deltaTime * _settings.SmoothSpeedChangeFlying);
            _runtimeData.RuntimeVelocity = _runtimeData.HorizontalVelocity;
            _settings.Controller.Move(_runtimeData.HorizontalVelocity * Time.deltaTime);
            _mtoaData.MainAnimationType = MainAnimationTypes.FlyIdle;

            if (_settings.ApplyFlyOffset)
            {
                _runtimeData.LocalPositionOfRotateParent = new Vector3(_runtimeData.MoveDirection.x*-2f, _settings.FlyOffsetY, _runtimeData.MoveDirection.z*-2f);
            }
            else
            {
                _runtimeData.LocalPositionOfRotateParent = new Vector3(0, _settings.FlyOffsetY,0);
            }

          
        }

        private void DispositionRotateParent()
        {
            _settings.RotateParent.localPosition = Vector3.Lerp( _settings.RotateParent.localPosition, _runtimeData.LocalPositionOfRotateParent, Time.unscaledDeltaTime*_settings.FlyOffsetSpeed);
        }

        private void OnFlyEnter()
        {
            _runtimeData.LocalPositionOfRotateParent = new Vector3(0, _settings.FlyOffsetY, 0f);
        }
        
        private void OnFlyExit()
        {
            _runtimeData.LocalPositionOfRotateParent = Vector3.zero;
            _runtimeData.VerticalVelocity = 0f;
            _runtimeData.HorizontalVelocity.y = 0f;
        }
    }
}
