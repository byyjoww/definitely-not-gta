using UnityEngine;

namespace DefinitelyNotGta.Movement
{
    public class PhysicsMovement
    {
        private Rigidbody rigidbody = default;
        private Axle[] axles = default;
        private SpeedInput speed = default;

        private float maxSpeed = 10f;
        private float maxMotorTorque = 1000f;
        private float maxTurnAngleBeforeSlow = 10f;        
        private float maxSteeringAngle = 55;

        public PhysicsMovement(Rigidbody rigidbody, Axle[] axles)
        {
            this.rigidbody = rigidbody;
            this.axles = axles;
            this.speed = new SpeedInput();
        }

        public void Accelerate()
        {
            float torque = maxMotorTorque;
            foreach (Axle axleInfo in axles)
            {
                if (axleInfo.HasMotor)
                {
                    axleInfo.LeftWheel.motorTorque = torque * speed.Current;
                    axleInfo.RightWheel.motorTorque = torque * speed.Current;
                }
            }

            if (rigidbody.velocity.magnitude > maxSpeed)
            {
                Vector3 velocity = rigidbody.velocity.normalized;
                velocity *= maxSpeed;

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
            float newSteer = (localDesiredVelocity.x / localDesiredVelocity.magnitude) * maxSteeringAngle;
            AdjustSpeedBasedOnTurnAngle(newSteer);
            float steer = movingForward ? newSteer : -newSteer;

            foreach (Axle axle in axles)
            {
                if (axle.HasSteering)
                {
                    axle.LeftWheel.steerAngle = steer;
                    axle.RightWheel.steerAngle = steer;
                }
            }

            Debug.DrawLine(rigidbody.transform.position, (rigidbody.transform.position) + localDesiredVelocity);
        }

        public void ApplyAntiFlip()
        {
            rigidbody.centerOfMass = new Vector3(0, 0, 0);
            rigidbody.AddForce(0, -500f, 0);
        }

        private void AdjustSpeedBasedOnTurnAngle(float turnAngle)
        {
            if (turnAngle > maxTurnAngleBeforeSlow || turnAngle < -maxTurnAngleBeforeSlow)
            {
                speed.Decrement();
            }
            else
            {
                speed.Increment();
            }
        }        
    }
}

