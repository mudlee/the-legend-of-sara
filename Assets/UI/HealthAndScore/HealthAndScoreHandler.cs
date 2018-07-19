using UnityEngine;
using UnityEngine.UI;

public class HealthAndScoreHandler : MonoBehaviour {
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _healthBar;
    [SerializeField] private Text _remainingText;

    private Color _originalHealth;
    private bool _almostOver = false;

    private void Start()
    {
        _originalHealth = _healthBar.color;
    }

    public void UpdateHealth(int health)
    {
        _slider.value = health;

        if(health<30)
        {
            _healthBar.color = Color.red;
        }
        else
        {
            _healthBar.color = _originalHealth;
        }
    }

    public void UpdateRemainingTime(int remainingTime)
    {
        _remainingText.text = remainingTime.ToString();

        if (!_almostOver && remainingTime<60)
        {
            _remainingText.color = Color.red;
            _almostOver = true;
        }
    }
}
