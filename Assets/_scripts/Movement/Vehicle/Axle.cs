using System.Runtime.Serialization;
using UnityEngine;

namespace DefinitelyNotGta.Movement
{
    [System.Serializable]
    public class Axle
    {
        [Header("Settings")]
        [SerializeField] private bool hasMotor = default;
        [SerializeField] private bool hasSteering = default;
        [SerializeField] private bool hasBrakes = default;

        [Header("References")]
        [SerializeField] private Wheel leftWheel;
        [SerializeField] private Wheel rightWheel;

        public bool HasMotor => hasMotor;
        public bool HasSteering => hasSteering;
        public bool HasBrakes => hasBrakes;

        public float currentSteeringAngle => leftWheel.collider.steerAngle;

        private void UpdateWheelTransform(Wheel wheel)
        {
            Vector3 wheelPosition = default;
            Quaternion wheelRotation = default;

            wheel.collider.GetWorldPose(out wheelPosition, out wheelRotation);

            wheel.mesh.position = wheelPosition;
            wheel.mesh.rotation = wheelRotation;
        }

        public void SetTorque(float torque)
        {
            if (HasMotor)
            {
                leftWheel.collider.motorTorque = torque;
                rightWheel.collider.motorTorque = torque;
            }

            UpdateWheelTransform(leftWheel);
            UpdateWheelTransform(rightWheel);
        }

        public void SetSteer(float steer)
        {
            if (HasSteering)
            {
                leftWheel.collider.steerAngle = steer;
                rightWheel.collider.steerAngle = steer;
            }

            UpdateWheelTransform(leftWheel);
            UpdateWheelTransform(rightWheel);
        }

        public void SetBrakeTorque(float brakeTorque)
        {
            if (HasBrakes)
            {
                leftWheel.collider.brakeTorque = brakeTorque;
                rightWheel.collider.brakeTorque = brakeTorque;
            }

            UpdateWheelTransform(leftWheel);
            UpdateWheelTransform(rightWheel);
        }
    }

    [System.Serializable]
    public class Wheel
    {
        public WheelCollider collider;
        public Transform mesh;
    }
}

