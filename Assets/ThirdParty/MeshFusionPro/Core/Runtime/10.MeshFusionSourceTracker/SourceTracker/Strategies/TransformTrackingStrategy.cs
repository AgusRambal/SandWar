using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public class TransformTrackingStrategy : ISourceTrackingStrategy
    {
        private Transform _transform;
        private DynamicCombinedObjectPart[] _parts;

        public TransformTrackingStrategy(Transform target, DynamicCombinedObjectPart[] parts)
        {
            _transform = target;
            _parts = parts;
        }

        public bool OnUpdate()
        {
            if (_transform.hasChanged)
            {
                for (int i = 0; i < _parts.Length; i++)
                    _parts[i].Move(_transform.localToWorldMatrix);

                _transform.hasChanged = false;

                return true;
            }

            return false;
        }
    }
}
