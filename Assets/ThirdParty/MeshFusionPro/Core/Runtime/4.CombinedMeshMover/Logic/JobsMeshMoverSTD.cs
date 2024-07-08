using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public class JobsMeshMoverSTD : IAsyncCombinedMeshMover, IDisposable
    {
        private MeshDataNativeArraysSTD _meshData;

        private NativeList<PartMoveInfo> _moveInfos;
        private JobHandle _handle;


        public JobsMeshMoverSTD(MeshDataNativeArraysSTD meshData)
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

            MovePartsJob transformationJob = new MovePartsJob()
            {
                vertices = _meshData.Vertices,
                normals = _meshData.Normals,
                tangents = _meshData.Tangents,
                bounds = _meshData.PartsBounds,
                localBounds = _meshData.PartsBoundsLocal,
                moveInfos = _moveInfos
            };

            RecalculateBoundsJob recalculateJob = new RecalculateBoundsJob()
            {
                bounds = _meshData.PartsBounds,
                boundingBox = meshBounds
            };

            _handle = recalculateJob.Schedule(
                transformationJob.Schedule(_moveInfos.Length, 4));
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
            public NativeArray<Vector3> vertices;

            [NativeDisableParallelForRestriction]
            public NativeArray<Vector3> normals;

            [NativeDisableParallelForRestriction]
            public NativeArray<Vector4> tangents;

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

                Matrix4x4 transformation = data.targetTransform;
                Matrix4x4 inverse = data.currentTransform.inverse;

                for (int i = start; i < end; i++)
                {
                    Vector3 vertex = vertices[i];
                    Vector3 normal = normals[i];
                    Vector4 tangent = tangents[i];
                    float tanW = tangent.w;

                    vertex = inverse.MultiplyPoint3x4(vertex);
                    vertex = transformation.MultiplyPoint3x4(vertex);

                    normal = inverse.MultiplyVector(normal);
                    normal = transformation.MultiplyVector(normal);

                    tangent = inverse.MultiplyVector(tangent);
                    tangent = transformation.MultiplyVector(tangent);
                    tangent.w = tanW;

                    vertices[i] = vertex;
                    normals[i] = normal;
                    tangents[i] = tangent;
                }

                bounds[partIdx] = localBounds[partIdx].Transform(transformation);
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

                for (int i = 0; i < bounds.Length; i++)
                    bb.Encapsulate(bounds[i]);

                boundingBox[0] = bb;
            }
        }
    }
}
