using System;
using Spacats.Input;
using Spacats.CharacterCamera;
using Spacats.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Spacats.CharacterController
{
    public class PlayerCharacterController : MonoBehaviour
    {
        private CharacterInputData _characterInput;
        [SerializeField] private CharacterCamera.CharacterCamera _characterCamera;
        [SerializeField] private PlayerCharacterInputRuntimeData _pRData;
        [SerializeField] private CharacterMovementController _movementController;
        [SerializeField] private CharacterRotationController _rotationController;
        
        private void Awake()
        {
            _pRData.Reset();
            _characterInput = InputController.Instance.CharacterInput;
        }

        private void OnDestroy()
        {
            
        }

        void FixedUpdate()
        {
            _movementController.TryMove(_pRData.MoveDirection);
        }

        void Update()
        {
            _pRData.MoveDirection = _characterCamera.GetMoveDirection;
            _pRData.MoveDirectionInverted = _characterCamera.GetMoveDirectionInverted;
            _pRData.ForwardVector = _characterCamera.GetForwardVector;

            _pRData.ForwardVector.y = 0f;
            _pRData.MovingBack = _rotationController.TryRotate(_pRData.MoveDirection,_pRData.MoveDirectionInverted, _pRData.ForwardVector, _characterInput.MoveDirection, _characterInput.MoveDirectionsLockBack);
        }
    }
}
