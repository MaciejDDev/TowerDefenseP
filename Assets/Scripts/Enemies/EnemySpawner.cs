using LevelGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{

    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] EnemyManager _enemyManager;

        [SerializeField] float _minTimeBetweenSpawns;
        [SerializeField] float _maxTimeBetweenSpawns;
        
        [SerializeField] int _maxMeeleeEnemyAmount;
        [SerializeField] int _maxRangedEnemyAmount;
        [SerializeField] int _maxTankEnemyAmount;


        List<Vector3> _enemyPath = new List<Vector3>();
        Cell _startCell;
        float _spawnTimer;
        
        public Cell GetStartCell => _startCell;



        private void Awake()
        {
            //SpawnEnemy("Tank");
            //SpawnEnemy("Meelee");
            //SpawnEnemy("Ranged");
        }

        public void ResetTimer()
        {
            _spawnTimer = 0f;
        }

        private void Update()
        {
            if (GameManager.Instance.IsGamePaused)
                return;

            if (_spawnTimer > 0)
            {
                _spawnTimer -= Time.deltaTime;
            }
            if (_spawnTimer <= 0)
            {
                StartCoroutine(SpawnWave());
                _spawnTimer += Random.Range(_minTimeBetweenSpawns, _maxTimeBetweenSpawns);
            }
        }
        private IEnumerator SpawnWave()
        {
            float meeleeAmount = Random.Range(1, _maxMeeleeEnemyAmount);
            float rangedAmount = Random.Range(1, _maxRangedEnemyAmount);
            float tankAmount = Random.Range(1, _maxTankEnemyAmount);

            for (int i = 0; i < meeleeAmount; i++)
            {
                SpawnEnemy("Meelee");
                yield return new WaitForSeconds(0.2f);
            }
            for (int i = 0; i < rangedAmount; i++)
            {
                SpawnEnemy("Ranged");
                yield return new WaitForSeconds(0.2f);
            }
            for (int i = 0; i < tankAmount; i++)
            {
                SpawnEnemy("Tank");
                yield return new WaitForSeconds(0.2f);
            }



        }

        public void SetStartCell(Cell startCell)
        {
            _startCell = startCell;
        }
        public void SetEnemiesPath(List<Vector3> path)
        {
            _enemyPath = path;
        }

        private void SpawnEnemy(string id)
        {
            Enemy enemy = _enemyManager.SpawnEnemy(id, transform.position, transform.rotation.eulerAngles);
            enemy.SetPath(_enemyPath);
        }

    }
}