using UnityEngine;

namespace Spacats.CharacterController
{
    public class CharacterSummaryController : MonoBehaviour
    {
        public bool IsPlayer = false;
        [SerializeField] private CharacterMovementController _movementController;
        [SerializeField] private CharacterRotationController _rotationController;
        [SerializeField] private CharacterInputRuntimeData _inputData;
        
        // private void Awake()
        // {
        //     //_pRData.Reset();
        // }

        public void SetInputData(CharacterInputRuntimeData  inputData)
        {
            _inputData = inputData;
        }

        void FixedUpdate()
        {
            _movementController.TryMove(_inputData.MoveDirectionV);
        }

        void Update()
        {
            // _inputData.MoveDirectionV = _characterCamera.GetMoveDirection;
            // _inputData.MoveDirectionInvertedV = _characterCamera.GetMoveDirectionInverted;
            // _inputData.ForwardVector = _characterCamera.GetForwardVector;
            // _inputData.MoveDirection = _characterInput.MoveDirection;
            // _inputData.MoveDirectionsLockBack =  _characterInput.MoveDirectionsLockBack;

            _inputData.ForwardVector.y = 0f;
            _inputData.MovingBack = _rotationController.TryRotate(_inputData);
        }
    }
}
