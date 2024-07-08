using System;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public abstract class CombinedMeshData
    {
        public abstract int PartsCount { get; }
        public abstract int VertexCount { get; }

        public abstract Bounds GetBounds();

        public abstract Bounds GetBounds(CombinedMeshPart part);

        public abstract IEnumerable<CombinedMeshPart> GetParts();
    }

    public class CombinedMeshDataInternal : CombinedMeshData, IDisposable
    {
        public override int PartsCount
        {
            get
            {
                return _parts.Count;
            }
        }
        public override int VertexCount
        {
            get
            {
                return _vertexCount;
            }
        }

        protected CombinedMesh Root
        {
            get
            {
                return _root;
            }
        }

        private CombinedMesh _root;
        private List<CombinedMeshPartInternal> _parts;

        private int _vertexCount;
        private int _trianglesCount;

        private List<Bounds> _partsBounds;
        private Bounds _boundingBox;


        public void Initialize(CombinedMesh root)
        {
            _root = root;
            _parts = new List<CombinedMeshPartInternal>();

            OnInitialized();
        }

        public override Bounds GetBounds()
        {
            return _boundingBox;
        }

        public override Bounds GetBounds(CombinedMeshPart part)
        {
            return _partsBounds[part.Index];
        }

        public override IEnumerable<CombinedMeshPart> GetParts()
        {
            for (int i = 0; i < _parts.Count; i++)
                yield return _parts[i];
        }

        public void Dispose()
        {
            OnDispose();
        }


        public CombinedMeshPart[] CreateParts(IList<MeshCombineInfo> infos)
        {
            CombinedMeshPart[] result = new CombinedMeshPart[infos.Count];

            for (int i = 0; i < result.Length; i++)
            {
                MeshCombineInfo info = infos[i];

                Mesh mesh = info.mesh;
                Matrix4x4 transform = info.transformMatrix;

                int vertStart = _vertexCount;
                int vertCount = info.vertexCount;
                int triStart = _trianglesCount;
                int triCount = info.trianglesCount;

                CombinedMeshPartInternal part = new CombinedMeshPartInternal(_root, _parts.Count,
                    vertStart, vertCount, triStart, triCount);

                _parts.Add(part);
                result[i] = part;

                _vertexCount += vertCount;
                _trianglesCount += triCount;

                OnAddPart(part, mesh, transform);
            }

            OnMeshUpdated();

            return result;
        }

        public void RemoveParts(IList<CombinedMeshPart> parts)
        {
            for (int c = 0; c < parts.Count; c++)
            {
                CombinedMeshPartInternal part = (CombinedMeshPartInternal)parts[c];

                OnRemovePart(part);

                int partIdx = _parts.IndexOf(part);

                int vertOffet = part.VertexCount;
                int triOffset = part.TrianglesCount;

                for (int i = partIdx + 1; i < _parts.Count; i++)
                {
                    CombinedMeshPartInternal current = _parts[i];

                    int newVertStart = current.VertexStart - vertOffet;
                    int newTriStart = current.TrianglesStart - triOffset;

                    current.Offset(current.Index - 1, newVertStart, newTriStart);
                }

                _parts.Remove(part);

                _vertexCount -= part.VertexCount;
                _trianglesCount -= part.TrianglesCount;
            }

            OnMeshUpdated();
        }


        protected virtual void OnInitialized()
        {
            _partsBounds = new List<Bounds>();
        }

        protected virtual void OnAddPart(CombinedMeshPart part, Mesh mesh, Matrix4x4 transform)
        {
            _partsBounds.Add(mesh.bounds.Transform(transform));
        }

        protected virtual void OnRemovePart(CombinedMeshPart part)
        {
            _partsBounds.RemoveAt(part.Index);
        }

        protected virtual void OnMeshUpdated()
        {
            _boundingBox = _root.Mesh.bounds;
        }

        protected virtual void OnDispose()
        {
            
        }
    }
}
