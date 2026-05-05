using System;
using System.Collections.Generic;
using Spacats.Utils;
using UnityEngine;
using UnityEngine.XR;

namespace Spacats.CharacterController
{
    public class CharacterLookAtController : MonoBehaviour
    {
        public bool DoLogic = true;
        [SerializeField] private float _stopRotateSpeed;
        [SerializeField] private Vector2 _maxAngleHor;
        [SerializeField] private Vector2 _maxDistVer;
        [SerializeField] private Vector3 _eyeRefPosition;
        private float _currentRotateMultiplier;
        private float _targetRotateMultiplier;
        private float _angleHor;
        [SerializeField] private float _distVer;
        [SerializeField] private Animator _animator;
        [SerializeField] private List<float> _weights = new List<float>();
        private Vector3 _currentLookAtPosition;
        
        private CharacterInputRuntimeData _inputData;

        public Transform tmpTest;
        
        private bool _prevInLogicPause = false;
        private bool _currentInLogicPause = false;
        
        public void Init(CharacterInputRuntimeData inputData)
        {
            _inputData = inputData;
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (!DoLogic) return;
            
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
           
            if (!_currentInLogicPause)
            {
                RefreshRotateMultiplier();
                _currentRotateMultiplier = Mathf.Lerp(_currentRotateMultiplier, _targetRotateMultiplier, Time.deltaTime * _stopRotateSpeed);
                _currentLookAtPosition = _inputData.LookAtPoint;
            }
            
            float finalWeight = _weights[0] * _currentRotateMultiplier;
            if (finalWeight<0) return;
            
            _animator.SetLookAtPosition(_currentLookAtPosition);
            _animator.SetLookAtWeight(finalWeight, _weights[1], _weights[2], _weights[3], _weights[4]);
        }

        private void RefreshRotateMultiplier()
        {
            Vector3 selfPos = gameObject.transform.position;
            Vector3 targetPointVer = _inputData.LookAtPoint;
            targetPointVer.x = selfPos.x;
            targetPointVer.z = selfPos.z;
            _distVer = selfPos.y - targetPointVer.y;
            _inputData.LookAtPoint.y = Mathf.Clamp(_inputData.LookAtPoint.y, 
                selfPos.y + _eyeRefPosition.y + _maxDistVer.x, 
                selfPos.y + _eyeRefPosition.y + _maxDistVer.y);
            
            
            Vector3 direction = (-selfPos +_inputData.LookAtPoint).normalized;
            //Vector3 targetPointVer = _inputData.LookAtPoint;
            //targetPointVer.x = selfPos.x;
            //targetPointVer.z = selfPos.z;
            
            //tmpTest.gameObject.transform.position = targetPointVer;
            
            //Vector3 directionHor = (-selfPos +targetPointVer).normalized;
            //Vector3 directionInv = (gameObject.transform.position -_inputData.LookAtPoint).normalized;
            _angleHor = Vector3.SignedAngle(transform.forward, direction, transform.up);
           
            _targetRotateMultiplier = 1f;
            if (_angleHor > 0 && _angleHor > _maxAngleHor.y) _targetRotateMultiplier = 0f;
            if (_angleHor < 0 && _angleHor < _maxAngleHor.x) _targetRotateMultiplier = 0f;
            //
            // if (_angleVer > 0 && _angleVer > _maxAngleVer.y) _targetRotateMultiplier = 0f;
            // if (_angleVer < 0 && _angleVer < _maxAngleVer.x) _targetRotateMultiplier = 0f;
            

        }
        
        private void OnLogicPauseEnter()
        { 
        }
        
        private void OnLogicPauseExit()
        { 
        }

    }
}
