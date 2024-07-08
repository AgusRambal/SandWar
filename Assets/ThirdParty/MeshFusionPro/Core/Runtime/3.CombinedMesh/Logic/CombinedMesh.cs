using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace NGS.MeshFusionPro
{
    public class CombinedMesh : IDisposable
    {
        public Mesh Mesh
        {
            get 
            { 
                return _mesh; 
            }
        }
        public CombinedMeshData MeshData
        {
            get
            {
                return _meshData;
            }
        }

        private Mesh _mesh;
        private CombinedMeshDataInternal _meshData;

        private IMeshCombiner _combiner;
        private IMeshCutter _cutter;


        public CombinedMesh(IMeshCombiner combiner, IMeshCutter cutter)
        {
            if (combiner == null)
                throw new ArgumentNullException("MeshCombiner not assigned");

            if (cutter == null)
                throw new ArgumentNullException("MeshCutter not assigned");

            _mesh = new Mesh();
            _mesh.MarkDynamic();

            _combiner = combiner;
            _cutter = cutter;

            _meshData = CreateMeshData();
            _meshData.Initialize(this);
        }

        public CombinedMeshPart[] Combine(IList<MeshCombineInfo> infos)
        {
            _combiner.Combine(_mesh, infos);

            return _meshData.CreateParts(infos);
        }

        public void Cut(IList<CombinedMeshPart> parts)
        {
            MeshCuttingInfo[] cuttingInfos = new MeshCuttingInfo[parts.Count];

            for (int i = 0; i < parts.Count; i++)
            {
                CombinedMeshPart part = parts[i];

                cuttingInfos[i] = new MeshCuttingInfo()
                {
                    vertexStart = part.VertexStart,
                    vertexCount = part.VertexCount,
                    triangleStart = part.TrianglesStart,
                    triangleCount = part.TrianglesCount
                };
            }

            _cutter.Cut(_mesh, cuttingInfos);
            _meshData.RemoveParts(parts);
        }

        public void Dispose()
        {
            _meshData.Dispose();
        }


        protected virtual CombinedMeshDataInternal CreateMeshData()
        {
            return new CombinedMeshDataInternal();
        }
    }

    public class CombinedMesh<TCombinedMeshData> : CombinedMesh
        where TCombinedMeshData : CombinedMeshDataInternal, new()
    {
        public TCombinedMeshData MeshDataInternal
        {
            get
            {
                return (TCombinedMeshData)MeshData;
            }
        }

        public CombinedMesh(IMeshCombiner combiner, IMeshCutter cutter)
            : base(combiner, cutter)
        {

        }

        protected override CombinedMeshDataInternal CreateMeshData()
        {
            return new TCombinedMeshData();
        }
    }
}
