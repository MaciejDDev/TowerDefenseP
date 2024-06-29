using UnityEngine;
using UnityEngine.UI;
namespace Health
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Image _healthBarImage;
        HealthController _healthController;

        [SerializeField] bool _isWorldSpaceBar;
        Camera _camera;
        bool isBound;
        private void Awake()
        {
            _camera = Camera.main;
        }


        private void Update()
        {
            if (_isWorldSpaceBar)
                transform.rotation = Quaternion.LookRotation(transform.position - _camera.transform.position);
        }

        public void SetUp(HealthController healthController)
        {
            _healthController = healthController;
            UpdateHealthBar();
            _healthController.OnHealthChanged += UpdateHealthBar;
            isBound = true;
        }
        private void OnEnable()
        {
            if (_healthController != null && !isBound)
                _healthController.OnHealthChanged += UpdateHealthBar;
            
        }
        private void OnDisable()
        {
            if (_healthController != null && isBound)
            {
                _healthController.OnHealthChanged -= UpdateHealthBar;
                isBound = false;
            }

        }
        private void UpdateHealthBar()
        {
            _healthBarImage.fillAmount = _healthController.HealthPercentage;
        }
    }
}