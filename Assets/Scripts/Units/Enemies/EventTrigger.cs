using System;
using System.Collections.Generic;
using UnityEngine;

namespace AiTest.Units
{
    [RequireComponent(typeof(Collider))]
    public class EventTrigger : MonoBehaviour
    {
        public event Action<Collider> OnTriggerEntered;
        public event Action<Collider> OnTriggerStayed;
        public event Action<Collider> OnTriggerExited;

        public List<Transform> TouchedTransforms { get; private set; } = new(5);

        private void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (TouchedTransforms.Contains(other.transform))
                return;

            TouchedTransforms.Add(other.transform);
            OnTriggerEntered?.Invoke(other);
        }

        private void OnTriggerStay(Collider other) => OnTriggerStayed?.Invoke(other);

        private void OnTriggerExit(Collider other)
        {
            if (!TouchedTransforms.Contains(other.transform))
                return;

            TouchedTransforms.Remove(other.transform);
            OnTriggerExited?.Invoke(other);
        }
    }
}
