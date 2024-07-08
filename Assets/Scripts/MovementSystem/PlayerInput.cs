using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Core.Characters;

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

    public float margin = 1f;
    SelectionManager selectionManager;

    private void Start()
    {
        if (selectionManager == null)        
            selectionManager = SelectionManager.Instance;        
    }

    private void Update()
    {
        HandleSelectionInput();
        HandleMovementInputs();
    }

    private void HandleMovementInputs()
    {
        if (Input.GetMouseButtonDown(1) && selectionManager.SelectedMarines.Count > 0)
        {
            if (Physics.Raycast(Camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, FloorLayers))
            {
                List<Marine> selectedMarines = ConvertHashSetMarinesToList();
                int cantMarines = selectedMarines.Count;
                float totRadius = CalculateTotalRadius(selectedMarines); // calculo del area en la que voy a asignar los posibles puntos para los agentes 

                List<Vector3> assignedPoints = new List<Vector3>();

                for (int i = 0; i < selectedMarines.Count; ++i)
                {
                    Vector3 randomPoint = GetRandomPointInCircle(hit.point, totRadius, assignedPoints);
                    assignedPoints.Add(randomPoint);
                    selectedMarines[i].MoveTo(randomPoint);
                }
            }

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitEnemy))
            {
                if (hitEnemy.transform.GetComponent<Insurgent>())
                {
                    foreach (Marine marine in selectionManager.SelectedMarines)
                    {
                        marine.target = hitEnemy.transform.GetComponent<Insurgent>();
                        marine.StartRotating(marine.target.transform);
                    }
                }
            }
        }
    }

    Vector3 GetRandomPointInCircle(Vector3 center, float radius, List<Vector3> assignedPoints)
    {
        Vector2 randomPoint2D = Random.insideUnitCircle * radius;
        Vector3 randomPoint = center + new Vector3(randomPoint2D.x, 0, randomPoint2D.y);

        int stop = 0;

        while (assignedPoints.Any(p => Vector3.Distance(randomPoint, p) < margin)) // Me aseguro que no me de una posicion ocupada por otro agente, teniendo en cuenta el radio y un margen
        {
            randomPoint2D = Random.insideUnitCircle * radius;
            randomPoint = center + new Vector3(randomPoint2D.x, 0, randomPoint2D.y);

            stop++;
            if (stop > 1000)
                break;
        }

        return randomPoint;
    }

    List<Marine> ConvertHashSetMarinesToList()
    {
        List<Marine> selectedMarines = new List<Marine>();
        foreach (Marine marine in  selectionManager.SelectedMarines)
        {
            selectedMarines.Add(marine);
        }
        return selectedMarines;
    }

    float CalculateTotalRadius(List<Marine> selectedMarines)
    {
        float sum = .0f;

        if (selectedMarines.Count > 0)
        {
            foreach (var marine in selectedMarines)
            {
                sum += marine.agent.radius + margin;
            }

            return sum / (2 * Mathf.PI);
        }

        return 0;
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
            if (marine == null) continue;
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
