using UnityEngine;

namespace CustomHelpers
{
    public static class MathHelper
    {
        public static bool IsApproximatelyTo(this float a, float b)
        {
            return Mathf.Approximately(a, b);
        }
    }
}