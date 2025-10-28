using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;


    [Header("Player UI")]
    public Image healthFill; // assign HealthBar_Fill Image (Image Type = Filled)


    [Header("Counters")]
    public Text killsText; // assign KillsText
    public Text attacksText; // assign AttacksText


    [Header("Panels")]
    public GameObject winPanel; // assign WinPanel (inactive by default)
    public GameObject gameOverPanel;// assign GameOverPanel (inactive by default)


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }


    public void UpdateHealth(float normalized) // normalized = 0..1
    {
        if (healthFill != null)
            healthFill.fillAmount = Mathf.Clamp01(normalized);
    }


    public void UpdateKills(int current, int max)
    {
        if (killsText != null)
            killsText.text = $"Zombies Killed: {current}/{max}";
    }


    public void UpdateAttacks(int current, int max)
    {
        if (attacksText != null)
            attacksText.text = $"Times Attacked: {current}/{max}";
    }


    public void ShowWin()
    {
        if (winPanel != null)
            winPanel.SetActive(true);
    }


    public void ShowGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }
}