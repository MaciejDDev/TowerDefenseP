using System.Collections.Generic;
using TerrainGeneration;
using UnityEngine;

namespace Utilities
{
    [CreateAssetMenu(menuName = "Config/(Prefab)FactoryConfig")]
    public class FactoryConfig: ScriptableObject
    {
        [SerializeField] protected int _initialObjAmount;
        [SerializeField] protected PoolableObject[] _prefabs;
        protected Dictionary<string, PoolableObject> _prefabsDict;

        public PoolableObject[] Prefabs => _prefabs;
        public int InitialObjAmount => _initialObjAmount;

        private void Awake()
        {
            _prefabsDict = new Dictionary<string, PoolableObject>();
            foreach (PoolableObject prefab in _prefabs)
            {
                _prefabsDict.Add(prefab.Id, prefab);
            }
            
        }

        public PoolableObject GetPrefabById(string id)
        {
            if (!_prefabsDict.TryGetValue(id, out var prefab))
            {
                throw new System.Exception($"Tile with id {id} does not exist");
            }
            return prefab;
        }
    }
}
