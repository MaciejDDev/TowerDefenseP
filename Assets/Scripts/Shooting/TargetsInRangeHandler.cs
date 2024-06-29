using Enemies;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TargetsInRangeHandler : MonoBehaviour
{
    [SerializeField] LayerMask _enemyLayer;
    List<Enemy> _enemiesInRange = new List<Enemy>();

    //SphereCollider _sphereCollider;
    float _range;

    public void  SetUpRangeCollider(float radius)
    {
        //_sphereCollider = GetComponent<SphereCollider>();
        //_sphereCollider.center = Vector3.zero;
        //_sphereCollider.radius = radius;
        _range = radius;
    }
    public Enemy SetTarget(Enemy target)
    {
        if (_enemiesInRange.Count == 0)
            return null;

        if (target == null)
            return _enemiesInRange.First();

        if (!_enemiesInRange.Contains(target))
            return _enemiesInRange.First();
        else
        {
            if (!target.gameObject.activeInHierarchy || Vector3.Distance(target.transform.position, transform.position) > _range)
            {
                _enemiesInRange.Remove(target);
                if (_enemiesInRange.Count == 0)
                    return null;
                else
                    return _enemiesInRange.First();
            }
        }
        return target;
    }

    public void CheckTarget(Enemy target)
    {
        if (!target.gameObject.activeInHierarchy || Vector3.Distance(target.transform.position, transform.position) > _range)
        {
            _enemiesInRange.Remove(target);
        }
    }

    public void CheckAreaForEnemies()
    {
        Collider[] hits = new Collider[15];
        int hitsAmount = Physics.OverlapSphereNonAlloc(transform.position, _range, hits, _enemyLayer);
        if (hitsAmount > 0)
        {
            List<Enemy> currentEnemies = new List<Enemy>();
            for (int i = 0; i < hitsAmount; i++)
            {
                var enemy = hits[i].GetComponent<Enemy>();
                if (enemy == null)
                    continue;
                currentEnemies.Add(enemy);
                if (_enemiesInRange.Contains(enemy))
                    continue;

                _enemiesInRange.Add(enemy);


            }
            List<Enemy> enemiesInRange = _enemiesInRange;
            for (int i = 0; i < enemiesInRange.Count; i++)
            {
                if (!currentEnemies.Contains(enemiesInRange[i]))
                {
                    _enemiesInRange.Remove(enemiesInRange[i]);
                }

            }
        }
        else
        {
            _enemiesInRange.Clear();
        }
    }


    /*private void OnTriggerEnter(Collider other)
    {
        var newEnemy = other.GetComponent<Enemy>();
        if (newEnemy == null || _enemiesInRange.Contains(newEnemy))
            return;

        _enemiesInRange.Add(other.GetComponent<Enemy>());
    }
    private void OnTriggerExit(Collider other)
    {
        var newEnemy = other.GetComponent<Enemy>();
        if (newEnemy == null || !_enemiesInRange.Contains(newEnemy))
            return;

        _enemiesInRange.Remove(other.GetComponent<Enemy>());
    }*/

}
