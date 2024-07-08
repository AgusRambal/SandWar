namespace NGS.MeshFusionPro
{
    public class SimpleToolsLWFactory : IMeshToolsFactory
    {
        public IMeshCombiner CreateMeshCombiner()
        {
            return new MeshCombinerSimpleLW();
        }

        public IMeshCutter CreateMeshCutter()
        {
            return new MeshCutterSimpleLW();
        }
    }
}
