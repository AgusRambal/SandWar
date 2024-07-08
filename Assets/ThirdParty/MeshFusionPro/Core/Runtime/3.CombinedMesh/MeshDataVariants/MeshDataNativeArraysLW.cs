using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public class MeshDataNativeArraysLW : CombinedMeshDataInternal
    {
        public NativeArray<LightweightVertex> Vertices { get { return _vertices; } }
        public NativeArray<Bounds> PartsBounds { get { return _partsBounds; } }
        public NativeArray<Bounds> Bounds { get { return _bounds; } }

        public NativeArray<LightweightVertex> VerticesLocal { get { return _verticesLocal; } }
        public NativeArray<Bounds> PartsBoundsLocal { get { return _partsBoundsLocal; } }

        private CombinedMesh _root;

        private NativeList<LightweightVertex> _vertices;
        private NativeList<Bounds> _partsBounds;
        private NativeArray<Bounds> _bounds;

        private NativeList<LightweightVertex> _verticesLocal;
        private NativeList<Bounds> _partsBoundsLocal;

        private List<Vector3> _tempVertices;
        private List<Vector3> _tempNormals;
        private List<Vector4> _tempTangents;
        private List<Vector2> _tempUV;
        private List<Vector2> _tempUV2;


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

            mesh.SetVertexBufferData(Vertices, 0, 0, _vertices.Length);
            mesh.bounds = _bounds[0];
        }


        protected override void OnInitialized()
        {
            _root = Root;

            VertexBufferUtil.ToLightweightBuffer(_root.Mesh);

            _vertices = new NativeList<LightweightVertex>(Allocator.Persistent);
            _partsBounds = new NativeList<Bounds>(Allocator.Persistent);
            _bounds = new NativeArray<Bounds>(1, Allocator.Persistent);

            _verticesLocal = new NativeList<LightweightVertex>(Allocator.Persistent);
            _partsBoundsLocal = new NativeList<Bounds>(Allocator.Persistent);

            _tempVertices = new List<Vector3>();
            _tempNormals = new List<Vector3>();
            _tempTangents = new List<Vector4>();
            _tempUV = new List<Vector2>();
            _tempUV2 = new List<Vector2>();
        }

        protected override void OnAddPart(CombinedMeshPart part, Mesh mesh, Matrix4x4 transform)
        {
            Bounds localBounds = mesh.bounds;
            Bounds bounds = localBounds.Transform(transform);

            _partsBounds.Add(bounds);
            _partsBoundsLocal.Add(localBounds);

            mesh.GetVertices(_tempVertices);
            mesh.GetNormals(_tempNormals);
            mesh.GetTangents(_tempTangents);
            mesh.GetUVs(0, _tempUV);
            mesh.GetUVs(1, _tempUV2);

            int vertexCount = mesh.vertexCount;

            for (int i = 0; i < vertexCount; i++)
            {
                LightweightVertex vertex = new LightweightVertex();

                vertex.Position = _tempVertices[i];
                vertex.Normal = _tempNormals[i];
                vertex.Tangent = _tempTangents[i];

                if (_tempUV.Count > 0)
                    vertex.UV = _tempUV[i];

                if (_tempUV2.Count > 0)
                    vertex.UV2 = _tempUV2[i];

                _verticesLocal.Add(vertex);
            }
        }

        protected override void OnRemovePart(CombinedMeshPart part)
        {
            _partsBounds.RemoveAt(part.Index);
            _verticesLocal.RemoveRange(part.VertexStart, part.VertexCount);
            _partsBoundsLocal.RemoveAt(part.Index);
        }

        protected override void OnMeshUpdated()
        {
            Mesh mesh = _root.Mesh;

            using (var meshDataArray = Mesh.AcquireReadOnlyMeshData(mesh))
            {
                Mesh.MeshData meshData = meshDataArray[0];

                _vertices.Clear();
                _vertices.AddRange(meshData.GetVertexData<LightweightVertex>());
            }

            _bounds[0] = mesh.bounds;
        }

        protected override void OnDispose()
        {
            _vertices.Dispose();
            _partsBounds.Dispose();
            _bounds.Dispose();

            _verticesLocal.Dispose();
            _partsBoundsLocal.Dispose();
        }
    }
}