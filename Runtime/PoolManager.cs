using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Spyro.Optimisation.ObjectManagement {
    public static class ObjectManager {

        static Dictionary<int, List<GameObject>> dict = new Dictionary<int, List<GameObject>>();
        public static int PoolGameObject(GameObject prefab, int ammount)
        {

            Transform parent = null;
            List<GameObject> pool = new List<GameObject>();
            try
            {
                ExecuteMethodOnDict(prefab, elementExists =>
                {
                    if (elementExists)
                    {
                        parent = dict[prefab.GetInstanceID()][0].transform.parent;
                        pool = dict[prefab.GetInstanceID()];
                        return;
                    }

                    parent = new GameObject($"{prefab.name}'s list").transform;

                });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            PoolValue(prefab, ammount, parent, pool);

            ExecuteMethodOnDict(prefab, elementExists =>
            {

                if (elementExists)
                {
                    dict[prefab.GetInstanceID()] = pool;
                    return;
                }
                dict.Add(prefab.GetInstanceID(), pool);

            });
            return pool.Count;

        }

        private static void PoolValue(GameObject prefab, int ammount, Transform parent, List<GameObject> pool)
        {
            for (int i = 0; i < ammount; i++)
            {

                GameObject obj = UnityEngine.Object.Instantiate(prefab, parent);
                obj.SetActive(false);
                pool.Add(obj);
            }


        }

        /// <summary>
        /// Executes a method that is dependant on the pool dictionary. The result of dict.ContainsKey determines the bool argument.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="executableMethod"></param>
        static void ExecuteMethodOnDict(GameObject value, Action<bool> executableMethod)
        {
            executableMethod.Invoke(dict.ContainsKey(value.GetInstanceID()));
        }

        /// <summary>
        /// Dynamically creates a set amount of gameObjects when called. Future calls of this method uses the pre-created gameObjects instead of creating new ones.
        /// Note; The created gameObjects are by default in-active. Enable them using the SetActive() method. Likewise, use the same method to remove any gameObjects from the scene.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="amountToPrecreate"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T DynamicInstantiate<T>(T value = null, bool defaultState = false, int amountToPrecreate = 300) where T : MonoBehaviour
        {

            foreach (var item in dict)
            {

                if (item.Value[0].GetComponent<T>() != null || (value != null && item.Key == value.gameObject.GetInstanceID()))
                {

                    foreach (var element in item.Value)
                    {

                        if (!element.gameObject.activeSelf)
                        {
                            element.gameObject.SetActive(defaultState);
                            return element.GetComponent<T>();
                        }
                    }

                    PoolValue(value.gameObject, 1, item.Value[0].transform.parent, item.Value);

                }
            }

            PoolGameObject(value.gameObject, amountToPrecreate);
            return DynamicInstantiate(value, defaultState, amountToPrecreate);

        }





        /// <summary>
        /// Dynamically creates a set amount of gameObjects when called. Future calls of this method uses the pre-created gameObjects instead of creating new ones.
        /// Note; The created gameObjects are by default in-active. Enable them using the SetActive() method. Likewise, use the same method to remove any gameObjects from the scene.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="amountToPrecreate"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static GameObject DynamicInstantiate(GameObject value, bool defaultState = false, int amountToPrecreate = 300)
        {

            foreach (var item in dict)
            {

                if (item.Key == value.gameObject.GetInstanceID())
                {

                    foreach (var element in item.Value)
                    {

                        if (!element.gameObject.activeSelf)
                        {
                            element.SetActive(defaultState);
                            return element;
                        }
                    }

                    PoolValue(value, 1, item.Value[0].transform.parent, item.Value);

                }
            }

            PoolGameObject(value, amountToPrecreate);
            return DynamicInstantiate(value, defaultState, amountToPrecreate);

        }

        /// <summary>
        /// Dynamically creates a set amount of gameObjects when called. Future calls of this method uses the pre-created gameObjects instead of creating new ones.
        /// Note; The created gameObjects are by default in-active. Enable them using the SetActive() method. Likewise, use the same method to remove any gameObjects from the scene.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="amountToPrecreate"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T DynamicComponentInstantiate<T>(T value, bool defaultState = false, int amountToPrecreate = 300) where T : Component
        {
            foreach (var item in dict)
            {

                if (item.Key == value.gameObject.GetInstanceID())
                {

                    foreach (var element in item.Value)
                    {

                        if (!element.gameObject.activeSelf)
                        {
                            element.gameObject.SetActive(defaultState);
                            return element.GetComponent<T>();
                        }
                    }

                    PoolValue(value.gameObject, 1, item.Value[0].transform.parent, item.Value);

                }
            }

            PoolGameObject(value.gameObject, amountToPrecreate);
            return DynamicComponentInstantiate(value, defaultState, amountToPrecreate);
        }

    }
}