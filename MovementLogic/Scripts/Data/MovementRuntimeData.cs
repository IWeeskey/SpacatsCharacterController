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

        /// <summary>
        /// Smoothed horizontal velocity (XZ) applied through CharacterController.Move.
        /// </summary>
        public Vector3 HorizontalVelocity;

        /// <summary>
        /// Current vertical velocity (gravity / stick-to-ground), applied through CharacterController.Move.
        /// </summary>
        public float VerticalVelocity;

        public bool Grounded;
        
        public bool WasPaused = false;
        
        public RaycastHit RHit = new RaycastHit();
        public Ray Ray = new Ray();

        public bool CurrentFlying;
        public bool PreviousFlying;
        public Vector3 LocalPositionOfRotateParent;
    }
}
