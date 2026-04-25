using Spacats.Utils;
using UnityEngine;

namespace Spacats.CharacterController
{
    public class CharacterMovementController : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 1f;
        [SerializeField] private Rigidbody _rigidbody;
        public void TryMove(Vector3 moveDirection)
        {
            if (PauseController.IsPaused) return;
            if (moveDirection.magnitude<0.1f) return;
            // Vector3 targetPosition = _rigidbody.position + moveDirection * _moveSpeed * Time.deltaTime;
            // _rigidbody.MovePosition(targetPosition);

            _rigidbody.linearVelocity = moveDirection * _moveSpeed;
        }
    }
}
