using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace NGS.MeshFusionPro
{
    public partial class SelectionTool
    {
        public interface IMeshFusionSourcesSelector
        {
            bool isAvailable { get; }

            void OnSourcesUpdated(IReadOnlyList<MeshFusionSource> sources);

            void OnInspectorGUI(ref bool sceneChanged);

            void OnDrawGizmos();
        }

        private abstract class BaseSourcesSelector<TMeshFusionSource> : IMeshFusionSourcesSelector
            where TMeshFusionSource : MeshFusionSource
        {
            protected static GUIStyle FoldoutGUIStyle { get; private set; }
            protected static GUIStyle ButtonGUIStyle { get; private set; }

            public abstract bool isAvailable { get; }

            protected RuntimeMeshFusion Controller
            {
                get
                {
                    return _parent.Controller;
                }
            }
            protected IReadOnlyList<TMeshFusionSource> Sources
            {
                get
                {
                    return _sources;
                }
            }
            protected bool IsUserPickedGameObjects
            {
                get
                {
                    return Selection.gameObjects.Length > 1 ||
                        (Selection.gameObjects.Length == 1 && Selection.activeGameObject != Controller.gameObject);
                }
            }

            public string Label { get; set; }
            protected bool DrawGizmo { get; set; }
            protected Color GizmoColor { get; set; }

            private SelectionTool _parent;
            private TMeshFusionSource[] _sources;
            protected Bounds[] _sourcesBounds;
            private AnimBool _foldout;


            public BaseSourcesSelector(SelectionTool parent)
            {
                _parent = parent;

                Label = "Default Label";

                _foldout = new AnimBool(false);
                _foldout.valueChanged.AddListener(parent.Repaint);
            }

            public void OnSourcesUpdated(IReadOnlyList<MeshFusionSource> sources)
            {
                _sources = sources
                    .Where(s => SourcesFilter(s))
                    .Select(s => s as TMeshFusionSource)
                    .ToArray();

                Bounds bounds = new Bounds();
                _sourcesBounds = _sources
                    .Where(s => s.TryGetBounds(ref bounds))
                    .Select(s => bounds)
                    .ToArray();
            }

            public void OnInspectorGUI(ref bool sceneChanged)
            {
                if (FoldoutGUIStyle == null || ButtonGUIStyle == null)
                    CreateGUIStyles();

                string label = string.Format("{0} ({1})", Label, Sources.Count);

                _foldout.target = EditorGUILayout.Foldout(_foldout.target, label, true, FoldoutGUIStyle);

                if (EditorGUILayout.BeginFadeGroup(_foldout.faded))
                {
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    DrawContent(ref sceneChanged);
                }

                EditorGUILayout.EndFadeGroup();

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }

            public virtual void OnDrawGizmos()
            {
                if (!DrawGizmo || _sources == null)
                    return;

                Gizmos.color = GizmoColor;

                for (int i = 0; i < _sourcesBounds.Length; i++)
                {
                    Bounds bounds = _sourcesBounds[i];
                    Gizmos.DrawWireCube(bounds.center, bounds.size);
                }
            }


            private void CreateGUIStyles()
            {
                FoldoutGUIStyle = new GUIStyle(EditorStyles.foldout);
                FoldoutGUIStyle.fontSize = 15;
                FoldoutGUIStyle.fontStyle = FontStyle.Bold;
                FoldoutGUIStyle.normal.textColor = Color.white;

                ButtonGUIStyle = new GUIStyle(GUI.skin.button);
                ButtonGUIStyle.fontSize = 12;
                ButtonGUIStyle.fixedHeight = 30;
                ButtonGUIStyle.margin = new RectOffset(5, 5, 5, 5);
                ButtonGUIStyle.border = new RectOffset(0, 0, 0, 0);
                ButtonGUIStyle.padding = new RectOffset(5, 5, 5, 5);
            }


            protected abstract void DrawContent(ref bool sceneChanged);

            protected virtual bool SourcesFilter(MeshFusionSource source)
            {
                return source is TMeshFusionSource && source.ControllerIndex == Controller.ControllerIndex;
            }

            protected virtual void AssignSource(GameObject go)
            {
                TMeshFusionSource source = go.AddComponent<TMeshFusionSource>();
                source.ControllerIndex = Controller.ControllerIndex;
            }

            protected virtual void ClearSource(TMeshFusionSource source)
            {
                SourceTracker tracker;
                if (source.TryGetComponent(out tracker))
                    UnityEngine.Object.DestroyImmediate(tracker);

                UnityEngine.Object.DestroyImmediate(source);
            }



            protected void AssignToGameObjects(Func<GameObject, bool> filter)
            {
                GameObject[] gos = UnityEngine.Object.FindObjectsOfType<GameObject>();

                int count = 0;

                for (int i = 0; i < gos.Length; i++)
                {
                    GameObject go = gos[i];

                    if (go.TryGetComponent(out MeshFusionSource s))
                        continue;

                    if (!filter(go))
                        continue;

                    AssignSource(go);

                    count++;
                }

                Debug.Log("Assigned " + count + " objects");
            }

            protected void AssignToSelectedGameObjects(Func<GameObject, bool> filter)
            {
                GameObject[] staticObjects = Selection.gameObjects
                    .SelectMany(go => go.GetComponentsInChildren<Transform>().Select(t => t.gameObject))
                    .ToArray();

                int count = 0;

                for (int i = 0; i < staticObjects.Length; i++)
                {
                    GameObject go = staticObjects[i];

                    if (go.TryGetComponent(out MeshFusionSource s))
                        continue;

                    if (!filter(go))
                        continue;

                    AssignSource(go);

                    count++;
                }

                Debug.Log("Assigned " + count + " objects");
            }

            protected void ClearAllSources()
            {
                for (int i = 0; i < _sources.Length; i++)
                    ClearSource(_sources[i]);

                Debug.Log("Removed " + _sources.Length + " sources");
            }

            protected void ClearSelectedSources()
            {
                TMeshFusionSource[] selectedSources = Selection.gameObjects
                    .SelectMany(go => go.GetComponentsInChildren<TMeshFusionSource>()
                    .Where(s => s.ControllerIndex == Controller.ControllerIndex))
                    .ToArray();

                for (int i = 0; i < selectedSources.Length; i++)
                    ClearSource(selectedSources[i]);

                Debug.Log("Removed " + selectedSources.Length + " sources");
            }

            protected void SelectSources()
            {
                Selection.objects = Sources.Select(s => s.gameObject).ToArray();
            }
        }

        private class StaticSourcesSelector : BaseSourcesSelector<StaticMeshFusionSource>
        {
            public override bool isAvailable
            {
                get
                {
                    return !Application.isPlaying;
                }
            }
            private bool _assignOnlyStatic;

            public StaticSourcesSelector(SelectionTool parent) : base(parent)
            {
                GizmoColor = Color.blue;
                Label = "Static Sources";
            }

            protected override void DrawContent(ref bool sceneChanged)
            {
                if (Sources.Count > 0)
                    DrawGizmo = EditorGUILayout.Toggle("Draw Gizmo", DrawGizmo);

                Color backColor = GUI.backgroundColor;
                bool picked = IsUserPickedGameObjects;

                if (picked)
                    _assignOnlyStatic = EditorGUILayout.Toggle("Assign Only Static", _assignOnlyStatic);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical();

                GUI.backgroundColor = Color.green;
                if (GUILayout.Button("Assign Auto", ButtonGUIStyle))
                {
                    AssignToGameObjects(go => GameObjectsFilter(go, true));
                    sceneChanged = true;
                }
                GUI.backgroundColor = backColor;

                EditorGUI.BeginDisabledGroup(!picked);

                if (GUILayout.Button("Assign Selected", ButtonGUIStyle))
                {
                    AssignToSelectedGameObjects(go => GameObjectsFilter(go, _assignOnlyStatic));
                    sceneChanged = true;
                }

                EditorGUI.EndDisabledGroup();

                EditorGUILayout.EndVertical();

                if (Sources.Count == 0)
                {
                    EditorGUILayout.EndHorizontal();
                    return;
                }

                EditorGUILayout.BeginVertical();

                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Clear All", ButtonGUIStyle))
                {
                    ClearAllSources();
                    sceneChanged = true;
                }
                GUI.backgroundColor = backColor;

                EditorGUI.BeginDisabledGroup(!picked);

                if (GUILayout.Button("Clear Selected", ButtonGUIStyle))
                {
                    ClearSelectedSources();
                    sceneChanged = true;
                }

                EditorGUI.EndDisabledGroup();

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();

                if (GUILayout.Button("Select", ButtonGUIStyle))
                    SelectSources();
            }

            private bool GameObjectsFilter(GameObject go, bool assignOnlyStatic)
            {
                if (assignOnlyStatic && !go.isStatic)
                    return false;

                if (!go.TryGetComponent(out MeshRenderer renderer))
                    return false;

                if (!go.TryGetComponent(out MeshFilter filter))
                    return false;

                LODGroup group = go.GetComponentInParent<LODGroup>();

                if (group != null && group.Contains(renderer))
                    return false;

                return true;
            }
        }

        private class LODGroupsSourcesSelector : BaseSourcesSelector<LODMeshFusionSource>
        {
            public override bool isAvailable
            {
                get
                {
                    return !Application.isPlaying;
                }
            }

            public LODGroupsSourcesSelector(SelectionTool parent) : base(parent)
            {
                GizmoColor = Color.yellow;
                Label = "LOD's Sources";
            }

            protected override void DrawContent(ref bool sceneChanged)
            {
                if (Sources.Count > 0)
                    DrawGizmo = EditorGUILayout.Toggle("Draw Gizmo", DrawGizmo);

                Color backColor = GUI.backgroundColor;
                bool picked = IsUserPickedGameObjects;

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical();

                GUI.backgroundColor = Color.green;
                if (GUILayout.Button("Assign Auto", ButtonGUIStyle))
                {
                    AssignToGameObjects(GameObjectsFilter);
                    sceneChanged = true;
                }
                GUI.backgroundColor = backColor;

                EditorGUI.BeginDisabledGroup(!picked);

                if (GUILayout.Button("Assign Selected", ButtonGUIStyle))
                {
                    AssignToSelectedGameObjects(GameObjectsFilter);
                    sceneChanged = true;
                }

                EditorGUI.EndDisabledGroup();

                EditorGUILayout.EndVertical();

                if (Sources.Count == 0)
                {
                    EditorGUILayout.EndHorizontal();
                    return;
                }

                EditorGUILayout.BeginVertical();

                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Clear All", ButtonGUIStyle))
                {
                    ClearAllSources();
                    sceneChanged = true;
                }
                GUI.backgroundColor = backColor;

                EditorGUI.BeginDisabledGroup(!picked);

                if (GUILayout.Button("Clear Selected", ButtonGUIStyle))
                {
                    ClearSelectedSources();
                    sceneChanged = true;
                }

                EditorGUI.EndDisabledGroup();

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();

                if (GUILayout.Button("Select", ButtonGUIStyle))
                    SelectSources();
            }

            private bool GameObjectsFilter(GameObject go)
            {
                return go.TryGetComponent(out LODGroup l);
            }
        }

        private class DynamicSourcesSelector : BaseSourcesSelector<DynamicMeshFusionSource>
        {
            public override bool isAvailable
            {
                get
                {
                    return !Application.isPlaying;
                }
            }

            public DynamicSourcesSelector(SelectionTool parent) : base(parent)
            {
                GizmoColor = Color.green;
                Label = "Dynamic Sources";
            }

            protected override void DrawContent(ref bool sceneChanged)
            {
                if (Sources.Count > 0)
                    DrawGizmo = EditorGUILayout.Toggle("Draw Gizmo", DrawGizmo);

                Color backColor = GUI.backgroundColor;

                GUI.backgroundColor = Color.green;
                if (GUILayout.Button("Assign Selected Rigidbodies", ButtonGUIStyle))
                {
                    AssignToSelectedGameObjects(go => GameObjectsFilter(go));
                    sceneChanged = true;
                }
                GUI.backgroundColor = backColor;

                if (Sources.Count == 0)
                    return;

                EditorGUILayout.BeginHorizontal();

                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Clear All", ButtonGUIStyle))
                {
                    ClearAllSources();
                    sceneChanged = true;
                }
                GUI.backgroundColor = backColor;

                if (GUILayout.Button("Clear Selected", ButtonGUIStyle))
                {
                    ClearSelectedSources();
                    sceneChanged = true;
                }

                EditorGUILayout.EndHorizontal();

                if (GUILayout.Button("Select", ButtonGUIStyle))
                    SelectSources();
            }


            protected override void AssignSource(GameObject go)
            {
                base.AssignSource(go);

                go.AddComponent<SourceTracker>();
            }

            private bool GameObjectsFilter(GameObject go)
            {
                if (go.isStatic)
                    return false;

                if (!go.TryGetComponent(out MeshRenderer renderer))
                    return false;

                if (!go.TryGetComponent(out MeshFilter filter))
                    return false;

                if (!go.TryGetComponent(out Rigidbody rigidbody))
                    return false;

                LODGroup group = go.GetComponentInParent<LODGroup>();

                if (group != null && group.Contains(renderer))
                    return false;

                return true;
            }
        }

        private class CombinedSourcesSelector : BaseSourcesSelector<MeshFusionSource>
        {
            public override bool isAvailable
            {
                get
                {
                    return Application.isPlaying;
                }
            }

            public CombinedSourcesSelector(SelectionTool tool) : base(tool)
            {
                Label = "Combined Sources";
                GizmoColor = Color.green;
            }

            protected override void DrawContent(ref bool sceneChanged)
            {
                if (Sources.Count == 0)
                {
                    EditorGUILayout.HelpBox("No combined sources found :(\nTry to refresh", MessageType.Info);
                    return;
                }

                DrawGizmo = EditorGUILayout.Toggle("Draw Gizmo", DrawGizmo);

                if (GUILayout.Button("Select", ButtonGUIStyle))
                    SelectSources();
            }

            protected override bool SourcesFilter(MeshFusionSource source)
            {
                return source.CombineStatus == SourceCombineStatus.Combined && 
                    source.ControllerIndex == Controller.ControllerIndex;
            }
        }

        private class SourcesWithErrorsSelector : BaseSourcesSelector<MeshFusionSource>
        {
            public override bool isAvailable
            {
                get
                {
                    return Application.isPlaying;
                }
            }

            public SourcesWithErrorsSelector(SelectionTool parent) : base(parent)
            {
                Label = "Sources With Errors";
                GizmoColor = Color.red;
            }

            protected override void DrawContent(ref bool sceneChanged)
            {
                if (Sources.Count == 0)
                {
                    EditorGUILayout.HelpBox("Sources with errors not found :)", MessageType.Info);
                    return;
                }

                DrawGizmo = EditorGUILayout.Toggle("Draw Gizmos", DrawGizmo);

                if (GUILayout.Button("Print Combine Errors", ButtonGUIStyle))
                    PrintCombineErrors();

                if (GUILayout.Button("Select", ButtonGUIStyle))
                    SelectSources();
            }

            protected override bool SourcesFilter(MeshFusionSource source)
            {
                return source.HasCombineErrors;
            }

            private void PrintCombineErrors()
            {
                for (int i = 0; i < Sources.Count; i++)
                    Debug.Log(Sources[i].gameObject.name + " errors : " + Sources[i].CombineErrors);
            }
        }

        private class IncompatibleSourcesSelector : BaseSourcesSelector<MeshFusionSource>
        {
            public override bool isAvailable
            {
                get
                {
                    return true;
                }
            }

            public IncompatibleSourcesSelector(SelectionTool tool) : base(tool)
            {
                Label = "Incompatible Sources";
            }

            protected override void DrawContent(ref bool sceneChanged)
            {
                if (Sources.Count == 0)
                {
                    EditorGUILayout.HelpBox("Incompatible sources not found :)", MessageType.Info);
                    return;
                }

                if (GUILayout.Button("Print Incompatibility Reasons", ButtonGUIStyle))
                    PrintIncompatibilityReasons();

                if (!Application.isPlaying)
                {
                    if (GUILayout.Button("Clear All", ButtonGUIStyle))
                    {
                        ClearAllSources();
                        sceneChanged = true;
                    }
                }

                if (GUILayout.Button("Select", ButtonGUIStyle))
                    SelectSources();
            }


            protected override bool SourcesFilter(MeshFusionSource source)
            {
                return source.IsIncompatible;
            }

            private void PrintIncompatibilityReasons()
            {
                for (int i = 0; i < Sources.Count; i++)
                {
                    MeshFusionSource source = Sources[i];

                    Debug.Log("Unable to combine '" + source.gameObject.name +
                        "' reason : " + source.IncompatibilityReason);
                }
            }
        }
    }
}
