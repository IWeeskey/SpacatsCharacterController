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
        [SerializeField] private Vector2 _maxAngle;
        private float _currentRotateMultiplier;
        private float _targetRotateMultiplier;
        private float _angle;
        [SerializeField] private Animator _animator;
        [SerializeField] private List<float> _weights = new List<float>();
        
        private CharacterInputRuntimeData _inputData;
        
        public void Init(CharacterInputRuntimeData inputData)
        {
            _inputData = inputData;
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (!DoLogic) return;
            if (PauseController.IsPaused) return;
            
            RefreshRotateMultiplier();
            float finalWeight = _weights[0] * _currentRotateMultiplier;
            if (finalWeight<0) return;
            
            _animator.SetLookAtPosition(_inputData.LookAtPoint);
            _animator.SetLookAtWeight(finalWeight, _weights[1], _weights[2], _weights[3], _weights[4]);
        }

        private void RefreshRotateMultiplier()
        {
            Vector3 direction = (-gameObject.transform.position +_inputData.LookAtPoint).normalized;
            _angle = Vector3.SignedAngle(gameObject.transform.forward, direction, Vector3.up);
            _targetRotateMultiplier = 1f;
            if (_angle > 0 && _angle > _maxAngle.y) _targetRotateMultiplier = 0f;
            if (_angle < 0 && _angle < _maxAngle.x) _targetRotateMultiplier = 0f;
            
            _currentRotateMultiplier = Mathf.Lerp(_currentRotateMultiplier, _targetRotateMultiplier, Time.deltaTime * _stopRotateSpeed);
        }
    }
}
