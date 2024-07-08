using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.MeshFusionPro.Example
{
    public class StaticHittable : MonoBehaviour, IHittable
    {
        public void Hitted(Ray ray, RaycastHit hitInfo)
        {
            Destroy(gameObject);
        }
    }
}
