using UnityEngine;
using UnityEngine.UI;

public class EscapeToExit : MonoBehaviour
{
    [Header("UI References")]
    public GameObject exitPanel;    // Assign your "Do you really want to exit?" panel here
    public Button yesButton;        // Assign the Yes button
    public Button noButton;         // Assign the No button

    private bool isPanelVisible = false;

    void Start()
    {
        // Make sure the panel is hidden when the game starts
        if (exitPanel != null)
            exitPanel.SetActive(false);

        // Add button listeners
        if (yesButton != null)
            yesButton.onClick.AddListener(OnYesClicked);

        if (noButton != null)
            noButton.onClick.AddListener(OnNoClicked);
    }

    void Update()
    {
        // When ESC is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleExitPanel();
        }
    }

    void ToggleExitPanel()
    {
        if (exitPanel == null) return;

        isPanelVisible = !isPanelVisible;
        exitPanel.SetActive(isPanelVisible);

        // Pause or resume game depending on panel state
        Time.timeScale = isPanelVisible ? 0f : 1f;
    }

    void OnYesClicked()
    {
        Debug.Log("Exiting game...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void OnNoClicked()
    {
        // Hide the panel and resume the game
        if (exitPanel != null)
            exitPanel.SetActive(false);

        Time.timeScale = 1f;
        isPanelVisible = false;
    }
}
