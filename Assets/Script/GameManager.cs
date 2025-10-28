using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    public int maxZombies = 20;
    public int maxAttacks = 100;


    private int kills = 0;
    private int attacks = 0;


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }


    public void RegisterKill()
    {
        kills++;
        UIManager.Instance.UpdateKills(kills, maxZombies);


        if (kills >= maxZombies)
        {
            // Player wins
            UIManager.Instance.ShowWin();
            // Optionally freeze time
            Time.timeScale = 0f;
        }
    }


    public void RegisterAttack()
    {
        attacks++;
        UIManager.Instance.UpdateAttacks(attacks, maxAttacks);


        if (attacks >= maxAttacks)
        {
            // Player loses
            UIManager.Instance.ShowGameOver();
            Time.timeScale = 0f;
        }
    }


    // Optional getters
    public int GetKills() => kills;
    public int GetAttacks() => attacks;
}