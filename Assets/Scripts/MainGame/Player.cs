using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Stats")]
    public float maxHealth = 100f;

    private float currentHealth;
    private bool isDead = false;

    public float CurrentHealth => currentHealth; // <- expose current health safely

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        Debug.Log($"Player takes {amount} damage. Health: {currentHealth}");

        if (currentHealth <= 0f)
            Die();
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Player has died. Game shutting down.");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}