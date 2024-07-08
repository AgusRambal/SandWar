namespace NGS.MeshFusionPro
{
    public interface IMeshToolsFactory
    {
        public IMeshCombiner CreateMeshCombiner();

        public IMeshCutter CreateMeshCutter();
    }
}
