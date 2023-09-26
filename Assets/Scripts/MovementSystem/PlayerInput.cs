using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Camera Camera;
    [SerializeField] private UIMethods uiMethods;
    [SerializeField] private RectTransform selectionBox;
    [SerializeField] private LayerMask UnitLayers;
    [SerializeField] private LayerMask FloorLayers;
    [SerializeField] private float dragDelay = 0.1f;

    private float mouseDownTime;
    private Vector2 startMousePosition;

    private void Update()
    {
        HandleSelectionInput();
        HandleMovementInputs();
    }

    private void HandleMovementInputs()
    {
        if (Input.GetMouseButtonDown(1) && SelectionManager.Instance.SelectedMarines.Count > 0)
        {
            if (Physics.Raycast(Camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, FloorLayers))
            {
                foreach (Marine marine in SelectionManager.Instance.SelectedMarines)
                {
                    marine.target = null;
                    marine.MoveTo(hit.point);
                }
            }

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitEnemy))
            {
                if (hitEnemy.transform.GetComponent<Insurgent>())
                {
                    foreach (Marine marine in SelectionManager.Instance.SelectedMarines)
                    {
                        marine.target = hitEnemy.transform.GetComponent<Insurgent>();
                        marine.customLookAtTarget.StartRotating(marine.target.transform);
                    }
                }
            }
        }
    }

    private void HandleSelectionInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selectionBox.sizeDelta = Vector2.zero;
            selectionBox.gameObject.SetActive(true);
            startMousePosition = Input.mousePosition;
            mouseDownTime = Time.time;
            SelectionManager.Instance.DeselectAll();

            if (UIMethods.isOverUI)
                return;

            uiMethods.DeselectAll();
        }

        else if (Input.GetMouseButton(0) && mouseDownTime + dragDelay < Time.time)
        {
            if (UIMethods.isOverUI)
                return;

            ResizeSelectionBox();
        }

        else if (Input.GetMouseButtonUp(0))
        {
            selectionBox.sizeDelta = Vector2.zero;
            selectionBox.gameObject.SetActive(false);

            if (Physics.Raycast(Camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, UnitLayers)
                        && hit.collider.TryGetComponent(out Marine marine))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    if (SelectionManager.Instance.IsSelected(marine))
                    {
                        SelectionManager.Instance.Deselect(marine);
                    }

                    else
                    {
                        SelectionManager.Instance.Select(marine);
                    }
                }

                else
                {
                    SelectionManager.Instance.DeselectAll();
                    SelectionManager.Instance.Select(marine);
                    uiMethods.OnSelectIcon(marine.characterSelectionImage, marine);
                }
            }

            /*else if (mouseDownTime + dragDelay > Time.time)
            {
                SelectionManager.Instance.DeselectAll();
            }*/

            mouseDownTime = 0;
        }
    }

    private void ResizeSelectionBox()
    {
        float width = Input.mousePosition.x - startMousePosition.x;
        float height = Input.mousePosition.y - startMousePosition.y;

        selectionBox.anchoredPosition = startMousePosition + new Vector2(width / 2, height / 2);
        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));

        Bounds bounds = new Bounds(selectionBox.anchoredPosition, selectionBox.sizeDelta);

        for (int i = 0; i < SelectionManager.Instance.AvailableMarines.Count; i++)
        {
            Marine marine = SelectionManager.Instance.AvailableMarines[i];
            if(marine == null) continue;
            if (UnitInSelectionBox(Camera.WorldToScreenPoint(SelectionManager.Instance.AvailableMarines[i].transform.position), bounds))
            {
                SelectionManager.Instance.Select(SelectionManager.Instance.AvailableMarines[i]);
            }

            else
            {
                SelectionManager.Instance.Deselect(SelectionManager.Instance.AvailableMarines[i]);
            }
        }
    }

    private bool UnitInSelectionBox(Vector2 position, Bounds bounds)
    {
        return position.x > bounds.min.x && position.x < bounds.max.x
            && position.y > bounds.min.y && position.y < bounds.max.y;
    }
}
