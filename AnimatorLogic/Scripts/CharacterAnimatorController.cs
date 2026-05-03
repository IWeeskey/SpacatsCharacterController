using System;
using Spacats.Utils;
using UnityEngine;

namespace Spacats.CharacterController
{
    public class CharacterAnimatorController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimatorStateTracker _stateTracker;
        
        [SerializeField] private float _speedChangeSpeed = 10f;
        [SerializeField] private float _locomotionChangeSpeed = 10f;
        private AnimatorToMovementData _atomData;
        private MovementToAnimatorData _mtoaData;
        private CharacterInputRuntimeData _inputData;

        private bool _subscribed = false;
        
        private float _currentTypeSpeed;
        private float _targetTypeSpeed;
        
        private float _currentLocoType;
        private float _targetLocoType;
        
        
        public void Init(CharacterInputRuntimeData inputData, AnimatorToMovementData atomData, MovementToAnimatorData mtoaData)
        {
            _inputData = inputData;
            _atomData = atomData;
            _mtoaData = mtoaData;
            _stateTracker.OnAnimationStateChanged+=OnStateEntered;
            _subscribed = true;
            _currentTypeSpeed = 0f;
            _currentLocoType = 0f;
        }

        private void OnDestroy()
        {
            if (!_subscribed) return;
            _stateTracker.OnAnimationStateChanged-=OnStateEntered;
            _subscribed = false;
        }

        public void OnStateEntered(AnimationTypes state, AnimationSubTypes subState)
        {
        }

        public void SyncData()
        {
            if (PauseController.IsPaused) return;
            _animator.SetFloat("MoveSpeed", 1f);

            if (_mtoaData.MainAnimationType == MainAnimationTypes.CrouchBackward ||
                _mtoaData.MainAnimationType == MainAnimationTypes.CrouchForward
                ||  _mtoaData.MainAnimationType == MainAnimationTypes.CrouchIdle)
            {
                ApplyIsCrouching();
            }
            else
            {
                ApplyBasicGrounded();
            }

            _currentLocoType = Mathf.Lerp(_currentLocoType,  _targetLocoType, Time.deltaTime * _locomotionChangeSpeed);
            _currentTypeSpeed = Mathf.Lerp(_currentTypeSpeed,  _targetTypeSpeed, Time.deltaTime * _speedChangeSpeed);
            _animator.SetFloat("SpeedType", _currentTypeSpeed);
            _animator.SetFloat("LocoType", _currentLocoType);

            ApplyAttack();
        }

        private void ApplyIsCrouching()
        {
            _targetLocoType = 1f;
            _targetTypeSpeed = 0f;
            if (_mtoaData.MainAnimationType == MainAnimationTypes.CrouchBackward) _targetTypeSpeed = -3f;
            else if (_mtoaData.MainAnimationType == MainAnimationTypes.CrouchForward) _targetTypeSpeed = 3f;
        }

        private void ApplyBasicGrounded()
        {
            _targetLocoType = 0f;
            _targetTypeSpeed = 0f;
            if (_mtoaData.MainAnimationType == MainAnimationTypes.SprintBackward) _targetTypeSpeed = -3f;
            else if (_mtoaData.MainAnimationType == MainAnimationTypes.RunBackward) _targetTypeSpeed = -2f;
            else if (_mtoaData.MainAnimationType == MainAnimationTypes.WalkBackward) _targetTypeSpeed = -1f;
            else if (_mtoaData.MainAnimationType == MainAnimationTypes.Idle) _targetTypeSpeed = 0f;
            else if (_mtoaData.MainAnimationType == MainAnimationTypes.WalkForward) _targetTypeSpeed = 1f;
            else if (_mtoaData.MainAnimationType == MainAnimationTypes.RunForward) _targetTypeSpeed = 2f;
            else if (_mtoaData.MainAnimationType == MainAnimationTypes.SprintForward) _targetTypeSpeed = 3f;
        }

        private void ApplyAttack()
        {
            _animator.SetBool("Attack", _inputData.Attacking);
        }
    }
}
