using System;
using Spacats.Utils;
using UnityEngine;

namespace Spacats.CharacterController.Misc
{
    public class TimeChanger : MonoBehaviour
    {
        public bool ChangeTime = false;

        private MonoTweenUnit _tUnit;
        private float _defaultFixedDeltaTime = 0f;
        public Vector2 MinMaxTime = new Vector2(0.1f,1f);

        private void Awake()
        {
            if (!ChangeTime) return;
            _defaultFixedDeltaTime = Time.fixedDeltaTime;
                
            _tUnit = new MonoTweenUnit(0, 1f, () => { }, f =>
            {
                SetNewTimeValue(Mathf.Lerp(MinMaxTime.x, MinMaxTime.y, f));
            }, () => { }, false, 999, 0);
            _tUnit.Start();
        }

        private void SetNewTimeValue(float value)
        {
            Time.timeScale = value;
            _defaultFixedDeltaTime *= value;
            //Time.uns
        }
    }
}
