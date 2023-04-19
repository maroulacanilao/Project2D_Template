using UnityEngine;

namespace CustomHelpers
{
    public static class MathHelper
    {
        public static bool IsApproximatelyTo(this float a, float b)
        {
            return Mathf.Approximately(a, b);
        }

        /// <summary>
        /// probability range should be between 0 and 1
        /// </summary>
        /// <param name="probability_"></param>
        /// <returns></returns>
        public static bool RandomBool(float probability_)
        {
            return UnityEngine.Random.Range(0, 1f) < probability_;
        }
        
        /// <summary>
        /// 50% chance of returning true
        /// </summary>
        /// <returns></returns>
        public static bool RandomBool()
        {
            return UnityEngine.Random.Range(0, 1f) < 0.5f;
        }
    }
}