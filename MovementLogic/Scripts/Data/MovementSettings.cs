using System;
using UnityEngine;

namespace Spacats.CharacterController
{
    [Serializable]
    public class MovementSettings
    {
        public Transform RotateParent;
        public LayerMask GroundLayers;
        public LayerMask WaterLayers;
        public float OnGroundThreshold = 0.5f;
        public float Gravity = -9.8f;
        
        public float SmoothSpeedChange = 1f;
        public Rigidbody Rigidbody;

        [Tooltip("X - forward; Y - Backward")]public Vector2 WalkSpeed = new Vector2(2,1);
        [Tooltip("X - forward; Y - Backward")]public Vector2 RunSpeed = new Vector2(6,3);
        [Tooltip("X - forward; Y - Backward")]public Vector2 SprintSpeed = new Vector2(12,6);
        [Tooltip("X - forward; Y - Backward")]public Vector2 CrouchSpeed = new Vector2(2,1);
        [Tooltip("X - forward; Y - Backward")]public Vector2 SwimSpeed = new Vector2(6,3);
        
        public float FlySpeed = 40;
        public float FlyOffsetY = 1f;
        public float FlyOffsetSpeed = 10f;
    }
}
