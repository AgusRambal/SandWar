using UnityEngine;

public class InteractWithEnviroment : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) 
            { 
                if (hit.transform.GetComponent<RecruitMarines>())
                {
                    EventManager.TriggerEvent(GenericEvents.OpenRecruitWindow);
                }
            }
        }
    }
}
