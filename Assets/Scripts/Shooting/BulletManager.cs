using UnityEngine;
using Utilities;

public class BulletManager : MonoBehaviour
{
    [SerializeField] FactoryConfig _bulletFactoryConfig;
    Factory _bulletFactory;


    public static BulletManager Instance { get; private set; }

    public void Init()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        _bulletFactory = new Factory(_bulletFactoryConfig);
    }

    public Bullet SpawnBullet(string id, Vector3 pos, Vector3 rot)
    {
        var bullet = _bulletFactory.Create(id, pos, rot) as Bullet;
        return bullet;

    }
    public void RecycleAllBullets()
    {
        _bulletFactory.RecycleAllPools();
    }

}
