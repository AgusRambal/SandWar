using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEditor;

using Object = UnityEngine.Object;

namespace NGS.MeshFusionPro
{
    public static class MeshFusionEditorUtil 
    {
        public static void CheckAllMeshFusionSources()
        {
            MeshFusionSource[] sources = Object.FindObjectsOfType<MeshFusionSource>();

            for (int i = 0; i < sources.Length; i++)
                sources[i].CheckCompatibility();
        }

        public static void DisableStaticBatching()
        {
            try
            {
                BuildTarget platform = EditorUserBuildSettings.activeBuildTarget;

                var method = typeof(PlayerSettings).GetMethod("SetBatchingForPlatform", BindingFlags.Static | BindingFlags.Default | BindingFlags.NonPublic);

                if (method == null)
                    return;

                object[] args = new object[] { platform, 0, 0 };

                method.Invoke(null, args);

                Debug.Log("Static Batching disabled!");
            }
            catch
            {
                Debug.LogError("Unable to disable static batching automatically :(" +
                    "\nDo it manually in PlayerSettings");
            }
        }

        public static void MakeAllMeshesReadable()
        {
            MeshFusionSource[] sources = Object.FindObjectsOfType<MeshFusionSource>();

            for (int i = 0; i < sources.Length; i++)
            {
                MeshFusionSource source = sources[i];

                if (source is LODMeshFusionSource)
                {
                    foreach (MeshFilter filter in source.GetComponentsInChildren<MeshFilter>())
                    {
                        if (filter.sharedMesh != null)
                            MakeMeshReadable(filter.sharedMesh);
                    }
                }
                else
                {
                    if (source.TryGetComponent(out MeshFilter filter))
                    {
                        if (filter.sharedMesh != null)
                            MakeMeshReadable(filter.sharedMesh);
                    }
                }
            }
        }


        private static void MakeMeshReadable(Mesh mesh)
        {
            try
            {
                if (mesh.isReadable)
                    return;

                string path = AssetDatabase.GetAssetPath(mesh.GetInstanceID());

                if (path == null || path == "")
                    Debug.Log("Unable find path for mesh : " + mesh.name);

                ModelImporter importer = (ModelImporter)AssetImporter.GetAtPath(path);

                importer.isReadable = true;
                importer.SaveAndReimport();

                Debug.Log(string.Format("Maked {0} mesh readable", mesh.name));
            }
            catch(Exception ex)
            {          
                Debug.Log(string.Format("Unable to make mesh {0} readable. Reason : {1}{2}", 
                    mesh.name, ex.Message, ex.StackTrace));
            }
        }
    }
}
