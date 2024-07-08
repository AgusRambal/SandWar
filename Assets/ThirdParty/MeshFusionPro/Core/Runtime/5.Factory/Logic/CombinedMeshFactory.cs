using UnityEngine;

namespace NGS.MeshFusionPro
{

    public class CombinedMeshFactory : ICombinedMeshFactory
    {
        private IMeshToolsFactory _tools;
        private IMovableCombinedMeshFactory _movableMeshFactory;


        public CombinedMeshFactory(MeshType meshType, CombineMethod combineMethod,
            MoveMethod moveMethod = MoveMethod.Simple)
        {
            if (meshType == MeshType.Standard)
            {
                _tools = new SimpleToolsSTDFactory();

                if (moveMethod == MoveMethod.Simple)
                    _movableMeshFactory = new SimpleMovableMeshSTDFactory(_tools);
                else
                    _movableMeshFactory = new JobsMovableMeshSTDFactory(_tools);
            }
            else
            {
                _tools = new SimpleToolsLWFactory();

                if (moveMethod == MoveMethod.Simple)
                    Debug.Log("Simple mover not implemented yet for lightweight meshes. " +
                        "Jobs mover will be used instead");

                _movableMeshFactory = new JobsMovableMeshLWFactory(_tools);
            }
        }


        public CombinedMesh CreateCombinedMesh()
        {
            return new CombinedMesh(
                _tools.CreateMeshCombiner(),
                _tools.CreateMeshCutter());
        }

        public CombinedMesh CreateMovableCombinedMesh(out ICombinedMeshMover mover)
        {
            return _movableMeshFactory.CreateMovableMesh(out mover);
        }
    }
}
