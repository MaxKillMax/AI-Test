using System;
using UnityEngine;

namespace AiTest.Units
{
    [CreateAssetMenu(fileName = nameof(FieldOfViewData), menuName = nameof(FieldOfViewData), order = 51)]
    public class FieldOfViewData : ScriptableObject
    {
        [SerializeField, Range(0, 180)] private float _angle = 75;
        [SerializeField, Range(0.1f, 100)] private float _length = 25;
        [SerializeField] private UnitType _searchingUnitType;

        public float Angle => _angle;
        public float Length => _length;
        public UnitType SearchingUnitType => _searchingUnitType;
    }
}
