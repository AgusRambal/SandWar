using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public interface ISourceTrackingStrategy
    {
        bool OnUpdate();
    }
}
