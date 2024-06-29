using AI;
using Health;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using static UnityEngine.GraphicsBuffer;

namespace Enemies
{

    public abstract class Enemy : PoolableObject, ITakeDamage
    {

        [SerializeField] EnemyStats _enemyStats;
        [SerializeField] HealthBar _healthBar;
        List<Vector3> _path;
        int _currentPathIndex = 0;

        HealthController _healthController;

        StateBase _currentState;
        Animator _animator;

        public List<Vector3> Path => _path;
        public int CurrentPathIndex => _currentPathIndex;
        public EnemyStats EnemyStats => _enemyStats;


        TowersInRangeHandler _targetsHandler;
        BulletShooter _bulletShooter;

        private TowerAgent _target;
        public TowerAgent Target => _target;

        public BulletShooter BulletShooter => _bulletShooter;

        private void Awake()
        {
        }

        internal override void Config(ObjectPool objectPool)
        {
            _objectPool = objectPool;

            _bulletShooter = GetComponent<BulletShooter>();
            _targetsHandler = GetComponentInChildren<TowersInRangeHandler>();
            _targetsHandler.SetUpRangeCollider(_enemyStats.AttackRange);

            _animator = GetComponent<Animator>();

            _healthController = new HealthController(_enemyStats.MaxHealth);
            _healthBar.SetUp(_healthController);

        }

        void Start()
        {
            _currentState = new RunState(this);
            _currentState.Enter();
        }

        public Vector3 GetPosition()
        {
            return transform.position + new Vector3(0f,1f,0f);
        }
        public bool ActiveInScene()
        {
            return this.gameObject.activeInHierarchy;
        }

        void Update() => _currentState.Execute();


        public void SetTowerTarget()
        {
            _target = _targetsHandler.SetTarget(_target);
        }
        public void ChangeState(StateBase newState)
        {
            _currentState.Exit();
            _currentState = newState;
            _currentState.Enter();

        }

        public void UpdatePathIndex()
        {
            if (_currentPathIndex < _path.Count - 1)
                _currentPathIndex++;
        }

        public void SetPath(List<Vector3> path)
        {
            _path = path;
        }
        internal override void Init()
        {
            _currentPathIndex = 0;
            _healthController.RegenAllHealth();
            _targetsHandler.ResetTargetList();
        }
        internal override void Release()
        {
            
        }

        public void TakeDamage(int amount)
        {
            _healthController.TakeDamage(amount);
            //Debug.Log("[Enemy.cs] : Taking Damage.");
            if (_healthController.HealthPercentage == 0)
            {
                GoldManager.Instance.AddReward(_enemyStats.GoldReward);
                _targetsHandler.RemoveTargetFromList(_target);
                Recycle();
            }
        }

        public void Heal(int amount)
        {
            _healthController.RestoreHealth(amount);
        }



        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _enemyStats.AttackRange);

            /*if (_target == null)
                return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.position, _target.transform.position);*/
        }

    }

}
