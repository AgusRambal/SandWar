using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public enum TrackingTarget { Transform, Rigidbody }

    [DisallowMultipleComponent]
    [RequireComponent(typeof(MeshFusionSource))]
    public class SourceTracker : MonoBehaviour
    {
        public bool IsDynamicObject
        {
            get
            {
                return _isDynamicObject;
            }
        }

        [field : SerializeField]
        public TrackingTarget TrackingTarget { get; set; }

        [field: SerializeField]
        public bool DisableWhenIdle { get; set; } = true;

        [field: SerializeField, Min(0.01f)]
        public float MaxIdleTime { get; set; } = 5f;

        [field: SerializeField]
        public bool WakeUpWhenCollision { get; set; } = true;

        [field : SerializeField]
        public bool TrackingDestroy { get; set; } = true;

        [SerializeField]
        private MeshFusionSource _source;

        [SerializeField]
        private bool _isDynamicObject;

        [SerializeField]
        private Rigidbody _rigidbody;

        private IEnumerable<ICombinedObjectPart> _parts;
        private ISourceTrackingStrategy _trackingStrategy;
        private float _idleTime;
        private Action _updateFunc;


        public void WakeUp()
        {
            enabled = true;
            _idleTime = 0f;
        }


        private void Reset()
        {
            _source = GetComponent<MeshFusionSource>();
            _isDynamicObject = _source is DynamicMeshFusionSource;

            if (TryGetComponent(out _rigidbody))
                TrackingTarget = TrackingTarget.Rigidbody;
        }

        private void Start()
        {
            if (_source == null)
            {
                _source = GetComponent<MeshFusionSource>();
                _isDynamicObject = _source is DynamicMeshFusionSource;
            }

            _source.onCombineFinished += OnCombineFinished;

            enabled = false;
        }

        private void Update()
        {
            try
            {
                _updateFunc?.Invoke();
            }
            catch
            {
                if (!_isDynamicObject)
                    enabled = false;
                else
                    throw;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!_isDynamicObject)
                return;

            if (WakeUpWhenCollision && _parts != null)
                WakeUp();
        }

        private void OnDestroy()
        {
            if (!gameObject.scene.isLoaded)
                return;

            if (!TrackingDestroy || _parts == null)
                return;

            foreach (var part in _parts)
            {
                if (part != null && part.Root != null)
                    part.Destroy();
            }
        }


        private void OnCombineFinished(MeshFusionSource source, IEnumerable<ICombinedObjectPart> parts)
        {
            _parts = parts;

            _source.onCombineFinished -= OnCombineFinished;

            if (_isDynamicObject)
            {
                _updateFunc = UpdateMoveTracker;

                if (TrackingTarget == TrackingTarget.Rigidbody && _rigidbody != null)
                {
                    _trackingStrategy = new RigidbodyTrackingStrategy(_rigidbody,
                        parts.Select(p => (DynamicCombinedObjectPart)p).ToArray());

                    if (DisableWhenIdle)
                        _updateFunc = UpdateMoveTrackerAndCheckIdle;
                }
                else
                {
                    _trackingStrategy = new TransformTrackingStrategy(transform,
                        parts.Select(p => (DynamicCombinedObjectPart)p).ToArray());
                }

                enabled = true;
            }
        }

        private void UpdateMoveTracker()
        {
            _trackingStrategy.OnUpdate();
        }

        private void UpdateMoveTrackerAndCheckIdle()
        {
            if (_trackingStrategy.OnUpdate())
            {
                _idleTime = 0f;
            }
            else
            {
                _idleTime += Time.deltaTime;

                if (_idleTime > MaxIdleTime)
                {
                    _idleTime = 0f;
                    enabled = false;
                }
            }
        }
    }
}
