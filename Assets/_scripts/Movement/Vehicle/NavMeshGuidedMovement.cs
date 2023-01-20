using System;
using UnityEngine;
using UnityEngine.AI;

namespace DefinitelyNotGta.Movement
{
    public class NavMeshGuidedMovement : IMovable, IDisposable
    {        
        private NavMeshMovement navAgentMovement = default;
        private PhysicsMovement physicsMovement = default;
        private NavMeshAgent navAgent = default;
        private Transform transform = default;
        private ITicker ticker = default;
        private Vector3? desiredPosition = default;

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

        public void Move(Vector3 position)
        {
            this.desiredPosition = position;
        }

        public void Stop()
        {
            this.desiredPosition = null;
        }

        private void DoMove()
        {
            if (desiredPosition.HasValue) { navAgentMovement.Move(desiredPosition.Value); }
            Vector3 desiredVelocity = desiredPosition.HasValue ? navAgent.desiredVelocity : Vector3.zero;
            Vector3 localDesiredVelocity = transform.InverseTransformVector(desiredVelocity);
            physicsMovement.Turn(localDesiredVelocity);
            physicsMovement.Accelerate();
            navAgent.nextPosition = transform.position;
        }

        public void Dispose()
        {
            ticker.OnTick -= DoMove;
        }
    }
}

