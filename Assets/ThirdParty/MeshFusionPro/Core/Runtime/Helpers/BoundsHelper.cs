using UnityEngine;

namespace NGS.MeshFusionPro
{
    public static class BoundsHelper
    {
        public static Bounds Transform(this Bounds bounds, Matrix4x4 transform)
        {
            Vector3 bMin = bounds.min;
            Vector3 bMax = bounds.max;

            Vector4 center = transform.GetColumn(3);

            Vector3 min = center;
            Vector3 max = center;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    float m = transform[i, j];

                    float a = m * bMin[j];
                    float b = m * bMax[j];

                    min[i] += a < b ? a : b;
                    max[i] += a < b ? b : a;
                }
            }

            return new Bounds
            {
                center = transform.MultiplyPoint3x4(bounds.center),
                size = max - min
            };
        }

        public static bool Contains(this Bounds bounds, Bounds target)
        {
            return bounds.Contains(target.min) && bounds.Contains(target.max);
        }
    }
}