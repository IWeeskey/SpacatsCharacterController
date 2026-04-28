using UnityEngine;

namespace Spacats.CharacterController
{
    public class CharacterSummaryController : MonoBehaviour
    {
        public bool IsPlayer = false;
        [SerializeField] private CharacterMovementController _movementController;
        [SerializeField] private CharacterRotationController _rotationController;
        [SerializeField] private CharacterInputRuntimeData _inputData;

        public Vector3 GetSelfForwardVector()
        {
            return _rotationController.GetForwardVector();
        }

        public void SetInputData(CharacterInputRuntimeData  inputData)
        {
            _inputData = inputData;
        }

        void FixedUpdate()
        {
            _movementController.TryMove(_inputData.MoveDirectionVector);
        }

        void Update()
        {
            _inputData.ForwardVector.y = 0f;
            _rotationController.TryRotate(_inputData);
        }
    }
}
