using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace NGS.MeshFusionPro
{
    public class MeshSeparatorSimple
    {
        private const int MAX_UV_CHANNELS = 4;

        private static Dictionary<Mesh, Mesh[]> _meshToSubmeshes;

        private static List<Vector3> _srcVertices;
        private static List<Vector3> _srcNormals;
        private static List<Vector4> _srcTangents;
        private static List<Vector2> _srcUV;

        private static List<int> _triangles;
        private static List<Vector3> _vertices;
        private static List<Vector3> _normals;
        private static List<Vector4> _tangents;
        private static List<Vector2> _uv;


        static MeshSeparatorSimple()
        {
            _meshToSubmeshes = new Dictionary<Mesh, Mesh[]>();

            _srcVertices = new List<Vector3>();
            _srcNormals = new List<Vector3>();
            _srcTangents = new List<Vector4>();
            _srcUV = new List<Vector2>();

            _triangles = new List<int>();
            _vertices = new List<Vector3>();
            _normals = new List<Vector3>();
            _tangents = new List<Vector4>();
            _uv = new List<Vector2>();
        }

        public Mesh GetSubmesh(Mesh source, int submesh)
        {
            Mesh[] submeshes;

            if (!_meshToSubmeshes.TryGetValue(source, out submeshes))
            {
                submeshes = Separate(source);

                _meshToSubmeshes.Add(source, submeshes);
            }

            return submeshes[submesh];
        }


        private Mesh[] Separate(Mesh mesh)
        {
            int subMeshCount = mesh.subMeshCount;

            Mesh[] separated = new Mesh[subMeshCount];

            CollectMeshData(mesh);

            for (int i = 0; i < subMeshCount; i++)
                separated[i] = CreateFromSubmesh(mesh, i);

            ClearData();

            return separated;
        }

        private void CollectMeshData(Mesh mesh)
        {
            mesh.GetVertices(_srcVertices);
            mesh.GetNormals(_srcNormals);
            mesh.GetTangents(_srcTangents);
        }

        private Mesh CreateFromSubmesh(Mesh mesh, int submesh)
        {
            SubMeshDescriptor desc = mesh.GetSubMesh(submesh);

            Mesh result = new Mesh();

            int trianglesCount = desc.indexCount;
            int vertexCount = desc.vertexCount;
            int vertexStart = desc.firstVertex;
            int vertexEnd = vertexStart + vertexCount;

            _vertices.Clear();
            _normals.Clear();
            _tangents.Clear();

            mesh.GetIndices(_triangles, submesh);

            for (int i = vertexStart; i < vertexEnd; i++)
            {
                _vertices.Add(_srcVertices[i]);
                _normals.Add(_srcNormals[i]);
                _tangents.Add(_srcTangents[i]);
            }

            for (int i = 0; i < trianglesCount; i++)
            {
                _triangles[i] -= vertexStart;
            }

            result.SetVertices(_vertices);
            result.SetNormals(_normals);
            result.SetTangents(_tangents);
            result.SetTriangles(_triangles, 0, false);
            result.bounds = desc.bounds;

            for (int i = 0; i < MAX_UV_CHANNELS; i++)
            {
                mesh.GetUVs(i, _srcUV);

                if (_srcUV.Count == 0)
                    continue;

                _uv.Clear();

                for (int c = vertexStart; c < vertexEnd; c++)
                {
                    _uv.Add(_srcUV[c]);
                }

                result.SetUVs(i, _uv);
            }

            return result;
        }

        private void ClearData()
        {
            _srcVertices.Clear();
            _srcNormals.Clear();
            _srcTangents.Clear();
            _srcUV.Clear();

            _triangles.Clear();
            _vertices.Clear();
            _normals.Clear();
            _tangents.Clear();
            _uv.Clear();
        }
    }
}
