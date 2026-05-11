using Spacats.Input;
using Spacats.Utils;
using UnityEngine;

namespace Spacats.CharacterController
{
    public class CharacterRotationController : MonoBehaviour
    {
        public bool DontLookBack = false;
        public int FramesToFixAngle = 2;//NEED TO BE CHANGED TO TIME INSTEAD OF FRAMES
        [SerializeField] private int _framesToFixAngleLeft = 0;
        //[SerializeField] private bool _movingBack = false;
        [SerializeField] private float _angle;
        [SerializeField] private float _fixedAngle;
        [SerializeField] private float _rotationSpeed = 1f;

        [SerializeField] private Vector3 _moveDirection;
        [SerializeField] private Vector3 _moveDirectionF;
        [SerializeField] private Vector2 _angleMinMax = new Vector2(-91, 92);
        [SerializeField] private float _angleBorderGap = 2f;
        
        [SerializeField] public Transform _rotateParent;
        [SerializeField] public bool _prevAtRight = false; 
        [SerializeField] public bool _prevAtLeft = false;
        private CharacterInputRuntimeData _inputData;
        public void Init(CharacterInputRuntimeData inputData)
        {
            _inputData = inputData;
        }

        public Vector3 GetForwardVector()
        {
            if (_rotateParent == null) return Vector3.zero;
            return _rotateParent.forward;
        }

        private bool IsFlying()
        {
            return _inputData.Flying;
        }

        public void TryRotate()
        {
            if (PauseController.IsPaused) return;
            if (_inputData.MoveDirection == MoveDirections.Idle) return;
            //_movingBack = false;
            _moveDirection = _inputData.MoveDirectionVector;
            if (_inputData.MoveDirectionsLockBack != _inputData.MoveDirection && !IsFlying())
            {
                _moveDirection *= -1f;
            }
            
            if (DontLookBack && _framesToFixAngleLeft<=0 && !IsFlying())
            {
                _angle = Vector3.SignedAngle(_inputData.ForwardVector, _moveDirection, Vector3.up);
                if (_angle >= _angleMinMax.x && _angle <= _angleMinMax.y)
                {
                    //ok
                }
                //out of _angleMinMax
                else 
                {
                    _moveDirection = _inputData.ForwardVector;
                    //_movingBack = true;
                }

                bool curLeft = IsAngleAtLeftBorder(_angle);
                bool curRight = IsAngleAtRightBorder(_angle);
                
                if (curLeft || curRight)
                {
                    if (curLeft && _prevAtRight) _framesToFixAngleLeft = FramesToFixAngle;
                    if (curRight && _prevAtLeft) _framesToFixAngleLeft = FramesToFixAngle;
                }

                _prevAtRight = curRight;
                _prevAtLeft = curLeft;
            }
            
            

            if (_framesToFixAngleLeft > 0)
            {
                _moveDirection = _inputData.ForwardVector;
                _framesToFixAngleLeft--;
            }

            Quaternion startQuat = _rotateParent.rotation;
            _rotateParent.LookAt(_moveDirection*10f + _rotateParent.position);
            Quaternion targetQuat = _rotateParent.rotation;
            
            _rotateParent.rotation = Quaternion.Lerp(startQuat, targetQuat, _rotationSpeed*Time.deltaTime);
        }

        private bool IsAngleAtLeftBorder(float angle)
        {
            if (angle <= _angleMinMax.x + _angleBorderGap && angle >= -180)
            {
                return true;
            }

            return false;
        }
        
        private bool IsAngleAtRightBorder(float angle)
        {
            if (angle >= _angleMinMax.y-_angleBorderGap && angle <= 180)
            {
                return true;
            }

            return false;
        }

       

    }
}