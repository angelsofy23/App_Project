using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoManager : MonoBehaviour
{
    public GameObject infoInputPanel; 
    public TMP_InputField usernameInput;
    public TMP_InputField emailInput;

    public TMP_InputField usernameDisplay;
    public TMP_InputField emailDisplay;

    void Start()
    {
        if (!PlayerPrefs.HasKey("Username"))
        {
            infoInputPanel.SetActive(true);
        }
        else
        {
            usernameDisplay.text = PlayerPrefs.GetString("Username");
            emailDisplay.text = PlayerPrefs.GetString("Email");
        }
    }

    public void SavePlayerInfoInputPanelButton()
    {
        string username = usernameInput.text;
        string email = emailInput.text;

        PlayerPrefs.SetString("Username", username);
        PlayerPrefs.SetString("Email", email);
        PlayerPrefs.Save();

        usernameDisplay.text = username;
        emailDisplay.text = email;

        infoInputPanel.SetActive(false); // Esconder o painel após salvar
    }
}

