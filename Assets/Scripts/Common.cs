using UnityEngine;

namespace Picking
{
    public static class ExtentionMethods
    {
    }

    [System.Serializable]
    public sealed class Circle
    {
        public Vector2 center;
        public float radius;

        public Circle()
        {
            center = new Vector2(0.0f, 0.0f);
            radius = 1.0f;
        }
        public Circle(Vector2 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }

        // 衝突判定
        public bool IsHit(Vector2 point)
        {
            var vector = point - center;
            return vector.sqrMagnitude <= radius * radius;
        }
        public bool IsHit(Circle circle)
        {
            var vector = circle.center - center;
            var distance = radius + circle.radius;
            return vector.sqrMagnitude <= distance * distance;
        }
    }
}
