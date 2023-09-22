using UnityEngine;

public class OptionsTab : MonoBehaviour
{
    [SerializeField] private TabType tabType;

    private void OnMouseDown()
    {
        MainMenu.Instance.SelectTab(tabType);
    }
}
