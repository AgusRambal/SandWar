using System.Collections.Generic;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public class SimpleMeshMoverSTD : ICombinedMeshMover
    {
        private MeshDataListsSTD _meshData;

        public SimpleMeshMoverSTD(MeshDataListsSTD meshData)
        {
            _meshData = meshData;
        }

        public void MoveParts(IList<PartMoveInfo> moveInfos)
        {
            List<Vector3> vertices = _meshData.Vertices;
            List<Vector3> normals = _meshData.Normals;
            List<Vector4> tangents = _meshData.Tangents;
            List<Bounds> bounds = _meshData.PartsBounds;
            List<Bounds> localBounds = _meshData.PartsBoundsLocal;

            Bounds boundingBox = _meshData.GetBounds();
            boundingBox.size = Vector3.zero;

            for (int c = 0; c < moveInfos.Count; c++)
            {
                PartMoveInfo info = moveInfos[c];

                int partIdx = info.partIndex;

                int start = info.vertexStart;
                int end = start + info.vertexCount;

                Matrix4x4 transform = info.targetTransform;
                Matrix4x4 inverse = info.currentTransform.inverse;

                for (int i = start; i < end; i++)
                {
                    Vector3 vertex = vertices[i];
                    Vector3 normal = normals[i];
                    Vector4 tangent = tangents[i];
                    float tanW = tangent.w;

                    vertex = inverse.MultiplyPoint3x4(vertex);
                    vertex = transform.MultiplyPoint3x4(vertex);

                    normal = inverse.MultiplyVector(normal);
                    normal = transform.MultiplyVector(normal);

                    tangent = inverse.MultiplyVector(tangent);
                    tangent = transform.MultiplyVector(tangent);
                    tangent.w = tanW;

                    vertices[i] = vertex;
                    normals[i] = normal;
                    tangents[i] = tangent;
                }

                bounds[partIdx] = localBounds[partIdx].Transform(transform);
            }

            for (int i = 0; i < bounds.Count; i++)
                boundingBox.Encapsulate(bounds[i]);

            _meshData.Bounds = boundingBox;
        }

        public void ApplyData()
        {
            _meshData.ApplyDataToMesh();
        }
    }
}
