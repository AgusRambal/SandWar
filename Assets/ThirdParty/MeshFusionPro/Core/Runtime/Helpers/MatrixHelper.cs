using UnityEngine;

namespace NGS.MeshFusionPro
{
    public static class MatrixHelper
    {
        public static Vector3 GetTranslation(this Matrix4x4 matrix)
        {
            return matrix.GetColumn(3);
        }

        public static Matrix4x4 SetTranslation(this Matrix4x4 matrix, Vector3 pos)
        {
            Matrix4x4 result = matrix;

            result.SetColumn(3, new Vector4(pos.x, pos.y, pos.z, 1));

            return result;
        }
    }
}
