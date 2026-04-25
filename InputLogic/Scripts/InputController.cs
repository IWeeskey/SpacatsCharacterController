using Spacats.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Spacats.Input
{
    public class InputController : Controller
    {
        private static InputController _instance;
        public static InputController Instance{get{return _instance;}}
        public static bool HasInstance => _instance != null;
        
        private string _lastInputActions = "";
        public string LastInputActions => _lastInputActions;
        
        
        public CharacterInput _characterInput = new CharacterInput();
        public CharacterInput CharacterInput => _characterInput;
        
        public LogicPauseInput _logicPauseInput = new LogicPauseInput();
        public LogicPauseInput LogicPauseInput => _logicPauseInput;
        
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

        protected override void CCreate()
        {
            base.CCreate();
        }

        protected override void CDispose()
        {
            base.CDispose();
        }

        #region Character Input
        
        public void OnMove(InputAction.CallbackContext context)
        {
            _characterInput.Movement = context.ReadValue<Vector2>();
        }
            

        #endregion
    }
}
