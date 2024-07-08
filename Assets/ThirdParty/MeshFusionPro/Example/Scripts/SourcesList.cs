using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.MeshFusionPro.Example
{
    public class SourcesList : MonoBehaviour
    {
        public static bool UpdatedDirty { get; set; }
        public static IReadOnlyCollection<MeshFusionSource> Sources
        {
            get
            {
                return _sources;
            }
        }
        public static IReadOnlyCollection<MeshRenderer> CombinedObjects
        {
            get
            {
                return _combinedObjects;
            }
        }

        private static HashSet<MeshRenderer> _combinedObjects = new HashSet<MeshRenderer>();
        private static HashSet<MeshFusionSource> _sources = new HashSet<MeshFusionSource>();

        private void Awake()
        {
            MeshFusionSource source = GetComponent<MeshFusionSource>();

            if (source == null)
                throw new MissingComponentException();

            source.onCombineFinished += OnSourceCombined;

            _sources.Add(source);

            UpdatedDirty = true;
        }

        private void OnSourceCombined(MeshFusionSource source, IEnumerable<ICombinedObjectPart> parts)
        {
            foreach (var part in parts)
            {
                if (part is CombinedLODGroupPart)
                {
                    MeshRenderer[] renderers = ((MonoBehaviour)part.Root).GetComponentsInChildren<MeshRenderer>();

                    for (int i = 0; i < renderers.Length; i++)
                        _combinedObjects.Add(renderers[i]);
                }
                else
                {
                    _combinedObjects.Add(((MonoBehaviour)part.Root).GetComponent<MeshRenderer>());
                }
            }

            UpdatedDirty = true;
        }
    }
}
