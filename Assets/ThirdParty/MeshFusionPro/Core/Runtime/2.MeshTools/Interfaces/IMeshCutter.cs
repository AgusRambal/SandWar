using System.Collections.Generic;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public interface IMeshCutter
    {
        public void Cut(Mesh mesh, MeshCuttingInfo cuttingInfo);

        public void Cut(Mesh mesh, IList<MeshCuttingInfo> cuttingInfos);
    }
}
