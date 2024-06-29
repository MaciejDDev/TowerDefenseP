using System;
using UnityEngine;
namespace Health
{
    [Serializable]
    public class HealthController
    {
        private int _maxHealth;
        private int _currentHealth;

        public float HealthPercentage => (float)_currentHealth / _maxHealth;

        public event Action OnHealthChanged;

        public HealthController(int maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;
        }

        public void TakeDamage(int amount)
        {
            UpdateHealth(-amount);
        }

        public void RegenAllHealth()
        {
            _currentHealth = _maxHealth;
            OnHealthChanged.Invoke();
        }

        public void RestoreHealth(int amount)
        {
            UpdateHealth(amount);
        }
        private void UpdateHealth(int amount)
        {
            _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, _maxHealth);
            OnHealthChanged.Invoke();
        }
    }
}