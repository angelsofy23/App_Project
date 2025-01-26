using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsBehavior : MonoBehaviour
{
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject mainMenu;

    void Start()
    {
        // Make sure options menu is hidden at start
        if (optionsMenu != null)
            optionsMenu.SetActive(false);
    }

    void Update()
    {

    }

    public void StartGame()
    {
        SceneManager.LoadScene("AppGame");
    }

    public void ShowOptionsMenu()
    {
        if (optionsMenu != null)
        {
            optionsMenu.SetActive(true);
            if (mainMenu != null)
                mainMenu.SetActive(false);
        }
    }

    public void BackToMainMenu()
    {
        if (mainMenu != null)
        {
            mainMenu.SetActive(true);
            if (optionsMenu != null)
                optionsMenu.SetActive(false);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
