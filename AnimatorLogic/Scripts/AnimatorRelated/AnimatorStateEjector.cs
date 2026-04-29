using UnityEngine;

namespace Spacats.CharacterController
{
    public class AnimatorStateEjector : StateMachineBehaviour
    {
        private AnimatorStateTracker _tracker;
        public AnimationTypes MainState = AnimationTypes.Idle;
        public AnimationSubTypes SubState = AnimationSubTypes.Idle_Default;


        private void CheckTracker(Animator animator)
        {
            if (_tracker is null) _tracker = animator.gameObject.GetComponent<AnimatorStateTracker>();
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            CheckTracker(animator);
            _tracker.StateEntered(MainState, SubState);
        }
    }
}
