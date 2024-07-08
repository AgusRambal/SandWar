using UnityEditor;

namespace NGS.MeshFusionPro
{
    public static class SerializedObjectHelper
    {
        public static SerializedProperty FindAutoProperty(this SerializedObject obj, string name)
        {
            string path = string.Format("<{0}>k__BackingField", name);

            return obj.FindProperty(path);
        }
    }
}
