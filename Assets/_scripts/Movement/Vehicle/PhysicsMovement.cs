using UnityEngine;

namespace DefinitelyNotGta.Movement
{
    public class PhysicsMovement
    {
        [System.Serializable]
        public class Config
        {
            [SerializeField] private float maxSpeed = 5f;
            [SerializeField] private float maxSteeringAngle = 55f;
            [SerializeField] private float motorTorque = 800f;
            [SerializeField] private float breakTorque = 10000f;
            [SerializeField] private float maxTurnAngleBeforeSlow = 35f;

            public float MaxSpeed => maxSpeed;
            public float MotorTorque => motorTorque;
            public float BreakTorque => breakTorque;
            public float MaxSteeringAngle => maxSteeringAngle;
            public float MaxTurnAngleBeforeSlow => maxTurnAngleBeforeSlow;
        }

        private Rigidbody rigidbody = default;
        private Axle[] axles = default;
        private SpeedInput speed = default;
        private Config config = default;
        private bool overMaxTurnAngle = false;

        public PhysicsMovement(Rigidbody rigidbody, Axle[] axles, Config config)
        {
            this.rigidbody = rigidbody;
            this.axles = axles;
            this.config = config;
            this.speed = new SpeedInput();
        }

        public void Accelerate(Vector3 magnitudeVector)
        {
            if (overMaxTurnAngle || magnitudeVector == Vector3.zero)
            {
                speed.Decrement();
            }
            else
            {
                speed.Increment();
            }

            foreach (Axle axle in axles)
            {
                axle.SetTorque(config.MotorTorque * speed.Current);
            }

            if (rigidbody.velocity.magnitude > config.MaxSpeed)
            {
                Vector3 velocity = rigidbody.velocity.normalized;
                velocity *= config.MaxSpeed;

                rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, velocity, 0.125f);
            }

            if (speed.Current == 0)
            {
                float friction = Mathf.Lerp(rigidbody.velocity.z, 0, 0.001f);
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, friction);
            }
        }

        public void Turn(Vector3 localDesiredVelocity)
        {
            bool movingForward = localDesiredVelocity.magnitude > 0;
            float newSteer = (localDesiredVelocity.x / localDesiredVelocity.magnitude) * config.MaxSteeringAngle;
            AdjustSpeedBasedOnTurnAngle(newSteer);
            float steer = movingForward ? newSteer : -newSteer;

            foreach (Axle axle in axles)
            {
                axle.SetSteer(Mathf.Lerp(axle.currentSteeringAngle, steer, 0.09f));
            }

            Debug.DrawLine(rigidbody.transform.position, (rigidbody.transform.position) + localDesiredVelocity);
        }

        public void Brake()
        {
            foreach (Axle axle in axles)
            {
                axle.SetBrakeTorque(config.BreakTorque);
            }
        }

        public void ApplyAntiFlip()
        {
            rigidbody.centerOfMass = new Vector3(0, 0, 0);
            rigidbody.AddForce(0, -500f, 0);
        }

        private void AdjustSpeedBasedOnTurnAngle(float turnAngle)
        {
            overMaxTurnAngle = turnAngle > config.MaxTurnAngleBeforeSlow || turnAngle < -config.MaxTurnAngleBeforeSlow;
        }
    }
}

