using Enemies;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowersInRangeHandler : MonoBehaviour
{
    List<TowerAgent> _enemiesInRange = new List<TowerAgent>();

    SphereCollider _sphereCollider;


    public void SetUpRangeCollider(float radius)
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.center = Vector3.zero;
        _sphereCollider.radius = radius;
    }
    public TowerAgent SetTarget(TowerAgent target)
    {
        if (_enemiesInRange.Count == 0)
            return null;

        if (target == null)
            return _enemiesInRange.First();

        if (!_enemiesInRange.Contains(target))
            return _enemiesInRange.First();
        else
        {
            if (!target.gameObject.activeInHierarchy)
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

    public void ResetTargetList()
    {
        _enemiesInRange.Clear();
    }

    public void RemoveTargetFromList(TowerAgent target)
    {
        _enemiesInRange.Remove(target);
    }

    private void OnTriggerEnter(Collider other)
    {
        var newEnemy = other.GetComponent<TowerAgent>();
        if (newEnemy == null || _enemiesInRange.Contains(newEnemy))
            return;

        _enemiesInRange.Add(other.GetComponent<TowerAgent>());
    }
    private void OnTriggerExit(Collider other)
    {
        var newEnemy = other.GetComponent<TowerAgent>();
        if (newEnemy == null || !_enemiesInRange.Contains(newEnemy))
            return;

        _enemiesInRange.Remove(other.GetComponent<TowerAgent>());
    }
}
