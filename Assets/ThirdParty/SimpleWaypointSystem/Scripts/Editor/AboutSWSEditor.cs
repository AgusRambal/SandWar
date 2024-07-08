/*  This file is part of the "Simple Waypoint System" project by FLOBUK.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using UnityEngine;
using UnityEditor;

/// <summary>
/// FLOBUK about/help/support window.
/// <summary>
public class AboutSWSEditor : EditorWindow
{
    [MenuItem("Window/Simple Waypoint System/About")]
    static void Init()
    {
        AboutSWSEditor aboutWindow = (AboutSWSEditor)EditorWindow.GetWindowWithRect
                (typeof(AboutSWSEditor), new Rect(0, 0, 300, 320), false, "About");
        aboutWindow.Show();
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(70);
        GUILayout.Label("Simple Waypoint System", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();
   
        GUILayout.BeginHorizontal();
        GUILayout.Space(70);
        GUILayout.Label("by FLOBUK");
        GUILayout.EndHorizontal();        
        GUILayout.Space(20);

        GUILayout.Label("Info", EditorStyles.boldLabel);

        if (GUILayout.Button("Homepage"))
        {
            Help.BrowseURL("https://flobuk.com");
        }
		
        if (GUILayout.Button("YouTube"))
        {
            Help.BrowseURL("https://www.youtube.com/@flobuk");
        }
        GUILayout.Space(5);

        GUILayout.Label("Support", EditorStyles.boldLabel);

        if (GUILayout.Button("Online Documentation"))
        {
            Help.BrowseURL("https://flobuk.gitlab.io/assets/docs/sws/");
        }

        if (GUILayout.Button("Scripting Reference"))
        {
            Help.BrowseURL("https://flobuk.gitlab.io/assets/docs/sws/api");
        }

        if (GUILayout.Button("Unity Forum"))
        {
            Help.BrowseURL("https://forum.unity3d.com/threads/115086/");
        }
        GUILayout.Space(5);

        GUILayout.Label("Support me!", EditorStyles.boldLabel);

        if (GUILayout.Button("Review Asset"))
        {
            Help.BrowseURL("https://assetstore.unity.com/packages/slug/2506?aid=1011lGiF&pubref=editor_sws");
        }

        if (GUILayout.Button("Donate"))
        {
            Help.BrowseURL("https://flobuk.com");
        }
    }
}