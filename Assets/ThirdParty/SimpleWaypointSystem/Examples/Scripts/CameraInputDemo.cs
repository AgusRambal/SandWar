/*  This file is part of the "Simple Waypoint System" project by FLOBUK.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using UnityEngine;

namespace SWS
{
    using DG.Tweening;

    /// <summary>
    /// Example: user input script which moves through waypoints one by one.
    /// <summary>
    public class CameraInputDemo : MonoBehaviour
    {
        /// <summary>
        /// Information text per waypoint, set at start and via messages.
        /// <summary>
        public string infoText = "Welcome to this customized input example";

        //reference to the movement script
        private splineMove myMove;


        //get references at start
        //initialize movement but don't start it yet
        void Start()
        {
            myMove = gameObject.GetComponent<splineMove>();
            myMove.StartMove();
            myMove.Pause();
        }


        //listens to user input
        void Update()
        {
            //do nothing in moving state
            if (!myMove.tween.IsActive() || myMove.tween.IsPlaying())
                return;

            //on up arrow, move forwards
            if (Input.GetKeyDown(KeyCode.UpArrow))
                myMove.Resume();
        }


        //display GUI stuff on screen
        void OnGUI()
        {
            //do nothing in moving state
            if (myMove.tween.IsActive() && myMove.tween.IsPlaying())
                return;

            //draw top right box with info text received from messages
            GUI.Box(new Rect(Screen.width - 150, Screen.height / 2, 150, 100), "");
            Rect infoPos = new Rect(Screen.width - 130, Screen.height / 2 + 10, 110, 90);
            GUI.Label(infoPos, infoText);
        }


        /// <summary>
        /// Called on each waypoint to set some information text in the UI.
        /// </summary>
        public void OnWaypointChange(int index)
        {
            //do not pause on first or last waypoint
            if (index > 0 && index != 5)
                myMove.Pause();

            switch (index)
            {
                case 1:
                    ShowInformation("You can see your current path progress in the upper camera view.");
                    break;
                case 2:
                    ShowInformation("This sample uses events to pause movement at waypoints.");
                    break;
                case 3:
                    ShowInformation("Also, events are being used to set the text in this corner.");
                    break;
                case 4:
                    ShowInformation("Additionally, we access the movement script to resume it based on user input.");
                    break;
                case 5:
                    ShowInformation("The path ends here. Reload this scene to start all over again.");
                    break;
            }
        }


        /// <summary>
        /// Receives text from messages.
        /// <summary>
        public void ShowInformation(string text)
        {
            infoText = text;
        }
    }
}