using System;
using System.Collections.Generic;
using Spacats.Utils;
using UnityEngine;

namespace Spacats.CharacterController
{
    public class CharacterLookAtController : MonoBehaviour
    {
        public bool DoLogic = true;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _lookAtSpeed = 20f;
        [SerializeField] private Transform _bonesParent;
        [SerializeField] private List<Transform> _bones = new List<Transform>();
        [SerializeField] private List<float> _weights = new List<float>();
        [SerializeField] private List<Vector3> _locks = new List<Vector3>();
        
        [SerializeField] private List<Vector3> _offsets = new List<Vector3>();
        [SerializeField] private List<Vector3> _offsetsLocal = new List<Vector3>();
        
        private List<Vector3> _refs = new List<Vector3>();
        private List<Quaternion> BeforeAnimatorBodyRotations = new List<Quaternion>();
        private List<Quaternion> AfterAnimatorBodyRotations = new List<Quaternion>();
        private List<Transform> _ones = new List<Transform>();
        
        private CharacterInputRuntimeData _inputData;
        
        public void Init(CharacterInputRuntimeData inputData)
        {
            _inputData = inputData;
            
            foreach (Transform tr in _bones)
            {
                // GameObject tmp = new GameObject();
                // tmp.transform.parent = tr;
                // tmp.transform.localPosition = Vector3.zero;
                // tmp.name = "Rotate_" + tr.name;
                //
                // tmp.transform.parent = gameObject.transform;
                //
                // RotateLookAtChestBones.Add(tmp.transform);
                _refs.Add(tr.localEulerAngles);
                BeforeAnimatorBodyRotations.Add(new Quaternion());
                AfterAnimatorBodyRotations.Add(new Quaternion());
            }
           
            //_animator.Update();
        }


        private void OnAnimatorIK(int layerIndex)
        {
            //Debug.Log("OnAnimatorIK: " + layerIndex);
            _animator.SetLookAtPosition(_inputData.LookAtPoint);
            _animator.SetLookAtWeight(_weights[0], _weights[1], _weights[2], _weights[3], _weights[4]);
        }


        /// <summary>
        /// BeforeAnimator
        /// </summary>
        public void ProcessFixedUpdate()
        {
            if (!DoLogic) return;
            if (PauseController.IsPaused) return;
            //return;
            
            for (int i = 0; i <= _bones.Count - 1; i++)
            {
                // Debug.Log(_bones[i].rotation);
                // BeforeAnimatorBodyRotations[i] = _bones[i].rotation;
                Quaternion rotationBefore = _bones[i].rotation;
                _bones[i].LookAt(_inputData.LookAtPoint);
                _bones[i].rotation = Quaternion.Lerp(rotationBefore, _bones[i].rotation, Time.deltaTime * _lookAtSpeed);
            }
        }
        
        /// <summary>
        /// BeforeAnimator
        /// </summary>
        public void ProcessUpdate()
        {
            if (!DoLogic) return;
            if (PauseController.IsPaused) return;
            
            for (int i = 0; i <= _bones.Count - 1; i++)
            {
                //Debug.Log(_bones[i].rotation);
                //BeforeAnimatorBodyRotations[i] = _bones[i].rotation;
                
                // Quaternion rotationBefore = _bones[i].rotation;
                // _bones[i].LookAt(_inputData.LookAtPoint);
                // _bones[i].rotation = Quaternion.Lerp(rotationBefore, _bones[i].rotation, Time.deltaTime * _lookAtSpeed);
            }
        }
        Vector3 tmpRef;
        public void ProcessLateUpdate()
        {
            if (!DoLogic) return;
            if (PauseController.IsPaused) return;
            
         
            return;
            //ApplyChestTargetRotations();
            for (int i = 0; i <= _bones.Count - 1; i++)
            {
                // AfterAnimatorBodyRotations[i] = _bones[i].rotation;
                // AfterAnimatorBodyRotations[i] = Quaternion.Lerp(AfterAnimatorBodyRotations[i], RotateLookAtChestBones[i].rotation, _weights[i]);
                // _bones[i].rotation = Quaternion.Lerp(BeforeAnimatorBodyRotations[i], AfterAnimatorBodyRotations[i], Time.deltaTime * _lookAtSpeed);
                Vector3 localEulersBefore = _bones[i].localEulerAngles;
                Quaternion rotationBefore = _bones[i].rotation;
                //Debug.Log(rotationBefore);
                //rotationBefore = BeforeAnimatorBodyRotations[i];
                _bones[i].LookAt(_inputData.LookAtPoint);
               //  Vector3 newLocalEulers = _bones[i].localEulerAngles;
               //  //Vector3 tmp = newLocalEulers;
               //  newLocalEulers = Vector3.SmoothDamp(localEulersBefore, newLocalEulers, ref tmpRef, Time.deltaTime * _lookAtSpeed);
               // Vector3 locks = _locks[i];
               // if (locks.x > 0) newLocalEulers.x = localEulersBefore.x;
               // if (locks.y > 0) newLocalEulers.y = localEulersBefore.y;
               // if (locks.z > 0) newLocalEulers.z = localEulersBefore.z;
               // _bones[i].localEulerAngles = newLocalEulers;
               
               
               // Quaternion newRotation = _bones[i].rotation;
               // _bones[i].rotation = rotationBefore;
               // _bones[i].rotation = rotationBefore;
               // //Quaternion.sm
               _bones[i].rotation = Quaternion.Lerp(rotationBefore, _bones[i].rotation, Time.deltaTime * _lookAtSpeed);
            }
        }
        
        void ApplyChestTargetRotations()
        {
            Vector3 tmpVector = Vector3.zero;
            for (int i = 0; i <= _bones.Count - 1; i++)
            {
                // RotateLookAtChestBones[i].position = _bones[i].position;
                // RotateLookAtChestBones[i].LookAt(_inputData.LookAtPoint);
                // RotateLookAtChestBones[i].eulerAngles = RotateLookAtChestBones[i].eulerAngles + _offsets[i];
                //
                // RotateLookAtChestBones[i].localEulerAngles = RotateLookAtChestBones[i].localEulerAngles + _offsetsLocal[i];
                // tmpVector = RotateLookAtChestBones[i].localEulerAngles;
                // tmpVector.x = RotateLookAtChestBones[i].localEulerAngles.z;
                // tmpVector.z = RotateLookAtChestBones[i].localEulerAngles.x * -1f;
                //
                // RotateLookAtChestBones[i].localEulerAngles = tmpVector;
            }
        }
    }
}
