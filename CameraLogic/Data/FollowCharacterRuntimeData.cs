using System;
using UnityEngine;

namespace Spacats.CharacterCamera
{
    [Serializable]
    public class FollowCharacterRuntimeData
    {
        public float CurrentZoomValue;
        public float TargetZoomValue;
        

        public Quaternion CurrentRotation;
        public Vector3 TargetEulers;
        
        public Vector3 MoveDirection;
        public Vector3 MoveDirectionLockBack;
        
        public void Reset(FollowCharacterSettings settings)
        {
            CurrentZoomValue = settings.DefaultZoom;

            TargetZoomValue = CurrentZoomValue;
            
            TargetEulers = Vector3.zero;
        }
    }
}
