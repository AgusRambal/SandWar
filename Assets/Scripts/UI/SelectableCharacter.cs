using UnityEngine;
using UnityEngine.UI;

public class SelectableCharacter : MonoBehaviour
{
    public UIMethods Uimethods;
    public Button selectCharacter;
    public GameObject selectedSprite;
    public Image marineImage;
    public MarineObject marineObject;

    private void Start()
    {
        selectCharacter.onClick.AddListener(() => Uimethods.OnSelectIcon(this, marineObject));
    }
}
