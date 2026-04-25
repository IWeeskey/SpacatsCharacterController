using System;
using UnityEngine;

namespace Spacats.CharacterController
{
    [Serializable]
    public class PlayerCharacterInputRuntimeData
    {
        public Vector3 MoveDirection;
        public Vector3 TargetEulers;
        
        public void Reset()
        {
            TargetEulers = Vector3.zero;
            MoveDirection =  Vector3.zero;
        }
    }
}
