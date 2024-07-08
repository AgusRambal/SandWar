using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public class CombinedObject : MonoBehaviour, ICombinedObject<CombinedObjectPart, CombineSource>
    {
        IReadOnlyList<ICombinedObjectPart> ICombinedObject.Parts
        {
            get
            {
                return _parts;
            }
        }
        public IReadOnlyList<CombinedObjectPart> Parts
        {
            get
            {
                return _parts;
            }
        }
        public RendererSettings RendererSettings
        {
            get
            {
                return _rendererSettings;
            }
        }

        public Bounds Bounds
        {
            get
            {
                Bounds local = LocalBounds;

                local.center += transform.position;

                return local;
            }
        }
        public Bounds LocalBounds
        {
            get { return _combinedMesh.MeshData.GetBounds(); }
        }
        public int VertexCount
        {
            get { return _combinedMesh.MeshData.VertexCount; }
        }

        public bool Updating
        {
            get
            {
                return _updating;
            }
            set
            {
                _updating = value;
                enabled = value;
            }
        }

        private CombinedMesh _combinedMesh;
        private CombinedMeshDataInternal _meshData;

        private List<CombinedObjectPart> _parts;
        private List<CombinedMeshPart> _destroyedMeshParts;
        private RendererSettings _rendererSettings;
        private bool _updating;


        private void Update()
        {
            if (!Updating)
            {
                enabled = false;
                return;
            }

            ForceUpdate();

            enabled = false;
        }

        private void OnDestroy()
        {
            _combinedMesh.Dispose();
        }


        public static CombinedObject Create(MeshType meshType, CombineMethod combineType,
            RendererSettings settings)
        {
            return Create(new CombinedMeshFactory(meshType, combineType), settings);
        }

        public static CombinedObject Create(ICombinedMeshFactory factory, RendererSettings settings)
        {
            CombinedMesh combinedMesh = factory.CreateCombinedMesh();

            return Create(combinedMesh, settings);
        }

        public static CombinedObject Create(CombinedMesh combinedMesh, RendererSettings settings)
        {
            GameObject go = new GameObject("Combined Object");

            CombinedObject combinedObj = go.AddComponent<CombinedObject>();
            combinedObj.Construct(combinedMesh, settings);

            return combinedObj;
        }

        private void Construct(CombinedMesh combinedMesh, RendererSettings settings)
        {
            _combinedMesh = combinedMesh;
            _meshData = (CombinedMeshDataInternal)_combinedMesh.MeshData;

            _parts = new List<CombinedObjectPart>();
            _destroyedMeshParts = new List<CombinedMeshPart>();

            _rendererSettings = settings;
            _updating = true;

            if (combinedMesh.MeshData.PartsCount > 0)
            {
                foreach (var meshPart in combinedMesh.MeshData.GetParts())
                    _parts.Add(new CombinedObjectPart(this, meshPart));
            }

            CreateMeshFilter(_combinedMesh.Mesh);
            CreateMeshRenderer(settings);
        }


        public void ForceUpdate()
        {
            if (_destroyedMeshParts.Count > 0)
            {
                _combinedMesh.Cut(_destroyedMeshParts);
                _destroyedMeshParts.Clear();
            }
        }

        public Bounds GetLocalBounds(CombinedObjectPart part)
        {
            return _meshData.GetBounds(part.MeshPart);
        }

        public Bounds GetBounds(CombinedObjectPart part)
        {
            Bounds bounds = GetLocalBounds(part);

            bounds.center += transform.position;

            return bounds;
        }

        public void Combine(IEnumerable<ICombineSource> sources)
        {
            Combine(sources.Select(s => (CombineSource)s));
        }

        public void Combine(IEnumerable<CombineSource> sources)
        {
            if (_parts.Count == 0)
                transform.position = GetAveragePosition(sources);

            Vector3 position = transform.position;

            int sourcesCount = sources.Count();
            int idx = 0;

            MeshCombineInfo[] infos = new MeshCombineInfo[sourcesCount];

            foreach (var source in sources)
            {
                MeshCombineInfo info = source.CombineInfo;

                info.transformMatrix = info.transformMatrix.SetTranslation(source.Position - position);

                infos[idx++] = info;
            }

            try
            {
                CombinedMeshPart[] meshParts = _combinedMesh.Combine(infos);

                idx = 0;
                foreach (var source in sources)
                {
                    CombinedObjectPart part = new CombinedObjectPart(this, meshParts[idx]);

                    _parts.Add(part);

                    source.Combined(this, part);

                    idx++;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message + ex.StackTrace;

                foreach (var source in sources)
                {
                    source.CombineError(this, errorMessage);
                    source.CombineFailed(this);
                }
            }
        }

        public void Destroy(CombinedObjectPart part)
        {
            if (_parts.Remove(part))
            {
                _destroyedMeshParts.Add(part.MeshPart);

                enabled = true;
            }
        }


        private void CreateMeshFilter(Mesh mesh)
        {
            MeshFilter filter = gameObject.AddComponent<MeshFilter>();
            filter.sharedMesh = mesh;
        }

        private void CreateMeshRenderer(RendererSettings settings)
        {
            MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = settings.material;
            renderer.shadowCastingMode = settings.shadowMode;
            renderer.receiveShadows = settings.receiveShadows;
            renderer.lightmapIndex = settings.lightmapIndex;
            renderer.realtimeLightmapIndex = settings.realtimeLightmapIndex;
            renderer.tag = settings.tag;
            renderer.gameObject.layer = settings.layer;
        }

        private Vector3 GetAveragePosition(IEnumerable<CombineSource> sources)
        {
            Vector3 average = Vector3.zero;

            int count = 0;

            foreach (var source in sources)
            {
                average += source.Position;
                count++;
            }

            return (average / count);
        }
    }
}
