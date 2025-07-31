using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;
    [SerializeField] private Image heart;

    [Header("Heart Sprites")]
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite halfHeart;
    [SerializeField] private Sprite emptyHeart;
    private Tween heartPulseTween;

    public void Setup(HealthSystem health)
    {
        // Підписка на подію
        health.OnHealthChanged += (current, max) =>
        {
            SetMaxHealth(max);
            SetHealth(current);
            UpdateHeartSprite();
            AnimationForHeart();
        };

        // Початкове відображення
        SetMaxHealth(health.maxHealth);
        SetHealth(health.currentHealth);
        UpdateHeartSprite();
    }

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(float health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void AnimationForHeart()
    {
        heart.transform.DOShakePosition(Random.Range(0.5f, 2f), 5f, 10);
    }

    public void UpdateHeartSprite()
    {
        float healthPercent = slider.normalizedValue;
        if (healthPercent > 0.5f)
        {
            heart.sprite = fullHeart;
        }
        else if (healthPercent > 0.2f)
        {
            heart.sprite = halfHeart;
        }
        else
        {
            heart.sprite = emptyHeart;
        }
    }
}
