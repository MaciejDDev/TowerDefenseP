using AI;
using Enemies;
using Health;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TerrainGeneration;
using UnityEngine;

public class TowerAgent : MonoBehaviour, ITakeDamage
{
    [Header("Tower Stats")]
    [SerializeField] BaseStats _towerStats;

    [Header("Health")]
    [SerializeField] HealthBar _towerHealthBar;

    Enemy _target;
    HealthController _healthController;
    float _attackTimer;

    TargetsInRangeHandler _targetsHandler;
    BulletShooter _bulletShooter;

    private void Awake()
    {
        _bulletShooter = GetComponent<BulletShooter>();
        _targetsHandler = GetComponentInChildren<TargetsInRangeHandler>();
        _targetsHandler.SetUpRangeCollider(_towerStats.AttackRange);
        
        _healthController = new HealthController(_towerStats.MaxHealth);
        _towerHealthBar.SetUp(_healthController);
    }

    public void ResetHealth()
    {
        _healthController.RegenAllHealth();
    }

    private void Update()
    {
        _targetsHandler.CheckAreaForEnemies();
        if (_attackTimer > 0)
        {
            _attackTimer -= Time.deltaTime;
        }
        if (_attackTimer <= 0)
        {
            _target = _targetsHandler.SetTarget(_target);
            if (_target != null)
                Attack();
        }
    }
    
    private void Attack()
    {
        //Debug.Log("[TowerAgent.cs] : Attacking Enemy.");
        _bulletShooter.Shoot(_target as ITakeDamage, _towerStats.AttackDamage);
        _attackTimer += _towerStats.AttackCooldown;
    }


    /*private void FixedUpdate()
    {
        if (_target != null)
            return;

        Collider[] colliders = new Collider[1];
        if (Physics.OverlapSphereNonAlloc(transform.position, _towerStats.AttackRange, colliders, _towerStats.TargetLayer) > 0)
        {
            Debug.Log("[TowerAgent.cs] : Target Available.");
            _target = colliders[0].GetComponent<Enemy>();
        }
    }*/


    public void TakeDamage(int amount)
    {
        //Debug.Log("[TowerAgent.cs] : Tanking damage from enemy.");
        _healthController.TakeDamage(amount);
        if (_healthController.HealthPercentage == 0)
        {
            //Game Over
            GameManager.Instance.EndGame();
        }
    }
    public void Heal(int amount)
    {
        _healthController.RestoreHealth(amount);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,_towerStats.AttackRange);

        if (_target == null)
            return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, _target.transform.position);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public bool ActiveInScene()
    {
        return this.gameObject.activeInHierarchy;
    }
}