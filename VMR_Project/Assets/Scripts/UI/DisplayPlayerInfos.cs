using UnityEngine;
using TMPro;

public class DisplayPlayerInfos : MonoBehaviour
{
    public TMP_InputField displayEmailInput;
    public TMP_InputField displayUsernameInput;

    void Start()
    {
        LoadPlayerData();
    }

    void LoadPlayerData()
    {
        string savedEmail = PlayerPrefs.GetString("Email", "");
        string savedUsername = PlayerPrefs.GetString("Username", "");

        displayEmailInput.text = savedEmail;
        displayUsernameInput.text = savedUsername;
    }
}

