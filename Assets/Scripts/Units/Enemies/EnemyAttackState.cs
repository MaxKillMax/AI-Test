using System;
using System.Collections.Generic;
using System.Linq;

namespace AiTest.Units.Enemies
{
    public class EnemyAttackState : IEnemyState
    {
        private event Action<EnemyStateType> OnStateSwitchRequested;
        private readonly EventTrigger _attackTrigger;
        private readonly ImmediateAttack _attack;

        public EnemyAttackState(Action<EnemyStateType> onStateSwitchRequested, EventTrigger attackTrigger, ImmediateAttack attack)
        {
            OnStateSwitchRequested = onStateSwitchRequested;
            _attackTrigger = attackTrigger;
            _attack = attack;
        }

        public void OnStateEnter()
        {
            List<Unit> units = _attackTrigger.TouchedTransforms.Select((t) => t.GetComponent<Unit>()).Where((u) => u != null && u.Type == _attack.TargetType).ToList();

            for (int i = 0; i < units.Count; i++)
                _attack.TryDestroy(units[i]);

            OnStateSwitchRequested?.Invoke(EnemyStateType.Idle);
        }

        public void OnStateExit() { }
    }
}
