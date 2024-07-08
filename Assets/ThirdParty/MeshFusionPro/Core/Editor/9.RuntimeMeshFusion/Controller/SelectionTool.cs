using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace NGS.MeshFusionPro
{
    public partial class SelectionTool
    {
        public RuntimeMeshFusion Controller { get; private set; }

        private IMeshFusionSourcesSelector[] _selectors;
        private RuntimeMeshFusionEditor _editor;

        public SelectionTool(RuntimeMeshFusion controller)
        {
            Controller = controller;

            _selectors = new IMeshFusionSourcesSelector[]
            {
                new StaticSourcesSelector(this),
                new LODGroupsSourcesSelector(this),
                new DynamicSourcesSelector(this),
                new CombinedSourcesSelector(this),
                new SourcesWithErrorsSelector(this),
                new IncompatibleSourcesSelector(this)
            };
        }

        public void AssignTargets(RuntimeMeshFusionEditor editor, RuntimeMeshFusion controller)
        {
            Controller = controller;
            _editor = editor;
        }

        public void OnInspectorGUI()
        {
            bool sceneChanged = false;

            for (int i = 0; i < _selectors.Length; i++)
            {
                if (_selectors[i].isAvailable)
                    _selectors[i].OnInspectorGUI(ref sceneChanged);
            }

            if (sceneChanged || GUILayout.Button("Refresh"))
                Refresh();
        }

        public void OnDrawGizmos()
        {
            for (int i = 0; i < _selectors.Length; i++)
            {
                IMeshFusionSourcesSelector manager = _selectors[i];

                if (manager.isAvailable)
                    _selectors[i].OnDrawGizmos();
            }
        }

        public void Refresh()
        {
            MeshFusionSource[] sources = Object.FindObjectsOfType<MeshFusionSource>().ToArray();

            for (int i = 0; i < _selectors.Length; i++)
            {
                IMeshFusionSourcesSelector manager = _selectors[i];

                if (manager.isAvailable)
                    _selectors[i].OnSourcesUpdated(sources);
            }
        }

        public void Repaint()
        {
            _editor?.Repaint();
        }
    }
}
