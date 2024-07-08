using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.MeshFusionPro.Example
{
    public class DynamicHittable : MonoBehaviour, IHittable
    {
        private Rigidbody _rigidbody;
        private SourceTracker _tracker;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _tracker = GetComponent<SourceTracker>();
        }

        public void Hitted(Ray ray, RaycastHit hitInfo)
        {
            Vector3 force = ray.direction.normalized * 500f;

            _rigidbody.AddForceAtPosition(force, hitInfo.point, ForceMode.Impulse);
            _tracker.WakeUp();
        }
    }
}
