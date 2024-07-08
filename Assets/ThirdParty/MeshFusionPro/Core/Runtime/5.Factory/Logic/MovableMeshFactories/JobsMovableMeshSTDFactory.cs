namespace NGS.MeshFusionPro
{
    public class JobsMovableMeshSTDFactory : IMovableCombinedMeshFactory
    {
        private IMeshToolsFactory _tools;

        public JobsMovableMeshSTDFactory(IMeshToolsFactory tools)
        {
            _tools = tools;
        }

        public CombinedMesh CreateMovableMesh(out ICombinedMeshMover mover)
        {
            CombinedMesh<MeshDataNativeArraysSTD> root = new CombinedMesh<MeshDataNativeArraysSTD>(
                _tools.CreateMeshCombiner(),
                _tools.CreateMeshCutter());

            mover = new JobsMeshMoverSTD(root.MeshDataInternal);

            return root;
        }
    }
}
