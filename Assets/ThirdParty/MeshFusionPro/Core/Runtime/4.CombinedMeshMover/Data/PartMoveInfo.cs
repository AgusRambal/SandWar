using UnityEngine;

namespace NGS.MeshFusionPro
{
    public struct PartMoveInfo
    {
        public int partIndex;
        public int vertexStart;
        public int vertexCount;
        public Matrix4x4 currentTransform;
        public Matrix4x4 targetTransform;
    }
}
