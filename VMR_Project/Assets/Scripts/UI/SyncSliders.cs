using UnityEngine;
using UnityEngine.UI;
public class SyncSliders : MonoBehaviour
{
    Slider slider;
    AudioSource audioSource;
    public string name_object_source;
    
    void Awake()
    {
        // Obtém o componente Slider anexado a este GameObject
        slider = GetComponent<Slider>();
        
        // Obtém o componente AudioSource de um GameObject com o nome especificado
        audioSource = GameObject.Find(name_object_source).GetComponent<AudioSource>();
        
        // Define o valor inicial do Slider com base no volume atual do AudioSource
        slider.value = audioSource.volume;
    }

}

