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
        
        [SerializeField] private CameraFollowTarget _followTarget;
        [SerializeField] private Transform _lookTransform;
        [SerializeField] private Transform _lookAtTransform;
        [SerializeField] private Transform _moveDirectionTransform;
        [SerializeField] private Transform _moveRotationTransform;
        [SerializeField] private FollowCharacterSettings  _followCharacterSettings;

        [SerializeField] private FollowCharacterRuntimeData  _cRData;
        // public Vector3 GetMoveDirection => _cRData.MoveDirection;
        // public Vector3 GetMoveDirectionInverted => _cRData.MoveDirectionLockBack;
        // public Vector3 GetForwardVector => transform.forward;
        //public Action OnInputProcessed;
        //public Action OnBeforeFollow;
        
        private Vector3 _followVelocityRef = new Vector3(0,0,0);
        
        private CharacterInputRuntimeData _playerInput = new CharacterInputRuntimeData();
        [SerializeField] private CharacterSummaryController _playerSummary;
        private void Awake()
        {
            GetInput();
            _cRData.Reset(_followCharacterSettings);
            _playerInput.Reset();
            _playerSummary.IsPlayer = true;
            _playerSummary.SetInputData(_playerInput);
            DoFollowCharacterInstant();
            Application.targetFrameRate = ApplicationFrameRate;
        }

        private void GetInput()
        {
            _characterInput = InputController.Instance.CharacterInput;
        }

        void Update()
        {
            ApplyInput();
        }

        void LateUpdate()
        {
            //OnBeforeFollow?.Invoke();
            DoFollowCharacter();
        }

        private void ApplyInput()
        {
            _cRData.TargetZoomValue -= _characterInput.ZoomDelta;
            _cRData.TargetZoomValue = Mathf.Clamp(_cRData.TargetZoomValue, _followCharacterSettings.MinMaxZoom.x, _followCharacterSettings.MinMaxZoom.y);

            _cRData.TargetEulers.y += _characterInput.LookDelta.x*_followCharacterSettings.RotationXModifier;//horizontal
            _cRData.TargetEulers.x += _characterInput.LookDelta.y*_followCharacterSettings.RotationYModifier;//vertical
            _cRData.TargetEulers.x = Mathf.Clamp(_cRData.TargetEulers.x, _followCharacterSettings.MinMaxRot.x, _followCharacterSettings.MinMaxRot.y);

            _moveDirectionTransform.localPosition = new Vector3(_characterInput.Movement.x, 0, _characterInput.Movement.y);
            Vector3 dirPosition = _moveDirectionTransform.position;
            Vector3 selfPosition = gameObject.transform.position;
            dirPosition.y = selfPosition.y;
            _cRData.MoveDirection = - selfPosition + dirPosition;
            _cRData.MoveDirectionLockBack = _cRData.MoveDirection;
            if (_characterInput.MoveDirection == MoveDirections.BackwardLeft || _characterInput.MoveDirection == MoveDirections.BackwardRight)
            {
                _moveDirectionTransform.localPosition = new Vector3(_characterInput.Movement.x*-1f, 0, _characterInput.Movement.y*-1f);
                dirPosition = _moveDirectionTransform.position;
                dirPosition.y = selfPosition.y;
                _cRData.MoveDirectionLockBack = - selfPosition + dirPosition;
            }


            ApplyToPlayerInput();
            //OnInputProcessed?.Invoke();
        }

        private void ApplyToPlayerInput()
        {
            _playerInput.MoveDirectionV = _cRData.MoveDirection;
            _playerInput.MoveDirectionInvertedV = _cRData.MoveDirectionLockBack;
            _playerInput.ForwardVector = transform.forward;
            _playerInput.MoveDirection = _characterInput.MoveDirection;
            _playerInput.MoveDirectionsLockBack =  _characterInput.MoveDirectionsLockBack;
        }

        private void DoFollowCharacter()
        {
            if (PauseController.IsPaused) return;
            
            DoFollowCharacter_position();
            DoFollowCharacter_zoom();
            DoFollowCharacter_rotation();
            
            _lookTransform.LookAt(_lookAtTransform);
        }

        private void DoFollowCharacter_position()
        {
            if (_followTarget==null) return;
            if (_lookAtTransform==null) return;
            
            Vector3 targetPosition = _followTarget.GetFollowPosition();
            //Vector3 selfPosition = transform.position;
            _lookAtTransform.localPosition = _followCharacterSettings.FixedLookAtOffset;
            //transform.position = Vector3.Lerp(selfPosition, targetPosition, Time.deltaTime*_followCharacterSettings.PositionFollowSpeed);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _followVelocityRef, 1f/_followCharacterSettings.PositionFollowSpeed);
        }

        private void DoFollowCharacter_zoom()
        {
            if (_followTarget==null) return;
            if (_lookTransform==null) return;
            _cRData.CurrentZoomValue = Mathf.Lerp(_cRData.CurrentZoomValue, _cRData.TargetZoomValue, Time.deltaTime*_followCharacterSettings.ZoomSpeed);
            
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

            gameObject.transform.rotation = Quaternion.Lerp(startQuat, targetQuat,Time.deltaTime * _followCharacterSettings.RotationFollowSpeed);
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
