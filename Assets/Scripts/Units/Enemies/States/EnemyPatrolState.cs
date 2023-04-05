using System;
using AiTest.Units.Enemies.Components;
using AiTest.Units.FieldsOfView;
using UnityEngine;

namespace AiTest.Units.Enemies.States
{
    public class EnemyPatrolState : IEnemyState
    {
        private event Action<EnemyStateType> OnStateSwitchRequested;

        private readonly FieldOfView _fieldOfView;
        private readonly TargetMovement _movement;
        private readonly float _movementSpeed;
        private readonly Transform[] _points;

        private Transform RandomPoint => _points[UnityEngine.Random.Range(0, _points.Length)];

        public EnemyPatrolState(Action<EnemyStateType> onStateSwitchRequested, FieldOfView fieldOfView, TargetMovement movement, float movementSpeed, Transform[] points)
        {
            OnStateSwitchRequested = onStateSwitchRequested;
            _fieldOfView = fieldOfView;
            _movement = movement;
            _movementSpeed = movementSpeed;
            _points = points;
        }

        public void OnStateEnter()
        {
            _movement.SetTarget(RandomPoint);

            if (!_movement.CanMove())
            {
                OnStateSwitchRequested?.Invoke(EnemyStateType.Patrol);
                return;
            }

            _movement.Speed = _movementSpeed;
            _movement.OnMoveEnded += () => OnStateSwitchRequested?.Invoke(EnemyStateType.Idle);
            _fieldOfView.OnFounded += (u) => OnStateSwitchRequested?.Invoke(EnemyStateType.TargetMoving);
            _movement.Move();
        }

        public void OnStateExit()
        {
            _movement.OnMoveEnded -= () => OnStateSwitchRequested?.Invoke(EnemyStateType.Idle);
            _fieldOfView.OnFounded -= (u) => OnStateSwitchRequested?.Invoke(EnemyStateType.TargetMoving);
            _movement.Stop();
        }
    }
}
