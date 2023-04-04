using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace AiTest
{
    public class Updater : MonoBehaviour
    {
        private static Updater Instance;

        private static event Action OnUpdate;

        private void Awake()
        {
            Assert.IsNull(transform.parent);

            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                OnUpdate = null;
                Destroy(this);
            }
        }

        private void Update()
        {
            OnUpdate?.Invoke();
        }

        public static void AddListener(Action action)
        {
            Assert.IsNotNull(Instance);
            OnUpdate += action;
        }

        public static void RemoveListener(Action action)
        {
            Assert.IsNotNull(Instance);
            OnUpdate -= action;
        }
    }
}
