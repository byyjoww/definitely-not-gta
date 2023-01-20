using DefinitelyNotGta.Movement;
using DefinitelyNotGta.Vehicles;
using UnityEngine;
using UnityEngine.AI;

namespace DefinitelyNotGta.Units
{
    public class Player : MonoBehaviour, IDriver, IMovable, ITeleportable
    {
        [SerializeField] private GameObject model = default;
        [SerializeField] private NavMeshAgent navAgent = default;
        [SerializeField] private new Collider collider = default;

        private IMovable movement = default;

        private void Awake()
        {
            movement = new NavMeshMovement(navAgent, transform);
        }

        public void Teleport(Vector3 position)
        {
            navAgent.enabled = false;
            gameObject.transform.position = position;
            navAgent.enabled = true;
        }

        public void EnterVehicle(Transform seat)
        {
            navAgent.enabled = false;
            collider.enabled = false;
            gameObject.transform.position = seat.position;
            gameObject.transform.SetParent(seat);
        }

        public void ExitVehicle(Transform exit)
        {
            gameObject.transform.SetParent(null);
            gameObject.transform.position = exit.position;
            collider.enabled = true;
            navAgent.enabled = true;            
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
            if (collider == null) { collider = GetComponent<Collider>(); }
        }
    }    
}