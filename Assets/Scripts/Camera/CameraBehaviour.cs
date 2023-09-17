using Cinemachine;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] private CinemachineVirtualCamera vCam;

    [Header("Features")]
    [SerializeField] private bool useEdgeScrolling = false;
    [SerializeField] private bool useDragPan = false;

    [Header("Variables")]
    [SerializeField] private float moveSpeed = 25;
    [SerializeField] private float moveSpeedShift = 50f;
    [SerializeField] private float rotateSpeed = 100f;
    [SerializeField] private float panSpeed = 1;
    [SerializeField] private float zoomOffsetMin = 5f;
    [SerializeField] private float zoomOffsetMax = 50f;
    [SerializeField] private float zoomSpeed = 10f;

    private bool dragPanMoveActive = false;
    private Vector2 lastMousePosition;
    private Vector3 followOffset;

    private void Awake()
    {
        followOffset = vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
    }

    private void Update()
    {
        Movement();

        if (useEdgeScrolling)
        {
            UseEdgeScrolling();
        }

        if (useDragPan)
        {
            UseDragPan();
        }

        Rotation();
        CameraZoom();
    }

    private void Movement()
    {
        Vector3 inputDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) inputDir.z = -1f;
        if (Input.GetKey(KeyCode.S)) inputDir.z = +1f;
        if (Input.GetKey(KeyCode.A)) inputDir.x = +1f;
        if (Input.GetKey(KeyCode.D)) inputDir.x = -1f;

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.position += moveSpeedShift * Time.deltaTime * moveDir;
        }

        else
        {
            transform.position += moveSpeed * Time.deltaTime * moveDir;
        }
    }

    private void UseEdgeScrolling()
    {
        Vector3 inputDir = new Vector3(0, 0, 0);

        int edgeScrollSize = 20;

        if (Input.mousePosition.x < edgeScrollSize) inputDir.x = -1f;
        if (Input.mousePosition.y < edgeScrollSize) inputDir.z = -1f;
        if (Input.mousePosition.x > Screen.width - edgeScrollSize) inputDir.x = +1f;
        if (Input.mousePosition.y > Screen.height - edgeScrollSize) inputDir.z = +1f;

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
        transform.position += moveSpeed * Time.deltaTime * moveDir;
    }

    private void UseDragPan()
    {
        Vector3 inputDir = new Vector3(0, 0, 0);

        if (Input.GetMouseButtonDown(0))
        {
            dragPanMoveActive = true;
            lastMousePosition = Input.mouseScrollDelta;
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragPanMoveActive = false;
        }

        if (dragPanMoveActive)
        {
            Vector2 mouseMovementDelta = (Vector2)Input.mousePosition - lastMousePosition;

            inputDir.x = mouseMovementDelta.x * panSpeed;
            inputDir.z = mouseMovementDelta.y * panSpeed;

            lastMousePosition = Input.mousePosition;
        }

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
        transform.position += moveSpeed * Time.deltaTime * moveDir;
    }

    private void Rotation()
    {
        float rotateDir = 0f;

        if (Input.GetMouseButton(2))
        {
            if (Input.GetAxis("Mouse X") < 0)
            {
                rotateDir = -1f;
            }

            if (Input.GetAxis("Mouse X") > 0)
            {
                rotateDir = +1f;
            }
        }

        if (Input.GetKey(KeyCode.Q)) rotateDir = +1f;
        if (Input.GetKey(KeyCode.E)) rotateDir = -1f;

        transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * Time.deltaTime, 0);
    }

    private void CameraZoom()
    {
        Vector3 zoomDir = followOffset.normalized;

        if (Input.mouseScrollDelta.y > 0)
        {
            followOffset -= zoomDir;
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            followOffset += zoomDir;
        }

        if (followOffset.magnitude < zoomOffsetMin)
        {
            followOffset = zoomDir * zoomOffsetMin;
        }  
        
        if (followOffset.magnitude > zoomOffsetMax)
        {
            followOffset = zoomDir * zoomOffsetMax;
        }

        vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = 
            Vector3.Lerp(vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, followOffset, Time.deltaTime * zoomSpeed);
    }
}