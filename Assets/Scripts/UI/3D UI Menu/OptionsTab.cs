using UnityEngine;

public class OptionsTab : MonoBehaviour
{
    [Header("Tab")]
    [SerializeField] private TabType tabType;

    private void OnMouseDown()
    {
        MainMenu.Instance.SelectTab(tabType);
        EventManager.TriggerEvent(GenericEvents.ButtonSound);
    }
}
