using UnityEngine;
using UnityEngine.AI;

namespace AiTest.Units.Enemies.Components
{
    public class EnemyAgentSpeedAnimator
    {
        private readonly NavMeshAgent _agent;
        private readonly EnemyAnimator _animator;
        private readonly float _maxSpeed;

        public EnemyAgentSpeedAnimator(NavMeshAgent agent, EnemyAnimator animator, float maxSpeed)
        {
            _agent = agent;
            _animator = animator;
            _maxSpeed = maxSpeed;

            Updater.AddListener(Update);
        }

        private void Update()
        {
            float speed = Mathf.Max(new float[] { Mathf.Abs(_agent.velocity.x), Mathf.Abs(_agent.velocity.y), Mathf.Abs(_agent.velocity.z) });
            _animator.SetLerpSpeed(speed / _maxSpeed);
        }
    }
}
