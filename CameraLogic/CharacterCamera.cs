using System;
using Spacats.CharacterController;
using Spacats.Input;
using Spacats.Utils;
using UnityEngine;

namespace Spacats.CharacterCamera
{
    public class CharacterCamera : MonoBehaviour
    {
        public int ApplicationFrameRate = 30;
        
        private CharacterInputData _characterInput;
        [SerializeField] private CharacterSummaryController _playerSummary;
        [SerializeField] private LogicPauseFollowTarget _pauseFollowHandler;
        [SerializeField] private CameraFollowTarget _followTarget;
        [SerializeField] private CameraFollowTarget _pauseFollowTarget;
        
        private CameraFollowTarget _currentFollowTarget;
        [SerializeField] private Transform _lookTransform;
        [SerializeField] private Transform _lookAtTransform;
        [SerializeField] private Transform _moveDirectionTransform;
        //[SerializeField] private Transform _moveRotationTransform;
        [SerializeField] private FollowCharacterSettings  _followCharacterSettings;

        [SerializeField] private FollowCharacterRuntimeData  _cRData;
        
        [SerializeField] private LogicPauseSettings _logicPauseSettings;
        [SerializeField] private LogicPauseRuntimeData _logicPauseRData;

        private bool _prevInLogicPause = false;
        private bool _currentInLogicPause = false;
        
        private Vector3 _followVelocityRef = new Vector3(0,0,0);
        
        private CharacterInputRuntimeData _playerInput = new CharacterInputRuntimeData();

        private void Awake()
        {
            GetInput();
            _cRData.Reset(_followCharacterSettings);
            _playerInput.Reset();
            _playerSummary.IsPlayer = true;
            _playerSummary.SetInputData(_playerInput);
          
            Application.targetFrameRate = ApplicationFrameRate;
            _pauseFollowTarget.gameObject.transform.SetParent(null);
            _prevInLogicPause = false;
            _currentInLogicPause = false;
            _currentFollowTarget =  _followTarget;
            OnLogicPauseExit();
            DoFollowCharacterInstant();
        }

        private float GetFixedDeltaTime()
        {
            return Time.fixedUnscaledDeltaTime;
            if (Time.timeScale <= 0f) return 0f;
            return Time.fixedDeltaTime / Time.timeScale;
        }
        
        private float GetDeltaTime()
        {
            return Time.unscaledDeltaTime;
            if (Time.timeScale <= 0f) return 0f;
            return Time.deltaTime / Time.timeScale;
        }
        
        private float GetSmoothDampSpeed()
        {
            if (Time.timeScale <= 0f) return 0f;
            if (_followCharacterSettings.PositionFollowSpeed <= 0f) return 0f;
            float actualFollowSpeed = _followCharacterSettings.PositionFollowSpeed / Time.timeScale;
            return 1f / actualFollowSpeed;
        }

        private void GetInput()
        {
            _characterInput = InputController.Instance.CharacterInput;
        }

        void Update()
        {
            ApplyInput();
        }

        private void FixedUpdate()
        {
            if (_currentInLogicPause)
            {
                MoveLogicPause();
            }
        }

        void LateUpdate()
        {
            _currentInLogicPause = PauseController.IsPaused;
            if (_currentInLogicPause && !_prevInLogicPause)
            {
                OnLogicPauseEnter();
            }
            
            if (!_currentInLogicPause && _prevInLogicPause)
            {
                OnLogicPauseExit();
            }
           
            _prevInLogicPause = PauseController.IsPaused;
            
            DoFollowCharacter();
        }

        private void OnLogicPauseEnter()
        { 
            _currentFollowTarget = _pauseFollowTarget;
            _pauseFollowHandler.EnablePhysics();
        }
        
        private void OnLogicPauseExit()
        { 
            _cRData.TargetZoomValue = _cRData.BeforePauseZoomValue;
            _currentFollowTarget = _followTarget;
            _pauseFollowHandler.DisablePhysics();
        }

        private void MoveLogicPause()
        {
            Vector3 direction = Vector3.zero;
            Vector3 forwardDir = _lookTransform.forward;
            Vector3 rightDir = _lookTransform.right;
            switch (_characterInput.MoveDirection)
            {
                case MoveDirections.Idle: direction =  Vector3.zero;break;
                
                case MoveDirections.Forward: direction =  forwardDir; break;
                case MoveDirections.Backward: direction =  forwardDir*-1f;break;
                case MoveDirections.Left: direction =  rightDir*-1f;break;
                case MoveDirections.Right: direction =  rightDir;break;
                
                case MoveDirections.ForwardLeft: direction = (forwardDir+rightDir*-1f)/2f;  break;
                case MoveDirections.ForwardRight: direction =  (forwardDir+rightDir)/2f; break;
                
                case MoveDirections.BackwardLeft: direction = (forwardDir*-1f+rightDir*-1f)/2f;  break;
                case MoveDirections.BackwardRight: direction =  (forwardDir*-1f+rightDir)/2f; break;
            }

            if (_characterInput.Sitting)
            {
                direction += _lookTransform.up*-1f;
                direction /= 2f;
            }
            else if (_characterInput.Jumping)
            {  
                direction += _lookTransform.up;
                direction /= 2f;
            }
            float speed = _logicPauseSettings.MoveSpeed;
            if (_characterInput.Sprinting) speed *= _logicPauseSettings.SprintMultiplier;
            _pauseFollowHandler.SetVelocity(direction*GetFixedDeltaTime()*speed);
        }

