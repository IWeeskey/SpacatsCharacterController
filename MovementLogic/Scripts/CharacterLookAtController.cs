using System;
using System.Collections.Generic;
using Spacats.Utils;
using UnityEngine;
using UnityEngine.XR;

namespace Spacats.CharacterController
{
    public class CharacterLookAtController : MonoBehaviour
    {
        public bool DoLogic = true;
        public float LookAtSpeed = 1f;
        //public Transform LookAtVisualizer;
        [SerializeField] private List<LookAtBoneSettings> _bonesData = new List<LookAtBoneSettings>();
        
        private CharacterInputRuntimeData _inputData;

        private float _dynamicWeight = 1f;
        private bool _prevInLogicPause = false;
        private bool _currentInLogicPause = false;
        private Vector3 _currentTargetPoint;
        private Vector3 _tmpRoHead = new Vector3();
        
        public void Init(CharacterInputRuntimeData inputData)
        {
            _inputData = inputData;
            
            foreach (LookAtBoneSettings s in _bonesData)
            {
                GameObject tmp = new GameObject();
                tmp.transform.parent = s.TransformLink;
                tmp.transform.localPosition = Vector3.zero;
                tmp.name = "Rotate_" + s.TransformLink.name;

                tmp.transform.parent = gameObject.transform;
                s.HelperTransform = tmp.transform;
                s.RotationBefore = s.TransformLink.rotation;
                s.RotationAfter = s.TransformLink.rotation;
            }
        }

        private bool IsFlying()
        {
            return _inputData.Flying;
        }

        // public void ProcessFixedUpdate()
        // {
        //     return;
        //     if (!DoLogic) return;
        //     LookBodyDirection_BeforeAnimator();
        // }
        
        public void ProcessUpdate()
        {
            if (!DoLogic) return;
            LookBodyDirection_BeforeAnimator();
        }

        public void ProcessLateUpdate()
        {
            if (!DoLogic) return;
            if (!PauseController.IsPaused) FixLookAtPoint();
            LookBodyDirection_AfterAnimator();
        }

        private void FixLookAtPoint()
        {
            _dynamicWeight = 1f;
            Vector3 startGlobalPoint = _inputData.LookAtPoint;
            Vector3 startLocalPoint = gameObject.transform.InverseTransformPoint(startGlobalPoint);

            if (startLocalPoint.z < -3f)
            {
                _dynamicWeight = 0f;
            }
            if (startLocalPoint.z<0) startLocalPoint.z = 0;
            Vector3 worldPoint = gameObject.transform.TransformPoint(startLocalPoint);
            _currentTargetPoint = worldPoint;
            
        }


        void ApplyChestTargetRotations()
        {
            for (int i = 0; i <= _bonesData.Count - 1; i++)
            {
                LookAtBoneSettings boneInfo = _bonesData[i];
                boneInfo.HelperTransform.position = boneInfo.TransformLink.position;
                boneInfo.HelperTransform.LookAt(_currentTargetPoint);
                boneInfo.HelperTransform.eulerAngles = boneInfo.HelperTransform.eulerAngles + boneInfo.GlobalOffset;

                boneInfo.HelperTransform.localEulerAngles = boneInfo.HelperTransform.localEulerAngles + boneInfo.LocalOffset;
                _tmpRoHead = boneInfo.HelperTransform.localEulerAngles;
                // _tmpRoHead.x = boneInfo.HelperTransform.localEulerAngles.z;
                // _tmpRoHead.z = boneInfo.HelperTransform.localEulerAngles.x * -1f;

                boneInfo.HelperTransform.localEulerAngles = _tmpRoHead;
            }
        }
        
        public void LookBodyDirection_BeforeAnimator()
        {
            for (int i = 0; i <= _bonesData.Count - 1; i++)
            {
                LookAtBoneSettings boneInfo = _bonesData[i];
                boneInfo.RotationBefore = boneInfo.TransformLink.rotation;
            }
        }

        // private void Update()
        // {
        //     LookBodyDirection_BeforeAnimator();
        // }

        public void LookBodyDirection_AfterAnimator()
        {
            //if (IsFlying()) return;
            ApplyChestTargetRotations();
            for (int i = 0; i <= _bonesData.Count - 1; i++)
            {
                LookAtBoneSettings boneInfo = _bonesData[i];
                boneInfo.RotationAfter = boneInfo.TransformLink.rotation;
                boneInfo.RotationAfter = Quaternion.Lerp(boneInfo.RotationAfter, boneInfo.HelperTransform.rotation, boneInfo.MaxLerpWeight*_dynamicWeight);
                boneInfo.TransformLink.rotation = Quaternion.Lerp(boneInfo.RotationBefore, boneInfo.RotationAfter, Time.deltaTime * LookAtSpeed);
            }

        }

    }
}
