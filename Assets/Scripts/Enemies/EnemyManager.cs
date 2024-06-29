using UnityEngine;
using Utilities;

namespace Enemies
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] FactoryConfig _enemyFactoryConfig;
        Factory _enemyFactory;

        [SerializeField] int _maxEnemiesOnScreen;

        int _spawnedEnemies;
        bool _factoryInitialised;

        public int MaxEnemiesOnScreen => _maxEnemiesOnScreen;


        public void Init()
        {
            _enemyFactory = new Factory(_enemyFactoryConfig);
            _factoryInitialised = true;
        }

        public Enemy SpawnEnemy(string id, Vector3 pos, Vector3 rot)
        {
            if (!_factoryInitialised)
                Init();
            Enemy spawnedEnemy = (Enemy)_enemyFactory.Create(id, pos, rot);
            return spawnedEnemy;

        }

        public void RecycleAllEnemies()
        {
            _enemyFactory.RecycleAllPools();
        }
    }
}