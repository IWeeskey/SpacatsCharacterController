using System;
using UnityEngine;

namespace Spacats.CharacterCamera
{
    [Serializable]
    public class FollowCharacterSettings
    {
        public float DefaultZoom = 5f;
        public Vector2 MinMaxZoom = new Vector2(0.1f, 20f);
        public Vector2 MinMaxRot = new Vector2(-20, 80f);
        
        public Vector3 FixedLookAtOffset = Vector3.zero;
        
        public float PositionFollowSpeed = 1f;
        public float RotationFollowSpeed = 1f;
        public float ZoomSpeed = 1f;
        
        
        public float LookYModifier = 0.25f;
        public float LookZModifier = -1f;
        
        public float RotationYModifier = 1f;
        public float RotationXModifier = 1f;
    }
}
