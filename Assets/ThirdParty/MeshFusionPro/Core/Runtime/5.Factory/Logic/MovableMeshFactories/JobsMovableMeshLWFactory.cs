namespace NGS.MeshFusionPro
{
    public class JobsMovableMeshLWFactory : IMovableCombinedMeshFactory
    {
        private IMeshToolsFactory _tools;

        public JobsMovableMeshLWFactory(IMeshToolsFactory tools)
        {
            _tools = tools;
        }

        public CombinedMesh CreateMovableMesh(out ICombinedMeshMover mover)
        {
            CombinedMesh<MeshDataNativeArraysLW> root = new CombinedMesh<MeshDataNativeArraysLW>(
                _tools.CreateMeshCombiner(),
                _tools.CreateMeshCutter());

            mover = new JobsMeshMoverLW(root.MeshDataInternal);

            return root;
        }
    }
}
