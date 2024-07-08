using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.MeshFusionPro.Example
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private float mouseSensitivity = 5f;

        [SerializeField]
        private float movementSpeed = 5f;

        [SerializeField]
        private float _height = 5f;

        private float _xRotation = 0f;
        private float _currentHeight;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;

            _currentHeight = transform.parent.position.y;
        }

        private void Update()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 100 * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 100 * Time.deltaTime;

            mouseX = Mathf.Clamp(mouseX, -10, 10);
            mouseY = Mathf.Clamp(mouseY, -10, 10);

            Rotate(mouseX, mouseY);

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 movement = CalculateMovement(horizontal, vertical);
            Vector3 position = transform.parent.position;

            _currentHeight = Mathf.Lerp(_currentHeight, GetHeight(), movementSpeed * Time.deltaTime);

            position += movement * movementSpeed * Time.deltaTime;
            position.y = _currentHeight;

            MoveToPoint(position);
        }


        private void Rotate(float mouseX, float mouseY)
        {
            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            transform.parent.Rotate(Vector3.up * mouseX);
        }

        private Vector3 CalculateMovement(float horizontal, float vertical)
        {
            Vector3 movement = transform.right * horizontal + transform.forward * vertical;

            movement.y = 0f;

            return movement.normalized;
        }

        private void MoveToPoint(Vector3 targetPoint)
        {
            Vector3 start = transform.parent.position;
            start.y = targetPoint.y;

            Ray ray = new Ray(start, targetPoint - start);

            if (Physics.Raycast(ray, out RaycastHit hit, 3f))
            {
                return;
            }

            transform.parent.position = targetPoint;
        }

        private float GetHeight()
        {
            Ray ray = new Ray(transform.parent.position + Vector3.up * 20, -transform.parent.up);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                return hit.point.y + _height;
            }

            return _height;
        }
    }
}