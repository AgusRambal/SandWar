using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace NGS.MeshFusionPro
{
    public static class VertexBufferUtil
    {
        private static readonly Dictionary<VertexAttribute, VertexAttributeDescriptor> StandardAttributesMap;
        private static readonly VertexAttributeDescriptor[] LightweightAttributesLayout;

        private static List<VertexAttributeDescriptor> _meshAttributes;


        static VertexBufferUtil()
        {
            LightweightAttributesLayout = new VertexAttributeDescriptor[]
            {
                new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3),
                new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.SNorm8, 4),
                new VertexAttributeDescriptor(VertexAttribute.Tangent, VertexAttributeFormat.SNorm8, 4),
                new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float16, 2),
                new VertexAttributeDescriptor(VertexAttribute.TexCoord1, VertexAttributeFormat.Float16, 2),
            };

            StandardAttributesMap = new Dictionary<VertexAttribute, VertexAttributeDescriptor>()
            {
                { VertexAttribute.Position, new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3) },
                { VertexAttribute.Normal, new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.Float32, 3) },
                { VertexAttribute.Tangent, new VertexAttributeDescriptor(VertexAttribute.Tangent, VertexAttributeFormat.Float32, 4) },
                { VertexAttribute.TexCoord0, new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2) },
                { VertexAttribute.TexCoord1, new VertexAttributeDescriptor(VertexAttribute.TexCoord1, VertexAttributeFormat.Float32, 2) },
                { VertexAttribute.TexCoord2, new VertexAttributeDescriptor(VertexAttribute.TexCoord2, VertexAttributeFormat.Float32, 2) },
                { VertexAttribute.TexCoord3, new VertexAttributeDescriptor(VertexAttribute.TexCoord3, VertexAttributeFormat.Float32, 2) }
            };

            _meshAttributes = new List<VertexAttributeDescriptor>();
        }

        public static bool IsStandardBuffer(Mesh mesh)
        {
            mesh.GetVertexAttributes(_meshAttributes);

            for (int i = 0; i < _meshAttributes.Count; i++)
            {
                VertexAttributeDescriptor attr = _meshAttributes[i];
                VertexAttributeDescriptor targetAttr = StandardAttributesMap[attr.attribute];

                if (attr.format != targetAttr.format)
                    return false;

                if (attr.dimension != targetAttr.dimension)
                    return false;
            }

            return true;
        }

        public static bool IsLightweightBuffer(Mesh mesh)
        {
            mesh.GetVertexAttributes(_meshAttributes);

            if (_meshAttributes.Count != LightweightAttributesLayout.Length)
                return false;

            for (int i = 0; i < _meshAttributes.Count; i++)
            {
                VertexAttributeDescriptor attr = _meshAttributes[i];
                VertexAttributeDescriptor targetAttr = LightweightAttributesLayout[i];

                if (attr.attribute != targetAttr.attribute)
                    return false;

                if (attr.format != targetAttr.format)
                    return false;

                if (attr.dimension != targetAttr.dimension)
                    return false;
            }

            return true;
        }

        public static void ToStandardBuffer(Mesh mesh)
        {
            if (IsStandardBuffer(mesh))
                return;

            for (int i = 0; i < _meshAttributes.Count; i++)
            {
                _meshAttributes[i] = StandardAttributesMap[_meshAttributes[i].attribute];
            }

            mesh.SetVertexBufferParams(mesh.vertexCount, _meshAttributes.ToArray());
        }

        public static void ToLightweightBuffer(Mesh mesh)
        {
            if (IsLightweightBuffer(mesh))
                return;

            mesh.SetVertexBufferParams(mesh.vertexCount, LightweightAttributesLayout);
        }
    }
}
