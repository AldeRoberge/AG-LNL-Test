using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

namespace Utils
{
    /**
     * Utility class to help find the nearest object of a given type.
     *
     * Example use :
     *     DoorController nearestDoor = GetNearestObjectOfType<DoorController>();
     */
    public static class NearestObjUtils
    {
        /// <summary>
        /// Returns the nearest GameObject of type T relative to a GameObject gameObject.
        /// Using Object.FindObjectsOfType<T>
        /// </summary>
        public static T GetNearestGameObject<T>(this GameObject gameObject) where T : MonoBehaviour
        {
            return GetNearestGameObject<T>(gameObject.transform.position);
        }

        /// <summary>
        /// Returns the nearest GameObject of type T relative to a Vector3 position.
        /// Using Object.FindObjectsOfType<T>
        /// </summary>
        public static T GetNearestGameObject<T>(Vector3 position) where T : MonoBehaviour
        {
            return GetNearestGameObject(position, Object.FindObjectsOfType<T>().ToList());
        }

        /// <summary>
        /// Returns the nearest GameObject of type T relative to a List of objects.
        /// </summary>
        public static T GetNearestGameObject<T>(GameObject obj, List<T> allObjs) where T : MonoBehaviour
        {
            return GetNearestGameObject<T>(obj.transform.position, allObjs);
        }

        /// <summary>
        /// Returns the nearest GameObject of type T relative to a List of objects.
        /// </summary>
        public static T GetNearestGameObject<T>(Vector3 myPosition, List<T> allObjs) where T : MonoBehaviour
        {
            T nearestObject = null;
            float nearestObjectDistance = float.MaxValue;

            foreach (T objects in allObjs)
            {
                float distance = Vector3.Distance(myPosition, objects.transform.position);

                if (distance < nearestObjectDistance)
                {
                    nearestObjectDistance = distance;
                    nearestObject = objects;
                }
            }

            return nearestObject;
        }

        public static List<T> GetSortedListOfGameObjectByDistance<T>(Vector3 position) where T : MonoBehaviour
        {
            List<T> distance = Object.FindObjectsOfType<T>().ToList();

            distance = distance.OrderBy(
                x => Vector2.Distance(position, x.transform.position)
            ).ToList();

            return distance;
        }
    }
}