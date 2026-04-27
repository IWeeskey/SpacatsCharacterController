using System;
using UnityEngine;

namespace Spacats.CharacterController
{
    [Serializable]
    public class PlayerCharacterInputRuntimeData
    {
        public Vector3 MoveDirection;
        public Vector3 MoveDirectionInverted;
        public Vector3 ForwardVector;
        public Vector3 TargetEulers;
        public bool MovingBack;
        
        public void Reset()
        {
            TargetEulers = Vector3.zero;
            MoveDirection =  Vector3.zero;
            ForwardVector = Vector3.zero;
            MovingBack = false;
        }
    }
}
