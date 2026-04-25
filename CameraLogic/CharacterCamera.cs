using System;
using Spacats.Input;
using UnityEngine;

namespace Spacats.CharacterCamera
{
    public class CharacterCamera : MonoBehaviour
    {
        private CharacterInputData _characterInput;
        
        [SerializeField] private CameraFollowTarget _followTarget;
        [SerializeField] private Transform _lookTransform;
        [SerializeField] private FollowCharacterSettings  _followCharacterSettings;

        [SerializeField] private FollowCharacterRuntimeData  _cRData;

        
        private void Awake()
        {
            GetInput();
            _cRData.Reset(_followCharacterSettings);
            DoFollowCharacterInstant();
        }

        private void GetInput()
        {
            _characterInput = InputController.Instance.CharacterInput;
        }

        void Update()
        {
            ApplyInput();
            DoFollowCharacter();
        }

        private void ApplyInput()
        {
            _cRData.TargetZoomValue -= _characterInput.ZoomDelta;
            _cRData.TargetZoomValue = Mathf.Clamp(_cRData.TargetZoomValue, _followCharacterSettings.MinMaxZoom.x, _followCharacterSettings.MinMaxZoom.y);

            _cRData.TargetEulers.y += _characterInput.LookDelta.x*_followCharacterSettings.RotationXModifier;//horizontal
            _cRData.TargetEulers.x += _characterInput.LookDelta.y*_followCharacterSettings.RotationYModifier;//vertical
            _cRData.TargetEulers.x = Mathf.Clamp(_cRData.TargetEulers.x, _followCharacterSettings.MinMaxRot.x, _followCharacterSettings.MinMaxRot.y);
        }

        private void DoFollowCharacter()
        {
            Vector3 targetPosition = DoFollowCharacter_position();
            DoFollowCharacter_zoom();
            DoFollowCharacter_rotation();
            
            _lookTransform.LookAt(targetPosition);
        }

        private Vector3 DoFollowCharacter_position()
        {
            if (_followTarget==null) return transform.forward;
            
            Vector3 targetPosition = _followTarget.GetFollowPosition();
            Vector3 selfPosition = transform.position;
            
            transform.position = Vector3.Lerp(selfPosition, targetPosition, Time.deltaTime*_followCharacterSettings.PositionFollowSpeed);
            return targetPosition;
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
            Vector3 targetPosition = _followTarget.GetFollowPosition();
            transform.position = targetPosition;
            
            if (_lookTransform==null) return;
            Vector3 lookOffset = Vector3.zero;
            lookOffset.y = _cRData.CurrentZoomValue * _followCharacterSettings.LookYModifier;
            lookOffset.z = _cRData.CurrentZoomValue * _followCharacterSettings.LookZModifier;
            _lookTransform.localPosition = lookOffset;

            gameObject.transform.localEulerAngles = _cRData.TargetEulers;
            
            
            _lookTransform.LookAt(targetPosition);
        }
    }
}
