using UnityEngine;

public class EditMenu : MonoBehaviour
{
    [SerializeField] private GameObject customizationTabs;

    private void OnMouseEnter()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        customizationTabs.SetActive(true);
        GetComponent<BoxCollider>().enabled = false;
        MainMenu.Instance.windowTypeSelected = WindowType.Marine;
    }
}
