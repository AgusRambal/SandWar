using System.Collections;
using Interfaces;
using UnityEngine;
using VehicleSystem.Transports;

namespace Transports.Ground
{
    public class Vehicle : Transport  
    {
        private float wheelRotationAmount = 0f;
        [SerializeField] private Transform leftFrontWheel,rightFrontWheel,leftRearWheel,rightRearWheel;

        #region ConstNumbers

        private const float WheelRotationFactor = 5f;
        private const float RotationLerpSpeed = 5f;
        private const float TiltAngle = 10f;  
        private const float TiltSpeed = 2f;

        #endregion
        
        protected override void Awake()
        {
            base.Awake();
            
            NavMeshAgent.acceleration = 4;
            NavMeshAgent.speed = 4;
            NavMeshAgent.angularSpeed = 0; 
            NavMeshAgent.updateRotation = false; 
            NavMeshAgent.autoBraking = true;
        }
        
        private void Update()
        {
            if (BasicStates == BasicStates.On && ImSelected)
            {
                 if (Input.GetMouseButtonDown(1))
                 {
                     SetDestinationToPointUnderCursor();
                 }
                 OrientTowardsDestination();                
            }
            RotateFrontWheelsBasedOnDirection();
            UpdateWheelRotationBasedOnSpeed();
        }
        
        private void RotateFrontWheelsBasedOnDirection()
        {
            if (NavMeshAgent && NavMeshAgent.hasPath)
            {
                Vector3 relativeDirection = transform.InverseTransformDirection(NavMeshAgent.desiredVelocity);
                float steeringAngle = Mathf.Atan2(relativeDirection.x, relativeDirection.z) * Mathf.Rad2Deg;

                
                Quaternion targetLeftRotation = Quaternion.Euler(0, steeringAngle, 0);
                Quaternion targetRightRotation = Quaternion.Euler(0, steeringAngle, 0);

                leftFrontWheel.localRotation = Quaternion.Lerp(leftFrontWheel.localRotation, targetLeftRotation, Time.deltaTime * 5f); 
                rightFrontWheel.localRotation = Quaternion.Lerp(rightFrontWheel.localRotation, targetRightRotation, Time.deltaTime * 5f);
            }
            else
            {
                leftFrontWheel.localRotation = Quaternion.Euler(0, 0, 0);
                rightFrontWheel.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
        private void UpdateWheelRotationBasedOnSpeed()
        {
            float speed = NavMeshAgent.velocity.magnitude;

            wheelRotationAmount += speed * WheelRotationFactor * Time.deltaTime;
            wheelRotationAmount %= 360f; 

            RotateWheel(leftFrontWheel);
            RotateWheel(rightFrontWheel);
            RotateWheel(leftRearWheel);
            RotateWheel(rightRearWheel);
        }
        private void RotateWheel(Transform wheel)
        {
            Vector3 wheelRotationVector = new Vector3(wheelRotationAmount, 0, 0);
            wheel.localRotation *= Quaternion.Euler(wheelRotationVector);
        }
        private void OrientTowardsDestination()
        {
            if (NavMeshAgent.velocity.sqrMagnitude > Mathf.Epsilon)
            {
                Quaternion lookRotation = Quaternion.LookRotation(NavMeshAgent.velocity.normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, RotationLerpSpeed * Time.deltaTime);
            }
            
            ApplyTilt();
        }
        private void ApplyTilt()
        {
            float tiltAmount = Vector3.Dot(transform.right, NavMeshAgent.velocity.normalized) * TiltAngle;
            Quaternion currentRotation = transform.rotation;
            Quaternion targetTilt = Quaternion.Euler(currentRotation.eulerAngles.x + tiltAmount, currentRotation.eulerAngles.y, currentRotation.eulerAngles.z - tiltAmount);
            
            transform.rotation = Quaternion.Slerp(currentRotation, targetTilt, TiltSpeed * Time.deltaTime);
        }
        private void SetDestinationToPointUnderCursor()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                NavMeshAgent.SetDestination(hit.point);
            }
        }
        
    }
}
