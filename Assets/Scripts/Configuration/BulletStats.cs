using UnityEngine;

[CreateAssetMenu(menuName = "Config/BulletStats")]
public class BulletStats : ScriptableObject
{
    [SerializeField] float _bulletSpeed;
    [SerializeField] float _lifeTime;



    public float BulletSpeed => _bulletSpeed;
    public float LifeTime => _lifeTime;
}