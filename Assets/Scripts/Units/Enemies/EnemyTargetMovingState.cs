using System;
using UnityEngine;

namespace AiTest.Units.Enemies
{
    public class EnemyTargetMovingState : IEnemyState
    {
        private event Action<EnemyStateType> OnStateSwitchRequested;

        private readonly FieldOfView _fieldOfView;
        private readonly TargetMovement _movement;
        private readonly EnemyAnimator _animator;
        private readonly EventTrigger _attackTrigger;
        private readonly ImmediateAttack _attack;
        private readonly float _movementSpeed;

        public EnemyTargetMovingState(Action<EnemyStateType> onStateSwitchRequested, FieldOfView fieldOfView, TargetMovement movement, EnemyAnimator animator, EventTrigger attackTrigger, ImmediateAttack attack, float movementSpeed)
        {
            OnStateSwitchRequested = onStateSwitchRequested;

            _attackTrigger = attackTrigger;
            _fieldOfView = fieldOfView;
            _movement = movement;
            _animator = animator;
            _attack = attack;
            _movementSpeed = movementSpeed;
        }

        public void OnStateEnter()
        {
            if (_fieldOfView.FoundedUnits.Count == 0)
            {
                OnStateSwitchRequested?.Invoke(EnemyStateType.Idle);
                return;
            }

            _attackTrigger.OnTriggerEntered += CheckCollider;
            _fieldOfView.OnMissed += (u) => OnStateSwitchRequested?.Invoke(EnemyStateType.Search);
            _animator.SetLerpSpeed(1);
            _movement.Speed = _movementSpeed;
            _movement.OnJumpStarted += StartFall;
            _movement.OnJumpEnded += EndFall;
            _movement.SetTarget(_fieldOfView.FoundedUnits[0].transform);
            _movement.CanJump(true);
            _movement.Move();
        }

        private void CheckCollider(Collider collider)
        {
            if (collider.TryGetComponent(out Unit unit) && unit.Type == _attack.TargetType)
                OnStateSwitchRequested?.Invoke(EnemyStateType.Attack);
        }

        private void StartFall()
        {
            _animator.StartFall();
        }

        private void EndFall()
        {
            _movement.Stop();
            _animator.EndFall();
            EndFallBehaviour.OnEndFallEnded += ResumeMoveAfterFall;
        }

        private void ResumeMoveAfterFall()
        {
            EndFallBehaviour.OnEndFallEnded -= ResumeMoveAfterFall;

            _movement.Move();
        }

        public void OnStateExit()
        {
            _attackTrigger.OnTriggerEntered -= CheckCollider;
            _fieldOfView.OnMissed -= (u) => OnStateSwitchRequested?.Invoke(EnemyStateType.Search);
            _movement.OnJumpStarted -= StartFall;
            _movement.OnJumpEnded -= EndFall;
            _movement.CanJump(false);
            _movement.Stop();
        }
    }
}
