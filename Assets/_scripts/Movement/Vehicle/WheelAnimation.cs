using DefinitelyNotGta.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace DefinitelyNotGta.Movement
{
    public class WheelAnimation
    {
        private Rigidbody rigidbody;
        private Dictionary<WheelCollider, Transform> wheels;

        public WheelAnimation(Rigidbody rigidbody, Axle[] axles, ITicker ticker)
        {
            this.rigidbody = rigidbody;
            wheels = new Dictionary<WheelCollider, Transform>();

            foreach (Axle axle in axles)
            {
                wheels.Add(axle.LeftWheel, axle.LeftWheel.transform.GetChild(0));
                wheels.Add(axle.RightWheel, axle.RightWheel.transform.GetChild(0));
            }

            ticker.OnTick += RotateWheels;
            ticker.OnTick += SpinWheels;
        }

        public void RotateWheels()
        {
            foreach (var kvp in wheels)
            {
                kvp.Value.localEulerAngles = new Vector3(0f, kvp.Key.steerAngle, 0f);
            }
        }

        public void SpinWheels()
        {
            foreach (var kvp in wheels)
            {
                Vector3 directionVector = rigidbody.transform.InverseTransformDirection(rigidbody.velocity);
                int direction = directionVector.z >= 0 ? 1 : -1;
                var circumference = (2 * Mathf.PI * kvp.Key.radius);
                kvp.Value.Rotate(rigidbody.velocity.magnitude * direction / circumference, 0f, 0f);
            }
        }
    }
}

