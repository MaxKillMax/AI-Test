using UnityEngine;

namespace AiTest
{
    public class MouseLocker : MonoBehaviour
    {
        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
