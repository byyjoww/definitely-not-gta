using DefinitelyNotGta.Utils;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace DefinitelyNotGta.Movement
{
    public class NavMeshGuidedMovement : IMovable, IDisposable
    {
        private NavMeshMovement navAgentMovement = default;
        private PhysicsMovement physicsMovement = default;
        private NavMeshAgent navAgent = default;
        private Transform transform = default;
        private ITicker ticker = default;
        private Vector3? destination = null;
        private UnityEvent onArrival = default;

        private bool hasDestination => destination.HasValue;

        public NavMeshGuidedMovement(NavMeshMovement navAgentMovement, PhysicsMovement physicsMovement, NavMeshAgent navAgent, Transform transform, ITicker ticker)
        {
            this.navAgentMovement = navAgentMovement;
            this.physicsMovement = physicsMovement;
            this.navAgent = navAgent;
            navAgent.updatePosition = false;
            navAgent.updateRotation = false;
            navAgent.updateUpAxis = false;
            this.transform = transform;
            this.ticker = ticker;
            ticker.OnTick += Move;
        }

        public UnityEvent MoveTo(Vector3 position)
        {
            // If destination is already set them stop before reassign
            // if (hasDestination) { Stop(); }
            Debug.Log($"Moving to: {position}");
            this.destination = position;
            navAgentMovement.MoveTo(destination.Value);
            onArrival = new UnityEvent();
            return onArrival;
        }

        public void Stop()
        {
            physicsMovement.Brake();
            this.destination = null;
            this.onArrival = null;
        }

        private void Move()
        {
            Vector3 desiredVelocity = destination.HasValue ? navAgent.desiredVelocity : Vector3.zero;
            Vector3 magnitudeVector = transform.InverseTransformVector((destination.Value - transform.position)).normalized;
            physicsMovement.Turn(magnitudeVector);
            physicsMovement.Accelerate(new Vector3(1, 1, 1));
            navAgent.nextPosition = transform.position;

            Debug.DrawRay(transform.position, magnitudeVector, Color.red);
            Debug.DrawRay(transform.position, desiredVelocity, Color.green);

            if (hasDestination && HasArrived())
            {
                // Stop();
                onArrival?.Invoke();
                // onArrival?.RemoveAllListeners();
            }
        }

        private bool HasArrived()
        {
            var dest = new Vector3(destination.Value.x, 0, destination.Value.z);
            var ag = new Vector3(navAgent.transform.position.x, 0, navAgent.transform.position.z);
            var distance = Vector3.Distance(dest, ag);

            Debug.Log($"distance {distance}");
            return distance <= 2;
        }

        public void Dispose()
        {
            ticker.OnTick -= Move;
        }
    }
}

