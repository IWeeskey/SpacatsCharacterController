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
        [SerializeField] private List<float> _weights = new List<float>();
        
        private CharacterInputRuntimeData _inputData;
        
        public void Init(CharacterInputRuntimeData inputData)
        {
            _inputData = inputData;
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (!DoLogic) return;
            _animator.SetLookAtPosition(_inputData.LookAtPoint);
            _animator.SetLookAtWeight(_weights[0], _weights[1], _weights[2], _weights[3], _weights[4]);
        }
    }
}
