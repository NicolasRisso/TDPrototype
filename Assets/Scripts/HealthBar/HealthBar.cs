using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Gradient gradient;

    private Image fill;
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        fill = transform.GetChild(0).GetComponent<Image>();
    }

    public void SetMaxHealth(float health)
    {
        Debug.Log(slider.name);
        slider.maxValue = health;
        slider.value = health;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(float health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
