using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Utilities
{
    public class ObjectPool
    {
        //Object in the pool 
        private readonly PoolableObject _prefab;
        //Active objects
        private readonly HashSet<PoolableObject> _spawnedObjects;
        //Recycled objects
        private Queue<PoolableObject> _objectsToSpawn;

        private GameObject _poolParent;

        public ObjectPool(PoolableObject prefab)
        {
            _prefab = prefab;
            _spawnedObjects = new HashSet<PoolableObject>();
        }

        public void Init(int initialAmount)
        {
            _objectsToSpawn = new Queue<PoolableObject>(initialAmount);
            _poolParent = new GameObject($"{_prefab.name}Pool");
            for (int i = 0; i < initialAmount; i++)
            {
                var instance = InstantiateNewInstance();
                instance.gameObject.SetActive(false);
                _objectsToSpawn.Enqueue(instance);
            }
        }

        private PoolableObject InstantiateNewInstance()
        {
            var instance = Object.Instantiate(_prefab);
            instance.transform.SetParent(_poolParent.transform, true);
            instance.Config(this);
            return instance;
        }

        public T Spawn<T>(Vector3 pos, Vector3 rot)
        {
            var poolableObject = GetInstance();
            _spawnedObjects.Add(poolableObject);
            poolableObject.transform.position = pos;
            Quaternion rotQuat = Quaternion.Euler(rot);
            poolableObject.transform.rotation = rotQuat;
            poolableObject.gameObject.SetActive(true);
            poolableObject.Init();
            return poolableObject.GetComponent<T>();
        }

        private PoolableObject GetInstance()
        {
            if (_objectsToSpawn.Count > 0)
                return _objectsToSpawn.Dequeue();

            Debug.LogWarning($"Not enough recycled objects for {_prefab.name}");

            var instance = InstantiateNewInstance();
            return instance;
        }

        public void RecycleGameObject(PoolableObject objToRecycle)
        {
            var wasActive = _spawnedObjects.Remove(objToRecycle);
            Assert.IsTrue(wasActive, $"{objToRecycle.name} was not instantiated");

            objToRecycle.gameObject.SetActive(false);
            objToRecycle.Release();
            _objectsToSpawn.Enqueue(objToRecycle);
        }

        public void RecyclePool()
        {
            List<PoolableObject> spawnedObj = _spawnedObjects.ToList();
            foreach (var obj in spawnedObj)
                RecycleGameObject(obj);
        }
    }
}