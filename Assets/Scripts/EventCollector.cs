using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AiTest
{
    public class EventCollector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public event Action OnDowned;
        public event Action OnUped;

        public void OnPointerDown(PointerEventData eventData) => OnDowned?.Invoke();

        public void OnPointerUp(PointerEventData eventData) => OnUped?.Invoke();

        private void OnMouseDown() => OnDowned?.Invoke();

        private void OnMouseUp() => OnUped?.Invoke();
    }
}
