using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // For restart and quit functionality

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Player UI")]
    public Image healthFill; // assign HealthBar_Fill Image (Image Type = Filled)

    [Header("Counters")]
    public TMP_Text killsText;    // assign KillsText
    public TMP_Text attacksText;  // assign AttacksText

    [Header("Panels")]
    public GameObject winPanel;      // assign WinPanel (inactive by default)
    public GameObject gameOverPanel; // assign GameOverPanel (inactive by default)

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Hide panels at the start
        if (winPanel != null) winPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }
    
    // 🩸 Updates player's health bar (0 to 1)
    public void UpdateHealth(float normalized)
    {
        if (healthFill != null)
            healthFill.fillAmount = Mathf.Clamp01(normalized);
    }

    // ☠️ Updates zombie kill counter
    public void UpdateKills(int current, int max)
    {
        if (killsText != null)
            killsText.text = $"Zombies Killed: {current}/{max}";
    }

    // 💀 Updates attack counter (how many times player got hit)
    public void UpdateAttacks(int current, int max)
    {
        if (attacksText != null)
            attacksText.text = $"Health Remaining:\r\n {current}";
    }

    // 🏆 Shows Win Panel
    public void ShowWin()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }
    }

    // 💀 Shows Game Over Panel
    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }
    }

    // 🔁 Restart Button (called from UI Button)
    public void OnRestartButtonClicked()
    {
        Time.timeScale = 1f; // Resume time
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 🚪 Optional: Quit Button (if you want one)
    public void OnQuitButtonClicked()
    {
        Time.timeScale = 1f;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
