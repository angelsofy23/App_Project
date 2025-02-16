using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsBehavior : MonoBehaviour
{
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject menuPausa;
    [SerializeField] private GameObject howToPlayPanel;
    
    void Start()
    {
        Time.timeScale = 1f;
        
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

    public void GoToOptionsMenu()
    {
        if (optionsMenu != null)
        {
            optionsMenu.SetActive(true);
            if (mainMenu != null)
                mainMenu.SetActive(false);
        }
    }

    public void OffOptionsMenu()
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

     public void ClickSoundButton()
    {
        //audioSource.PlayOneShot(hoverSound);
        AudioManager.Instance.PlaySoundEffects("Click Button");
    }

    public void PauseGame()
    {
            menuPausa.SetActive(true);
            Time.timeScale = 0f;
    }

    public void ContinueGame()
    {
        menuPausa.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GoHome()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GoHowToPlayPanel()
    {
        howToPlayPanel.SetActive(true);
    }

    public void OffHowToPlayPanel()
    {
        howToPlayPanel.SetActive(false);
    }
}
