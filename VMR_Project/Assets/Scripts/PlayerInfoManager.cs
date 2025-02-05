using UnityEngine;
using TMPro;

public class PlayerDataManager : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField usernameInput;
    public TMP_InputField displayEmailInput;
    public TMP_InputField displayUsernameInput;
    public GameObject registrationPanel;
    public GameObject editPanel;
    public TMP_InputField editEmailInput;
    public TMP_InputField editUsernameInput;

    void Start()
    {
        if (PlayerPrefs.HasKey("HasRegistered"))
        {
            registrationPanel.SetActive(false);
            LoadPlayerData();
        }
        else
        {
            registrationPanel.SetActive(true);
        }
    }

    public void SavePlayerDataButton()
    {
        PlayerPrefs.SetString("Email", emailInput.text);
        PlayerPrefs.SetString("Username", usernameInput.text);
        PlayerPrefs.SetInt("HasRegistered", 1);
        PlayerPrefs.Save();
        LoadPlayerData();
        registrationPanel.SetActive(false);
    }

    void LoadPlayerData()
    {
        string savedEmail = PlayerPrefs.GetString("Email", "");
        string savedUsername = PlayerPrefs.GetString("Username", "");

        displayEmailInput.text = savedEmail;
        displayUsernameInput.text = savedUsername;
    }

    public void OpenEditPanelButton()
    {
        editPanel.SetActive(true);
        editEmailInput.text = PlayerPrefs.GetString("Email", "");
        editUsernameInput.text = PlayerPrefs.GetString("Username", "");
    }

    public void SaveEditedPlayerDataButton()
    {
        PlayerPrefs.SetString("Email", editEmailInput.text);
        PlayerPrefs.SetString("Username", editUsernameInput.text);
        PlayerPrefs.Save();
        LoadPlayerData();
        editPanel.SetActive(false);
    }

    public void OffEditPanel()
    {
        editPanel.SetActive(false);
    }
}
