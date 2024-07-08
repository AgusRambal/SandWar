using System;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public class DynamicCombineSource : ICombineSource<DynamicCombinedObject, DynamicCombinedObjectPart>
    {
        public CombineSource Base
        {
            get { return _base; }
        }
        public Vector3 Position
        {
            get { return _base.Position; }
        }
        public Bounds Bounds
        {
            get { return _base.Bounds; }
        }

        public event Action<ICombinedObject, ICombinedObjectPart> onCombined;
        public event Action<ICombinedObject, string> onCombineError;
        public event Action<ICombinedObject> onCombineFailed;

        public event Action<DynamicCombinedObject, DynamicCombinedObjectPart> onCombinedTyped;
        public event Action<DynamicCombinedObject, string> onCombineErrorTyped;
        public event Action<DynamicCombinedObject> onCombineFailedTyped;

        private CombineSource _base;


        public DynamicCombineSource(GameObject go, int submeshIndex = 0)
        {
            _base = new CombineSource(go, submeshIndex);
        }

        public DynamicCombineSource(Mesh mesh, MeshRenderer renderer, int submeshIndex = 0)
        {
            _base = new CombineSource(mesh, renderer, submeshIndex);
        }

        public DynamicCombineSource(MeshCombineInfo info, RendererSettings settings)
        {
            _base = new CombineSource(info, settings);
        }

        public DynamicCombineSource(MeshCombineInfo info, RendererSettings settings, Bounds bounds)
        {
            _base = new CombineSource(info, settings, bounds);
        }


        public void Combined(DynamicCombinedObject root, DynamicCombinedObjectPart part)
        {
            onCombined?.Invoke(root, part);
            onCombinedTyped?.Invoke(root, part);
        }

        public void CombineError(DynamicCombinedObject root, string errorMessage)
        {
            if (onCombineError == null && onCombineErrorTyped == null)
            {
                Debug.Log("Error during combine " + root.name + ", reason :" + errorMessage);
                return;
            }

            onCombineError?.Invoke(root, errorMessage);
            onCombineErrorTyped?.Invoke(root, errorMessage);
        }

        public void CombineFailed(DynamicCombinedObject root)
        {
            onCombineFailed?.Invoke(root);
            onCombineFailedTyped?.Invoke(root);
        }
    }
}
