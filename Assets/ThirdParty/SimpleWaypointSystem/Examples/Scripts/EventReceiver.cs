/*  This file is part of the "Simple Waypoint System" project by FLOBUK.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace SWS
{
    /// <summary>
    /// Example: some methods invoked by events, demonstrating runtime adjustments.
    /// <summary>
    public class EventReceiver : MonoBehaviour
    {
        public void MyMethod()
        {
            //your own method!
        }


        public void PrintWaypoint(int index)
        {
            Debug.Log(index.ToString());
        }


        //prints text to the console
        public void PrintText(string text)
        {
            Debug.Log(text);
        }


        //delay movement on specific waypoints
        public void DelayAtWaypoint(int index)
        {
            switch (index)
            {
                case 1:
                case 3:
                    GetComponent<splineMove>().Pause(3);
                    break;
            }
        }


        //sets the transform's y-axis to the desired rotation
        //could be used in 2D for rotating a sprite at path ends
        public void RotateSprite(float newRot)
        {
            Vector3 currentRot = transform.eulerAngles;
            currentRot.y = newRot;
            transform.eulerAngles = currentRot;
        }


        //sets a new destination for a navmesh agent,
        //leaving its path and returning to it after a few seconds.
        //used in the event sample for redirecting the agent
        public void SetDestination(Object target)
        {
            StartCoroutine(SetDestinationRoutine(target));
        }

        private IEnumerator SetDestinationRoutine(Object target)
        {
            //get references
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            navMove myMove = GetComponent<navMove>();
            GameObject tar = (GameObject)target as GameObject;

            //increase agent speed
            myMove.ChangeSpeed(4);
            //set new destination of the navmesh agent
            agent.SetDestination(tar.transform.position);

            //wait until the path has been calculated
            while (agent.pathPending)
                yield return null;
            //wait until agent reached its destination
            float remain = agent.remainingDistance;
            while (remain == Mathf.Infinity || remain - agent.stoppingDistance > float.Epsilon
            || agent.pathStatus != NavMeshPathStatus.PathComplete)
            {
                remain = agent.remainingDistance;
                yield return null;
            }

            //wait a few seconds at the destination,
            //then decrease agent speed and restart movement routine
            yield return new WaitForSeconds(4);
            myMove.ChangeSpeed(1.5f);
            myMove.moveToPath = true;
            myMove.StartMove();
        }


        //activates an object for an amount of time
        //used in the event sample for activating a particle effect
        public void ActivateForTime(Object target)
        {
            StartCoroutine(ActivateForTimeRoutine(target));
        }

        private IEnumerator ActivateForTimeRoutine(Object target)
        {
            GameObject tar = (GameObject)target as GameObject;
            tar.SetActive(true);

            yield return new WaitForSeconds(6);
            tar.SetActive(false);
        }
    }
}