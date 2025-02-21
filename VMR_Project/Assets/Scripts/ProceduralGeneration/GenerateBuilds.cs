using UnityEngine;
using Random = UnityEngine.Random;

public class GenerateBuilds : MonoBehaviour
{
    [SerializeField] private GameObject[] buildingPrefabs; // Referência ao array de prefabs dos prédios
    [SerializeField] private int numberOfBuildings = 100; // Quantidade de prédios a serem gerados
    
    [SerializeField] private MapLimits mapLimits; // Referência ao ScriptableObject de limites do Mapa

    private Transform parentTransform; // Referência ao objeto vazio (parent)

    void Start()
    {
        parentTransform = transform; // O script está anexado ao objeto vazio (empty object)
        GenerateBuildings();
    }

    void GenerateBuildings()
    {
        for (int i = 0; i < numberOfBuildings; i++)
        {
            // Exibir a posição gerada na consola
            Debug.Log($"Posição do prédio {i + 1}");

            // Escolher um prefab aleatório
            GameObject buildingPrefab = buildingPrefabs[Random.Range(0, buildingPrefabs.Length)];

            // Instanciar o prédio na posição gerada como filho do objeto vazio
            GameObject buildingInstance = Instantiate(buildingPrefab, GetRandomBuildingPosition(), Quaternion.identity);

            // Definir o objeto vazio (parent) como o pai do prédio instanciado
            buildingInstance.transform.SetParent(parentTransform);

            // Verificar se o prédio recém-instanciado está a colidir com outros prédios
            if (IsCollidingWithOtherBuildings(buildingInstance))
            {
                // Destroi o prédio recém-criado se estiver a colidri com outro
                Debug.Log($"Prédio {buildingInstance.name} destruído por colisão ao ser instanciado");
                Destroy(buildingInstance);
            }
        }
    }

    Vector3 GetRandomBuildingPosition()
    {
        // Colocar prédios nas bordas da pista
        bool placeOnXAxis = Random.Range(0, 2) == 0;

        float xPos = 0f;
        float zPos = 0f;

        if (placeOnXAxis)
        {
            xPos = Random.Range(mapLimits.MinX, mapLimits.MaxX);
            zPos = Random.Range(mapLimits.MinZ, mapLimits.MaxZ);

            // Afasta um pouco da pista
            if (Random.Range(0, 2) == 0)
            {
                zPos = mapLimits.MinZ - 10f; // Borda inferior
            }
            else
            {
                zPos = mapLimits.MaxZ + 10f; // Borda superior
            }
        }
        else
        {
            zPos = Random.Range(mapLimits.MinZ, mapLimits.MaxZ);
            xPos = Random.Range(mapLimits.MinX, mapLimits.MaxX);

            // Afasta um pouco da pista
            if (Random.Range(0, 2) == 0)
            {
                xPos = mapLimits.MinX - 10f; // Borda esquerda
            }
            else
            {
                xPos = mapLimits.MaxX + 10f; // Borda direita
            }
        }

        return new Vector3(xPos, 0f, zPos);
    }
    
    // Função para verificar a colisão com outros prédios
    bool IsCollidingWithOtherBuildings(GameObject buildingInstance)
    {
        // Obtém o colisor do prédio instanciado
        Collider buildingCollider = buildingInstance.GetComponent<Collider>();

        // Verifica se o colisor se estás a sobrepor a outros colisores de prédios
        Collider[] hitColliders = Physics.OverlapBox(buildingCollider.bounds.center, buildingCollider.bounds.extents,
            Quaternion.identity);

        // Verifica se há colisões com outros prédios
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Building") && hitCollider.gameObject != buildingInstance)
            {
                return true; // Está a colidir com outro prédio
            }
        }

        return false; // Não há colisões
    }
}
