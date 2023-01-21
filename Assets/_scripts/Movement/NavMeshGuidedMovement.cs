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
            if (hasDestination) { Stop(); }
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
            Vector3 magnitudeVector = transform.InverseTransformVector(desiredVelocity);
            physicsMovement.Turn(magnitudeVector);
            physicsMovement.Accelerate(magnitudeVector);
            navAgent.nextPosition = transform.position;

            Debug.Log(navAgent.nextPosition);

            Debug.DrawRay(transform.position, magnitudeVector, Color.red);
            Debug.DrawRay(transform.position, desiredVelocity, Color.green);

            if (hasDestination && HasArrived())
            {
                Stop();
                onArrival?.Invoke();
                onArrival?.RemoveAllListeners();
            }
        }

        private bool HasArrived()
        {
            var destination = new Vector3(navAgent.destination.x, navAgent.transform.position.y, navAgent.destination.z);
            var distance = Vector3.Distance(destination, navAgent.transform.position);
            return distance <= navAgent.stoppingDistance + float.Epsilon;
        }

        public void Dispose()
        {
            ticker.OnTick -= Move;
        }
    }
}

