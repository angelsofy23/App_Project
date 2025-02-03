using UnityEngine;
using TMPro;

public class MobileKeyboardFix : MonoBehaviour
{
    public TMP_InputField inputField;
    private TouchScreenKeyboard keyboard;

    void Start()
    {
        inputField.onSelect.AddListener(OpenKeyboard);
    }

    void OpenKeyboard(string text)
    {
        if (keyboard == null || !keyboard.active)
        {
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        }
    }
}
