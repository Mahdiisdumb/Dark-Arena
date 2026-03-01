using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;
    public Slider healthSlider;

    void Start()
    {
        if (playerHealth == null)
            Debug.LogError("PlayerHealth reference not set!");

        if (healthSlider == null)
            Debug.LogError("Health Slider reference not set!");

        healthSlider.maxValue = playerHealth.maxHealth;
        healthSlider.value = playerHealth.CurrentHealth;
    }

    void Update()
    {
        if (playerHealth == null || healthSlider == null) return;

        healthSlider.value = playerHealth.CurrentHealth;
    }
}