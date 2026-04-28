using System;
using UnityEngine;

namespace Spacats.CharacterController
{
    [Serializable]
    public class MovementRuntimeData
    {
        public SpaceStates State;
        public float DistanceToGround;
        public Vector3 RuntimeVelocity;
        public float HorizontalSpeed;
        public Vector3 RigidBodyVelocity;
        public float RigidBodySpeed;
        public Vector3 MoveDirection;
        
        public bool WasPaused = false;
        
        public RaycastHit RHit = new RaycastHit();
        public Ray Ray = new Ray();
    }
}
