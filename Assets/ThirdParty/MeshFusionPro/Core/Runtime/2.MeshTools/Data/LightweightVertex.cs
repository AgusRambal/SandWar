using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;

namespace NGS.MeshFusionPro
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LightweightVertex
    {
        public Vector3 Position
        {
            get
            {
                return pos;
            }
            set
            {
                pos = value;
            }
        }
        public Vector3 Normal
        {
            get
            {
                return new Vector3(normX / 127f, normY / 127f, normZ / 127f);
            }
            set
            {
                normX = (sbyte)(value.x * 127);
                normY = (sbyte)(value.y * 127);
                normZ = (sbyte)(value.z * 127);
                normW = 1;
            }
        }
        public Vector4 Tangent
        {
            get
            {
                return new Vector4(tanX / 127f, tanY / 127f, tanZ / 127f, tanW / 127f);
            }

            set
            {
                tanX = (sbyte)(value.x * 127);
                tanY = (sbyte)(value.y * 127);
                tanZ = (sbyte)(value.z * 127);
                tanW = (sbyte)(value.w * 127);
            }
        }
        public Vector2 UV
        {
            get
            {
                return new Vector2(uv.x, uv.y);
            }
            set
            {
                uv = new half2((half)value.x, (half)value.y);
            }
        }
        public Vector2 UV2
        {
            get
            {
                return new Vector2(uv2.x, uv2.y);
            }
            set
            {
                uv2 = new half2((half)value.x, (half)value.y);
            }
        }

        public float3 pos;
        public sbyte normX, normY, normZ, normW;
        public sbyte tanX, tanY, tanZ, tanW;
        public half2 uv;
        public half2 uv2;
    }
}
