using System;
using AiTest.Units.Enemies.Components;
using AiTest.Units.FieldsOfView;
using UnityEngine;

namespace AiTest.Units.Enemies.States
{
    public class EnemyIdleState : IEnemyState
    {
        private event Action<EnemyStateType> OnStateSwitchRequested;

        private readonly FieldOfView _fieldOfView;
        private readonly TargetMovement _movement;

        private readonly float _waitTime;
        private float _currentTime;

        public EnemyIdleState(Action<EnemyStateType> onStateSwitchRequested, FieldOfView fieldOfView, TargetMovement movement, float waitTime)
        {
            OnStateSwitchRequested = onStateSwitchRequested;
            _fieldOfView = fieldOfView;
            _movement = movement;
            _waitTime = waitTime;
        }

        public void OnStateEnter()
        {
            _currentTime = _waitTime;
            _fieldOfView.OnFounded += (u) => OnStateSwitchRequested?.Invoke(EnemyStateType.TargetMoving);
            Updater.AddListener(Update);
            _movement.Stop();
        }

        private void Update()
        {
            _currentTime -= Time.deltaTime;

            if (_currentTime <= 0)
                OnStateSwitchRequested?.Invoke(EnemyStateType.Patrol);
        }

        public void OnStateExit()
        {
            _fieldOfView.OnFounded -= (u) => OnStateSwitchRequested.Invoke(EnemyStateType.TargetMoving);
            Updater.RemoveListener(Update);
        }
    }
}
