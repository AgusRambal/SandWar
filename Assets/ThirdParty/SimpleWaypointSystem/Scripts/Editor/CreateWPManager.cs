/*  This file is part of the "Simple Waypoint System" project by FLOBUK.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using UnityEditor;
using UnityEngine;

namespace SWS
{
    /// <summary>
    /// Adds a new Waypoint Manager gameobject to the scene.
    /// <summary>
    public class CreateWPManager : EditorWindow
    {
        //add menu named "Waypoint Manager" to the Window menu
        [MenuItem("Window/Simple Waypoint System/Waypoint Manager")]

        //initialize method
        static void Init()
        {
            //search for a waypoint manager object within current scene
            WaypointManager wpManager = FindObjectOfType<WaypointManager>(true);

            //if no waypoint manager script was found
            if (wpManager == null)
            {
                //create a new gameobject and attach the WaypointManager component to it
                wpManager = new GameObject("WaypointManager").AddComponent<WaypointManager>();
            }

            //in both cases, select the gameobject
            Selection.activeGameObject = wpManager.gameObject;
        }
    }
}