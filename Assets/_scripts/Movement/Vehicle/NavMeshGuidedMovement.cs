﻿using System;
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
        private Vector3? desiredPosition = null;

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
            Debug.Log($"Moving to: {position}");
            this.desiredPosition = position;
            navAgentMovement.Move(desiredPosition.Value);
        }

        public void Stop()
        {
            this.desiredPosition = null;
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
                Stop();
                physicsMovement.Break();
            }
        }

        private bool HasArrived()
        {
            return Vector3.Distance(navAgent.destination, navAgent.transform.position) < navAgent.stoppingDistance + float.Epsilon;
        }

        public void Dispose()
        {
            ticker.OnTick -= DoMove;
        }
    }
}

