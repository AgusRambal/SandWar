using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public class CombinedLODGroup : MonoBehaviour, ICombinedObject<CombinedLODGroupPart, LODGroupCombineSource>
    {
        IReadOnlyList<ICombinedObjectPart> ICombinedObject.Parts
        {
            get
            {
                return _parts;
            }
        }
        public IReadOnlyList<CombinedLODGroupPart> Parts
        {
            get
            {
                return _parts;
            }
        }
        public LODGroupSettings Settings
        {
            get
            {
                return _settings;
            }
        }
        public Bounds Bounds
        {
            get
            {
                Bounds bounds = _localBounds;

                bounds.center += transform.position;

                return bounds;
            }
        }

        private LODGroup _group;
        private List<CombinedLODGroupPart> _parts;
        private LevelOfDetailCombiner[] _levelCombiners;

        private LOD[] _lods;
        private Bounds _localBounds;
        private int _lodCount;

        private LODGroupSettings _settings;
        private bool _updateLODs;


        private void LateUpdate()
        {
            RecalculateBounds();

            _group.RecalculateBounds();

            enabled = false;
        }


        public static CombinedLODGroup Create(MeshType meshType, CombineMethod combineMethod,
            LODGroupSettings settings, int vertexLimit = 45000)
        {
            return Create(new CombinedMeshFactory(meshType, combineMethod), settings, vertexLimit);
        }

        public static CombinedLODGroup Create(ICombinedMeshFactory factory, LODGroupSettings settings,
            int vertexLimit = 45000)
        {
            GameObject go = new GameObject("CombinedLODGroup");

            CombinedLODGroup combined = go.AddComponent<CombinedLODGroup>();
            combined.Construct(settings, factory, vertexLimit);

            return combined;
        }

        private void Construct(LODGroupSettings settings, ICombinedMeshFactory factory, int vertexLimit)
        {
            if (factory == null)
                throw new ArgumentException("CombinedLODGroup::factory is null");

            _group = gameObject.AddComponent<LODGroup>();
            _parts = new List<CombinedLODGroupPart>();

            _group.fadeMode = settings.fadeMode;
            _group.animateCrossFading = settings.animateCrossFading;

            _settings = settings;
            _lodCount = _settings.lodCount;

            _levelCombiners = new LevelOfDetailCombiner[_lodCount];
            _lods = new LOD[_lodCount];

            float div = 1f / _lodCount;

            for (int i = 0; i < _settings.lodCount; i++)
            {
                _levelCombiners[i] = new LevelOfDetailCombiner(i, this, factory, vertexLimit);
                _lods[i] = new LOD
                {
                    fadeTransitionWidth = _settings.fadeTransitionsWidth[i],
                    screenRelativeTransitionHeight = 1f - (div * (i + 1)),
                    renderers = null
                };
            }

            enabled = false;
        }


        public void Combine(IEnumerable<ICombineSource> sources)
        {
            Combine(sources.Select(s => (LODGroupCombineSource)s));
        }

        public void Combine(IEnumerable<LODGroupCombineSource> sourceGroups)
        {
            if (sourceGroups == null || sourceGroups.Count() == 0)
                throw new ArgumentException("CombinedLODGroup::sources is null");

            LODGroupCombineSource[] sources = sourceGroups.ToArray();

            if (_parts.Count == 0)
                CentralizePosition(sources);

            List<CombinedObjectPart>[] parts = FillCombinersAndCreateBaseParts(sources);

            for (int i = 0; i < _lodCount; i++)
                _levelCombiners[i].Combine();

            if (_updateLODs)
            {
                UpdateLODs();
                _updateLODs = false;
            }

            CreatePartsAndNotifySources(sources, parts);

            enabled = true;
        }

        public void Destroy(CombinedLODGroupPart part, IList<CombinedObjectPart> baseParts)
        {
            if (_parts.Remove(part))
            {
                for (int i = 0; i < baseParts.Count; i++)
                    baseParts[i].Destroy();

                enabled = true;
            }
        }


        private void CentralizePosition(LODGroupCombineSource[] sources)
        {
            Vector3 position = Vector3.zero;

            for (int i = 0; i < sources.Length; i++)
                position += sources[i].Position;

            transform.position = position / sources.Length;
        }

        private List<CombinedObjectPart>[] FillCombinersAndCreateBaseParts(LODGroupCombineSource[] sourceGroups)
        {
            List<CombinedObjectPart>[] parts = new List<CombinedObjectPart>[sourceGroups.Length];

            for (int groupIdx = 0; groupIdx < sourceGroups.Length; groupIdx++)
            {
                LODGroupCombineSource sourceGroup = sourceGroups[groupIdx];

                parts[groupIdx] = new List<CombinedObjectPart>();

                for (int level = 0; level < _lodCount; level++)
                {
                    CombineSource[] lodSources = sourceGroup.BaseSources[level];

                    for (int srcIdx = 0; srcIdx < lodSources.Length; srcIdx++)
                    {
                        CombineSource source = lodSources[srcIdx];

                        int g = groupIdx;

                        source.onCombinedTyped += (o, p) => parts[g].Add(p);
                        source.onCombineErrorTyped += (root, msg) => sourceGroup.CombineError(this, msg);
                    }

                    _levelCombiners[level].AddSources(lodSources);
                }
            }

            return parts;
        }

        private void RecalculateBounds()
        {
            _localBounds = new Bounds(transform.position, Vector3.zero);

            for (int i = 0; i < _levelCombiners.Length; i++)
            {
                _localBounds.Encapsulate(_levelCombiners[i].CalculateBounds());
            }

            _localBounds.center -= transform.position;
        }

        private void UpdateLODs()
        {
            for (int i = 0; i < _lodCount; i++)
            {
                LOD lod = _lods[i];

                lod.renderers = _levelCombiners[i].GetRenderers();

                _lods[i] = lod;
            }

            _group.SetLODs(_lods);
        }

        private void CreatePartsAndNotifySources(LODGroupCombineSource[] sourceGroups,
            List<CombinedObjectPart>[] combinedParts)
        {
            for (int i = 0; i < sourceGroups.Length; i++)
            {
                LODGroupCombineSource source = sourceGroups[i];
                List<CombinedObjectPart> baseParts = combinedParts[i];

                if (baseParts.Count == 0)
                {
                    source.CombineFailed(this);
                    continue;
                }

                CombinedLODGroupPart part = new CombinedLODGroupPart(this, baseParts);

                _parts.Add(part);

                source.Combined(this, part);
            }
        }


        private class LevelOfDetailCombiner : StaticObjectsCombiner
        {
            private Transform _transform;

            private CombinedLODGroup _group;
            private Renderer[] _renderers;
            private int _level;

            public LevelOfDetailCombiner(int level, CombinedLODGroup group, ICombinedMeshFactory factory,
                int vertexLimit) : base(factory, vertexLimit)
            {
                _level = level;
                _group = group;

                _transform = new GameObject("LOD" + _level).transform;
                _transform.parent = group.transform;
                _transform.localPosition = Vector3.zero;
            }

            public Renderer[] GetRenderers()
            {
                if (_renderers == null || _renderers.Length != CombinedObjects.Count)
                {
                    UpdateRenderersList();
                }

                return _renderers;
            }

            public Bounds CalculateBounds()
            {
                Bounds bounds = new Bounds(_group.transform.position, Vector3.zero);

                GetRenderers();

                for (int i = 0; i < _renderers.Length; i++)
                {
                    if (CombinedObjects[i].Parts.Count > 0)
                        bounds.Encapsulate(_renderers[i].bounds);
                }

                return bounds;
            }


            protected override CombinedObject CreateCombinedObject(CombineSource source)
            {
                CombinedObject root = base.CreateCombinedObject(source);

                root.transform.parent = _transform;

                _group._updateLODs = true;

                return root;
            }

            private void UpdateRenderersList()
            {
                _renderers = CombinedObjects.Select(r => r.GetComponent<Renderer>()).ToArray();
            }
        }
    }
}
