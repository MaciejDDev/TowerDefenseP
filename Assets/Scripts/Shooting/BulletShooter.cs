using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{

    [SerializeField] Transform _bulletStart;
    [SerializeField] string _bulletId;


    private void Awake()
    {
        
    }
    public void Shoot(ITakeDamage target, int damage)
    {
        var bullet = BulletManager.Instance.SpawnBullet(_bulletId, _bulletStart.position, Quaternion.LookRotation(target.GetPosition()).eulerAngles);
        bullet.SetTarget(target, damage);
        
    }


}
