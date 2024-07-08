namespace NGS.MeshFusionPro
{
    public class SimpleMovableMeshSTDFactory : IMovableCombinedMeshFactory
    {
        private IMeshToolsFactory _tools;

        public SimpleMovableMeshSTDFactory(IMeshToolsFactory tools) 
        {
            _tools = tools;
        }

        public CombinedMesh CreateMovableMesh(out ICombinedMeshMover mover)
        {
            CombinedMesh<MeshDataListsSTD> root = new CombinedMesh<MeshDataListsSTD>(
                _tools.CreateMeshCombiner(),
                _tools.CreateMeshCutter());

            mover = new SimpleMeshMoverSTD(root.MeshDataInternal);

            return root;
        }
    }
}
