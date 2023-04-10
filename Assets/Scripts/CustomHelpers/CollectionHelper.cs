using System.Collections.Generic;
using UnityEngine;

namespace CustomHelpers
{
    public static class CollectionHelper
    {
        #region Random Generic

        /// <summary>
        /// Shuffle the list in place using the Fisher-Yates method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this IList<T> list)
        {
            System.Random rng = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }

        /// <summary>
        /// Return a random item from the list.
        /// Sampling with replacement.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T GetRandomItem<T>(this IList<T> list)
        {
            if (list.Count == 0) throw new System.IndexOutOfRangeException("Cannot select a random item from an empty list");
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        /// <summary>
        /// Removes a random item from the list, returning that item.
        /// Sampling without replacement.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T RemoveRandomItem<T>(this IList<T> list)
        {
            if (list.Count == 0) throw new System.IndexOutOfRangeException("Cannot remove a random item from an empty list");
            int index = UnityEngine.Random.Range(0, list.Count);
            T item = list[index];
            list.RemoveAt(index);
            return item;
        }

        #endregion

        #region Destroy Collection

        
        public static void DestroyGameObjects(this GameObject[] arr)
        {
            for ( int i = arr.Length - 1; i > -1; --i)
            {
                Object.Destroy(arr[i]);
            }
        }

        public static void DestroyGameObjects(this List<GameObject> list)
        {
            for ( int i = list.Count - 1; i > -1; --i)
            {
                Object.Destroy(list[i]);
                list.RemoveAt(i);
            }
            list.Clear();
        }

        public static void DestroyComponents(this List<Component> list)
        {
            for ( int i = list.Count - 1; i > -1; --i)
            {
                Object.Destroy(list[i].gameObject);
                list.RemoveAt(i);
            }
            list.Clear();
        }
        
        public static void DestroyComponents(this Component[] list)
        {
            for ( int i = list.Length - 1; i > -1; --i)
            {
                Object.Destroy(list[i].gameObject);
            }
        }

        public static void DestroyChildren(this Transform transform)
        {
            for (var i = transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(transform.GetChild(i).gameObject);
            }
        }

        #endregion
    }
}
