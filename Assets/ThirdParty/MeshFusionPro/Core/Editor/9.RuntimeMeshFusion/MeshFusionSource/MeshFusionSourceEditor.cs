using UnityEditor;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MeshFusionSource), true)]
    public class MeshFusionSourceEditor : Editor
    {
        private SerializedProperty _combineStatusProp;
        private SerializedProperty _combineAtStartProp;
        private SerializedProperty _controllerIndexProp;
        private SerializedProperty _combineErrorStrategyProp;
        private SerializedProperty _afterCombineActionProp;

        private SerializedProperty _incompatibilityProp;
        private SerializedProperty _incompatibilityReasonProp;
        private SerializedProperty _hasCombineErrorsProp;
        private SerializedProperty _combineErrorsProp;
        private SerializedProperty _onCombineUnityEventProp;
        private bool _changed = false;


        private void OnEnable()
        {
            MeshFusionSource target = (MeshFusionSource)base.target;
           
            _combineStatusProp = serializedObject.FindAutoProperty(nameof(target.CombineStatus));
            _combineAtStartProp = serializedObject.FindAutoProperty(nameof(target.CombineAtStart));
            _controllerIndexProp = serializedObject.FindAutoProperty(nameof(target.ControllerIndex));
            _combineErrorStrategyProp = serializedObject.FindAutoProperty(nameof(target.CombineErrorStrategy));
            _afterCombineActionProp = serializedObject.FindAutoProperty(nameof(target.AfterCombineAction));

            _incompatibilityProp = serializedObject.FindAutoProperty(nameof(target.IsIncompatible));
            _incompatibilityReasonProp = serializedObject.FindAutoProperty(nameof(target.IncompatibilityReason));
            _hasCombineErrorsProp = serializedObject.FindAutoProperty(nameof(target.HasCombineErrors));
            _combineErrorsProp = serializedObject.FindAutoProperty(nameof(target.CombineErrors));

            _onCombineUnityEventProp = serializedObject.FindProperty(nameof(target.onCombineFinishedUnityEvent));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawHelpBoxes();

            DrawProperties();

            if (_changed)
            {
                ApplyModifiedProperties();
                _changed = false;
            }
        }


        private void DrawHelpBoxes()
        {
            if (_incompatibilityProp.boolValue && !_incompatibilityProp.hasMultipleDifferentValues)
            {
                string text = _incompatibilityReasonProp.stringValue;

                EditorGUILayout.HelpBox(text, MessageType.Warning);

                EditorGUILayout.Space();
            }

            if (_hasCombineErrorsProp.boolValue && !_hasCombineErrorsProp.hasMultipleDifferentValues)
            {
                string text = _combineErrorsProp.stringValue;

                EditorGUILayout.HelpBox(text, MessageType.Error);

                EditorGUILayout.Space();
            }
        }

        private void DrawProperties()
        {
            EditorGUI.BeginChangeCheck();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(_combineStatusProp);
            EditorGUI.EndDisabledGroup();

            if (Application.isPlaying)
                EditorGUI.BeginDisabledGroup(true);

            EditorGUILayout.PropertyField(_controllerIndexProp);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_combineAtStartProp);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_combineErrorStrategyProp, new GUIContent("On Combine Error"));
            EditorGUILayout.PropertyField(_afterCombineActionProp);

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (!_onCombineUnityEventProp.hasMultipleDifferentValues)
                EditorGUILayout.PropertyField(_onCombineUnityEventProp, new GUIContent("OnCombineFinished"));

            if (Application.isPlaying)
                EditorGUI.EndDisabledGroup();

            if (EditorGUI.EndChangeCheck())
                _changed = true;
        }

        private void ApplyModifiedProperties()
        {
            foreach (var target in targets)
            {
                MeshFusionSource source = (MeshFusionSource)target;

                if (!_controllerIndexProp.hasMultipleDifferentValues)
                    source.ControllerIndex = _controllerIndexProp.intValue;

                if (!_combineAtStartProp.hasMultipleDifferentValues)
                    source.CombineAtStart = _combineAtStartProp.boolValue;

                if (!_combineErrorStrategyProp.hasMultipleDifferentValues)
                    source.CombineErrorStrategy = (CombineErrorStrategy)_combineErrorStrategyProp.enumValueIndex;

                if (!_afterCombineActionProp.hasMultipleDifferentValues)
                    source.AfterCombineAction = (AfterCombineAction)_afterCombineActionProp.enumValueIndex;

                if (!_onCombineUnityEventProp.hasMultipleDifferentValues)
                    _onCombineUnityEventProp.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
