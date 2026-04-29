using System;
using UnityEngine;

namespace Spacats.CharacterController
{
    public class AnimatorStateTracker : MonoBehaviour
    {
        public AnimationTypes MainState = AnimationTypes.Idle;
        public AnimationSubTypes SubState = AnimationSubTypes.Idle_Default;
        public Action<AnimationTypes, AnimationSubTypes> OnAnimationStateChanged;

        
        
        public void StateEntered(AnimationTypes state, AnimationSubTypes subState)
        {
            MainState = state;
            SubState = subState;
            OnAnimationStateChanged?.Invoke(state, subState);
        }
    }
}
