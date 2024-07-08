using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.MeshFusionPro.Example
{
    public interface IHittable
    {
        void Hitted(Ray ray, RaycastHit hitInfo);
    }
}
