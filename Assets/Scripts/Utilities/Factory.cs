using System.Collections.Generic;
using UnityEngine;


namespace Utilities
{
    public  class Factory
    {
        private readonly FactoryConfig _config;
        private readonly Dictionary<string, ObjectPool> _pools;
        
        
        
        public Factory(FactoryConfig config)
        {
            _config = config;
            var prefabs = _config.Prefabs;
            _pools = new Dictionary<string, ObjectPool>();
            foreach ( var prefab in prefabs )
            {
                var objectPool = new ObjectPool( prefab );
                objectPool.Init(_config.InitialObjAmount);
                _pools.Add(prefab.Id, objectPool);
            }
        }
        
        public PoolableObject Create(string id, Vector3 pos, Vector3 rotation)
        {
            var objectPool = _pools[id];
            return objectPool.Spawn<PoolableObject>(pos, rotation);
        }

        public void RecycleAllPools()
        {
            foreach ( var pool in _pools.Values )
                pool.RecyclePool();
        }

    }
}
