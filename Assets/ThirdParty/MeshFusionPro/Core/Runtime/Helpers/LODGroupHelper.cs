using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public static class LODGroupHelper
    {
        public static bool Contains(this LODGroup group, Renderer renderer)
        {
            LOD[] lods = group.GetLODs();

            for (int i = 0; i < lods.Length; i++)
            {
                Renderer[] renderers = lods[i].renderers;

                for (int c = 0; c < renderers.Length; c++)
                {
                    if (renderers[c] == renderer)
                        return true;
                }
            }

            return false;
        }
    }
}
