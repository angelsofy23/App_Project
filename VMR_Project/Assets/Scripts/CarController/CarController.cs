using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    // Variáveis de entrada do jogador
    private float horizontalInput, verticalInput; //movimento
    private float currentSteerAngle, currentbreakForce; //viragem e travagem
    private bool isBreaking;

    private bool isManualBraking = false;

    // Sistema de entrada do jogador - para fazer o movimento através do Input System
    private PlayerInput playerInput;
    private InputAction moveAction;

    [Header("Sounds and Effects")]
    public ParticleSystem[] smokeEffects; // Efeito de fumo ao travar
    private bool smokeEffectsEnabled; 

    public AudioSource engineSound; // Som do motor

    [Header("Lap")] //Sistema de voltas
    public int maxLaps; // Número máximo de voltas
    public int currentLap; // Volta atual

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
        // Configuração inicial do som do motor
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
        GetInput(); // Captura a entrada do jogador
        HandleMotor(); // Aplica aceleração e travagem
        CalculateSteering(); // Calcula a direção do volante
        UpdateWheels(); // Atualiza a posição das rodas
        ApplyTransformToWheels(); // Aplica transformações visuais às rodas
        UpdateEngineSound(); // Atualiza o som do motor baseado na velocidade
    }

    private void HandleMotor()
    {
        // Aplica torque aos motores das rodas dianteiras
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;

        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        // Aplica força de travagem a todas as rodas
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

        float targetPitch = Mathf.Lerp(0.7f, 1.0f, smoothFactor);  // Intervalo mais baixo
        float targetVolume = Mathf.Lerp(0.1f, 0.2f, smoothFactor);  // Volume mais baixo também

        
        engineSound.pitch = targetPitch;
        engineSound.volume = targetVolume;
    }

    private void CalculateSteering()  // Calcula o ângulo de direção do carro
    {
        float targetSteerAngle = maxSteerAngle * horizontalInput;
        currentSteerAngle = Mathf.Lerp(currentSteerAngle, targetSteerAngle, Time.deltaTime * steeringSpeed);
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()  // Atualiza a posição das rodas com base nos colliders
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        // Obtém a posição e rotação da roda a partir do collider
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);

        // Aplica a rotação e posição ao transform da roda
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    public void ApplyTransformToWheels()
    {
        // Atualiza a posição e rotação de todas as rodas
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
        // Ativa ou desativa os efeitos de fumo nas rodas
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
        // Incrementa a contagem de voltas e exibe na consola
        currentLap++;
        Debug.Log(gameObject.name + " Lap: " + currentLap);
    }

    public void BoostSpeed(float duration, float multiplier)
    {
        // Inicia um boost de velocidade por tempo determinado
        StartCoroutine(BoostSpeedCoroutine(duration, multiplier));
    }

    private IEnumerator BoostSpeedCoroutine(float duration, float multiplier)
    {
        // Aumenta temporariamente a força do motor
        float originalMotorForce = motorForce;
        motorForce *= multiplier;

        // Aguarda o tempo do boost
        yield return new WaitForSeconds(duration);

        // Restaura a força original do motor
        motorForce = originalMotorForce;
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

        // Se os botões não estiverem  a ser usados, o joystick controla o travão
        if (!isManualBraking)
        {
            verticalInput = inputVector.y;
            isBreaking = Input.GetKey(KeyCode.Space); // Mantém o travão manual pelo teclado
        }
    }

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"]; // Captura o movimento do joystick
    }

}
