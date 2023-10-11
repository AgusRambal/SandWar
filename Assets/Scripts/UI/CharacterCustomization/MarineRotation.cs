using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarineRotation : MonoBehaviour 
{
    //Usage for the CustomManager
    public List<Transform> transformsList = new List<Transform>();
    
    private Transform marineTransform;
    private Vector3 lastMousePosition;
    
    private void Awake() {
        marineTransform = transform;
    }

    private void Update() {
        if (Input.GetMouseButton(1)) {
            Vector3 mouseDelta = lastMousePosition - Input.mousePosition;
            
            // Fix huge jumps when Unity loses focus
            //mouseDelta.y = Mathf.Clamp(mouseDelta.y, -200, +200);
            mouseDelta.x = Mathf.Clamp(mouseDelta.x, -200, +200);

            float rotateSpeed = .2f;

            marineTransform.localEulerAngles += new Vector3(0f, mouseDelta.x, 0f) * rotateSpeed;

            float rotationXMin = -7f;
            float rotationXMax = +10f;

            float localEulerAnglesX = marineTransform.localEulerAngles.x;
            if (localEulerAnglesX > 180) {
                localEulerAnglesX -= 360f;
            }
            float rotationX = Mathf.Clamp(localEulerAnglesX, rotationXMin, rotationXMax);

            marineTransform.localEulerAngles = new Vector3(rotationX, marineTransform.localEulerAngles.y, marineTransform.localEulerAngles.z);
        }
        
        lastMousePosition = Input.mousePosition;
    }
}