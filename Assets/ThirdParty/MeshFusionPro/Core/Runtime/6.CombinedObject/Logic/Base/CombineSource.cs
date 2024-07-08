using System;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public class CombineSource : ICombineSource<CombinedObject, CombinedObjectPart>
    {
        public Vector3 Position { get; private set; }
        public Bounds Bounds { get; private set; }
        public MeshCombineInfo CombineInfo { get; private set; }
        public RendererSettings RendererSettings { get; private set; }

        public event Action<ICombinedObject, ICombinedObjectPart> onCombined;
        public event Action<ICombinedObject, string> onCombineError;
        public event Action<ICombinedObject> onCombineFailed;

        public event Action<CombinedObject, CombinedObjectPart> onCombinedTyped;
        public event Action<CombinedObject, string> onCombineErrorTyped;
        public event Action<CombinedObject> onCombineFailedTyped;


        public CombineSource(GameObject go, int submeshIndex = 0)
            : this(go.GetComponent<MeshFilter>().mesh, go.GetComponent<MeshRenderer>(), submeshIndex)
        {

        }

        public CombineSource(Mesh mesh, MeshRenderer renderer, int submeshIndex = 0)
        {
            if (mesh == null)
                throw new ArgumentException("Mesh is null");

            if (renderer == null)
                throw new ArgumentException("Mesh Renderer is null");

            if (submeshIndex >= mesh.subMeshCount)
                throw new ArgumentException("Submesh index is greater the submeshCount");

            MeshCombineInfo info = new MeshCombineInfo(
                mesh,
                renderer.localToWorldMatrix,
                renderer.lightmapScaleOffset,
                renderer.realtimeLightmapScaleOffset,
                submeshIndex);

            RendererSettings settings = new RendererSettings(renderer, submeshIndex);

            Construct(info, settings, renderer.bounds);
        }

        public CombineSource(MeshCombineInfo info, RendererSettings settings) :
            this(info, settings, info.mesh.bounds.Transform(info.transformMatrix))
        {

        }

        public CombineSource(MeshCombineInfo info, RendererSettings settings, Bounds bounds)
        {
            Construct(info, settings, bounds);
        }

        private void Construct(MeshCombineInfo info, RendererSettings settings, Bounds bounds)
        {
            CombineInfo = info;
            RendererSettings = settings;
            Position = info.transformMatrix.GetTranslation();
            Bounds = bounds;
        }


        public void Combined(CombinedObject root, CombinedObjectPart part)
        {
            onCombined?.Invoke(root, part);
            onCombinedTyped?.Invoke(root, part);
        }

        public void CombineError(CombinedObject root, string errorMessage)
        {
            if (onCombineError == null && onCombinedTyped == null)
            {
                Debug.Log("Error during combine " + root.name + ", reason :" + errorMessage);
                return;
            }

            onCombineError?.Invoke(root, errorMessage);
            onCombineErrorTyped?.Invoke(root, errorMessage);
        }

        public void CombineFailed(CombinedObject root)
        {
            onCombineFailed?.Invoke(root);
            onCombineFailedTyped?.Invoke(root);
        }
    }
}
