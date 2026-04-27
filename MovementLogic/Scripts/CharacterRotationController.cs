using Spacats.Utils;
using UnityEngine;

namespace Spacats.CharacterController
{
    public class CharacterRotationController : MonoBehaviour
    {
        public bool DontLookBack = false;
        [SerializeField] private bool _movingBack = false;
        [SerializeField] private float _angle;
        [SerializeField] private float _fixedAngle;
        [SerializeField] private float _rotationSpeed = 1f;

        [SerializeField] private Vector3 _moveDirection;
        [SerializeField] private Vector3 _moveDirectionF;
        [SerializeField] private Vector2 _angleMinMax = new Vector2(-90, 90);
        
        [SerializeField] public Transform _rotateParent;
        //[SerializeField] private Rigidbody _rigidbody;
        public bool TryRotate(Vector3 moveDirection, Vector3 forwardVector)
        {
            if (PauseController.IsPaused) return _movingBack;
            if (moveDirection.magnitude<0.1f) return _movingBack;
            _movingBack = false;
            if (DontLookBack)
            {
                _moveDirection = moveDirection;
                _angle = Vector3.SignedAngle(forwardVector, moveDirection, Vector3.up);
                if (_angle >= _angleMinMax.x && _angle <= _angleMinMax.y)
                {
                    //ok
                }

                else
                {
                    moveDirection = forwardVector;
                    _movingBack = true;
                }
                
                //_moveDirectionF = ClampVectorAngleRange(forwardVector, moveDirection, -90, 90);
                //moveDirection = _moveDirectionF;
                //_fixedAngle =  Vector3.SignedAngle(forwardVector, moveDirection, Vector3.up);
            }
            
            Quaternion startQuat = _rotateParent.rotation;
            _rotateParent.LookAt(moveDirection*10f + _rotateParent.position);
            Quaternion targetQuat = _rotateParent.rotation;
            
            _rotateParent.rotation = Quaternion.Lerp(startQuat, targetQuat, _rotationSpeed*Time.deltaTime);
            return _movingBack;
        }
        
        public Vector3 ClampVectorAngleRange(Vector3 v1, Vector3 v3, float minAngle, float maxAngle)
        {
            float currentAngle = Vector3.SignedAngle(v1, v3, Vector3.up);
    
            // Ограничиваем угол
            float clampedAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);
    
            // Если угол не изменился, возвращаем исходный вектор
            if (Mathf.Approximately(currentAngle, clampedAngle))
                return v3;
    
            // Создаём повёрнутый вектор
            return Quaternion.Euler(0, clampedAngle, 0) * v1;
        }


    }
}