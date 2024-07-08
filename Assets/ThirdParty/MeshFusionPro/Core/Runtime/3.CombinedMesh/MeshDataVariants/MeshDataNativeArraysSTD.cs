using System;
using Unity.Collections;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public class MeshDataNativeArraysSTD : CombinedMeshDataInternal
    {
        public NativeArray<Vector3> Vertices { get { return _vertices; } }
        public NativeArray<Vector3> Normals { get { return _normals; } }
        public NativeArray<Vector4> Tangents { get { return _tangents; } }
        public NativeArray<Bounds> PartsBounds { get { return _partsBounds; } }
        public NativeArray<Bounds> PartsBoundsLocal { get { return _partsBoundsLocal; } }
        public NativeArray<Bounds> Bounds { get { return _bounds; } }

        private CombinedMesh _root;
        private NativeList<Vector3> _vertices;
        private NativeList<Vector3> _normals;
        private NativeList<Vector4> _tangents;
        private NativeList<Bounds> _partsBounds;
        private NativeList<Bounds> _partsBoundsLocal;
        private NativeArray<Bounds> _bounds;


        public override Bounds GetBounds()
        {
            return _bounds[0];
        }

        public override Bounds GetBounds(CombinedMeshPart part)
        {
            return _partsBounds[part.Index];
        }

        public void ApplyDataToMesh()
        {
            Mesh mesh = _root.Mesh;

            mesh.SetVertices(Vertices);
            mesh.SetNormals(Normals);
            mesh.SetTangents(Tangents);

            mesh.bounds = _bounds[0];
        }


        protected override void OnInitialized()
        {
            _root = Root;
            _vertices = new NativeList<Vector3>(Allocator.Persistent);
            _normals = new NativeList<Vector3>(Allocator.Persistent);
            _tangents = new NativeList<Vector4>(Allocator.Persistent);
            _partsBounds = new NativeList<Bounds>(Allocator.Persistent);
            _partsBoundsLocal = new NativeList<Bounds>(Allocator.Persistent);
            _bounds = new NativeArray<Bounds>(1, Allocator.Persistent);
        }

        protected override void OnAddPart(CombinedMeshPart part, Mesh mesh, Matrix4x4 transform)
        {
            Bounds localBounds = mesh.bounds;
            Bounds bounds = localBounds.Transform(transform);

            _partsBounds.Add(bounds);
            _partsBoundsLocal.Add(localBounds);
        }

        protected override void OnRemovePart(CombinedMeshPart part)
        {
            _partsBounds.RemoveAt(part.Index);
            _partsBoundsLocal.RemoveAt(part.Index);
        }

        protected override void OnMeshUpdated()
        {
            Mesh mesh = _root.Mesh;
            int length = mesh.vertexCount;

            _vertices.ResizeUninitialized(length);
            _normals.ResizeUninitialized(length);
            _tangents.ResizeUninitialized(length);

            using (var meshDataArray = Mesh.AcquireReadOnlyMeshData(mesh))
            {
                Mesh.MeshData meshData = meshDataArray[0];

                meshData.GetVertices(_vertices);
                meshData.GetNormals(_normals);
                meshData.GetTangents(_tangents);
            }

            _bounds[0] = mesh.bounds;
        }

        protected override void OnDispose()
        {
            _vertices.Dispose();
            _normals.Dispose();
            _tangents.Dispose();
            _partsBounds.Dispose();
            _partsBoundsLocal.Dispose();
            _bounds.Dispose();
        }
    }
}
