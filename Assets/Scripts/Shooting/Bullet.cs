using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class Bullet : PoolableObject
{
    [SerializeField] BulletStats _bulletstats;

    ITakeDamage _target;

    int _damage;
    internal override void Config(ObjectPool objectPool)
    {
        _objectPool = objectPool;
    }

    internal override void Init()
    {
        //throw new System.NotImplementedException();
        //_target = 
        Invoke(nameof(RecycleAfterTime), _bulletstats.LifeTime);
    }

    private void RecycleAfterTime()
    {

    }
    public void SetTarget(ITakeDamage target, int damage)
    {
        _target = target;
        _damage = damage;
    }
    internal override void Release()
    {
        //throw new System.NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_target.ActiveInScene())
            Recycle();
        transform.LookAt(_target.GetPosition());
        var moveDir = _target.GetPosition() - transform.position;
        var moveDist = _bulletstats.BulletSpeed * Time.deltaTime * moveDir.normalized;
        transform.position += moveDist;
    }

    private void OnTriggerEnter(Collider other)
    {
        var damageTaker = other.GetComponent<ITakeDamage>();
        if (damageTaker == _target)
        {
            Recycle();
            _target.TakeDamage(_damage);
        }
        
    }
}
