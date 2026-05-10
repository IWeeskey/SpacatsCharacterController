using System;
using Spacats.Input;
using UnityEngine;

namespace Spacats.CharacterController
{
    [Serializable]
    public class SimpleNpcFollowerData
    {
        [SerializeField] private CharacterInputRuntimeData _inputData;
        [SerializeField] private CharacterSummaryController _thisCharacterSummary;
        private Vector3 _targetPosition;
        private Vector3 _selfPosition;
        private Vector3 _directionToTarget;
        private float _distanceToTarget;
        
        private Vector2 _minMaxDistance = new Vector2(4f,5f);
        private float _sprintThreshold = 10f;

        public void Init(Vector2 minMaxDistance, float sprintThreshold)
        {
            _minMaxDistance =  minMaxDistance;
            _sprintThreshold = sprintThreshold;
            _inputData.Reset();
            _thisCharacterSummary.SetInputData(_inputData);
        }
        
        public void Follow(Transform target, Vector3 lookAtPosition)
        {
            _targetPosition = target.position;
            _selfPosition = _thisCharacterSummary.transform.position;
            _directionToTarget =  (_targetPosition - _selfPosition).normalized;
            _directionToTarget.y = 0;
            _distanceToTarget = Vector3.Distance(_targetPosition, _selfPosition);

            _inputData.LookAtPoint =  lookAtPosition;
            
            if (_distanceToTarget >= _minMaxDistance.x && _distanceToTarget <= _minMaxDistance.y)
            {
                //good, stay at place
                HandleStayAtPlace();
                return;
            }

            if (_distanceToTarget < _minMaxDistance.x)
            {
                MoveBackFromTarget();
                return;
            }
            
            if (_distanceToTarget > _minMaxDistance.y)
            {
                MoveToTarget(_distanceToTarget>=_sprintThreshold);
                return;
            }
        }

        
        private void HandleStayAtPlace()
        {
            _inputData.MoveType = MoveInputTypes.Idle;
            _inputData.MoveDirection = MoveDirections.Idle;
            _inputData.MoveDirectionsLockBack = MoveDirections.Idle;
            _inputData.ForwardVector = _thisCharacterSummary.GetSelfForwardVector();
            _inputData.MoveDirectionVector = Vector3.zero;
        }
        
        private void MoveBackFromTarget()
        {
            _inputData.MoveType = MoveInputTypes.Walk;
            _inputData.MoveDirection = MoveDirections.Backward;
            _inputData.MoveDirectionsLockBack = MoveDirections.Forward;
            _inputData.ForwardVector = _thisCharacterSummary.GetSelfForwardVector();
            _inputData.MoveDirectionVector = _directionToTarget*-1f;
        }
        
        private void MoveToTarget(bool sprint = false)
        {
            if (sprint) _inputData.MoveType = MoveInputTypes.Sprint;
            else _inputData.MoveType = MoveInputTypes.Run;
            
            _inputData.MoveDirection = MoveDirections.Forward;
            _inputData.MoveDirectionsLockBack = MoveDirections.Forward;
            _inputData.ForwardVector = _thisCharacterSummary.GetSelfForwardVector();
            _inputData.MoveDirectionVector = _directionToTarget;
        }
    }
}
