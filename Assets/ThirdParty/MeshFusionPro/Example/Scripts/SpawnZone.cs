using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.MeshFusionPro.Example
{
    public class SpawnZone : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] _sources;

        [SerializeField]
        private float _spawnRadius;

        [SerializeField]
        private int _spawnCount;

        [SerializeField]
        private float _minExtrude;

        [SerializeField]
        private float _maxExtrude;

        private void Awake()
        {
            enabled = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                Spawn();
        }

        private void OnTriggerEnter(Collider other)
        {
            enabled = true;
        }

        private void OnTriggerExit(Collider other)
        {
            enabled = false;
        }


        private void Spawn()
        {
            for (int i = 0; i < _spawnCount; i++)
            {
                GameObject source = _sources[Random.Range(0, _sources.Length)];
                GameObject go = Instantiate(source);

                Vector3 position = GetSpawnPosition();

                go.transform.position = position;
                go.transform.rotation = Random.rotation;
            }
        }

        private Vector3 GetSpawnPosition()
        {
            Vector3 point = transform.position;

            point.x += Random.insideUnitCircle.x * _spawnRadius;
            point.y = 1000;
            point.z += Random.insideUnitCircle.y * _spawnRadius;

            Ray ray = new Ray(point, Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 hitPoint = hit.point;

                hitPoint.y += Random.Range(_minExtrude, _maxExtrude);

                return hitPoint;
            }

            throw new System.InvalidOperationException();
        }
    }
}
