using System;
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
        
        public void TryMove(Vector3 moveDirection)
        {
            _runtimeData.RigidBodyVelocity = _settings.Rigidbody.linearVelocity;
            _runtimeData.RigidBodySpeed = _runtimeData.RigidBodyVelocity.magnitude;
            if (PauseController.IsPaused)
            {
                //_settings.Rigidbody.for
                _settings.Rigidbody.linearVelocity = Vector3.zero;
                _settings.Rigidbody.angularVelocity = Vector3.zero;
                //_settings.Rigidbody.isKinematic = true;
                _runtimeData.WasPaused = true;
                return;
            }

            _runtimeData.MoveDirection = moveDirection;
            
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
            _runtimeData.RuntimeVelocity = _runtimeData.MoveDirection * _settings.ForwardSpeed;
            _runtimeData.RuntimeVelocity.y = _runtimeData.DistanceToGround*-0.5f;

            _settings.Rigidbody.linearVelocity = Vector3.Lerp(_settings.Rigidbody.linearVelocity,
                _runtimeData.RuntimeVelocity, Time.fixedDeltaTime * _settings.SmoothSpeedChange);
        }
        
        private void ProcessInAir()
        {
            _runtimeData.RuntimeVelocity.y = _settings.Gravity;
            //_settings.Rigidbody.linearVelocity = _runtimeData.RuntimeVelocity;
            _settings.Rigidbody.linearVelocity = Vector3.Lerp(_settings.Rigidbody.linearVelocity,
                _runtimeData.RuntimeVelocity, Time.fixedDeltaTime * _settings.SmoothSpeedChange);
        }
    }
}
