using System;
using UnityEngine;

namespace Spacats.CharacterController
{
    public class CharacterSummaryController : MonoBehaviour
    {
        public bool IsPlayer = false;
        [SerializeField] private CharacterMovementController _movementController;
        [SerializeField] private CharacterRotationController _rotationController;
        [SerializeField] private CharacterAnimatorController _animatorController;
        [SerializeField] private CharacterLookAtController _lookAtController;
        [SerializeField] private CharacterInputRuntimeData _inputData;
        
        [SerializeField] private AnimatorToMovementData _atomData = new AnimatorToMovementData();
        [SerializeField] private MovementToAnimatorData _mtoaData = new MovementToAnimatorData();

        private bool _initialized = false;

        public Vector3 GetSelfForwardVector()
        {
            return _rotationController.GetForwardVector();
        }

        public void SetInputData(CharacterInputRuntimeData  inputData)
        {
            _atomData.Reset();
            _mtoaData.Reset();
            
            _inputData = inputData;
            _movementController.Init(inputData, _atomData, _mtoaData);
            _rotationController.Init(inputData);
            _animatorController?.Init(inputData, _atomData, _mtoaData);
            _lookAtController?.Init(inputData);
            _initialized = true;
        }

        void FixedUpdate()
        {
            if (!_initialized) return;
            _movementController.TryMove();
            //_lookAtController?.ProcessFixedUpdate();
        }

        void Update()
        { 
            if (!_initialized) return;
            _inputData.ForwardVector.y = 0f;
            _rotationController.TryRotate();
            _animatorController?.SyncData();
            _lookAtController?.ProcessUpdate();
        }

        private void LateUpdate()
        {
            if (!_initialized) return;
            _lookAtController?.ProcessLateUpdate();
        }
    }
}
