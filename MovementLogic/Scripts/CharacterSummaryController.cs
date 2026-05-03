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
        }

        void FixedUpdate()
        {
            _movementController.TryMove();
        }

        void Update()
        {
            _inputData.ForwardVector.y = 0f;
            _rotationController.TryRotate();
            _animatorController?.SyncData();
        }

        private void LateUpdate()
        {
        }
    }
}
