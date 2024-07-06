using Cinemachine;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachineVirtualCamera vCam;

    [Header("Features")]
    [SerializeField] private bool useEdgeScrolling = false;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 25f;
    [SerializeField] private float moveSpeedShift = 50f;
    [SerializeField] private float moveSmoothTime = 0.1f;

    [Header("Rotation Settings")]
    [SerializeField] private float rotateSpeed = 100f;
    [SerializeField] private float rotateSmoothTime = 0.1f;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomOffsetMin = 5f;
    [SerializeField] private float zoomOffsetMax = 50f;
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float zoomSmoothTime = 0.1f;
    [SerializeField] private float zoomStep = 2f;

    private Vector3 targetPosition;
    private Vector3 currentVelocity;
    private float targetRotation;
    private float rotationVelocity;
    private Vector3 followOffset;
    private Vector3 zoomVelocity;

    private void Awake()
    {
        followOffset = vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
        targetPosition = transform.position;
        targetRotation = transform.eulerAngles.y;
    }

    private void Update()
    {
        HandleMovement();
        HandleEdgeScrolling();
        HandleRotation();
        HandleZoom();
    }

    private void HandleMovement()
    {
        Vector3 inputDir = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) inputDir.z = -1f;
        if (Input.GetKey(KeyCode.S)) inputDir.z = +1f;
        if (Input.GetKey(KeyCode.A)) inputDir.x = +1f;
        if (Input.GetKey(KeyCode.D)) inputDir.x = -1f;

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

        float speed = Input.GetKey(KeyCode.LeftShift) ? moveSpeedShift : moveSpeed;
        targetPosition += speed * Time.deltaTime * moveDir;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, moveSmoothTime);
    }

    private void HandleEdgeScrolling()
    {
        if (!useEdgeScrolling) return;

        Vector3 inputDir = Vector3.zero;
        int edgeScrollSize = 20;

        if (Input.mousePosition.x < edgeScrollSize) inputDir.x = +1f;
        if (Input.mousePosition.y < edgeScrollSize) inputDir.z = +1f;
        if (Input.mousePosition.x > Screen.width - edgeScrollSize) inputDir.x = -1f;
        if (Input.mousePosition.y > Screen.height - edgeScrollSize) inputDir.z = -1f;

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
        targetPosition += moveSpeed * Time.deltaTime * moveDir;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, moveSmoothTime);
    }

    private void HandleRotation()
    {
        float rotateDir = 0f;

        if (Input.GetMouseButton(2))
        {
            rotateDir = Input.GetAxis("Mouse X");
        }

        if (Input.GetKey(KeyCode.Q)) rotateDir = -1f;
        if (Input.GetKey(KeyCode.E)) rotateDir = 1f;

        targetRotation += rotateDir * rotateSpeed * Time.deltaTime;
        float smoothedRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotateSmoothTime);

        transform.rotation = Quaternion.Euler(0, smoothedRotation, 0);
    }

    private void HandleZoom()
    {
        float scrollInput = Input.mouseScrollDelta.y * zoomStep;
        Vector3 zoomDir = followOffset.normalized * scrollInput;
        Vector3 newFollowOffset = followOffset - zoomDir;

        if (newFollowOffset.magnitude < zoomOffsetMin)
        {
            newFollowOffset = followOffset.normalized * zoomOffsetMin;
        }
        else if (newFollowOffset.magnitude > zoomOffsetMax)
        {
            newFollowOffset = followOffset.normalized * zoomOffsetMax;
        }

        followOffset = Vector3.SmoothDamp(followOffset, newFollowOffset, ref zoomVelocity, zoomSmoothTime);

        if (followOffset.magnitude < zoomOffsetMin)
        {
            followOffset = followOffset.normalized * zoomOffsetMin;
        }
        else if (followOffset.magnitude > zoomOffsetMax)
        {
            followOffset = followOffset.normalized * zoomOffsetMax;
        }

        vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = followOffset;
    }
}
