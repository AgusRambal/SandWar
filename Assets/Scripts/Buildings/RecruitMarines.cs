using UnityEngine;

public class RecruitMarines : MonoBehaviour
{
    private void OnMouseDown()
    {
        EventManager.TriggerEvent(GenericEvents.OpenRecruitWindow);
    }
}
