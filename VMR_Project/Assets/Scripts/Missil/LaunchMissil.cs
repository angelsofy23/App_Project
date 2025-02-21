using UnityEngine;
using UnityEngine.UI;

public class LaunchMissil : MonoBehaviour
{
    // Prefab do m�ssil, velocidade do m�ssil e bot�o para lan�ar
    [SerializeField] private GameObject missilPrefab;
    [SerializeField] private float missilSpeed;

    // Refer�ncia ao bot�o de lan�amento do m�ssil
    [SerializeField] private Button missilButton;
    private float transparencyValue = 125f;
    private bool canLaunchMissil = true;

    // Tempo de recarga do bot�o
    private float time = 10.0f;
    private float timer;
    
    void Start()
    {
        // make rotation be 0 0 0
        transform.rotation = Quaternion.Euler(0, 0, 0);
        // Inicializa o timer com o valor definido
        timer = time;
        // Adiciona um listener para o evento de clique do bot�o
        missilButton.onClick.AddListener(OnMissilButtonPressed);
    }
    
    void Update()
    {
        // Se n�o for poss�vel lan�ar o m�ssil, vamos atualizar o timer
        if (!canLaunchMissil)
        {
            timer += Time.deltaTime;

            if (timer >= time)
            {
                // Permite o lan�amento do m�ssil novamente
                canLaunchMissil = true;
                // Torna o bot�o interativo de novo
                missilButton.interactable = true;
            }
        }
    }

    private void OnMissilButtonPressed()
    {
        // Se o lan�amento do m�ssil for permitido
        if (canLaunchMissil)
        {
            // Cria o m�ssil no local e rota��o do objeto que tem esse script
            var missil = Instantiate(missilPrefab, transform.position, transform.rotation);
            // D� ao m�ssil uma velocidade inicial para ele se mover
            missil.GetComponent<Rigidbody>().linearVelocity = - transform.right * missilSpeed;

            // Ajusta a configura��o de transpar�ncia do bot�o
            SetButtonConfiguration(transparencyValue);

            // Impede o lan�amento de outro m�ssil at� que o tempo de recarga seja conclu�do
            canLaunchMissil = false;
            // Desabilita a intera��o com o bot�o at� que o recarregamento seja conclu�do
            missilButton.interactable = false;

            // Reinicia o timer para come�ar a contagem do tempo de recarga
            timer = 0;  
        }
    }

    private void SetButtonConfiguration(float value)
    {
        // Obt�m a configura��o atual de cores do bot�o
        ColorBlock colors = missilButton.colors;

        // Ajusta a transpar�ncia da cor normal do bot�o
        colors.normalColor = new Color(colors.normalColor.r, colors.normalColor.g, colors.normalColor.b, value);

        // Aplica as novas cores ao bot�o
        missilButton.colors = colors;
    }
}
