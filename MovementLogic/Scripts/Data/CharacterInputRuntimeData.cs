using System;
using Spacats.Input;
using UnityEngine;

namespace Spacats.CharacterController
{
    [Serializable]
    public class CharacterInputRuntimeData
    {
        /// <summary>
        /// Direction in which character moves in global 
        /// </summary>
        public Vector3 MoveDirectionVector;
        /// <summary>
        /// Where character looks
        /// </summary>
        public Vector3 ForwardVector;
        
        public bool Sitting;
        public bool Jumping;
        public bool Sprinting;
        
        public MoveDirections MoveDirection;
        public MoveDirections MoveDirectionsLockBack;
        
        public void Reset()
        {
            MoveDirectionVector =  Vector3.zero;
            ForwardVector = Vector3.zero;
            MoveDirection = MoveDirections.Idle;
            MoveDirectionsLockBack = MoveDirections.Idle;

            Sitting = false;
            Jumping = false;
            Sprinting = false;
        }

        public bool IsMovingBack()
        {
            return MoveDirection== MoveDirections.Backward || MoveDirection == MoveDirections.BackwardLeft || MoveDirection == MoveDirections.BackwardRight;
        }
    }
}
