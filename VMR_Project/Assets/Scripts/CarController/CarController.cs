using UnityEngine;

public class CarController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;

    [Header("Sounds and Effects")]
    public ParticleSystem[] smokeEffects;
    private bool smokeEffectsEnabled;

    public AudioSource engineSound;

    [Header("Lap")]
    public int maxLaps;
    public int currentLap;

    // Configurações
    [SerializeField] private float motorForce, breakForce, maxSteerAngle, steeringSpeed;
    [SerializeField] private float maximumSpeed = 200f; // Velocidade máxima do carro

    private float currentSpeed;
    private bool handBrake;

    // Colliders das rodas
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Rodas
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    void Start()
    {
        engineSound.loop = true;
        engineSound.playOnAwake = false;
        engineSound.volume = 0.1f;
        engineSound.pitch = 0.7f;
        engineSound.Play();

        maxLaps = FindObjectOfType<FinishSystem>().maxLaps;
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        CalculateSteering();
        UpdateWheels();
        ApplyTransformToWheels();
        UpdateEngineSound();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;

        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;

        // Calcula a velocidade do carro em km/h
        currentSpeed = Mathf.Abs(frontLeftWheelCollider.rpm) * (2 * Mathf.PI * frontLeftWheelCollider.radius) * 60 / 1000;
        handBrake = isBreaking;

        // Ativa o fumo quando o carro estiver a travar
        if (isBreaking && !smokeEffectsEnabled)
        {
            EnableSmokeEffects(true);
            smokeEffectsEnabled = true;
        }
        else if (!isBreaking && smokeEffectsEnabled)
        {
            EnableSmokeEffects(false);
            smokeEffectsEnabled = false;
        }
    }

    private void UpdateEngineSound()
    {
        // Normaliza a velocidade do carro entre 0 e 1
        float normalizedSpeed = Mathf.Clamp(currentSpeed / maximumSpeed, 0f, 1f);

        // Aumentar ainda mais a suavização com um valor maior no Power
        float smoothFactor = Mathf.Pow(normalizedSpeed, 0.3f);  // Aumento mais suave

        // Altere a interpolação para que o som mude mais suavemente
        // Para o pitch, use uma variação ainda mais controlada
        float targetPitch = Mathf.Lerp(0.7f, 1.0f, smoothFactor);  // Intervalo mais baixo
        float targetVolume = Mathf.Lerp(0.1f, 0.2f, smoothFactor);  // Volume mais baixo também

        // Aplicando o novo pitch e volume
        engineSound.pitch = targetPitch;
        engineSound.volume = targetVolume;
    }

    private void CalculateSteering()
    {
        float targetSteerAngle = maxSteerAngle * horizontalInput;
        currentSteerAngle = Mathf.Lerp(currentSteerAngle, targetSteerAngle, Time.deltaTime * steeringSpeed);
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    public void ApplyTransformToWheels()
    {
        Vector3 position;
        Quaternion rotation;

        frontLeftWheelCollider.GetWorldPose(out position, out rotation);
        frontLeftWheelTransform.position = position;
        frontLeftWheelTransform.rotation = rotation;

        frontRightWheelCollider.GetWorldPose(out position, out rotation);
        frontRightWheelTransform.position = position;
        frontRightWheelTransform.rotation = rotation;

        rearLeftWheelCollider.GetWorldPose(out position, out rotation);
        rearLeftWheelTransform.position = position;
        rearLeftWheelTransform.rotation = rotation;

        rearRightWheelCollider.GetWorldPose(out position, out rotation);
        rearRightWheelTransform.position = position;
        rearRightWheelTransform.rotation = rotation;
    }

    private void EnableSmokeEffects(bool enable)
    {
        foreach (ParticleSystem smokeEffect in smokeEffects)
        {
            if (enable)
            {
                smokeEffect.Play();
            }
            else
            {
                smokeEffect.Stop();
            }
        }
    }
    public void IncreaseLap()
    {
        currentLap++;
        Debug.Log(gameObject.name + " Lap: " + currentLap);
    }
}
