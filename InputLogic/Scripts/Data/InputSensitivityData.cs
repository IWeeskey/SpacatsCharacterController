using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Spacats.Input
{
    [Serializable]
    public struct InputSensitivityData
    {
#pragma warning disable CS0162
        
        public float CharacterLookSensitivityX_PC;
        public float CharacterLookSensitivityY_PC;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float CharacterLookSensitivityX()
        {
#if UNITY_STANDALONE
            return CharacterLookSensitivityX_PC;
#endif
            return 1f;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float CharacterLookSensitivityY()
        {
#if UNITY_STANDALONE
            return CharacterLookSensitivityY_PC;
#endif
            return 1f;
        }
        
#pragma warning restore CS0162
    }
}
