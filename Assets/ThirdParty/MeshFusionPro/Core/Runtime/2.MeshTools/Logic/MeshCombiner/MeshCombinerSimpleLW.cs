using System.Collections.Generic;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public class MeshCombinerSimpleLW : MeshCombinerBase
    {
        private MeshCombinerSimpleSTD _stdCombiner;

        public MeshCombinerSimpleLW()
        {
            _stdCombiner = new MeshCombinerSimpleSTD();
        }

        protected override void CombineInternal(Mesh mesh, IList<MeshCombineInfo> infos)
        {
            VertexBufferUtil.ToStandardBuffer(mesh);

            try
            {
                _stdCombiner.Combine(mesh, infos);
                VertexBufferUtil.ToLightweightBuffer(mesh);
            }
            catch
            {
                VertexBufferUtil.ToLightweightBuffer(mesh);
                throw;
            }
        }
    }
}
