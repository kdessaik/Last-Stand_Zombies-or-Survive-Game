using UnityEngine;


public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;


    void Start()
    {
        currentHealth = maxHealth;
        // Initialize UI
        if (UIManager.Instance != null)
            UIManager.Instance.UpdateHealth(1f);
    }


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);


        // Update UI (normalized)
        if (UIManager.Instance != null)
            UIManager.Instance.UpdateHealth((float)currentHealth / maxHealth);


        // Register attack in GameManager (counts toward 100)
        if (GameManager.Instance != null)
            GameManager.Instance.RegisterAttack();


        if (currentHealth <= 0)
        {
            Die();
        }
    }


    void Die()
    {
        Debug.Log("Player died");
        // optional: disable player controls
        if (UIManager.Instance != null)
            UIManager.Instance.ShowGameOver();


        Time.timeScale = 0f;
    }
}