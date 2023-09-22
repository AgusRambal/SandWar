using UnityEngine;

public class SellOil : MonoBehaviour
{
    private void OnMouseDown()
    {
        EventManager.TriggerEvent(GenericEvents.OpenSellOilWindow);
    }
}
