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
        [SerializeField] private WheelCollider leftWheel = default;
        [SerializeField] private WheelCollider rightWheel = default;

        public WheelCollider LeftWheel => leftWheel;
        public WheelCollider RightWheel => rightWheel;
        public bool HasMotor => hasMotor;
        public bool HasSteering => hasSteering;
        public bool HasBrakes => hasBrakes;

        public void SetAxle(WheelCollider leftWheel, WheelCollider rightWheel)
        {
            this.leftWheel = leftWheel;
            this.rightWheel = rightWheel;
        }
    }
}

