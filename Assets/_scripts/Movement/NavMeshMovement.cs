using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace DefinitelyNotGta.Movement
{
    public class NavMeshMovement : IMovable, IDisposable
    {
        private NavMeshAgent navAgent = default;
        private Transform transform = default;
        private float maxNavMeshSampleDistance = 10;
        private ITicker ticker = default;
        private Vector3? desiredPosition = null;
        private UnityEvent onArrival = default;

        private bool isMoving => desiredPosition.HasValue;

        public NavMeshMovement(NavMeshAgent navAgent, Transform transform, ITicker ticker)
        {
            this.navAgent = navAgent;
            this.transform = transform;
            this.ticker = ticker;
        }

        public UnityEvent Move(Vector3 position)
        {
            if (isMoving) { Stop(); }
            navAgent.isStopped = false;            
            if (NavMesh.SamplePosition(position, out NavMeshHit hit, maxNavMeshSampleDistance, -1))
            {
                desiredPosition = hit.position;
                navAgent.SetDestination(hit.position);
                onArrival = new UnityEvent();
                ticker.OnTick += CheckArrival;
                return onArrival;
            }
            return null;
        }

        public void Stop()
        {
            navAgent.SetDestination(transform.position);
            navAgent.isStopped = true;
            ticker.OnTick -= CheckArrival;
            desiredPosition = null;
            onArrival = null;
        }

        private void CheckArrival()
        {
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
            if (desiredPosition.HasValue) { Stop(); }
        }
    }
}

