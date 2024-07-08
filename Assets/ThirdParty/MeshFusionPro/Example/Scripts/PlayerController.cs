using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.MeshFusionPro.Example
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera;

        private bool _hitterEnabled = true;

        public void Update()
        {
            if (Input.GetMouseButtonDown(0) && _hitterEnabled)
                Hit();
        }


        private void ToggleHitter()
        {
            _hitterEnabled = !_hitterEnabled;
        }

        private void Hit()
        {
            Ray ray = _camera.ViewportPointToRay(new Vector2(0.5f, 0.5f));

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                IHittable hittable = hit.collider.GetComponent<IHittable>();
                hittable?.Hitted(ray, hit);
            }
        }
    }
}
