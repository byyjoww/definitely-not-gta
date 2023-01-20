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
        private Vector3? desiredPosition = null;
        private UnityEvent onArrival = default;

        private bool isMoving => desiredPosition.HasValue;        

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
            ticker.OnTick += DoMove;
        }

        public UnityEvent Move(Vector3 position)
        {
            if (isMoving) { Stop(); }
            Debug.Log($"Moving to: {position}");
            this.desiredPosition = position;
            navAgentMovement.Move(desiredPosition.Value);
            onArrival = new UnityEvent();
            return onArrival;
        }

        public void Stop()
        {
            physicsMovement.Break();
            this.desiredPosition = null;            
            this.onArrival = null;
        }

        private void DoMove()
        {
            Vector3 desiredVelocity = desiredPosition.HasValue ? navAgent.desiredVelocity : Vector3.zero;
            Vector3 localDesiredVelocity = transform.InverseTransformVector(desiredVelocity);
            physicsMovement.Turn(localDesiredVelocity);
            physicsMovement.Accelerate(localDesiredVelocity);
            navAgent.nextPosition = transform.position;

            if (desiredPosition.HasValue && HasArrived())
            {
                var arrival = onArrival;
                Stop();
                arrival?.Invoke();
                arrival?.RemoveAllListeners();
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
            ticker.OnTick -= DoMove;
        }
    }
}

