using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public class DynamicCombinedObject : MonoBehaviour, ICombinedObject<DynamicCombinedObjectPart, DynamicCombineSource>
    {
        IReadOnlyList<ICombinedObjectPart> ICombinedObject.Parts
        {
            get
            {
                return _parts;
            }
        }
        public IReadOnlyList<DynamicCombinedObjectPart> Parts
        {
            get
            {
                return _parts;
            }
        }
        public RendererSettings RendererSettings
        {
            get
            {
                return _baseObject.RendererSettings;
            }
        }

        public Bounds Bounds
        {
            get
            {
                return _baseObject.Bounds;
            }
        }
        public Bounds LocalBounds
        {
            get
            {
                return _baseObject.LocalBounds;
            }
        }
        public int VertexCount
        {
            get
            {
                return _baseObject.VertexCount;
            }
        }

        private CombinedObject _baseObject;
        private List<DynamicCombinedObjectPart> _parts;

        private ICombinedMeshMover _meshMover;

        private List<DynamicCombinedObjectPartInternal> _movedParts;
        private List<PartMoveInfo> _movedPartsData;

        private bool _partsMoved;


        private void Update()
        {
            if (_movedParts.Count > 0)
            {
                if (_meshMover is IAsyncCombinedMeshMover asyncMover)
                {
                    asyncMover.MovePartsAsync(_movedPartsData);
                }
                else
                {
                    _meshMover.MoveParts(_movedPartsData);
                }

                for (int i = 0; i < _movedParts.Count; i++)
                    _movedParts[i].PositionUpdated();

                _movedParts.Clear();
                _movedPartsData.Clear();

                _partsMoved = true;
            }
        }

        private void LateUpdate()
        {
            if (_partsMoved)
            {
                if (_meshMover is IAsyncCombinedMeshMover asyncMover)
                    asyncMover.FinishAsyncMoving();

                _meshMover.ApplyData();

                _partsMoved = false;
            }

            _baseObject.ForceUpdate();

            if (_movedParts.Count == 0)
                enabled = false;
        }

        private void OnDestroy()
        {
            if (_meshMover is IDisposable d)
                d.Dispose();
        }


        public static DynamicCombinedObject Create(MeshType meshType, CombineMethod combineMethod,
            MoveMethod moveMethod, RendererSettings settings)
        {
            ICombinedMeshFactory factory =
                new CombinedMeshFactory(meshType, combineMethod, moveMethod);

            return Create(factory, settings);
        }

        public static DynamicCombinedObject Create(ICombinedMeshFactory factory, RendererSettings settings)
        {
            ICombinedMeshMover mover;
            CombinedMesh combinedMesh = factory.CreateMovableCombinedMesh(out mover);

            CombinedObject baseObject = CombinedObject.Create(combinedMesh, settings);

            DynamicCombinedObject dynamicObject = baseObject.gameObject.AddComponent<DynamicCombinedObject>();
            dynamicObject.Construct(baseObject, mover);

            return dynamicObject;
        }

        private void Construct(CombinedObject baseObject, ICombinedMeshMover mover)
        {
            gameObject.name = "Dynamic Combined Object";

            _baseObject = baseObject;
            _meshMover = mover;

            _parts = new List<DynamicCombinedObjectPart>();
            _movedParts = new List<DynamicCombinedObjectPartInternal>();
            _movedPartsData = new List<PartMoveInfo>();

            _baseObject.Updating = false;
        }


        public void Combine(IEnumerable<ICombineSource> sources)
        {
            Combine(sources.Select(s => (DynamicCombineSource)s));
        }

        public void Combine(IEnumerable<DynamicCombineSource> sources)
        {
            int sourcesCount = sources.Count();

            CombineSource[] baseSources = new CombineSource[sourcesCount];
            DynamicCombinedObjectPart[] result = new DynamicCombinedObjectPart[sourcesCount];

            int idx = 0;
            foreach (var source in sources)
            {
                CombineSource baseSource = source.Base;

                baseSources[idx++] = baseSource;

                baseSource.onCombinedTyped += (root, part) =>
                {
                    DynamicCombinedObjectPart dynamicPart = new DynamicCombinedObjectPartInternal(this, part,
                        baseSource.CombineInfo.transformMatrix);

                    _parts.Add(dynamicPart);

                    source.Combined(this, dynamicPart);
                };

                baseSource.onCombineErrorTyped += (root, message) => source.CombineError(this, message);
                baseSource.onCombineFailedTyped += (root) => source.CombineFailed(this);
            }

            _baseObject.Combine(baseSources);
        }

        public void UpdatePart(DynamicCombinedObjectPartInternal part)
        {
            _movedParts.Add(part);
            _movedPartsData.Add(part.CreateMoveInfo());

            enabled = true;
        }

        public void Destroy(DynamicCombinedObjectPart dynamicPart, CombinedObjectPart basePart)
        {
            if (_parts.Remove(dynamicPart))
            {
                basePart.Destroy();
                enabled = true;
            }
        }
    }
}
