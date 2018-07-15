using UnityEngine;
using UnityEngine.UI;

public class HealthAndScoreHandler : MonoBehaviour {
    [SerializeField] private Slider _slider;

    public void UpdateHealth(int health)
    {
        _slider.value = health;
    }
}
