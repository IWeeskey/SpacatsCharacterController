using System;
using UnityEngine;

namespace Spacats.Input
{
    [Serializable]
    public class CharacterInputData
    {
        public MoveDirections MoveDirection;
        public MoveDirections MoveDirectionsLockBack;
        public bool Sitting;
        public bool Jumping;
        public bool Sprinting;
        public Vector2 Movement;
        public Vector2 LookDelta;
        public float ZoomDelta;
        public ButtonPhaseEnum AttackPhase;
        public bool IsAttacking =>AttackPhase== ButtonPhaseEnum.OnDown || AttackPhase== ButtonPhaseEnum.OnHoldTriggered;

        public void Reset()
        {
            AttackPhase = ButtonPhaseEnum.OnUp;
            Movement = Vector2.zero;
            LookDelta = Vector2.zero;
            ZoomDelta = 0;
            Sitting = false;
            Jumping = false;
            Sprinting =  false;
        }
        
    }
}
