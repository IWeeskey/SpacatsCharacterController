using Spacats.Input;
using Spacats.CharacterCamera;
using Spacats.Utils;
using UnityEngine;

namespace Spacats.CharacterController
{
    public class PlayerCharacterController : MonoBehaviour
    {
        private CharacterInputData _characterInput;
        [SerializeField] private CharacterCamera.CharacterCamera _characterCamera;
        [SerializeField] private PlayerCharacterInputRuntimeData _pRData;
        [SerializeField] private CharacterMovementController _movementController;
        private void Awake()
        {
            GetInput();
            _pRData.Reset();
        }
        
        private void GetInput()
        {
            _characterInput = InputController.Instance.CharacterInput;
        }
        
        void FixedUpdate()
        {
            if (PauseController.IsPaused) return;
            ApplyInput();
            _movementController.TryMove(_pRData.MoveDirection);
        }
        
        private void ApplyInput()
        {
            _pRData.MoveDirection = _characterCamera.GetMoveDirection;
        }
    }
}
