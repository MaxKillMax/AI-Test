using UnityEngine;

namespace AiTest
{
    public static class TransformExtensions
    {
        public static Vector3 GetDirection(this Transform origin, Transform target)
        {
            Vector3 heading = target.position - origin.position;
            float distance = heading.magnitude;
            return heading / distance;
        }
    }
}
