using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

// Alias para sensores
using AccelerometerInput = UnityEngine.InputSystem.Accelerometer;
using GyroscopeInput = UnityEngine.InputSystem.Gyroscope;

public class WarriorController : MonoBehaviour
{
    [Header("Movimiento")]
    public float speedFactor = 5f;   // Factor de velocidad
    public float rotationSpeed = 2f; // Velocidad de interpolación de rotación

    [Header("UI Texts")]
    public TextMeshProUGUI accelText;
    public TextMeshProUGUI gyroText;

    private Quaternion currentRotation;

    void OnEnable()
    {
        // Activar dispositivos si existen
        if (AccelerometerInput.current != null)
            InputSystem.EnableDevice(AccelerometerInput.current);

        if (GyroscopeInput.current != null)
            InputSystem.EnableDevice(GyroscopeInput.current);
    }

    void Start()
    {
        currentRotation = transform.rotation;
    }

    void Update()
    {
        MoverConAcelerometro();
        RotarConGiroscopio();
    }

    void MoverConAcelerometro()
    {
        if (AccelerometerInput.current != null)
        {
            Vector3 accel = AccelerometerInput.current.acceleration.ReadValue();

            // Usamos el eje Y como inclinación hacia adelante/atrás
            float forward = accel.y;
            Vector3 move = transform.forward * forward * speedFactor * Time.deltaTime;
            transform.Translate(move, Space.World);

            // Mostrar en UI
            accelText.text = "Accel: " + accel.ToString("F2");
        }
        else
        {
            accelText.text = "Accel: No detectado";
        }
    }

    void RotarConGiroscopio()
    {
        if (GyroscopeInput.current != null)
        {
            Vector3 angularVel = GyroscopeInput.current.angularVelocity.ReadValue();

            // Convertimos velocidad angular en rotación incremental
            Quaternion deltaRotation = Quaternion.Euler(angularVel * Mathf.Rad2Deg * Time.deltaTime);

            // Acumulamos rotación
            currentRotation *= deltaRotation;

            // Aplicamos suavizado
            transform.rotation = Quaternion.Slerp(transform.rotation, currentRotation, Time.deltaTime * rotationSpeed);

            // Mostrar en UI
            gyroText.text = "Gyro: " + angularVel.ToString("F2");
        }
        else
        {
            gyroText.text = "Gyro: No detectado";
        }
    }
}