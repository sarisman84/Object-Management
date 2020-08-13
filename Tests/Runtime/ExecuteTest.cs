using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using NUnit.Framework;

using UnityEngine;
using UnityEngine.TestTools;

namespace Spyro.Optimisation.ObjectManagement.Test {
    public class ExecuteTest {


        GameObject obj;
        Rigidbody rigidBody;
        CustomObject customObject;
        // A Test behaves as an ordinary method

        int currentObjCount;


        [SetUp]
        public void CreateObjectToPool()
        {
            obj = new GameObject("Test");
            rigidBody = new GameObject("RigidBody_Test").AddComponent<Rigidbody>();
            customObject = new GameObject("Custom Object").AddComponent<CustomObject>();


            currentObjCount = 300;
            ObjectManager.PoolGameObject(obj, currentObjCount);
          
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [Test]
        public void CreatingGameObjectViaPool()
        {
            GameObject result = ObjectManager.DynamicInstantiate(obj);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.gameObject.activeSelf);
        }

        [Test]
        public void CreatingGenericObjectsViaPool()
        {
            Rigidbody result = ObjectManager.DynamicComponentInstantiate(rigidBody);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.gameObject.activeSelf);

        }


        [Test]
        public void CreatingGenericObjectsOfTypeMonoBehaivourViaPool()
        {
            CustomObject result = ObjectManager.DynamicInstantiate(customObject);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.gameObject.activeSelf);
        }


        [Test]
        public void CreateMoreGameObjectsInPool()
        {

            int newCount = ObjectManager.PoolGameObject(obj, 100);
            Assert.AreEqual(400, newCount);

        }



    }

    public class CustomObject : MonoBehaviour {

    }
}