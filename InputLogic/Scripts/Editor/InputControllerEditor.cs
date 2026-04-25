#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Spacats.Input
{
    [CustomEditor(typeof(InputController), true)]
    public class InputControllerEditor : Editor
    {
        private int _tabIndex = 0;
        private readonly string[] _tabHeaders = { "Input Settings", "Controller Settings" };
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            SetDefaultParameters();

            _tabIndex = GUILayout.Toolbar(_tabIndex, _tabHeaders);
            EditorGUILayout.Space();

            switch (_tabIndex)
            {
                case 0:
                    DrawSettings();
                    break;
                case 1:
                    DrawControllerSettings();
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
        
        private void DrawControllerSettings()
        {
            SerializedProperty executeInEditor = serializedObject.FindProperty("ExecuteInEditor");
            EditorGUILayout.PropertyField(executeInEditor);
            
            SerializedProperty executeOrder = serializedObject.FindProperty("ExecuteOrder");
            EditorGUILayout.PropertyField(executeOrder);

            SerializedProperty showLogs = serializedObject.FindProperty("ShowLogs");
            EditorGUILayout.PropertyField(showLogs);

            SerializedProperty showCLogs = serializedObject.FindProperty("ShowCLogs");
            EditorGUILayout.PropertyField(showCLogs);
        }
        
        private void SetDefaultParameters()
        {
            InputController targetScript = (InputController)target;
            targetScript.UniqueTag = "";
        }
        
        private void DrawSettings()
        {
            DrawFields();
            DrawButtons();
        }
        
        private void DrawFields()
        {
            InputController targetScript = (InputController)target;

            //GUILayout.TextArea("Last Input Actions: " + targetScript.LastInputActions);
            // GUILayout.TextArea("Active Tweens: " + targetScript.ActiveTweensCount);
            // GUILayout.TextArea("Tweens pool: " + targetScript.TweensListCount);
            //
            
            SerializedProperty config = serializedObject.FindProperty("_config");
            EditorGUILayout.PropertyField(config);
            
            SerializedProperty characterInput = serializedObject.FindProperty("_characterInput");
            EditorGUILayout.PropertyField(characterInput);
            
            SerializedProperty logicPauseInput = serializedObject.FindProperty("_logicPauseInput");
            EditorGUILayout.PropertyField(logicPauseInput);
        }

        private void DrawButtons()
        {
            InputController targetScript = (InputController)target;
            if (GUILayout.Button("Refresh Settings"))
            {
                targetScript.RefreshSettings();
            }
        }
    }
}
#endif
