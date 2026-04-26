using System;
using Spacats.Utils;
using UnityEngine;

namespace Spacats.CharacterController
{
    public class CharacterMovementController : MonoBehaviour
    {
        [SerializeField] private LayerMask _groundLayers;
        [SerializeField] private float _distanceToGround;
        
        [SerializeField] private float _onGroundThreshold = 0.5f;
        [SerializeField] private float _gravity = -9.8f;
        [SerializeField] private float _moveSpeed = 1f;
        //[SerializeField] private float _changeDirectionSpeed = 1f;
        [SerializeField] private Rigidbody _rigidbody;
        private Vector3 _unpausedVelocity;
        private bool _wasPaused = false;
        
        private RaycastHit _rHit = new RaycastHit();
        private Ray _ray = new Ray();

        [SerializeField] private Vector3 _runtimeVelocity;
         
        public void TryMove(Vector3 moveDirection)
        {
            if (PauseController.IsPaused)
            {
                _rigidbody.linearVelocity = Vector3.zero;
                _wasPaused = true;
                return;
            }

            DetectGround();

            if (_wasPaused)
            {
                _wasPaused = false;
                _rigidbody.linearVelocity = _unpausedVelocity;
            }
            //if (moveDirection.magnitude<0.1f) return;
            
            //Vector3 startVelocity = _rigidbody.linearVelocity;
            _runtimeVelocity = moveDirection * _moveSpeed;
            _runtimeVelocity.y = _rigidbody.linearVelocity.y;
            if (_distanceToGround>_onGroundThreshold)  _runtimeVelocity.y = _gravity;
            
            _rigidbody.linearVelocity = _runtimeVelocity;
            _unpausedVelocity = _rigidbody.linearVelocity;
        }

        private void DetectGround()
        {
            _ray.origin = _rigidbody.position;
            _ray.direction = Vector3.down;
            
            Physics.Raycast(_ray, out _rHit, Mathf.Infinity, _groundLayers);
            // if (_rHit.collider == null)
            // {
            //     _distanceToGround = 999f;
            //     return;
            // }

            _distanceToGround = _rHit.distance;
        }
    }
}
