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
        public float Gravity = -9.8f;
        
        public float SmoothSpeedChange = 1f;
        public float SmoothSpeedChangeFlying = 5f;
        public UnityEngine.CharacterController Controller;

        [Header("Grounded check")]
        [Tooltip("Vertical offset (downwards) of the grounded sphere check origin from the controller transform position.")]
        public float GroundedOffset = 0.15f;
        [Tooltip("Radius of the grounded sphere check. Should roughly match the controller radius.")]
        public float GroundedRadius = 0.4f;
        [Tooltip("Maximum fall speed (absolute value).")]
        public float TerminalVelocity = 53f;

        [Tooltip("X - forward; Y - Backward")]public Vector2 WalkSpeed = new Vector2(2,1);
        [Tooltip("X - forward; Y - Backward")]public Vector2 RunSpeed = new Vector2(6,3);
        [Tooltip("X - forward; Y - Backward")]public Vector2 SprintSpeed = new Vector2(12,6);
        [Tooltip("X - forward; Y - Backward")]public Vector2 CrouchSpeed = new Vector2(2,1);
        [Tooltip("X - forward; Y - Backward")]public Vector2 SwimSpeed = new Vector2(6,3);
        
        public float FlySpeed = 40;
        public float FlyOffsetY = 1f;
        public bool ApplyFlyOffset = false;
        public float FlyOffsetSpeed = 10f;
    }
}
