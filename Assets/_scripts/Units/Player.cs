using DefinitelyNotGta.Movement;
using DefinitelyNotGta.Utils;
using DefinitelyNotGta.Vehicles;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace DefinitelyNotGta.Units
{
    public class Player : MonoBehaviour, IDriver, IMovable, ITicker
    {
        [SerializeField] private Animator animator = default;
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
            gameObject.transform.rotation = seat.rotation;
            gameObject.transform.SetParent(seat);
        }

        public void ExitVehicle(Transform exit)
        {
            gameObject.transform.SetParent(null);
            gameObject.transform.position = exit.position;
            gameObject.transform.rotation = exit.rotation;
            collider.enabled = true;
            navAgent.enabled = true;
        }

        public UnityEvent MoveTo(Vector3 position)
        {
            var m = movement.MoveTo(position);
            SetMoving();
            m.AddListener(SetIdle);
            return m;
        }

        public void Stop()
        {
            movement.Stop();
        }

        private void Update()
        {
            OnTick?.Invoke();
        }

        private void SetIdle()
        {
            animator.SetBool("isMoving", false);
        }

        private void SetMoving()
        {
            animator.SetBool("isMoving", true);
        }

        private void OnValidate()
        {
            if (navAgent == null) { navAgent = GetComponent<NavMeshAgent>(); }
            if (collider == null) { collider = GetComponent<Collider>(); }
        }
    }
}