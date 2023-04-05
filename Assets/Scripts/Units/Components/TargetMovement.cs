using System;
using UnityEngine;
using UnityEngine.AI;

namespace AiTest.Units
{
    public class TargetMovement
    {
        public event Action OnMoveEnded;
        public event Action OnJumpStarted;
        public event Action OnJumpEnded;

        // TODO: Create enum or separate static class
        private const int WalkOnlyIndex = 1;
        private const int WalkAndJumpIndex = 5;

        private const float EndDistance = 0.1f;

        private readonly NavMeshAgent _agent;
        private Transform _target;

        private bool IsEnabled { get;  set; }
        private bool IsJumping { get; set; }
        public float Speed { get => _agent.speed; set => _agent.speed = value; }

        public TargetMovement(NavMeshAgent agent)
        {
            _agent = agent;
            Updater.AddListener(Update);
            Stop();
        }

        public void CanJump(bool state)
        {
            _agent.areaMask = state ? WalkAndJumpIndex : WalkOnlyIndex;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public void Stop()
        {
            _agent.isStopped = true;
            IsEnabled = false;
        }

        public bool CanMove() => _target != null && _agent.CalculatePath(_target.position, new());

        public void Move()
        {
            if (_target == null)
            {
                Debug.LogWarning("Try start move without target");
                return;
            }

            _agent.isStopped = false;
            IsEnabled = true;
            IsJumping = false;
            _agent.SetDestination(_target.position);
        }

        private void Update()
        {
            if (!IsEnabled)
                return;

            _agent.SetDestination(_target.position);

            CheckJump();
            CheckEnd();
        }

        private void CheckJump()
        {
            if (_agent.isOnOffMeshLink)
            {
                IsJumping = true;
                OnJumpStarted?.Invoke();
            }
            else if (IsJumping && !_agent.isOnOffMeshLink)
            {
                IsJumping = false;
                OnJumpEnded?.Invoke();
            }
        }

        private void CheckEnd()
        {
            if (_agent.remainingDistance <= EndDistance || _agent.pathStatus == NavMeshPathStatus.PathPartial || _agent.pathStatus == NavMeshPathStatus.PathInvalid)
            {
                OnMoveEnded?.Invoke();
                Stop();
            }
        }
    }
}
