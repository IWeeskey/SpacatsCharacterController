using System;
using UnityEngine;

namespace Spacats.CharacterController
{
    [Serializable]
    public class MovementToAnimatorData
    {
        public MainAnimationTypes MainAnimationType;
        public UpperBodyAnimationTypes UpperBodyAnimationType;
        public void Reset()
        {
            MainAnimationType = MainAnimationTypes.Idle;
            UpperBodyAnimationType = UpperBodyAnimationTypes.Idle;
        }
    }
}
