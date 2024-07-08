using System.Collections.Generic;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public class LODMeshFusionSource : MeshFusionSource
    {
        [SerializeField, HideInInspector]
        private LODGroup _group;

        private LODGroupCombineSource _source;
        private CombinedLODGroupPart _part;
        private Bounds? _savedBounds;


        public override bool TryGetBounds(ref Bounds bounds)
        {
            if (_source != null)
            {
                bounds = _source.Bounds;
                return true;
            }

            if (_part != null)
            {
                bounds = _part.Bounds;
                return true;
            }

            if (_savedBounds.HasValue)
            {
                bounds = _savedBounds.Value;
                return true;
            }

            if (IsIncompatible)
                return false;

            LOD[] lods = _group.GetLODs();
            bounds = new Bounds(transform.position, Vector3.zero);

            for (int i = 0; i < lods.Length; i++)
            {
                Renderer[] renderers = lods[i].renderers;

                for (int c = 0; c < renderers.Length; c++)
                {
                    if (renderers[c] == null)
                        continue;

                    bounds.Encapsulate(renderers[c].bounds);
                }
            }

            _savedBounds = bounds;

            return true;
        }

        protected override void OnSourceCombinedInternal(ICombinedObject root, ICombinedObjectPart part)
        {
            _part = (CombinedLODGroupPart)part;
        }


        protected override bool CheckCompatibilityAndGetComponents(out string incompatibilityReason)
        {
            if (_group == null)
                _group = GetComponent<LODGroup>();

            if (_group == null)
            {
                incompatibilityReason = "LODGroup not found";
                return false;
            }

            incompatibilityReason = "";

            MeshFilter filter = null;
            Mesh mesh = null;
            LOD[] lods = _group.GetLODs();

            for (int i = 0; i < lods.Length; i++)
            {
                Renderer[] renderers = lods[i].renderers;

                for (int c = 0; c < renderers.Length; c++)
                {
                    MeshRenderer renderer = renderers[c] as MeshRenderer;

                    if (renderer == null)
                        continue;

                    if (CanCreateCombineSource(renderer.gameObject,
                        ref incompatibilityReason, 
                        ref renderer, 
                        ref filter, 
                        ref mesh))
                    {
                        return true;
                    }

                    filter = null;
                    mesh = null;
                }
            }
            
            incompatibilityReason = "Compatible renderers not found";

            return false;
        }

        protected override void CreateSources()
        {
            _source = new LODGroupCombineSource(_group);
        }

        protected override IEnumerable<ICombineSource> GetCombineSources()
        {
            if (_source == null)
                yield break;

            yield return _source;
        }

        protected override IEnumerable<ICombinedObjectPart> GetCombinedParts()
        {
            if (_part == null)
                yield break;

            yield return _part;
        }

        protected override void ClearSources()
        {
            if (_source == null)
                return;

            _source = null;
        }

        protected override void ClearParts()
        {
            if (_part == null)
                return;

            _part = null;
        }


        protected override void ToggleComponents(bool enabled)
        {
            LOD[] lods = _group.GetLODs();

            for (int i = 0; i < lods.Length; i++)
            {
                Renderer[] renderers = lods[i].renderers;

                for (int c = 0; c < renderers.Length; c++)
                {
                    Renderer renderer = renderers[c];

                    if (renderer == null)
                        continue;

                    renderers[c].enabled = enabled;
                }
            }

            _group.enabled = enabled;
        }
    }
}
