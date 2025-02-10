using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CameraManager : MonoBehaviour
{
    public GameObject settingsPanel, cameraPanel; // Painéis da UI
    public RawImage cameraView, photoFrame; // RawImage para a câmera e para mostrar a última foto
    private WebCamTexture webcamTexture; // Para capturar a câmera
    private Texture2D capturedImage; // Para armazenar a foto

    void Start()
    {
        
    }

    public void OpenCamera()
    {
        settingsPanel.SetActive(false);
        cameraPanel.SetActive(true);

        if (webcamTexture == null)
        {
            webcamTexture = new WebCamTexture();
            cameraView.texture = webcamTexture;
        }

        webcamTexture.Play(); // Iniciar a câmera
    }

    public void CapturePhoto()
    {
        StartCoroutine(TakePhoto());
    }

    IEnumerator TakePhoto()
    {
        yield return new WaitForEndOfFrame();

        // Criar uma textura do que a câmera está a mostrar
        capturedImage = new Texture2D(webcamTexture.width, webcamTexture.height);
        capturedImage.SetPixels(webcamTexture.GetPixels());
        capturedImage.Apply();

        // Salvar a imagem no dispositivo
        string path = Path.Combine(Application.persistentDataPath, "captured_photo.png");
        File.WriteAllBytes(path, capturedImage.EncodeToPNG());

        // Mostrar a foto no Settings
        photoFrame.texture = capturedImage;

        // Guardar na galeria do telemóvel
        NativeGallery.SaveImageToGallery(path, "Overdrive", "photo.png");

    }

    public void CloseCamera()
    {
        if (webcamTexture != null)
        {
            webcamTexture.Stop();
        }

        cameraPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }
}
