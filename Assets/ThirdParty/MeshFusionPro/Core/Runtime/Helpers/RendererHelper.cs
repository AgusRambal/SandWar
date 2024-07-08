using System.Collections.Generic;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    public static class RendererHelper
    {
        private static List<Material> _materials;

        static RendererHelper()
        {
            _materials = new List<Material>();
        }

        public static Material GetSharedMaterialWithoutAlloc(this Renderer renderer, int index)
        {
            renderer.GetSharedMaterials(_materials);

            return _materials[index];
        }
    }
}
