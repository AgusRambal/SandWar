using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

using Object = UnityEngine.Object;

namespace NGS.MeshFusionPro
{
    public class MeshCombinerSimpleSTD : MeshCombinerBase
    {
        private MeshSeparatorSimple _meshSeparator;

        public MeshCombinerSimpleSTD()
        {
            _meshSeparator = new MeshSeparatorSimple();
        }

        protected override void CombineInternal(Mesh mesh, IList<MeshCombineInfo> infos)
        {
            CombineInstance[] instances = new CombineInstance[infos.Count + 1];

            Mesh copy = Object.Instantiate(mesh);
            instances[0] = CreateCombineInstance(new MeshCombineInfo(copy));

            for (int i = 0; i < infos.Count; i++)
                instances[i + 1] = CreateCombineInstance(infos[i]);

            if (mesh.indexFormat == IndexFormat.UInt16)
            {
                int totalVertices = mesh.vertexCount + CalculateVertexCount(infos);

                if (totalVertices >= 65535)
                    mesh.indexFormat = IndexFormat.UInt32;
            }

            mesh.CombineMeshes(instances, true, true, true);
        }

        private CombineInstance CreateCombineInstance(MeshCombineInfo info)
        {
            Mesh mesh = info.mesh;

            if (mesh.subMeshCount > 1)
                mesh = _meshSeparator.GetSubmesh(mesh, info.submeshIndex);

            return new CombineInstance()
            {
                mesh = mesh,
                subMeshIndex = 0,
                transform = info.transformMatrix,
                lightmapScaleOffset = info.lightmapScaleOffset,
                realtimeLightmapScaleOffset = info.realtimeLightmapScaleOffset
            };
        }

        private int CalculateVertexCount(IList<MeshCombineInfo> infos)
        {
            int vertexCount = 0;

            for (int i = 0; i < infos.Count; i++)
            {
                vertexCount += infos[i].vertexCount;
            }

            return vertexCount;
        }
    }
}
