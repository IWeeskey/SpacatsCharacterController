using UnityEngine;

namespace Spacats.CharacterController
{
    public class CharacterSummaryController : MonoBehaviour
    {
        public bool IsPlayer = false;
        [SerializeField] private CharacterMovementController _movementController;
        [SerializeField] private CharacterRotationController _rotationController;
        [SerializeField] private CharacterInputRuntimeData _inputData;

        public void SetInputData(CharacterInputRuntimeData  inputData)
        {
            _inputData = inputData;
        }

        void FixedUpdate()
        {
            if (IsPlayer) _movementController.TryMove(_inputData.MoveDirectionV);
        }

        void Update()
        {
            if (IsPlayer)
            {
                _inputData.ForwardVector.y = 0f;
                _inputData.MovingBack = _rotationController.TryRotate(_inputData);
            }
            
        }
    }
}
