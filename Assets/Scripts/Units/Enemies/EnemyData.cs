using System;
using System.Collections.Generic;
using AiTest.Units.Enemies.States;
using AiTest.Units.FieldsOfView;
using UnityEngine;

namespace AiTest.Units.Enemies
{
    [CreateAssetMenu(fileName = nameof(EnemyData), menuName = nameof(EnemyData), order = 51)]
    public class EnemyData : ScriptableObject
    {
        [SerializeField, Range(0, 100)] private float _idleTime = 3;
        [SerializeField, Range(0, 100)] private float _patrolSpeed = 4;
        [SerializeField, Range(0, 100)] private float _targetMovingSpeed = 8;
        [SerializeField, Range(0, 100)] private float _searchSpeed = 8;
        [SerializeField, Range(0, 100)] private float _searchTime = 3;
        [SerializeField, Range(0.1f, 100)] private float _attackRange = 1.5f;
        [SerializeField] private FieldOfViewData _fieldOfViewData;
        [SerializeField] private UnitType _attackTargetType;
        [SerializeField] private EnemyStateType _startState = EnemyStateType.Idle;

        public float MaxSpeed => Mathf.Max(new float[] { _patrolSpeed, _targetMovingSpeed, _searchSpeed });
        public float AttackRange => _attackRange;
        public UnitType AttackTargetType => _attackTargetType;
        public FieldOfViewData FieldOfViewData => _fieldOfViewData;

        public Dictionary<EnemyStateType, IEnemyState> GetStates(Enemy enemy, out EnemyStateType startState)
        {
            startState = _startState;

            return new()
            {
                { EnemyStateType.Idle, new EnemyIdleState(enemy.SetState, enemy.FieldOfView, enemy.Movement, _idleTime) },
                { EnemyStateType.Patrol, new EnemyPatrolState(enemy.SetState, enemy.FieldOfView, enemy.Movement, _patrolSpeed, enemy.PatrolPoints) },
                { EnemyStateType.Attack, new EnemyAttackState(enemy.SetState, enemy.AttackTrigger, enemy.Attack) },
                { EnemyStateType.Search, new EnemySearchState(enemy.SetState, enemy.FieldOfView, enemy.Movement, _searchTime, _searchSpeed) },
                { EnemyStateType.TargetMoving, new EnemyTargetMovingState(enemy.SetState, enemy.FieldOfView, enemy.Movement, enemy.Animator, enemy.AttackTrigger, enemy.Attack, _targetMovingSpeed) }
            };
        }
    }
}
