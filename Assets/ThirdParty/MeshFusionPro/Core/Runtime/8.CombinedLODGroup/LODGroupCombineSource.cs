using System;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public class LODGroupCombineSource : ICombineSource<CombinedLODGroup, CombinedLODGroupPart>
    {
        public Vector3 Position { get; private set; }
        public Bounds Bounds
        {
            get { return _bounds; }
        }

        public LODGroup LODGroup { get; private set; }
        public LODGroupSettings Settings
        {
            get { return _settings; }
        }
        public CombineSource[][] BaseSources
        {
            get { return _sources; }
        }

        public event Action<ICombinedObject, ICombinedObjectPart> onCombined;
        public event Action<ICombinedObject, string> onCombineError;
        public event Action<ICombinedObject> onCombineFailed;

        public event Action<CombinedLODGroup, CombinedLODGroupPart> onCombinedTyped;
        public event Action<CombinedLODGroup, string> onCombineErrorTyped;
        public event Action<CombinedLODGroup> onCombineFailedTyped;

        private Bounds _bounds;
        private LODGroupSettings _settings;
        private CombineSource[][] _sources;


        public LODGroupCombineSource(LODGroup group)
        {
            LODGroup = group;
            Position = group.transform.position;

            LOD[] lods = group.GetLODs();

            _settings = new LODGroupSettings(group);
            _sources = new CombineSource[_settings.lodCount][];
            _bounds = new Bounds(group.localReferencePoint + Position, Vector3.zero);

            for (int i = 0; i < lods.Length; i++)
            {
                Renderer[] renderers = lods[i].renderers;

                CreateSourcesArray(i, renderers);
                FillSources(i, renderers);
            }
        }

        public void Combined(CombinedLODGroup root, CombinedLODGroupPart part)
        {
            onCombined?.Invoke(root, part);
            onCombinedTyped?.Invoke(root, part);
        }

        public void CombineError(CombinedLODGroup root, string errorMessage)
        {
            if (onCombineError == null && onCombineErrorTyped == null)
            {
                Debug.Log("Combine error occured : " + root.name + ", reason : " + errorMessage);
                return;
            }

            onCombineError?.Invoke(root, errorMessage);
            onCombineErrorTyped?.Invoke(root, errorMessage);
        }

        public void CombineFailed(CombinedLODGroup root)
        {
            onCombineFailed?.Invoke(root);
            onCombineFailedTyped?.Invoke(root);
        }


        private void CreateSourcesArray(int level, Renderer[] renderers)
        {
            int count = 0;

            for (int i = 0; i < renderers.Length; i++)
            {
                Renderer renderer = renderers[i];

                if (renderer == null)
                    continue;

                if (!(renderer is MeshRenderer))
                    continue;

                MeshFilter filter = renderer.GetComponent<MeshFilter>();

                if (filter == null || filter.sharedMesh == null)
                    continue;

                count += renderers[i].sharedMaterials.Length;
            }

            _sources[level] = new CombineSource[count];
        }

        private void FillSources(int level, Renderer[] renderers)
        {
            int srcIndex = 0;
            for (int i = 0; i < renderers.Length; i++)
            {
                Renderer renderer = renderers[i];

                if (renderer == null)
                    continue;

                if (!(renderer is MeshRenderer))
                    continue;

                MeshFilter filter = renderer.GetComponent<MeshFilter>();

                if (filter == null || filter.sharedMesh == null)
                    continue;

                Mesh mesh = renderer.GetComponent<MeshFilter>().sharedMesh;

                for (int c = 0; c < mesh.subMeshCount; c++)
                {
                    _sources[level][srcIndex] = new CombineSource(mesh, (MeshRenderer)renderer, c);
                    srcIndex++;
                }

                _bounds.Encapsulate(renderer.bounds);
            }
        }
    }
}
