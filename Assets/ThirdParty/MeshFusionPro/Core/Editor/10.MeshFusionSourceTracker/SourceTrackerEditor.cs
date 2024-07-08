using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace NGS.MeshFusionPro
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SourceTracker))]
    public class SourceTrackerEditor : Editor
    {
        protected new SourceTracker target
        {
            get
            {
                return base.target as SourceTracker;
            }
        }

        private SerializedProperty _trackingDestroyProp;
        private SerializedProperty _isDynamicObjectProp;
        private SerializedProperty _trackingTargetProp;
        private SerializedProperty _disableWhenIdleProp;
        private SerializedProperty _maxIdleTimeProp;
        private SerializedProperty _wakeUpWhenCollisionProp;

        private void OnEnable()
        {
            _trackingDestroyProp = serializedObject.FindAutoProperty(nameof(target.TrackingDestroy));
            _isDynamicObjectProp = serializedObject.FindProperty("_isDynamicObject");
            _trackingTargetProp = serializedObject.FindAutoProperty(nameof(target.TrackingTarget));
            _disableWhenIdleProp = serializedObject.FindAutoProperty(nameof(target.DisableWhenIdle));
            _maxIdleTimeProp = serializedObject.FindAutoProperty(nameof(target.MaxIdleTime));
            _wakeUpWhenCollisionProp = serializedObject.FindAutoProperty(nameof(target.WakeUpWhenCollision));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            if (_isDynamicObjectProp.boolValue)
            {
                if (!_isDynamicObjectProp.hasMultipleDifferentValues)
                {
                    EditorGUILayout.PropertyField(_trackingTargetProp);

                    if (!_trackingTargetProp.hasMultipleDifferentValues &&
                        ((TrackingTarget)_trackingTargetProp.enumValueIndex) == TrackingTarget.Rigidbody)
                    {
                        DrawRigidbodiesOptions();
                    }
                }
            }

            EditorGUILayout.PropertyField(_trackingDestroyProp);

            if (EditorGUI.EndChangeCheck())
                ApplyChanges();
        }

        private void DrawRigidbodiesOptions()
        {
            EditorGUILayout.PropertyField(_disableWhenIdleProp);

            if (!_disableWhenIdleProp.boolValue || _disableWhenIdleProp.hasMultipleDifferentValues)
                return;

            EditorGUILayout.PropertyField(_maxIdleTimeProp);
            EditorGUILayout.PropertyField(_wakeUpWhenCollisionProp);
        }

        private void ApplyChanges()
        {
            foreach (var target in targets)
            {
                SourceTracker tracker = target as SourceTracker;

                if (!_trackingTargetProp.hasMultipleDifferentValues)
                {
                    TrackingTarget trackingTarget = (TrackingTarget)_trackingTargetProp.enumValueIndex;

                    tracker.TrackingTarget = trackingTarget;

                    if (trackingTarget == TrackingTarget.Rigidbody)
                    {
                        if (!_disableWhenIdleProp.hasMultipleDifferentValues)
                        {
                            tracker.DisableWhenIdle = _disableWhenIdleProp.boolValue;

                            if (tracker.DisableWhenIdle)
                            {
                                if (!_maxIdleTimeProp.hasMultipleDifferentValues)
                                    tracker.MaxIdleTime = _maxIdleTimeProp.floatValue;

                                if (!_wakeUpWhenCollisionProp.hasMultipleDifferentValues)
                                    tracker.WakeUpWhenCollision = _wakeUpWhenCollisionProp.boolValue;
                            }
                        }
                    }
                }

                if (!_trackingDestroyProp.hasMultipleDifferentValues)
                    tracker.TrackingDestroy = _trackingDestroyProp.boolValue;
            }
        }
    }
}
