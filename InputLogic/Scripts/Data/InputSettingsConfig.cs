using UnityEngine;
using System;
using System.Collections.Generic;

namespace Spacats.Input
{
    [CreateAssetMenu(menuName = "Spacats/InputSettings Config", fileName = "InputSettingsConfig")]
    public class InputSettingsConfig : ScriptableObject
    {
        [SerializeField] private InputSensitivityData _sensitivityData;
        public InputSensitivityData  SensitivityData => _sensitivityData;
    }
}