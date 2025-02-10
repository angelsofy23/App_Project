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
    private int currentCameraIndex = 0; // Track current camera

    void Start()
    {
        StartCoroutine(RequestCameraPermission());
    }

    IEnumerator RequestCameraPermission()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            Debug.LogError("Permissão de câmera negada!");
            yield break;
        }
    }

    public void OpenCamera()
    {
        settingsPanel.SetActive(false);
        cameraPanel.SetActive(true);
        StartCoroutine(StartCamera());
    }

    IEnumerator StartCamera()
    {
        if (WebCamTexture.devices.Length == 0)
        {
            Debug.LogError("Nenhuma câmera disponível!");
            yield break;
        }

        webcamTexture = new WebCamTexture(WebCamTexture.devices[currentCameraIndex].name);
        cameraView.texture = webcamTexture;
        webcamTexture.Play();

        while (webcamTexture.width < 100)
        {
            yield return null;
        }
    }

    public void SwitchCamera()
    {
        if (webcamTexture != null)
        {
            webcamTexture.Stop();
        }

        currentCameraIndex++;
        if (currentCameraIndex >= WebCamTexture.devices.Length)
        {
            currentCameraIndex = 0;
        }

        StartCoroutine(StartCamera());
    }

    public void CapturePhoto()
    {
        StartCoroutine(TakePhoto());
    }

    IEnumerator TakePhoto()
    {
        yield return new WaitForEndOfFrame();

        capturedImage = new Texture2D(webcamTexture.width, webcamTexture.height);
        capturedImage.SetPixels(webcamTexture.GetPixels());
        capturedImage.Apply();

        // Salvar a imagem no dispositivo
        string path = Path.Combine(Application.persistentDataPath, "captured_photo.png");
        File.WriteAllBytes(path, capturedImage.EncodeToPNG());
        Debug.Log("Foto salva em: " + path);

        // Mostrar a foto no Settings
        photoFrame.texture = capturedImage;

        // Guardar na galeria do telemóvel
        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(path, "Overdrive", "photo.png");
        Debug.Log("Permissão da galeria: " + permission);
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
