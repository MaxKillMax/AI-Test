using System;
using System.Collections.Generic;
using AiTest.Units.Components;
using AiTest.Units.Enemies.Components;
using AiTest.Units.Enemies.States;
using AiTest.Units.FieldsOfView;
using UnityEngine;
using UnityEngine.AI;

namespace AiTest.Units.Enemies
{
    public class Enemy : Unit
    {
        [SerializeField] private EnemyData _data;

        [SerializeField] private Transform[] _patrolPoints;
        [SerializeField] private EventTrigger _attackTrigger;
        [SerializeField] private EventTrigger _fieldOfViewTrigger;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private EnemyAnimator _animator;

        private EnemyAgentSpeedAnimator _speedAnimator;
        private TargetMovement _movement;
        private ImmediateAttack _attack;
        private FieldOfView _fieldOfView;

        private Dictionary<EnemyStateType, IEnemyState> _states = new(5);

        private EnemyStateType _stateType;
        private IEnemyState _state;

        public EventTrigger AttackTrigger => _attackTrigger;
        public Transform[] PatrolPoints => _patrolPoints;
        public TargetMovement Movement => _movement;
        public EnemyAnimator Animator => _animator;
        public ImmediateAttack Attack => _attack;
        public FieldOfView FieldOfView => _fieldOfView;

        private void Awake()
        {
            _speedAnimator = new(_agent, _animator, _data.MaxSpeed);
            _attackTrigger.transform.localScale = new(_data.AttackRange, _data.AttackRange, _data.AttackRange);
            _movement = new(_agent);
            _attack = new(_data.AttackTargetType);
            _fieldOfView = new(_fieldOfViewTrigger, _data.FieldOfViewData.Angle, _data.FieldOfViewData.Length, _data.FieldOfViewData.SearchingUnitType);

            _states = _data.GetStates(this, out EnemyStateType startState);
            SetState(startState);
        }

        public void SetState(EnemyStateType stateType)
        {
            if (!_states.TryGetValue(stateType, out IEnemyState newState))
                throw new NotImplementedException();

            if (_state == newState)
                return;

            _state?.OnStateExit();
            _stateType = stateType;
            _state = newState;
            _state.OnStateEnter();
        }
    }
}
