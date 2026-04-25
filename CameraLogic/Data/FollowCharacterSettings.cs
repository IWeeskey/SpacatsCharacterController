using System;
using UnityEngine;

namespace Spacats.CharacterCamera
{
    [Serializable]
    public class FollowCharacterSettings
    {
        public float DefaultZoom = 5f;
        public Vector2 MinMaxZoom = new Vector2(0.1f, 20f);
        
        public float PositionFollowSpeed = 1f;
        public float ZoomSpeed = 1f;
        public float LookYDevider = 0.25f;
        public float LookZDevider = -1f;
    }
}
