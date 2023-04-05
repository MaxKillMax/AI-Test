using UnityEngine;

namespace AiTest.Units
{
    [DisallowMultipleComponent]
    public class Unit : MonoBehaviour
    {
        [SerializeField] private UnitType _type;

        public UnitType Type => _type;
    }
}
