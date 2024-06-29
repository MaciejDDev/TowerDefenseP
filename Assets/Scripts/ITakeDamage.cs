using Health;
using UnityEngine;

public interface ITakeDamage
{
    public void TakeDamage(int amount);
    public void Heal(int amount);

    public Vector3 GetPosition();

    public bool ActiveInScene();

}