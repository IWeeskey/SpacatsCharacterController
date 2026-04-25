using System;
using UnityEngine;

namespace Spacats.CharacterCamera
{
    [Serializable]
    public class FollowCharacterRuntimeData
    {
        public float CurrentZoomValue;
        public float TargetZoomValue;
        public void Reset(FollowCharacterSettings settings)
        {
            CurrentZoomValue = settings.DefaultZoom;

            TargetZoomValue = CurrentZoomValue;
        }
    }
}
