using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PhotoManager : MonoBehaviour
{
    public RawImage photoFrame; // Referência ao RawImage na cena de pausa

    void Start()
    {
        LoadCapturedPhoto();
    }

    void LoadCapturedPhoto()
    {
        string path = Path.Combine(Application.persistentDataPath, "captured_photo.png");
        if (File.Exists(path))
        {
            byte[] fileData = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData); // Carrega a imagem como uma textura
            photoFrame.texture = texture;
        }
        else
        {
            Debug.LogError("Foto capturada não encontrada!");
        }
    }
}