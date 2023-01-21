using DefinitelyNotGta.Movement;
using DefinitelyNotGta.Utils;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace DefinitelyNotGta.Vehicles
{
    public class Automobile : MonoBehaviour, IVehicle, ITicker
    {
        [SerializeField] private PhysicsMovement.Config config = default;
        [SerializeField] private Transform seat = default;
        [SerializeField] private Transform exit = default;
        [SerializeField] private NavMeshAgent navAgent = default;
        [SerializeField] private new Rigidbody rigidbody = default;
        [SerializeField] private Axle[] axles = default;
        [SerializeField] private Transform steertingAxis;

        private IDriver driver = default;
        private IMovable movement = default;

        public event UnityAction OnTick;

        private void Awake()
        {
            var navMeshMovement = new NavMeshMovement(navAgent, rigidbody.transform, this);
            var physicsMovement = new PhysicsMovement(rigidbody, axles, config);
            movement = new NavMeshGuidedMovement(navMeshMovement, physicsMovement, navAgent, steertingAxis, this);
        }

        private void FixedUpdate()
        {
            OnTick?.Invoke();
        }

        public void StartDriving(IDriver driver)
        {
            if (this.driver != null)
            {
                Debug.LogError($"Vehicle {name} already has a driver.");
                return;
            }

            this.driver = driver;
            this.driver.EnterVehicle(seat);
        }

        public IDriver StopDriving()
        {
            if (driver == null)
            {
                Debug.LogError($"Vehicle {name} doesn't have a driver.");
                return null;
            }

            driver.ExitVehicle(exit);
            IDriver current = this.driver;
            driver = null;
            return current;
        }

        public UnityEvent MoveTo(Vector3 position)
        {
            return movement.MoveTo(position);
        }

        public void Stop()
        {
            movement.Stop();
        }

        private void OnDestroy()
        {
            if (movement is IDisposable mDisposable) { mDisposable.Dispose(); }
        }

        private void OnValidate()
        {
            if (navAgent == null) { navAgent = GetComponent<NavMeshAgent>(); }
        }
    }
}
