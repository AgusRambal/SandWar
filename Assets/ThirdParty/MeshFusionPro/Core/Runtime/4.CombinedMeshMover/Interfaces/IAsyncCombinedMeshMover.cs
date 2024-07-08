using System.Collections.Generic;

namespace NGS.MeshFusionPro
{
    public interface IAsyncCombinedMeshMover : ICombinedMeshMover
    {
        void MovePartsAsync(IList<PartMoveInfo> moveInfos);

        void FinishAsyncMoving();
    }
}