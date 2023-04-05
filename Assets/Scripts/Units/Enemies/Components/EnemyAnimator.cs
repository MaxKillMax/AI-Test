using UnityEngine;

namespace AiTest.Units.Enemies.Components
{
    public class EnemyAnimator : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Animator _animator;
        [SerializeField] private string _speedFloatKey;
        [SerializeField] private string _startFallKey;
        [SerializeField] private string _endFallKey;
        [SerializeField] private string _openDoorKey;

        public void SetLerpSpeed(float speed)
        {
            Mathf.Lerp(0, 1, speed);
            _animator.SetFloat(_speedFloatKey, speed);
        }

        public void StartFall() => _animator.SetTrigger(_startFallKey);

        public void EndFall() => _animator.SetTrigger(_endFallKey);

        public void OpenDoor() => _animator.SetTrigger(_openDoorKey);
    }
}
