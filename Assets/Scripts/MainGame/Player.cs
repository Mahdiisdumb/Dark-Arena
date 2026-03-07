using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Stats")]
    public float maxHealth = 100f;

    float currentHealth;
    bool isDead = false;

    public float CurrentHealth => currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        Debug.Log("Player takes " + amount + " damage. Health: " + currentHealth);

        if (currentHealth <= 0f)
            Die();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Player died.");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}