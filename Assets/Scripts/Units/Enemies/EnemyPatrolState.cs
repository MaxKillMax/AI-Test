using System;
using UnityEngine;

namespace AiTest.Units.Enemies
{
    public class EnemyPatrolState : IEnemyState
    {
        private event Action<EnemyStateType> OnStateSwitchRequested;

        private readonly FieldOfView _fieldOfView;
        private readonly TargetMovement _movement;
        private readonly EnemyAnimator _animator;
        private readonly float _movementSpeed;
        private readonly Transform[] _points;

        private Transform RandomPoint => _points[UnityEngine.Random.Range(0, _points.Length)];

        public EnemyPatrolState(Action<EnemyStateType> onStateSwitchRequested, FieldOfView fieldOfView, TargetMovement movement, EnemyAnimator animator, float movementSpeed, Transform[] points)
        {
            OnStateSwitchRequested = onStateSwitchRequested;
            _fieldOfView = fieldOfView;
            _movement = movement;
            _animator = animator;
            _movementSpeed = movementSpeed;
            _points = points;
        }

        public void OnStateEnter()
        {
            _movement.Speed = _movementSpeed;
            _movement.OnMoveEnded += () => OnStateSwitchRequested?.Invoke(EnemyStateType.Idle);
            _fieldOfView.OnFounded += (u) => OnStateSwitchRequested?.Invoke(EnemyStateType.TargetMoving);
            _movement.SetTarget(RandomPoint);
            _movement.Move();
            _animator.SetLerpSpeed(0.5f);
        }

        public void OnStateExit()
        {
            _movement.OnMoveEnded -= () => OnStateSwitchRequested?.Invoke(EnemyStateType.Idle);
            _fieldOfView.OnFounded -= (u) => OnStateSwitchRequested?.Invoke(EnemyStateType.TargetMoving);
            _movement.Stop();
            _animator.SetLerpSpeed(0);
        }
    }
}
