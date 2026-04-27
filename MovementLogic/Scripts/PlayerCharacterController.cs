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
            _pRData.Reset();
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
            _pRData.ForwardVector = _characterCamera.GetForwardVector;
            _pRData.ForwardVector.y = 0f;
            _rotationController.TryRotate(_pRData.MoveDirection, _pRData.ForwardVector);
        }
    }
}
