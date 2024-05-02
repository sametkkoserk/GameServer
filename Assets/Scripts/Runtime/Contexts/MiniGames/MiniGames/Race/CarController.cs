using UnityEngine;

namespace Runtime.Contexts.MiniGames.MiniGames.Race
{
    public class CarController : MonoBehaviour
    {
        public MiniGameController miniGameController;
        public ushort clientId;
        private float horizontalInput, verticalInput;
        private float currentSteerAngle, currentbreakForce;
        private bool isBreaking;
        private int currentState = -1;
        public bool isFlipped;
        public float flippedTimer = 3f;

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


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("CheckPoint"))
            {
                int _index = other.gameObject.GetComponent<CheckPointController>().index;
                if (_index == currentState + 1)
                {
                    Debug.Log(other.gameObject.tag);
                    currentState = _index;
                    miniGameController.SetPlayerState(clientId,currentState);
                }
            }
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

        private void Update()
        {
            isFlipped = (Mathf.Abs(transform.rotation.eulerAngles.x)>80&&Mathf.Abs(transform.rotation.eulerAngles.x)<280)||(Mathf.Abs(transform.rotation.eulerAngles.z)>80&&Mathf.Abs(transform.rotation.eulerAngles.z)<280);
            if (isFlipped)
            {
                flippedTimer -= Time.deltaTime;
            }
            else
            {
                flippedTimer = 3f;
            }

            if (flippedTimer < 0)
            {
                transform.position=miniGameController.miniGameMapGenerationVo.checkPointsPos[currentState >= 0 ? currentState : 0].ToVector3();
                transform.rotation = miniGameController.miniGameMapGenerationVo.checkPointsRot[currentState >= 0 ? currentState : 0].ToQuaternion();
            }
        }
    }
}