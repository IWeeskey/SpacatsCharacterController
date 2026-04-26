using System;
using Spacats.Input;
using Spacats.CharacterCamera;
using Spacats.Utils;
using UnityEngine;

namespace Spacats.CharacterController
{
    public class PlayerCharacterController : MonoBehaviour
    {
        //private CharacterInputData _characterInput;
        [SerializeField] private CharacterCamera.CharacterCamera _characterCamera;
        [SerializeField] private PlayerCharacterInputRuntimeData _pRData;
        [SerializeField] private CharacterMovementController _movementController;
        [SerializeField] private CharacterRotationController _rotationController;
        private void Awake()
        {
            GetInput();
            _pRData.Reset();
            _characterCamera.OnInputProcessed += OnInputUpdated;
            _characterCamera.OnBeforeFollow += OnBeforeCameraUpdate;
        }

        private void OnDestroy()
        {
            _characterCamera.OnInputProcessed -= OnInputUpdated;
            _characterCamera.OnBeforeFollow -= OnBeforeCameraUpdate;
        }

        private void GetInput()
        {
            //_characterInput = InputController.Instance.CharacterInput;
        }

        void OnInputUpdated()
        {
            if (PauseController.IsPaused) return;
            _pRData.MoveDirection = _characterCamera.GetMoveDirection;
            //_rotationController.TryRotate(_pRData.MoveDirection);
        }

        void OnBeforeCameraUpdate()
        {
            // if (PauseController.IsPaused) return;
            // _movementController.TryMove(_pRData.MoveDirection);
            // _rotationController.TryRotate(_pRData.MoveDirection);
            //_rotationController.TryRotate(_pRData.MoveDirection);
        }

        void FixedUpdate()
        {
            if (PauseController.IsPaused) return;
            _movementController.TryMove(_pRData.MoveDirection);
            //_rotationController.TryRotate(_pRData.MoveDirection);
        }

        void Update()
        {
            _rotationController.TryRotate(_pRData.MoveDirection);
        }

        // private void LateUpdate()
        // {
        //     
        // }

        // private void FixedUpdate()
        // {
        //     if (PauseController.IsPaused) return;
        //     ApplyInput();
        //     _movementController.TryMove(_pRData.MoveDirection);
        //     //_rotationController.TryRotate(_pRData.MoveDirection);
        // }
    }
}
