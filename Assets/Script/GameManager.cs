using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int maxZombies = 20;
    public int maxAttacks = 100; // start from 100
    private int kills = 0;
    private int attacksRemaining; // will count down from 100

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Initialize attack countdown at start
        attacksRemaining = maxAttacks;
        UIManager.Instance.UpdateAttacks(attacksRemaining, maxAttacks);

    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Ensure time resumes before restarting
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RegisterKill()
    {
        kills++;
        UIManager.Instance.UpdateKills(kills, maxZombies);

        if (kills >= maxZombies)
        {
            // Player wins
            UIManager.Instance.ShowWin();
            Time.timeScale = 0f;
        }
    }

    public void RegisterAttack()
    {
        // Each attack reduces remaining count
        attacksRemaining--;
        UIManager.Instance.UpdateAttacks(attacksRemaining, maxAttacks);

        if (attacksRemaining <= 0)
        {
            // Player loses
            UIManager.Instance.ShowGameOver();
            Time.timeScale = 0f;
        }
    }

    // Optional getters
    public int GetKills() => kills;
    public int GetAttacksRemaining() => attacksRemaining;
}
