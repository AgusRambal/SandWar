using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public class MeshCutterSimpleSTD : IMeshCutter
    {
        private static List<Vector3> _vertices;
        private static List<Vector3> _normals;
        private static List<Vector4> _tangents;
        private static List<int> _triangles;
        private static List<Vector2>[] _uvs;
        private static int _maxUVsCount = 4;

        static MeshCutterSimpleSTD()
        {
            _vertices = new List<Vector3>();
            _normals = new List<Vector3>();
            _tangents = new List<Vector4>();
            _triangles = new List<int>();
            _uvs = new List<Vector2>[_maxUVsCount];

            for (int i = 0; i < _maxUVsCount; i++)
                _uvs[i] = new List<Vector2>();
        }

        public void Cut(Mesh mesh, MeshCuttingInfo info)
        {
            Cut(mesh, new[] { info });
        }

        public void Cut(Mesh mesh, IList<MeshCuttingInfo> infos)
        {
            ValidateMeshOrThrowException(mesh);

            CollectData(mesh);

            foreach (var info in infos.OrderByDescending(i => i.triangleStart))
            {
                RemoveData(info);
                OffsetTriangles(info);
            }

            ApplyDataToMesh(mesh);

            ClearData();
        }


        private void ValidateMeshOrThrowException(Mesh mesh)
        {
            if (mesh == null)
                throw new ArgumentNullException("mesh is null");

            if (mesh.subMeshCount > 1)
                throw new ArgumentException("SimpleMeshCutter::'mesh' should has only 1 submesh");
        }

        private void CollectData(Mesh mesh)
        {
            mesh.GetVertices(_vertices);
            mesh.GetNormals(_normals);
            mesh.GetTangents(_tangents);
            mesh.GetTriangles(_triangles, 0);

            for (int i = 0; i < _maxUVsCount; i++)
                mesh.GetUVs(i, _uvs[i]);
        }

        private void RemoveData(MeshCuttingInfo cuttingInfo)
        {
            int start = cuttingInfo.vertexStart;
            int count = cuttingInfo.vertexCount;

            _vertices.RemoveRange(start, count);

            if (_normals.Count > 0)
                _normals.RemoveRange(start, count);

            if (_tangents.Count > 0)
                _tangents.RemoveRange(start, count);

            _triangles.RemoveRange(cuttingInfo.triangleStart, cuttingInfo.triangleCount);

            for (int c = 0; c < _uvs.Length; c++)
            {
                if (_uvs[c].Count > 0)
                    _uvs[c].RemoveRange(start, count);
            }
        }

        private void OffsetTriangles(MeshCuttingInfo info)
        {
            int offset = info.vertexCount;

            int start = info.triangleStart;
            int count = _triangles.Count;

            for (int i = start; i < count; i++)
                _triangles[i] -= offset;
        }

        private void ApplyDataToMesh(Mesh mesh)
        {
            mesh.SetTriangles(_triangles, 0);
            mesh.SetVertices(_vertices);
            mesh.SetNormals(_normals);
            mesh.SetTangents(_tangents);

            for (int i = 0; i < _maxUVsCount; i++)
            {
                if (_uvs[i].Count > 0)
                    mesh.SetUVs(i, _uvs[i]);
            }
        }

        private void ClearData()
        {
            _vertices.Clear();
            _normals.Clear();
            _tangents.Clear();
            _triangles.Clear();

            for (int i = 0; i < _maxUVsCount; i++)
                _uvs[i].Clear();
        }
    }
}
