using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Spacats.CharacterController
{
    public class AnimatorEventsCatcher : MonoBehaviour
    {
        public Action OnFootStep;
        public Action OnAttack;
        public Action OnInteraction;

        public void OnAnimationEventStep()
        {
            OnFootStep?.Invoke();
        }

        public void OnAnimationManFall()
        {

        }

        public void OnAnimationAttack()
        {
            OnAttack?.Invoke();
        }

        public void OnAnimationInteraction()
        {
            OnInteraction?.Invoke();
        }
    }
}
