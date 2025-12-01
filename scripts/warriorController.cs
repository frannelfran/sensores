using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

// Alias para sensores
using AccelerometerInput = UnityEngine.InputSystem.Accelerometer;
using MagneticFieldInput = UnityEngine.InputSystem.MagneticFieldSensor;

public class WarriorController : MonoBehaviour
{
    [Header("Movimiento")]
    public float speedFactor = 5f;   // Factor de velocidad
    public float rotationSpeed = 2f; // Velocidad de interpolación de rotación

    [Header("GPS Rango permitido")]
    public float latMin = 28.45f, latMax = 28.46f;
    public float lonMin = -16.28f, lonMax = -16.27f;

    [Header("UI Texts")]
    public TextMeshProUGUI accelText;
    public TextMeshProUGUI magneticText;
    public TextMeshProUGUI gpsText;

    private Quaternion currentRotation;

    void OnEnable()
    {
        if (AccelerometerInput.current != null)
            InputSystem.EnableDevice(AccelerometerInput.current);

        if (MagneticFieldInput.current != null)
            InputSystem.EnableDevice(MagneticFieldInput.current);
    }

    void Start()
    {
        currentRotation = transform.rotation;

        // Iniciar GPS
        if (Input.location.isEnabledByUser)
            Input.location.Start();
        else
            gpsText.text = "GPS: No habilitado";
    }

    void Update()
    {
        OrientarAlNorte();
        MoverConAcelerometro();
        ComprobarGPS();
    }

    void OrientarAlNorte()
    {
        if (MagneticFieldInput.current != null)
        {
            Vector3 magnetic = MagneticFieldInput.current.magneticField.ReadValue();

            // Proyectamos en plano XZ para obtener dirección norte
            Vector3 north = new Vector3(magnetic.x, 0, magnetic.z);

            if (north.sqrMagnitude > 0.01f)
            {
                // Ajuste de orientación: Horizontal Izquierda
                // Rotamos 90° alrededor de Y para que la izquierda vertical sea la parte inferior
                Quaternion correction = Quaternion.Euler(0, -90, 0);

                Quaternion targetRotation = Quaternion.LookRotation(north.normalized, Vector3.up) * correction;

                // Interpolación suave con Slerp
                currentRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                transform.rotation = currentRotation;
            }

            magneticText.text = "Magnetic: " + magnetic.ToString("F2");
        }
        else
        {
            magneticText.text = "Magnetic: No detectado";
        }
    }

    void MoverConAcelerometro()
    {
        if (AccelerometerInput.current != null)
        {
            Vector3 accel = AccelerometerInput.current.acceleration.ReadValue();

            // Movimiento hacia adelante/atrás con eje Z invertido
            float forward = -accel.z;
            Vector3 move = transform.forward * forward * speedFactor * Time.deltaTime;
            transform.Translate(move, Space.World);

            accelText.text = "Accel: " + accel.ToString("F2");
        }
        else
        {
            accelText.text = "Accel: No detectado";
        }
    }

    void ComprobarGPS()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            var loc = Input.location.lastData;
            gpsText.text = $"GPS: Lat {loc.latitude:F5}, Lon {loc.longitude:F5}";

            if (loc.latitude < latMin || loc.latitude > latMax ||
                loc.longitude < lonMin || loc.longitude > lonMax)
            {
                // Si está fuera del rango, no se mueve
                Debug.Log("Fuera de rango GPS, guerrero detenido");
                // Opcional: puedes poner animación de idle aquí
            }
        }
        else
        {
            gpsText.text = "GPS: No disponible";
        }
    }
}