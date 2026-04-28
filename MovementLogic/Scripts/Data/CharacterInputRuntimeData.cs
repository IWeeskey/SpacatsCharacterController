using System;
using Spacats.Input;
using UnityEngine;

namespace Spacats.CharacterController
{
    [Serializable]
    public class CharacterInputRuntimeData
    {
        public Vector3 MoveDirectionV;
        public Vector3 MoveDirectionInvertedV;
        public Vector3 ForwardVector;
        public bool MovingBack;
        
        public MoveDirections MoveDirection;
        public MoveDirections MoveDirectionsLockBack;
        
        public void Reset()
        {
            MoveDirectionV =  Vector3.zero;
            MoveDirectionInvertedV =  Vector3.zero;
            ForwardVector = Vector3.zero;
            MovingBack = false;
            MoveDirection = MoveDirections.Idle;
            MoveDirectionsLockBack = MoveDirections.Idle;
        }
    }
}
