using System;
using System.Collections.Generic;
using UnityEngine;

namespace AiTest.Units.FieldsOfView
{
    public class FieldOfView 
    {
        public event Action<Unit> OnFounded;
        public event Action<Unit> OnMissed;

        private const int LayerMask = 1;

        private readonly Transform _transform;
        private readonly float _angle;
        private readonly UnitType _searchingUnitType;

        private readonly Dictionary<Transform, Unit> _potentialUnits = new(10);

        public List<Unit> FoundedUnits { get; private set; } = new(10);

        public FieldOfView(EventTrigger eventTrigger, float angle, float length, UnitType searchingUnitType)
        {
            _transform = eventTrigger.transform;

            eventTrigger.OnTriggerEntered += CheckColliderEntering;
            eventTrigger.OnTriggerStayed += CheckColliderStaying;
            eventTrigger.OnTriggerExited += CheckColliderExiting;

            _angle = angle;
            _searchingUnitType = searchingUnitType;

            eventTrigger.transform.localScale = new(length, length, length);
        }

        #region Handle collisions

        private void CheckColliderEntering(Collider collider)
        {
            if (!IsSearchingUnitTransform(collider.transform, out Unit unit))
                return;

            if (IsBehindAnObstacle(collider.transform) || !IsInAngles(collider.transform))
                _potentialUnits.Add(collider.transform, unit);
            else
                FindUnit(unit);
        }

        private void CheckColliderStaying(Collider collider)
        {
            if (!_potentialUnits.TryGetValue(collider.transform, out Unit unit))
                return;

            if (IsBehindAnObstacle(collider.transform) || !IsInAngles(collider.transform))
                return;

            _potentialUnits.Remove(collider.transform);
            FindUnit(unit);
        }

        private void CheckColliderExiting(Collider collider)
        {
            if (!IsSearchingUnitTransform(collider.transform, out Unit unit))
                return;

            if (FoundedUnits.Contains(unit))
                MissUnit(unit);
            else if (_potentialUnits.ContainsKey(collider.transform))
                _potentialUnits.Remove(collider.transform);
        }

        #endregion

        private void FindUnit(Unit unit)
        {
            FoundedUnits.Add(unit);
            OnFounded?.Invoke(unit);
        }

        private void MissUnit(Unit unit)
        {
            FoundedUnits.Remove(unit);
            OnMissed?.Invoke(unit);
        }

        #region Checks

        private bool IsInAngles(Transform transform)
        {
            Vector3 targetDirection = _transform.GetDirection(transform);
            Vector3 transformDirection = _transform.forward;
            return Vector3.Angle(transformDirection, targetDirection) <= _angle;
        }

        private bool IsSearchingUnitTransform(Transform transform, out Unit unit) => transform.TryGetComponent(out unit) && unit.Type == _searchingUnitType;

        private bool IsBehindAnObstacle(Transform transform) => !Physics.Raycast(_transform.position, _transform.GetDirection(transform), out RaycastHit hit, float.MaxValue, LayerMask) || hit.transform != transform.transform;

        #endregion
    }
}
