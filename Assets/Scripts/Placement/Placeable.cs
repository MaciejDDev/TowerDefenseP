using Enemies;
using Health;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TerrainGeneration;
using UnityEngine;
using Utilities;

public class Placeable : PoolableObject, ITakeDamage
{
    [SerializeField] BuildingStats _stats;
    BuildableFloorTile _floorTile;


    [Header("Health")]
    [SerializeField] HealthBar _towerHealthBar;

    HealthController _healthController;
    float _attackTimer;
    Enemy _target;

    TargetsInRangeHandler _targetsHandler;
    BulletShooter _bulletShooter;

    public BuildingStats Stats => _stats;

    private void Awake()
    {
    }

 
    internal override void Config(ObjectPool objectPool)
    {
        _objectPool = objectPool;

        _bulletShooter = GetComponent<BulletShooter>();

        _targetsHandler = GetComponentInChildren<TargetsInRangeHandler>();
        _targetsHandler.SetUpRangeCollider(_stats.AttackRange);

        _healthController = new HealthController(_stats.MaxHealth);
        _towerHealthBar.SetUp(_healthController);

    }

    private void Update()
    {
        _targetsHandler.CheckAreaForEnemies();
        TakeDamage(1);
        if (_attackTimer > 0)
        {
            _attackTimer -= Time.deltaTime;
        }
        if (_attackTimer <= 0)
        {
            _target = _targetsHandler.SetTarget(_target);
            if (_target != null)
            {
                Attack();
            }
        }
    }

    public void SetTile(BuildableFloorTile floorTile)
    {
        _floorTile = floorTile;
    }

    private void Attack()
    {
        //Debug.Log("[Placeable.cs] : Attacking Enemy.");
        _bulletShooter.Shoot(_target as ITakeDamage, _stats.AttackDamage);
        //_target.TakeDamage(_stats.AttackDamage);
        _attackTimer += _stats.AttackCooldown;
    }

    /*private void FixedUpdate()
    {
        if (_target != null)
            return;

        Collider[] colliders = new Collider[1];
        if (Physics.OverlapSphereNonAlloc(transform.position, _stats.AttackRange, colliders, _stats.TargetLayer) > 0)
        {
            Debug.Log("[TowerAgent.cs] : Target Available.");
            _target = colliders[0].GetComponent<Enemy>();
        }
    }*/

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public bool ActiveInScene()
    {
        return this.gameObject.activeInHierarchy;
    }

    public void TakeDamage(int amount)
    {
        _healthController.TakeDamage(amount);
        if (_healthController.HealthPercentage == 0)
        {
            _floorTile.HasBuilding = false;
            Recycle();
        }
    }
    public void Heal(int amount)
    {
        _healthController.RestoreHealth(amount);
    }
    internal override void Init()
    {
        _healthController.RegenAllHealth();
    }

    internal override void Release()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _stats.AttackRange);

        /*if (_target == null)
            return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, _target.transform.position);*/
    }
}
