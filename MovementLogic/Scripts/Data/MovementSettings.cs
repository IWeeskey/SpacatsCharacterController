using System;
using UnityEngine;

namespace Spacats.CharacterController
{
    [Serializable]
    public class MovementSettings
    {
        public LayerMask GroundLayers;
        public LayerMask WaterLayers;
        public float OnGroundThreshold = 0.5f;
        public float Gravity = -9.8f;
        
        public float SmoothSpeedChange = 1f;
        public Rigidbody Rigidbody;
        
        public float ForwardSpeed = 1f;
        public float BackwardSpeed = 1f;
        
        public float SitSpeed = 1f;
        public float SwimSpeed = 1f;
        
        public float SprintMultiplier = 1f;
        public float WalkMultiplier = 0.5f;
    }
}
