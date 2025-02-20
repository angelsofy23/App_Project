using UnityEngine;
using UnityEngine.UI;

public class LaunchMissil : MonoBehaviour
{
    // Prefab do míssil, velocidade do míssil e botão para lançar
    [SerializeField] private GameObject missilPrefab;
    [SerializeField] private float missilSpeed;

    // Referência ao botão de lançamento do míssil
    [SerializeField] private Button missilButton;
    private float transparencyValue = 125f;
    private bool canLaunchMissil = true;

    // Tempo de recarga do botão
    private float time = 10.0f;
    private float timer;
    
    void Start()
    {
        // Inicializa o timer com o valor definido
        timer = time;
        // Adiciona um listener para o evento de clique do botão
        missilButton.onClick.AddListener(OnMissilButtonPressed);
    }
    
    void Update()
    {
        // Se não for possível lançar o míssil, vamos atualizar o timer
        if (!canLaunchMissil)
        {
            timer += Time.deltaTime;

            if (timer >= time)
            {
                // Permite o lançamento do míssil novamente
                canLaunchMissil = true;
                // Torna o botão interativo de novo
                missilButton.interactable = true;
            }
        }
    }

    private void OnMissilButtonPressed()
    {
        // Se o lançamento do míssil for permitido
        if (canLaunchMissil)
        {
            // Cria o míssil no local e rotação do objeto que tem esse script
            var missil = Instantiate(missilPrefab, transform.position, transform.rotation);
            // Dá ao míssil uma velocidade inicial para ele se mover
            missil.GetComponent<Rigidbody>().linearVelocity = transform.forward * missilSpeed;

            // Ajusta a configuração de transparência do botão
            SetButtonConfiguration(transparencyValue);

            // Impede o lançamento de outro míssil até que o tempo de recarga seja concluído
            canLaunchMissil = false;
            // Desabilita a interação com o botão até que o recarregamento seja concluído
            missilButton.interactable = false;

            // Reinicia o timer para começar a contagem do tempo de recarga
            timer = 0;  
        }
    }

    private void SetButtonConfiguration(float value)
    {
        // Obtém a configuração atual de cores do botão
        ColorBlock colors = missilButton.colors;

        // Ajusta a transparência da cor normal do botão
        colors.normalColor = new Color(colors.normalColor.r, colors.normalColor.g, colors.normalColor.b, value);

        // Aplica as novas cores ao botão
        missilButton.colors = colors;
    }
}
