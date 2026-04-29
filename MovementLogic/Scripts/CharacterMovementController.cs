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
        }

        public void TryMove()
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
            _runtimeData.Ray.origin = _settings.Rigidbody.position;
            _runtimeData.Ray.direction = Vector3.down;
            
            Physics.Raycast(_runtimeData.Ray, out _runtimeData.RHit, Mathf.Infinity, _settings.GroundLayers);
            // if (_rHit.collider == null)
            // {
            //     _distanceToGround = 999f;
            //     return;
            // }

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
                _runtimeData.RuntimeVelocity, Time.fixedDeltaTime * _settings.SmoothSpeedChange);
            
            if (_inputData.MoveDirection == MoveDirections.Idle) _mtoaData.MainAnimationType = MainAnimationTypes.Idle;
        }

        private float ProcessOnGroundSpeed()
        {
            float speed = _settings.ForwardSpeed;
            _mtoaData.MainAnimationType = MainAnimationTypes.RunForward;
            
            if (_inputData.MoveType == MoveInputTypes.Sprint)
            {
                speed = _settings.ForwardSpeed*_settings.SprintMultiplier;
                _mtoaData.MainAnimationType = MainAnimationTypes.SprintForward;
                if (_inputData.IsMovingBack())
                {
                    speed = _settings.BackwardSpeed*_settings.SprintMultiplier;
                    _mtoaData.MainAnimationType = MainAnimationTypes.SprintBackward;
                }
                
                return speed;
            }
            
            if (_inputData.MoveType == MoveInputTypes.Crouch)
            {
                speed = _settings.SitSpeed;
                _mtoaData.MainAnimationType = MainAnimationTypes.CrouchForward;
                if (_inputData.IsMovingBack())
                {
                    _mtoaData.MainAnimationType = MainAnimationTypes.CrouchBackward;
                }
                
                return speed;
            }
            
            if (_inputData.MoveType == MoveInputTypes.Walk)
            {
                speed = _settings.ForwardSpeed*_settings.WalkMultiplier;
                _mtoaData.MainAnimationType = MainAnimationTypes.WalkForward;
                if (_inputData.IsMovingBack())
                {
                    speed = _settings.BackwardSpeed*_settings.WalkMultiplier;
                    _mtoaData.MainAnimationType = MainAnimationTypes.WalkBackward;
                }
                
                return speed;
            }
            
            if (_inputData.IsMovingBack())
            {
                speed = _settings.BackwardSpeed;
                _mtoaData.MainAnimationType = MainAnimationTypes.RunBackward;
            }
            
            return speed;
        }

        private void ProcessInAir()
        {
            _runtimeData.RuntimeVelocity.y = _settings.Gravity;
            _settings.Rigidbody.linearVelocity = Vector3.Lerp(_settings.Rigidbody.linearVelocity,
                _runtimeData.RuntimeVelocity, Time.fixedDeltaTime * _settings.SmoothSpeedChange);
        }
    }
}
