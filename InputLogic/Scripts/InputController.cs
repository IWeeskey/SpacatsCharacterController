using Spacats.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;

namespace Spacats.Input
{
    public class InputController : Controller
    {
        private static InputController _instance;
        public static InputController Instance{get{return _instance;}}
        public static bool HasInstance => _instance != null;
        
        // private string _lastInputActions = "";
        // public string LastInputActions => _lastInputActions;
        
        
        [SerializeField] private CharacterInputData _characterInput = new CharacterInputData();
        public CharacterInputData CharacterInput => _characterInput;
        
        [SerializeField] private LogicPauseInputData _logicPauseInput = new LogicPauseInputData();
        public LogicPauseInputData LogicPauseInput => _logicPauseInput;
        private InputSensitivityData _sensitivityData;
        [SerializeField] private InputSettingsConfig _config;
        
        
        #region ControllerBasics
        protected override void COnRegister()
        {
            base.COnRegister();
            _instance = this;
        }

        public override void COnSceneUnloading(Scene scene)
        {
            base.COnSceneUnloading(scene);
        }

        public override void COnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            base.COnSceneLoaded(scene, mode);
        }

        protected override void COnDisable()
        {
            base.COnDisable();
        }

        public void RefreshSettings()
        {
            if (_config == null)
            {
                Debug.LogError("InputSettingsConfig is not set!");
                return;
            }
            _sensitivityData = _config.SensitivityData;
        }

        protected override void CCreate()
        {
            base.CCreate();
            _characterInput.Reset();
            _logicPauseInput.Reset();

            RefreshSettings();
        }

        protected override void CDispose()
        {
            base.CDispose();
            _characterInput.Reset();
            _logicPauseInput.Reset();
        }
        #endregion

        #region CharacterInput
        public void OnMove(InputAction.CallbackContext context)
        {
            _characterInput.Movement = context.ReadValue<Vector2>();
        }
        
        public void OnAttack(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    _characterInput.AttackPhase = ButtonPhaseEnum.OnDown;
                    break;
                case InputActionPhase.Performed: 
                    //_characterInput.AttackPhase = ButtonPhaseEnum.OnHoldTriggered;
                    break;
                case InputActionPhase.Canceled:
                    _characterInput.AttackPhase = ButtonPhaseEnum.OnUp;
                    break;
            }
        }
        
        public void OnLookDelta(InputAction.CallbackContext context)
        {
            Vector2 value = context.ReadValue<Vector2>();
            
            value.x *= _sensitivityData.CharacterLookSensitivityX();
            value.y *= _sensitivityData.CharacterLookSensitivityY();
            
            _characterInput.LookDelta = value;
        }

        #endregion
    }
}
