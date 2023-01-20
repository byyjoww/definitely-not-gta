using DefinitelyNotGta.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace DefinitelyNotGta.Vehicles
{
    public class Automobile : MonoBehaviour, IVehicle
    {
        [SerializeField] private Transform seat = default;
        [SerializeField] private Transform exit = default;
        [SerializeField] private NavMeshAgent navAgent = default;

        private IDriver driver = default;
        private IMovable movement = default;

        private void Awake()
        {
            movement = new NavMeshMovement(navAgent, transform);
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

        public void Move(Vector3 position)
        {
            movement.Move(position);
        }

        public void Stop()
        {
            movement.Stop();
        }

        private void OnValidate()
        {
            if (navAgent == null) { navAgent = GetComponent<NavMeshAgent>(); }
        }
    }
}
