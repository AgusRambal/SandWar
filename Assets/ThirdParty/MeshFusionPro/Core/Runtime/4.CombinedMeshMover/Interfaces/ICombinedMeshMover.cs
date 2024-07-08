using System.Collections.Generic;

namespace NGS.MeshFusionPro
{
    public interface ICombinedMeshMover
    {
        void MoveParts(IList<PartMoveInfo> moveInfos);

        void ApplyData();
    }
}
