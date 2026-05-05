using System;
using UnityEngine;

namespace Spacats.CharacterController
{
    [Serializable]
    public class LookAtBoneSettings
    {
        public Transform TransformLink;
        public float MaxLerpWeight;
        public Vector3 LocalOffset;
        public Vector3 GlobalOffset;
        public Quaternion RotationBefore;
        public Quaternion RotationAfter;

        [HideInInspector] public Transform HelperTransform;
    }
}
