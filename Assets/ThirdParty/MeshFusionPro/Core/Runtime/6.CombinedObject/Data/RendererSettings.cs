using UnityEngine;
using UnityEngine.Rendering;

namespace NGS.MeshFusionPro
{
    public struct RendererSettings
    {
        public string tag;
        public int layer;
        public Material material;
        public ShadowCastingMode shadowMode;
        public bool receiveShadows;
        public int lightmapIndex;
        public int realtimeLightmapIndex;

        public RendererSettings(
            Material material,
            ShadowCastingMode shadowMode = ShadowCastingMode.On,
            bool receiveShadows = true,
            int lightmapIndex = -1,
            int realtimeLightmapIndex = -1,
            string tag = "Untagged",
            int layer = 0)
        {
            this.material = material;
            this.shadowMode = shadowMode;
            this.receiveShadows = receiveShadows;
            this.lightmapIndex = lightmapIndex;
            this.realtimeLightmapIndex = realtimeLightmapIndex;
            this.tag = tag;
            this.layer = layer;
        }

        public RendererSettings(Renderer renderer, int materialIndex = 0)
        {
            material = renderer.GetSharedMaterialWithoutAlloc(materialIndex);
            shadowMode = renderer.shadowCastingMode;
            receiveShadows = renderer.receiveShadows;
            lightmapIndex = renderer.lightmapIndex;
            realtimeLightmapIndex = renderer.realtimeLightmapIndex;
            tag = renderer.tag;
            layer = renderer.gameObject.layer;
        }
    }
}
