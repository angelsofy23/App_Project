using UnityEngine;
using TMPro;

public class PlayerDataManager : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField usernameInput;
    public TMP_InputField displayEmailInput; // InputField onde será mostrado o email guardado
    public TMP_InputField displayUsernameInput; // InputField onde será mostrado o username guardado
    public GameObject registrationPanel; // Painel de registo

    void Start()
    {
        // Verifica se o jogador já preencheu os dados antes
        if (PlayerPrefs.HasKey("HasRegistered"))
        {
            registrationPanel.SetActive(false); // Esconde o painel se já tiver jogado antes
            
            string savedEmail = PlayerPrefs.GetString("Email", "");
            string savedUsername = PlayerPrefs.GetString("Username", "");

            // Preenche os campos de registo (caso o jogador queira mudar os dados)
            emailInput.text = savedEmail;
            usernameInput.text = savedUsername;

            // Preenche as caixas de input no menu principal (ou em qualquer outro sítio)
            if (displayEmailInput != null) displayEmailInput.text = savedEmail;
            if (displayUsernameInput != null) displayUsernameInput.text = savedUsername;
        }
        else
        {
            registrationPanel.SetActive(true); // Mostra o painel se for a primeira vez
        }
    }

    public void SavePlayerDataButton()
    {
        // Guarda os dados no PlayerPrefs
        PlayerPrefs.SetString("Email", emailInput.text);
        PlayerPrefs.SetString("Username", usernameInput.text);
        PlayerPrefs.SetInt("HasRegistered", 1); // Marca que já registrou
        PlayerPrefs.Save(); // Salva os dados

        // Atualiza os campos onde as informações são exibidas
        if (displayEmailInput != null) displayEmailInput.text = emailInput.text;
        if (displayUsernameInput != null) displayUsernameInput.text = usernameInput.text;

        registrationPanel.SetActive(false); // Esconde o painel
    }
}
