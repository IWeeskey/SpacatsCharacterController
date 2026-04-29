using Spacats.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;

namespace Spacats.Input
{
    [ExecuteInEditMode]
    [DefaultExecutionOrder(-10)]
    public class InputController : Controller
    {
        private static InputController _instance;
        public static InputController Instance{get{return _instance;}}
        public static bool HasInstance => _instance != null;
        
        // private string _lastInputActions = "";
        // public string LastInputActions => _lastInputActions;
        
        
        [SerializeField] private CharacterInputData _characterInput;
        public CharacterInputData CharacterInput => _characterInput;
        
        [SerializeField] private LogicPauseInputData _logicPauseInput;
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
            _characterInput.Movement.Normalize();
            MoveDirections closestDirection = MoveDirections.Idle;
            float closestDistance = float.MaxValue;
            for (int i = 0; i < MoveDirectionsHelper.DirectionVectors.Length; i++)
            {
                float dist = (_characterInput.Movement - MoveDirectionsHelper.DirectionVectors[i]).magnitude;
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closestDirection = (MoveDirections)i;
                }
            }

            _characterInput.MoveDirection = closestDirection;
            _characterInput.MoveDirectionsLockBack = closestDirection;
            if (_characterInput.MoveDirection == MoveDirections.BackwardLeft)
                _characterInput.MoveDirectionsLockBack = MoveDirections.ForwardRight;
            if (_characterInput.MoveDirection == MoveDirections.BackwardRight)
                _characterInput.MoveDirectionsLockBack = MoveDirections.ForwardLeft;

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
        
        public void OnSit(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:_characterInput.Crouching = true; break;
                case InputActionPhase.Canceled:_characterInput.Crouching = false; break;
            }
            RefreshMoveType();
        }
        
        public void OnWalk(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:_characterInput.Walking = true; break;
                case InputActionPhase.Canceled:_characterInput.Walking = false; break;
            }
            RefreshMoveType();
        }
        
        public void OnJump(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:_characterInput.Jumping = true; break;
                case InputActionPhase.Canceled:_characterInput.Jumping = false; break;
            }
            
        }
        
        public void OnSprint(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:_characterInput.Sprinting = true; break;
                case InputActionPhase.Canceled:_characterInput.Sprinting = false; break;
            }

            RefreshMoveType();
        }
        
        public void OnLookDelta(InputAction.CallbackContext context)
        {
            Vector2 value = context.ReadValue<Vector2>();
            
            value.x *= _sensitivityData.CharacterLookSensitivityX();
            value.y *= _sensitivityData.CharacterLookSensitivityY();
            
            _characterInput.LookDelta = value;
        }
        
        public void OnZoomDelta(InputAction.CallbackContext context)
        {
            Vector2 value = context.ReadValue<Vector2>();
            value.y *= _sensitivityData.CharacterZoomSensitivity();
            _characterInput.ZoomDelta = value.y;
        }

        private void RefreshMoveType()
        {
            _characterInput.MoveType = MoveInputTypes.Idle;
            if (_characterInput.MoveDirection!= MoveDirections.Idle)  _characterInput.MoveType = MoveInputTypes.Run;

            if (_characterInput.Sprinting)
            {
                _characterInput.MoveType = MoveInputTypes.Sprint;
                return;
            }
            
            if (_characterInput.Crouching)
            {
                _characterInput.MoveType = MoveInputTypes.Crouch;
                return;
            }
            
            if (_characterInput.Walking)
            {
                _characterInput.MoveType = MoveInputTypes.Walk;
                return;
            }
            
        }

        #endregion
    }
}
