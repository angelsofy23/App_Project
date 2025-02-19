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
    public TMP_Text errorMessage;
    public TMP_Text errorMessageChangePanel;

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
        if (string.IsNullOrWhiteSpace(emailInput.text) || string.IsNullOrWhiteSpace(usernameInput.text))
        {
            errorMessage.gameObject.SetActive(true);
            return;
        }

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
            if (string.IsNullOrWhiteSpace(editEmailInput.text) || string.IsNullOrWhiteSpace(editUsernameInput.text))
        {
            errorMessageChangePanel.gameObject.SetActive(true);
            return;
        }

        PlayerPrefs.SetString("Email", editEmailInput.text);
        PlayerPrefs.SetString("Username", editUsernameInput.text);
        PlayerPrefs.Save();
        LoadPlayerData();
        editPanel.SetActive(false);
    }

    public void OffEditPanel()
    {
        editPanel.SetActive(false);
        errorMessageChangePanel.gameObject.SetActive(false);
    }
}
