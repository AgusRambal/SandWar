using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace NGS.MeshFusionPro
{
    [CustomEditor(typeof(RuntimeMeshFusion))]
    public class RuntimeMeshFusionEditor : Editor
    {
        private static Dictionary<int, SelectionTool> _indexToSelection =
            new Dictionary<int, SelectionTool>();

        protected new RuntimeMeshFusion target
        {
            get
            {
                return (RuntimeMeshFusion) base.target;
            }
        }
        private SelectionTool _selectionTool;
        private GUIStyle _titleLabelStyle;


        private void OnEnable()
        {
            int idx = target.ControllerIndex;

            if (!_indexToSelection.TryGetValue(idx, out _selectionTool))
            {
                _selectionTool = new SelectionTool(target);
                _indexToSelection.Add(idx, _selectionTool);
            }

            _selectionTool.AssignTargets(this, target);
            _selectionTool.Refresh();

            _titleLabelStyle = new GUIStyle();
            _titleLabelStyle.fontSize = 24;
            _titleLabelStyle.fontStyle = FontStyle.Bold;
            _titleLabelStyle.alignment = TextAnchor.MiddleLeft;
            _titleLabelStyle.normal.textColor = Color.white;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("MeshFusion Pro", _titleLabelStyle);

            DrawSeparatorLine(1, 2);

            EditorGUILayout.Space();

            if (Application.isPlaying)
            {
                target.DrawGizmo = EditorGUILayout.Toggle("Draw Gizmo", target.DrawGizmo);
                EditorGUILayout.Space();
            }

            target.ControllerIndex = EditorGUILayout.IntField("Controller Index", target.ControllerIndex);

            EditorGUILayout.Space();

            target.CellSize = EditorGUILayout.IntField("Cell Size", target.CellSize);
            target.LimitVertices = EditorGUILayout.Toggle("65k Vertices Limit", target.LimitVertices);

            EditorGUILayout.Space();

            target.MeshType = (MeshType) EditorGUILayout.EnumPopup("Mesh Type", target.MeshType);
            target.MoveMethod = (MoveMethod)EditorGUILayout.EnumPopup("Move Method", target.MoveMethod);

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (_selectionTool.Controller.gameObject != null)
                _selectionTool.OnInspectorGUI();

            EditorGUILayout.Space();

            if (Application.isPlaying)
                return;

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Disable Static\nBatching"))
            {
                MeshFusionEditorUtil.DisableStaticBatching();
                MeshFusionEditorUtil.CheckAllMeshFusionSources();
            }

            if (GUILayout.Button("Make Meshes\nReadable"))
            {
                MeshFusionEditorUtil.MakeAllMeshesReadable();
                MeshFusionEditorUtil.CheckAllMeshFusionSources();
                _selectionTool.Refresh();
            }

            EditorGUILayout.EndHorizontal();
        }


        private void DrawSeparatorLine(float thickness, float padding)
        {
            Rect previousRect = GUILayoutUtility.GetLastRect();
            GUILayout.Space(padding);
            EditorGUILayout.LabelField("", GUILayout.Height(thickness));
            Rect lineRect = GUILayoutUtility.GetLastRect();
            lineRect.x = previousRect.x;
            lineRect.width = previousRect.width;
            EditorGUI.DrawRect(lineRect, Color.gray);
            GUILayout.Space(padding);
        }


        [MenuItem("Tools/NGSTools/MeshFusion Pro")]
        private static void CreateRuntimeMeshFusion()
        {
            GameObject go = new GameObject("RuntimeMeshFusion");
            go.AddComponent<RuntimeMeshFusion>();

            Selection.activeObject = go;
        }

        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
        private static void OnDrawGizmos(RuntimeMeshFusion controller, GizmoType gizmoType)
        {
            SelectionTool tool;
            if (_indexToSelection.TryGetValue(controller.ControllerIndex, out tool))
                tool.OnDrawGizmos();
        }
    }
}
