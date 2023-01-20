using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DefinitelyNotGta.Movement
{
    public class NavMeshMovement : IMovable
    {
        private NavMeshAgent navAgent = default;
        private Transform transform = default;
        private float maxNavMeshSampleDistance = 10;

        public NavMeshMovement(NavMeshAgent navAgent, Transform transform)
        {
            this.navAgent = navAgent;
            this.transform = transform;
        }

        public void Move(Vector3 position)
        {
            navAgent.isStopped = false;
            if (NavMesh.SamplePosition(position, out NavMeshHit hit, maxNavMeshSampleDistance, -1))
            {
                navAgent.SetDestination(hit.position);
            }
        }

        public void Stop()
        {
            navAgent.SetDestination(transform.position);
            navAgent.isStopped = true;
        }
    }
}

