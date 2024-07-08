namespace NGS.MeshFusionPro
{
    public class SimpleToolsSTDFactory : IMeshToolsFactory
    {
        public IMeshCombiner CreateMeshCombiner()
        {
            return new MeshCombinerSimpleSTD();
        }

        public IMeshCutter CreateMeshCutter()
        {
            return new MeshCutterSimpleSTD();
        }
    }
}
