using System;
using UnityEngine;

namespace Spacats.CharacterCamera
{
    [Serializable]
    public class LogicPauseSettings
    {
        public float MoveSpeed = 10f;
        public float SprintMultiplier = 4f;
        public float SmoothReturnSpeed = 5f;
        public float ReturnSpeed = 5f;
    }
}