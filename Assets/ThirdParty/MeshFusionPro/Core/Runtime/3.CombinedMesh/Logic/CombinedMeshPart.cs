namespace NGS.MeshFusionPro
{
    public class CombinedMeshPart
    {
        public CombinedMesh Root { get; private set; }

        public int Index { get; protected set; }

        public int VertexStart { get; protected set; }
        public int VertexCount { get; protected set; }

        public int TrianglesStart { get; protected set; }
        public int TrianglesCount { get; protected set; }

        public CombinedMeshPart(CombinedMesh root, int index,
            int vertexStart, int vertexCount, int trianglesStart, int trianglesCount)
        {
            Root = root;
            Index = index;

            VertexStart = vertexStart;
            VertexCount = vertexCount;

            TrianglesStart = trianglesStart;
            TrianglesCount = trianglesCount;
        }
    }

    public class CombinedMeshPartInternal : CombinedMeshPart
    {
        public CombinedMeshPartInternal(CombinedMesh root, 
            int index, 
            int vertexStart, 
            int vertexCount, 
            int trianglesStart, 
            int trianglesCount) : base(root, index, vertexStart, vertexCount, trianglesStart, trianglesCount)
        {
            
        }

        public void Offset(int newIndex, int newVertexStart, int newTrianglesStart)
        {
            Index = newIndex;

            VertexStart = newVertexStart;

            TrianglesStart = newTrianglesStart;
        }
    }
}
