using System;
using UnityEngine;

namespace Spacats.CharacterCamera
{
    [Serializable]
    public class FollowCharacterRuntimeData
    {
        public float CurrentZoomValue;
        public float TargetZoomValue;
        public float BackBlockedZoomValue;
        public float BeforePauseZoomValue;

        public Quaternion CurrentRotation;
        public Vector3 TargetEulers;
        
        public Vector3 MoveDirection;
        public Vector3 MoveDirectionLockBack;
        
        public void Reset(FollowCharacterSettings settings)
        {
            CurrentZoomValue = settings.DefaultZoom;
            BeforePauseZoomValue = CurrentZoomValue;
            TargetZoomValue = CurrentZoomValue;
            BackBlockedZoomValue = 9999f;
            TargetEulers = Vector3.zero;
        }
    }
}
