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
        public float MoveSpeed = 1f;
        public Rigidbody Rigidbody;
    }
}
