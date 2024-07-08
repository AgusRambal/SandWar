using System.Collections.Generic;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public interface IMeshCombiner
    {
        void Combine(Mesh mesh, IList<MeshCombineInfo> combineInfos);
    }
}
