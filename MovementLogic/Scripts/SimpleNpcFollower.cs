using System;
using System.Collections.Generic;
using Spacats.CharacterCamera;
using Spacats.Input;
using UnityEngine;

namespace Spacats.CharacterController
{
    public class SimpleNpcFollower : MonoBehaviour
    {
        [SerializeField] private List<SimpleNpcFollowerData> _data = new  List<SimpleNpcFollowerData>();
        [SerializeField] private Transform _transformToFollow;
        [SerializeField] private CameraFollowTarget _lookAtTarget;

        [SerializeField] private Vector2 _minMaxDistance = new Vector2(4f,5f);
        [SerializeField] private float _sprintThreshold = 10f;

       
        
        private void Awake()
        {
            foreach (SimpleNpcFollowerData entry in _data)
            {
                entry.Init(_minMaxDistance, _sprintThreshold);
            }
        }

        private void FixedUpdate()
        {
            foreach (SimpleNpcFollowerData entry in _data)
            {
                entry.Follow(_transformToFollow, _lookAtTarget.GetFollowPosition());
            }
        }

       
    }
}
