using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    public GameObject _enemy;
    public int _yPadding;

    [SerializeField] private Image _healthBar;
    private Slider _slider;

    private void Start()
    {
        _slider = GetComponent<Slider>();
    }

    public void UpdateHealth(int health)
    {
        _slider.value = health;

        if (health < 30)
        {
            _healthBar.color = Color.red;
        }
    }

    private void Update()
    {
        if (_enemy == null)
        {
            return;
        }

        Vector3 screenPos = Camera.main.WorldToScreenPoint(_enemy.transform.position);
        gameObject.transform.position = screenPos + new Vector3(0, _yPadding, 0);
    }
}
