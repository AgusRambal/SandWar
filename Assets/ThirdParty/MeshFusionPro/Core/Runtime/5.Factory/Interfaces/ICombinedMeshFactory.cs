using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public enum MeshType { Standard, Lightweight }
    public enum CombineMethod { Simple }
    public enum MoveMethod { Simple, Jobs }

    public interface ICombinedMeshFactory
    {
        public CombinedMesh CreateCombinedMesh();

        public CombinedMesh CreateMovableCombinedMesh(out ICombinedMeshMover mover);
    }
}
