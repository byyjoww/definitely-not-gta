using DefinitelyNotGta.Movement;
using DefinitelyNotGta.Vehicles;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace DefinitelyNotGta.Units
{
    public class Player : MonoBehaviour, IDriver, IMovable, ITeleportable, ITicker
    {
        [SerializeField] private GameObject model = default;
        [SerializeField] private NavMeshAgent navAgent = default;
        [SerializeField] private new Collider collider = default;

        private IMovable movement = default;

        public event UnityAction OnTick;

        private void Awake()
        {
            movement = new NavMeshMovement(navAgent, transform, this);
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

        public UnityEvent Move(Vector3 position)
        {
            return movement.Move(position);
        }

        public void Stop()
        {
            movement.Stop();
        }

        private void Update()
        {
            OnTick?.Invoke();
        }

        private void OnValidate()
        {
            if (navAgent == null) { navAgent = GetComponent<NavMeshAgent>(); }
            if (collider == null) { collider = GetComponent<Collider>(); }
        }
    }    
}