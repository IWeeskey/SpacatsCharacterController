using System;
using UnityEngine;

namespace Spacats.Input
{
    [Serializable]
    public class CharacterInputData
    {
        public MoveTypes MoveType;
        public MoveDirections MoveDirection;
        public MoveDirections MoveDirectionsLockBack;
        public bool Jumping;
        public bool Sprinting;
        public bool Walking;
        public bool Crouching;
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
            Jumping = false;
            Sprinting = false;
            Walking = false;
            Crouching = false;
        }
        
    }
}
