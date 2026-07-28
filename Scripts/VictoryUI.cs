using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject victoryPanel;

    [Header("Escenas")]
    [SerializeField] private int menuSceneIndex = 0;

    private void Awake()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
    }

    public void Show()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f; // pausa el juego
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneIndex);
    }
}