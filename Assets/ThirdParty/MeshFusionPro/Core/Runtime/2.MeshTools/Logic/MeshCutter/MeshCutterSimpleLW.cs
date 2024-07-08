using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public class MeshCutterSimpleLW : IMeshCutter
    {
        private static List<int> _triangles;

        static MeshCutterSimpleLW()
        {
            _triangles = new List<int>();
        }

        public void Cut(Mesh mesh, MeshCuttingInfo info)
        {
            Cut(mesh, new[] { info });
        }

        public void Cut(Mesh mesh, IList<MeshCuttingInfo> infos)
        {
            ValidateMeshOrThrowException(mesh);

            mesh.GetTriangles(_triangles, 0);

            using (Mesh.MeshDataArray meshDataArray = Mesh.AcquireReadOnlyMeshData(mesh))
            {
                NativeArray<LightweightVertex> readVertices = meshDataArray[0].GetVertexData<LightweightVertex>();
                NativeList<LightweightVertex> vertices = new NativeList<LightweightVertex>(Allocator.Temp);

                vertices.CopyFrom(readVertices);

                foreach (var info in infos.OrderByDescending(i => i.triangleStart))
                {
                    vertices.RemoveRange(info.vertexStart, info.vertexCount);

                    _triangles.RemoveRange(info.triangleStart, info.triangleCount);

                    OffsetTriangles(info);
                }

                mesh.SetTriangles(_triangles, 0);
                mesh.SetVertexBufferData((NativeArray<LightweightVertex>)vertices, 0, 0, vertices.Length);
            }

            _triangles.Clear();
        }


        private void ValidateMeshOrThrowException(Mesh mesh)
        {
            if (mesh == null)
                throw new ArgumentNullException("mesh");

            if (mesh.subMeshCount > 1)
                throw new ArgumentException("SimpleMeshCutter::'mesh' should has only 1 submesh");
        }

        private void OffsetTriangles(MeshCuttingInfo info)
        {
            int offset = info.vertexCount;

            int start = info.triangleStart;
            int count = _triangles.Count;

            for (int i = start; i < count; i++)
                _triangles[i] -= offset;
        }
    }
}
