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
        }

        private void DoFollowCharacter()
        {
            if (_followTarget==null) return;
            
            Vector3 targetPosition = _followTarget.GetFollowPosition();
            Vector3 selfPosition = transform.position;
            
            transform.position = Vector3.Lerp(selfPosition, targetPosition, Time.deltaTime*_followCharacterSettings.PositionFollowSpeed);
            
            if (_lookTransform==null) return;
            _cRData.CurrentZoomValue = Mathf.Lerp(_cRData.CurrentZoomValue, _cRData.TargetZoomValue, Time.deltaTime*_followCharacterSettings.ZoomSpeed);
            
            Vector3 lookOffset = Vector3.zero;
            lookOffset.y = _cRData.CurrentZoomValue * _followCharacterSettings.LookYDevider;
            lookOffset.z = _cRData.CurrentZoomValue * _followCharacterSettings.LookZDevider;
            _lookTransform.localPosition = lookOffset;
            
            _lookTransform.LookAt(targetPosition);
        }

        private void DoFollowCharacterInstant()
        {
            if (_followTarget==null) return;
            Vector3 targetPosition = _followTarget.GetFollowPosition();
            transform.position = targetPosition;
            
            if (_lookTransform==null) return;
            Vector3 lookOffset = Vector3.zero;
            lookOffset.y = _cRData.CurrentZoomValue * _followCharacterSettings.LookYDevider;
            lookOffset.z = _cRData.CurrentZoomValue * _followCharacterSettings.LookZDevider;
            _lookTransform.localPosition = lookOffset;
            
            _lookTransform.LookAt(targetPosition);
            
        }
    }
}
