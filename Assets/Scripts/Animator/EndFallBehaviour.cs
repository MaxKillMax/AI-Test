using System;
using UnityEngine;

namespace AiTest
{
    public class EndFallBehaviour : StateMachineBehaviour
    {
        public static event Action OnEndFallEnded;

        [SerializeField] private string _startFallTrigger;
        [SerializeField] private string _endFallTrigger;

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            OnEndFallEnded?.Invoke();
            animator.ResetTrigger(_startFallTrigger);
            animator.ResetTrigger(_endFallTrigger);
        }
    }
}
