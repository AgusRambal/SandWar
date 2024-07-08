using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public class RigidbodyTrackingStrategy : ISourceTrackingStrategy
    {
        public float VelocityThreashold
        {
            get
            {
                return _velocityThreashold;
            }

            set
            {
                _velocityThreashold = Mathf.Max(0, value);
            }
        }
        public float AngularVelocityThreashold
        {
            get
            {
                return _angularVelocityThreashold;
            }

            set
            {
                _angularVelocityThreashold = Mathf.Max(0, value);
            }
        }

        private Rigidbody _rigidbody;
        private Transform _transform;
        private DynamicCombinedObjectPart[] _parts;

        private float _velocityThreashold = 0.5f;
        private float _angularVelocityThreashold = 0.3f;

        public RigidbodyTrackingStrategy(Rigidbody target, DynamicCombinedObjectPart[] parts)
        {
            _rigidbody = target;
            _transform = target.transform;
            _parts = parts;
        }

        public bool OnUpdate()
        {
            float velocity = _rigidbody.velocity.magnitude;
            float angularVelocity = _rigidbody.angularVelocity.magnitude;

            if (velocity > _velocityThreashold || angularVelocity > _angularVelocityThreashold)
            {
                for (int i = 0; i < _parts.Length; i++)
                    _parts[i].Move(_transform.localToWorldMatrix);

                return true;
            }

            return false;
        }
    }
}
