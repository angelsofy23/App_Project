using UnityEngine;
using UnityEngine.UI;

public class LaunchMissil : MonoBehaviour
{
    [SerializeField] private GameObject missilPrefab;
    [SerializeField] private float missilSpeed;
    
    [SerializeField] private Button missilButton;
    private float transparencyValue = 125f;
    private bool canLaunchMissil = true;

    private float time = 10.0f;
    private float timer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = time;
        missilButton.onClick.AddListener(OnMissilButtonPressed);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!canLaunchMissil)
        {
            timer += Time.deltaTime;

            if (timer >= time)
            {
                canLaunchMissil = true;
                missilButton.interactable = true;
            }
        }
    }

    private void OnMissilButtonPressed()
    {
        if (canLaunchMissil)
        {
            var missil = Instantiate(missilPrefab, transform.position, transform.rotation);
            missil.GetComponent<Rigidbody>().linearVelocity = transform.forward * missilSpeed;
        
            SetButtonConfiguration(transparencyValue);
        
            canLaunchMissil = false;
            missilButton.interactable = false;

            timer = 0;  
        }
    }

    private void SetButtonConfiguration(float value)
    {
        ColorBlock colors = missilButton.colors;
        colors.normalColor = new Color(colors.normalColor.r, colors.normalColor.g, colors.normalColor.b, value);
        missilButton.colors = colors;
    }
}
