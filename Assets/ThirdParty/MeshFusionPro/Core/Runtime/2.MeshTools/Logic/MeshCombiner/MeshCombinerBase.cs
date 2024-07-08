using System;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public abstract class MeshCombinerBase : IMeshCombiner
    {
        public void Combine(Mesh mesh, IList<MeshCombineInfo> infos)
        {
            ValidateMesh(mesh);
            ValidateCombineInfos(infos);

            CombineInternal(mesh, infos);
        }

        protected abstract void CombineInternal(Mesh mesh, IList<MeshCombineInfo> infos);


        protected virtual void ValidateMesh(Mesh mesh)
        {
            if (mesh == null)
                throw new ArgumentNullException("mesh");

            if (mesh.subMeshCount > 1)
                throw new ArgumentException("BaseMeshCombiner::input 'mesh' should have 1 submesh");
        }

        protected virtual void ValidateCombineInfos(IList<MeshCombineInfo> infos)
        {
            if (infos == null)
                throw new ArgumentNullException("MeshCombineInfos is null");

            for (int i = 0; i < infos.Count; i++)
            {
                if (infos[i].mesh == null)
                    throw new ArgumentNullException("MeshCombineInfo.mesh is null");
            }
        }
    }
}
