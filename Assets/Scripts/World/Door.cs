using UnityEngine;

namespace AiTest.World
{
    [RequireComponent(typeof(EventCollector))]
    public class Door : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<EventCollector>().OnDowned += Close;
        }

        private void Close() => gameObject.SetActive(false);
    }
}
