namespace NGS.MeshFusionPro
{
    public interface IMovableCombinedMeshFactory
    {
        public CombinedMesh CreateMovableMesh(out ICombinedMeshMover mover);
    }
}
