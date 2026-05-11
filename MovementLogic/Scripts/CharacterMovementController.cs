using System;
using Spacats.Input;
using Spacats.Utils;
using UnityEngine;

namespace Spacats.CharacterController
{
    /// <summary>
    /// Works at fixed update
    /// </summary>
    public class CharacterMovementController : MonoBehaviour
    {
        [SerializeField] private MovementRuntimeData _runtimeData;
        [SerializeField] private MovementSettings _settings;
        private CharacterInputRuntimeData _inputData;
        private AnimatorToMovementData _atomData;
        private MovementToAnimatorData _mtoaData;
        public void Init(CharacterInputRuntimeData inputData, AnimatorToMovementData atomData, MovementToAnimatorData mtoaData)
        {
            _inputData = inputData;
            _atomData = atomData;
            _mtoaData = mtoaData;
            _runtimeData.CurrentFlying = false;
            _runtimeData.PreviousFlying = false;
        }

        public void CallUpdate()
        {
            if (PauseController.IsPaused)
            {
                return;
            }
            
            DispositionRotateParent();
        }

        public void TryMoveFixedUpdate()
        {
            _runtimeData.RigidBodyVelocity = _settings.Rigidbody.linearVelocity;
            _runtimeData.RigidBodySpeed = _runtimeData.RigidBodyVelocity.magnitude;
            _mtoaData.MainAnimationType = MainAnimationTypes.Idle;
            if (PauseController.IsPaused)
            {
                //_settings.Rigidbody.for
                _settings.Rigidbody.linearVelocity = Vector3.zero;
                _settings.Rigidbody.angularVelocity = Vector3.zero;
                //_settings.Rigidbody.isKinematic = true;
                _runtimeData.WasPaused = true;
                return;
            }

            _runtimeData.MoveDirection = _inputData.MoveDirectionVector;
            
            if (_runtimeData.WasPaused)
            {
                //_settings.Rigidbody.isKinematic = false;
                _runtimeData.WasPaused = false;
                _settings.Rigidbody.linearVelocity = _runtimeData.RuntimeVelocity;
            }

            GetDistanceToGround();
            CalculateState();
            RefreshCurrentSpeed();
            
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
                return;
            }

            switch (_runtimeData.State)
            {
                case SpaceStates.InAir: ProcessInAir(); break;
                case SpaceStates.OnGround: ProcessOnGround(); break;
                case SpaceStates.InWater: break;
            }
        }

        private void RefreshCurrentSpeed()
        {
            Vector3 horizontalVelocity = _settings.Rigidbody.linearVelocity;
            horizontalVelocity.y = 0f;
            _runtimeData.HorizontalSpeed = horizontalVelocity.magnitude;
        }

        private void GetDistanceToGround()
        {
            _runtimeData.Ray.origin = _settings.Rigidbody.position+Vector3.up*0.5f;
            _runtimeData.Ray.direction = Vector3.down;
            
            Physics.Raycast(_runtimeData.Ray, out _runtimeData.RHit, Mathf.Infinity, _settings.GroundLayers);
            if (_runtimeData.RHit.collider == null)
            {
                _runtimeData.DistanceToGround = 999f;
                return;
            }

            _runtimeData.DistanceToGround = _runtimeData.RHit.distance;
        }

        private void CalculateState()
        {
            if (_runtimeData.DistanceToGround < _settings.OnGroundThreshold)
            {
                _runtimeData.State = SpaceStates.OnGround;
                return;
            }
            
            _runtimeData.State = SpaceStates.InAir;
        }

        private void ProcessOnGround()
        {
            
            float speed = ProcessOnGroundSpeed();
            
            _runtimeData.RuntimeVelocity = _runtimeData.MoveDirection * speed;
            _runtimeData.RuntimeVelocity.y = _runtimeData.DistanceToGround*-0.5f;

            _settings.Rigidbody.linearVelocity = Vector3.Lerp(_settings.Rigidbody.linearVelocity,
                _runtimeData.RuntimeVelocity, Time.fixedUnscaledDeltaTime * _settings.SmoothSpeedChange);

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
            _runtimeData.RuntimeVelocity = _settings.Rigidbody.linearVelocity;
            _runtimeData.RuntimeVelocity.y = _settings.Gravity;
            _settings.Rigidbody.linearVelocity = Vector3.Lerp(_settings.Rigidbody.linearVelocity,
                _runtimeData.RuntimeVelocity, Time.fixedUnscaledDeltaTime * _settings.SmoothSpeedChange);
        }
        
        private void ProcessFlying()
        {
            float speed = _settings.FlySpeed;
            _runtimeData.RuntimeVelocity = _inputData.FlyDirectionVector * speed;
            _settings.Rigidbody.linearVelocity = Vector3.Lerp(_settings.Rigidbody.linearVelocity, _runtimeData.RuntimeVelocity, Time.fixedUnscaledDeltaTime * _settings.SmoothSpeedChangeFlying);
            _mtoaData.MainAnimationType = MainAnimationTypes.FlyIdle;

            _runtimeData.LocalPositionOfRotateParent = new Vector3(_runtimeData.MoveDirection.x*-2f, _settings.FlyOffsetY,
                _runtimeData.MoveDirection.z*-2f);
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
        }
    }
}
