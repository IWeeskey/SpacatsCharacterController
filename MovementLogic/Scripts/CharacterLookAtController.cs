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
        public float Speed = 10f;
        [SerializeField] private Animator _animator;
        [SerializeField] private List<float> _weights = new List<float>();
        [SerializeField] private List<Transform> _bones = new List<Transform>();
        private List<Transform> _boneRefs = new List<Transform>();
        [SerializeField] private List<float> _bonesWeights = new List<float>();
        
        [SerializeField] private List<Quaternion> _bonesQuats = new List<Quaternion>();
        
        private CharacterInputRuntimeData _inputData;
        
        public void Init(CharacterInputRuntimeData inputData)
        {
            _inputData = inputData;
            
            for (int i = 0; i < _bones.Count; i++)
            {
                _bonesQuats.Add(_bones[i].rotation);
                GameObject newGO = new GameObject();
                newGO.name = _bones[i].name;
                newGO.transform.SetParent(gameObject.transform);
                _boneRefs.Add( newGO.transform);
                
                
            }
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (!DoLogic) return;
            _animator.SetLookAtPosition(_inputData.LookAtPoint);
            _animator.SetLookAtWeight(_weights[0], _weights[1], _weights[2], _weights[3], _weights[4]);

            for (int i = 0; i < _bones.Count; i++)
            {
                Transform bone = _bones[i];
                
                
                //_bonesQuats[i] = bone.rotation;
                //bone.LookAt(_inputData.LookAtPoint);
            }
        }

        private void FixedUpdate()
        {
            //_animator.Update(0);
            for (int i = 0; i < _bones.Count; i++)
            {
                Transform bone = _bones[i];
                _bonesQuats[i] = bone.rotation;
                //bone.LookAt(_inputData.LookAtPoint);
                // Transform refBone = _boneRefs[i];
                // refBone.position = bone.position;
                // refBone.LookAt(_inputData.LookAtPoint);
            }
        }

        // private void LateUpdate()
        // {
        //     for (int i = 0; i < _bones.Count; i++)
        //     {
        //         Transform bone = _bones[i];
        //         Quaternion beforeQuat = _bonesQuats[i];
        //         bone.LookAt(_inputData.LookAtPoint);
        //         Quaternion targetQuat = bone.rotation;
        //         
        //         bone.rotation = Quaternion.Lerp(beforeQuat, targetQuat, Time.deltaTime * Speed);
        //     }
        // }

        public void LateUpdate()
        {
            //_animator.Update(0);
            return;
            for (int i = 0; i < _bones.Count; i++)
            {
                Transform bone = _bones[i];
                
                /*Transform refBone = _boneRefs[i];
                refBone.position = bone.position;
                refBone.LookAt(_inputData.LookAtPoint);*/
                
                Transform refBone = _boneRefs[i];
                refBone.position = bone.position;
                refBone.LookAt(_inputData.LookAtPoint);
                
                Quaternion targetQuat = refBone.rotation;
                
                Quaternion beforeQuat = _bonesQuats[i];
                //beforeQuat = bone.rotation;
                bone.LookAt(_inputData.LookAtPoint);
                targetQuat = bone.rotation;
                
                bone.rotation = Quaternion.Lerp(beforeQuat, targetQuat, Time.deltaTime * Speed);
            }
        }
    }
}
