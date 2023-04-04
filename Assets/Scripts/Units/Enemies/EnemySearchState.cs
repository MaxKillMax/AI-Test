using System;
using UnityEngine;

namespace AiTest.Units.Enemies
{
    public class EnemySearchState : IEnemyState
    {
        private event Action<EnemyStateType> OnStateSwitchRequested;

        private readonly FieldOfView _fieldOfView;
        private readonly TargetMovement _movement;
        private readonly EnemyAnimator _animator;
        private readonly float _movementSpeed;

        private readonly float _searchTime;
        private float _currentTime;

        private Unit _target;

        public EnemySearchState(Action<EnemyStateType> onStateSwitchRequested, FieldOfView fieldOfView, TargetMovement movement, EnemyAnimator animator, float searchTime, float movementSpeed)
        {
            OnStateSwitchRequested = onStateSwitchRequested;
            _fieldOfView = fieldOfView;
            _movement = movement;
            _animator = animator;
            _searchTime = searchTime;
            _movementSpeed = movementSpeed;

            fieldOfView.OnMissed += RememberMissedTarget;
        }

        private void RememberMissedTarget(Unit unit)
        {
            _target = unit;
        }

        public void OnStateEnter()
        {
            if (_target == null)
            {
                Debug.LogWarning("Try start search without missed target");
                OnStateSwitchRequested?.Invoke(EnemyStateType.Idle);
                return;
            }

            if (_fieldOfView.FoundedUnits.Count > 0)
            {
                OnStateSwitchRequested?.Invoke(EnemyStateType.TargetMoving);
                return;
            }

            Updater.AddListener(Update);
            _fieldOfView.OnFounded += (u) => OnStateSwitchRequested?.Invoke(EnemyStateType.TargetMoving);
            _movement.Speed = _movementSpeed;
            _movement.SetTarget(_target.transform);
            _movement.Move();
            _animator.SetLerpSpeed(1);

            _currentTime = _searchTime;
        }

        private void Update()
        {
            _currentTime -= Time.deltaTime;

            if (_currentTime <= 0)
                OnStateSwitchRequested?.Invoke(EnemyStateType.Idle);
        }

        public void OnStateExit()
        {
            Updater.RemoveListener(Update);
            _fieldOfView.OnFounded -= (u) => OnStateSwitchRequested?.Invoke(EnemyStateType.TargetMoving);
            _movement.Stop();
        }
    }
}
