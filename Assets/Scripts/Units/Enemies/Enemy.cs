using System;
using System.Collections.Generic;
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
        [SerializeField] private TargetMovement _movement;
        [SerializeField] private EnemyAnimator _animator;
        [SerializeField] private ImmediateAttack _attack;
        [SerializeField] private FieldOfView _fieldOfView;

        private Dictionary<EnemyStateType, IEnemyState> _states = new(5);

        private IEnemyState _state;
        private EnemyStateType _stateType;

        public EventTrigger AttackTrigger => _attackTrigger;
        public Transform[] PatrolPoints => _patrolPoints;
        public TargetMovement Movement => _movement;
        public EnemyAnimator Animator => _animator;
        public ImmediateAttack Attack => _attack;
        public FieldOfView FieldOfView => _fieldOfView;

        private void Awake()
        {
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
