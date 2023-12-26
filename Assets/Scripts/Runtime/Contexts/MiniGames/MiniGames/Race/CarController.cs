using System;
using UnityEngine;

namespace Runtime.Contexts.MiniGames.MiniGames.Race
{
    public class CarController : MonoBehaviour
    {
        public ushort clientId;
        private float horizontalInput, verticalInput;
        private float currentSteerAngle, currentbreakForce;
        private bool isBreaking;

        // Settings
        [SerializeField] private float motorForce, breakForce, maxSteerAngle;

        // Wheel Colliders
        [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
        [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

        // Wheels
        [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
        [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

        private void HandleMotor() {
            frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
            frontRightWheelCollider.motorTorque = verticalInput * motorForce;
            currentbreakForce = isBreaking ? breakForce : 0f;
            ApplyBreaking();
        }

        private void ApplyBreaking() {
            frontRightWheelCollider.brakeTorque = currentbreakForce;
            frontLeftWheelCollider.brakeTorque = currentbreakForce;
            rearLeftWheelCollider.brakeTorque = currentbreakForce;
            rearRightWheelCollider.brakeTorque = currentbreakForce;
        }

        private void HandleSteering() {
            currentSteerAngle = maxSteerAngle * horizontalInput;
            frontLeftWheelCollider.steerAngle = currentSteerAngle;
            frontRightWheelCollider.steerAngle = currentSteerAngle;
        }

        private void UpdateWheels() {
            UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
            UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
            UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
            UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        }

        private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform) {
            Vector3 pos;
            Quaternion rot; 
            wheelCollider.GetWorldPose(out pos, out rot);
            wheelTransform.rotation = rot;
            wheelTransform.position = pos;
        }

        public void SetValues(float verticalAxis, float horizontalAxis, bool space)
        {
            // Steering Input
            horizontalInput = horizontalAxis;

            // Acceleration Input
            verticalInput = verticalAxis;

            // Breaking Input
            isBreaking = space;
            
            HandleMotor();
            HandleSteering();
            UpdateWheels();
        }
    }
}