using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;

    private bool isManualBraking = false;


    private PlayerInput playerInput;
    private InputAction moveAction;

    [Header("Sounds and Effects")]
    public ParticleSystem[] smokeEffects;
    private bool smokeEffectsEnabled;

    public AudioSource engineSound;

    [Header("Lap")]
    public int maxLaps;
    public int currentLap;

    // Speed Boost
    private bool isBoosting = false;
    private float originalMotorForce;

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

        originalMotorForce = motorForce; // Salva o valor original ao iniciar o jogo

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

    public void BoostSpeed(float duration, float multiplier)
    {
        StartCoroutine(BoostSpeedCoroutine(duration, multiplier));
    }

    private IEnumerator BoostSpeedCoroutine(float duration, float multiplier)
    {
        float originalMotorForce = motorForce;
        motorForce *= multiplier;

        yield return new WaitForSeconds(duration);

        motorForce = originalMotorForce;
    }

    public void ActivateSpeedBoost()
    {
        if (!isBoosting)
        {
            Debug.Log("Speed Boost ativado!");
            StartCoroutine(SpeedBoostEffect(3f, 9.0f)); // 3 segundos de boost com 1.5x a velocidade
        }
    }

    //Lógica dos botões de aceleração e travagem

    public void StartBraking()
    {
        isManualBraking = true;
        isBreaking = true;
        verticalInput = 0f; // Impede aceleração enquanto trava
    }

    public void StopBraking()
    {
        isManualBraking = false;
        isBreaking = false;
    }

    private void GetInput()
    {
        Vector2 inputVector = moveAction.ReadValue<Vector2>();
        horizontalInput = inputVector.x;

        // Se os botões não estiverem sendo usados, o joystick controla o travão
        if (!isManualBraking)
        {
            verticalInput = inputVector.y;
            isBreaking = Input.GetKey(KeyCode.Space); // Mantém o travão manual pelo teclado
        }
    }


    private IEnumerator SpeedBoostEffect(float duration, float multiplier)
    {
        isBoosting = true;
        motorForce *= multiplier;

        yield return new WaitForSeconds(duration);

        motorForce = originalMotorForce;
        isBoosting = false;
    }

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"]; // Captura o movimento do joystick
    }

}
