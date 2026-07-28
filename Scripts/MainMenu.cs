using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private int gameSceneIndex = 1; // el numero de orden en Build Settings (Menu suele ser 0, el juego 1)

    [Header("Panel de Como Jugar")]
    [SerializeField] private GameObject howToPlayPanel;

    private void Awake()
    {
        // Arranca cerrado, se abre solo si le das click al boton
        if (howToPlayPanel != null)
        {
            howToPlayPanel.SetActive(false);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneIndex);
    }

    public void OpenHowToPlay()
    {
        if (howToPlayPanel != null)
        {
            howToPlayPanel.SetActive(true);
        }
    }

    public void CloseHowToPlay()
    {
        if (howToPlayPanel != null)
        {
            howToPlayPanel.SetActive(false);
        }
    }

    public void QuitGame()
    {
        Debug.Log("Cerrando el juego...");
        Application.Quit();
    }
}