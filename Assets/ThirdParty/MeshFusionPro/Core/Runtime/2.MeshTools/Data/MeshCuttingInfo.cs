namespace NGS.MeshFusionPro
{
    public struct MeshCuttingInfo
    {
        public int vertexStart;
        public int vertexCount;
        public int triangleStart;
        public int triangleCount;

        public MeshCuttingInfo(int vertexStart, int vertexCount, int triangleStart, int triangleCount)
        {
            this.vertexStart = vertexStart;
            this.vertexCount = vertexCount;
            this.triangleStart = triangleStart;
            this.triangleCount = triangleCount;
        }
    }
}
