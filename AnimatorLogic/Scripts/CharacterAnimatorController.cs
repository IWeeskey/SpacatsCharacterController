using System;
using UnityEngine;

namespace Spacats.CharacterController
{
    public class CharacterAnimatorController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimatorStateTracker _stateTracker;
        private AnimatorToMovementData _atomData;
        private MovementToAnimatorData _mtoaData;
        private CharacterInputRuntimeData _inputData;

        private bool _subscribed = false;
        
        public void Init(CharacterInputRuntimeData inputData, AnimatorToMovementData atomData, MovementToAnimatorData mtoaData)
        {
            _inputData = inputData;
            _atomData = atomData;
            _mtoaData = mtoaData;
            _stateTracker.OnAnimationStateChanged+=OnStateEntered;
            _subscribed = true;
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
            _animator.SetFloat("MoveSpeed", 1f);
            _animator.SetInteger("MainAnimationType", (int)_mtoaData.MainAnimationType);
            //_animator.SetBool("Backward", _inputData.IsMovingBack());
        }
    }
}
