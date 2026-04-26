using Spacats.Utils;
using UnityEngine;

namespace Spacats.CharacterController
{
    public class CharacterRotationController : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 1f;

        [SerializeField] public Transform _rotateParent;
        //[SerializeField] private Rigidbody _rigidbody;
        public void TryRotate(Vector3 moveDirection)
        {
            if (PauseController.IsPaused) return;
            if (moveDirection.magnitude<0.1f) return;
            Quaternion startQuat = _rotateParent.rotation;
            _rotateParent.LookAt(moveDirection*10f + _rotateParent.position);
            Quaternion targetQuat = _rotateParent.rotation;
            
            _rotateParent.rotation = Quaternion.Lerp(startQuat, targetQuat, _rotationSpeed*Time.deltaTime);

            //gameObject.transform.rotation = startQuat;
            // Vector3 lookAtVector = moveDirection * 10f + gameObject.transform.position;
            // Vector3 direction = 
            
            //Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            //Quaternion deltaRotation = Quaternion.Euler(new Vector3(0,10,0) * Time.fixedDeltaTime);
            //_rigidbody.rotation = Quaternion.Lerp(startQuat, targetQuat, _rotationSpeed*Time.fixedDeltaTime);
            //_rigidbody.MoveRotation(targetRotation);
            //Quaternion.le

            //_rigidbody.rotation = Quaternion.Lerp(startQuat, targetQuat, _rotationSpeed*Time.deltaTime);
            // Vector3 targetPosition = _rigidbody.position + moveDirection * _moveSpeed * Time.deltaTime;
            // _rigidbody.MovePosition(targetPosition);

            //_rigidbody.linearVelocity = moveDirection * _moveSpeed;
        }
    }
}