        private void ApplyInput()
        {
            
            if (!_currentInLogicPause) _cRData.TargetZoomValue -= _characterInput.ZoomDelta;
            else _cRData.TargetZoomValue = _followCharacterSettings.MinMaxZoom.x;
            _cRData.TargetZoomValue = Mathf.Clamp(_cRData.TargetZoomValue, _followCharacterSettings.MinMaxZoom.x, _followCharacterSettings.MinMaxZoom.y);

            _cRData.TargetEulers.y += _characterInput.LookDelta.x*_followCharacterSettings.RotationXModifier;//horizontal
            _cRData.TargetEulers.x += _characterInput.LookDelta.y*_followCharacterSettings.RotationYModifier;//vertical
            _cRData.TargetEulers.x = Mathf.Clamp(_cRData.TargetEulers.x, _followCharacterSettings.MinMaxRot.x, _followCharacterSettings.MinMaxRot.y);
            Vector3 inputDirection = MoveDirectionsHelper.DirectionVectors[(int)_characterInput.MoveDirection];
            _moveDirectionTransform.localPosition = new Vector3(inputDirection.x, 0, inputDirection.y);
            Vector3 dirPosition = _moveDirectionTransform.position;
            Vector3 selfPosition = gameObject.transform.position;
            dirPosition.y = selfPosition.y;
            _cRData.MoveDirection = - selfPosition + dirPosition;
            _cRData.MoveDirectionLockBack = _cRData.MoveDirection;
            if (_characterInput.MoveDirection == MoveDirections.BackwardLeft || _characterInput.MoveDirection == MoveDirections.BackwardRight)
            {
                _moveDirectionTransform.localPosition = new Vector3(inputDirection.x*-1f, 0, inputDirection.y*-1f);
                dirPosition = _moveDirectionTransform.position;
                dirPosition.y = selfPosition.y;
                _cRData.MoveDirectionLockBack = - selfPosition + dirPosition;
            }


            ApplyToPlayerInput();
            //OnInputProcessed?.Invoke();
        }

        private void ApplyToPlayerInput()
        {
            _playerInput.MoveDirectionVector = _cRData.MoveDirection;
            _playerInput.ForwardVector = transform.forward;
            _playerInput.MoveDirection = _characterInput.MoveDirection;
            _playerInput.MoveDirectionsLockBack =  _characterInput.MoveDirectionsLockBack;

            _playerInput.Sitting = _characterInput.Sitting;
            _playerInput.Jumping = _characterInput.Jumping;
            _playerInput.Sprinting = _characterInput.Sprinting;
        }

        private void DoFollowCharacter()
        {
            //if (PauseController.IsPaused) return;
            
            DoFollowCharacter_position();
            DoFollowCharacter_zoom();
            DoFollowCharacter_rotation();
            
            _lookTransform.LookAt(_lookAtTransform);
        }

        private void DoFollowCharacter_position()
        {
            if (_currentFollowTarget==null) return;
            if (_lookAtTransform==null) return;
            
            Vector3 targetPosition = _currentFollowTarget.GetFollowPosition();
           
            //Vector3 selfPosition = transform.position;
            _lookAtTransform.localPosition = _followCharacterSettings.FixedLookAtOffset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _followVelocityRef, GetSmoothDampSpeed());
            
            if (!_currentInLogicPause) _pauseFollowTarget.transform.position = _lookTransform.position;
        }

        private void DoFollowCharacter_zoom()
        {
            if (_currentFollowTarget==null) return;
            if (_lookTransform==null) return;
            _cRData.CurrentZoomValue = Mathf.Lerp(_cRData.CurrentZoomValue, _cRData.TargetZoomValue, GetDeltaTime() *_followCharacterSettings.ZoomSpeed);
            if (!_currentInLogicPause) _cRData.BeforePauseZoomValue = _cRData.CurrentZoomValue;
            
            Vector3 lookOffset = Vector3.zero;
            lookOffset.y = _cRData.CurrentZoomValue * _followCharacterSettings.LookYModifier;
            lookOffset.z = _cRData.CurrentZoomValue * _followCharacterSettings.LookZModifier;
            _lookTransform.localPosition = lookOffset;
        }

        private void DoFollowCharacter_rotation()
        {
            Quaternion startQuat = gameObject.transform.rotation;
            gameObject.transform.localEulerAngles = _cRData.TargetEulers;
            Quaternion targetQuat = gameObject.transform.rotation;

            gameObject.transform.rotation = Quaternion.Lerp(startQuat, targetQuat,GetDeltaTime() * _followCharacterSettings.RotationFollowSpeed);
        }

        private void DoFollowCharacterInstant()
        {
            if (_followTarget==null) return;
            if (_lookAtTransform==null) return;
            Vector3 targetPosition = _followTarget.GetFollowPosition();
            transform.position = targetPosition;
            _lookAtTransform.localPosition = _followCharacterSettings.FixedLookAtOffset;
            
            if (_lookTransform==null) return;
            Vector3 lookOffset = Vector3.zero;
            lookOffset.y = _cRData.CurrentZoomValue * _followCharacterSettings.LookYModifier;
            lookOffset.z = _cRData.CurrentZoomValue * _followCharacterSettings.LookZModifier;
            _lookTransform.localPosition = lookOffset;

            gameObject.transform.localEulerAngles = _cRData.TargetEulers;
            
            
            _lookTransform.LookAt(_lookAtTransform);
        }
    }
}
