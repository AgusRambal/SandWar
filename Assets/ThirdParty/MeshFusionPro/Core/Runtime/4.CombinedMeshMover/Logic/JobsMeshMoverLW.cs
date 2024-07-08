using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public class JobsMeshMoverLW : IAsyncCombinedMeshMover, IDisposable
    {
        private MeshDataNativeArraysLW _meshData;

        private NativeList<PartMoveInfo> _moveInfos;
        private JobHandle _handle;


        public JobsMeshMoverLW(MeshDataNativeArraysLW meshData)
        {
            _meshData = meshData;
            _moveInfos = new NativeList<PartMoveInfo>(Allocator.Persistent);
        }

        public void MoveParts(IList<PartMoveInfo> moveInfos)
        {
            MovePartsAsync(moveInfos);
            FinishAsyncMoving();
        }

        public void MovePartsAsync(IList<PartMoveInfo> moveInfos)
        {
            NativeArray<Bounds> meshBounds = _meshData.Bounds;
            meshBounds[0] = new Bounds(_meshData.GetBounds().center, Vector3.zero);

            _moveInfos.Clear();

            for (int i = 0; i < moveInfos.Count; i++)
                _moveInfos.Add(moveInfos[i]);

            MovePartsJob movePartsJob = new MovePartsJob()
            {
                vertices = _meshData.Vertices,
                bounds = _meshData.PartsBounds,
                localVertices = _meshData.VerticesLocal,
                localBounds = _meshData.PartsBoundsLocal,
                moveInfos = _moveInfos
            };

            RecalculateBoundsJob recalculateBoundsJob = new RecalculateBoundsJob
            {
                bounds = _meshData.PartsBounds,
                boundingBox = meshBounds
            };

            _handle = recalculateBoundsJob.Schedule(movePartsJob.Schedule(_moveInfos.Length, 4));
        }

        public void FinishAsyncMoving()
        {
            _handle.Complete();
        }

        public void ApplyData()
        {
            if (!_handle.IsCompleted)
                FinishAsyncMoving();

            _meshData.ApplyDataToMesh();
        }

        public void Dispose()
        {
            _moveInfos.Dispose();
        }


        [BurstCompile]
        private struct MovePartsJob : IJobParallelFor
        {
            [NativeDisableParallelForRestriction]
            public NativeArray<LightweightVertex> vertices;

            [ReadOnly]
            public NativeArray<LightweightVertex> localVertices;

            [WriteOnly]
            [NativeDisableParallelForRestriction]
            public NativeArray<Bounds> bounds;

            [ReadOnly]
            [NativeDisableParallelForRestriction]
            public NativeArray<Bounds> localBounds;

            [ReadOnly]
            public NativeList<PartMoveInfo> moveInfos;

            public void Execute(int idx)
            {
                PartMoveInfo data = moveInfos[idx];

                int partIdx = data.partIndex;
                int start = data.vertexStart;
                int end = start + data.vertexCount;

                Matrix4x4 transform = data.targetTransform;

                for (int i = start; i < end; i++)
                {
                    LightweightVertex vertexLocal = localVertices[i];

                    float tanW = vertexLocal.tanW;

                    Vector3 normal = transform.MultiplyVector(vertexLocal.Normal).normalized;
                    Vector4 tangent = transform.MultiplyVector(vertexLocal.Tangent).normalized;
                    tangent.w = tanW;

                    LightweightVertex vertex = new LightweightVertex
                    {
                        Position = transform.MultiplyPoint3x4(vertexLocal.Position),
                        Normal = normal,
                        Tangent = tangent,
                        uv = vertexLocal.uv,
                        uv2 = vertexLocal.uv2
                    };

                    vertices[i] = vertex;
                }

                bounds[partIdx] = localBounds[partIdx].Transform(transform);
            }
        }

        [BurstCompile]
        private struct RecalculateBoundsJob : IJob
        {
            public NativeArray<Bounds> bounds;
            public NativeArray<Bounds> boundingBox;

            public void Execute()
            {
                Bounds bb = boundingBox[0];

                bb.size = Vector3.zero;

                for (int i = 0; i < bounds.Length; i++)
                    bb.Encapsulate(bounds[i]);

                boundingBox[0] = bb;
            }
        }
    }
}
