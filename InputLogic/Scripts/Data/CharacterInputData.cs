using System;
using UnityEngine;

namespace Spacats.Input
{
    [Serializable]
    public class CharacterInputData
    {
        public Vector2 Movement;
        public Vector2 LookDelta;
        public ButtonPhaseEnum AttackPhase;
        public bool IsAttacking =>AttackPhase== ButtonPhaseEnum.OnDown || AttackPhase== ButtonPhaseEnum.OnHoldTriggered;

        public void Reset()
        {
            AttackPhase = ButtonPhaseEnum.OnUp;
            Movement = Vector2.zero;
            LookDelta = Vector2.zero;
        }
        
    }
}
