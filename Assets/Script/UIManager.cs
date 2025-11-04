using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Player UI")]
    public Image healthFill; // assign HealthBar_Fill Image (Image Type = Filled)

    [Header("Counters")]
    public TMP_Text killsText;
    public TMP_Text attacksText;

    [Header("Panels")]
    public GameObject startPanel;     // NEW: "Click to Start" panel
    public GameObject winPanel;
    public GameObject gameOverPanel;

    [Header("Audio Clips")]
    public AudioClip winClip;
    public AudioClip loseClip;
    public AudioClip startClip;

    private AudioSource audioSource;
    private bool gameStarted = false; // Has the game begun?

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Hide everything except start panel
        if (winPanel != null) winPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        if (startPanel != null) startPanel.SetActive(true);

        // Pause at start until player clicks
        Time.timeScale = 0f;

        // Setup audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        // 👆 Click Left Mouse Button to start
        if (!gameStarted && Input.GetMouseButtonDown(0))
        {
            StartGame();
        }

        // ⎋ ESC to exit after game is over or win
        if ((gameOverPanel.activeSelf || winPanel.activeSelf) && Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    // 🎬 Start the game when player clicks
    void StartGame()
    {
        gameStarted = true;
        if (startPanel != null)
            startPanel.SetActive(false);

        Time.timeScale = 1f; // Resume game

        if (startClip != null)
            audioSource.PlayOneShot(startClip);
    }

    // 🩸 Update health bar
    public void UpdateHealth(float normalized)
    {
        if (healthFill != null)
            healthFill.fillAmount = Mathf.Clamp01(normalized);
    }

    // ☠️ Update kills
    public void UpdateKills(int current, int max)
    {
        if (killsText != null)
            killsText.text = $"Zombies Killed: {current}/{max}";
    }

    // ❤️ Update attacks / health remaining
    public void UpdateAttacks(int current, int max)
    {
        if (attacksText != null)
            attacksText.text = $"Health Remaining: {current}";
    }

    // 🏆 Show Win
    public void ShowWin()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            if (winClip != null) audioSource.PlayOneShot(winClip);
            Time.timeScale = 0f;
        }
    }

    // 💀 Show Game Over
    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            if (loseClip != null) audioSource.PlayOneShot(loseClip);
            Time.timeScale = 0f;
        }
    }

    // 🔁 Restart button
    public void OnRestartButtonClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 🚪 ESC or Quit button
    public void QuitGame()
    {
        Time.timeScale = 1f;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // 🚪 Optional Quit button (UI)
    public void OnQuitButtonClicked()
    {
        QuitGame();
    }
}
