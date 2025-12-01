# Sensores

1. Crear una aplicación en Unity que muestre en la UI los valores de todos los sensores disponibles en tu móvil.

Para realizar esta práctica he creado en la escena un canvas con los textos pertenecientes a cada sensor y he agregado un elemento vacío que contiene un trozo de código que obtiene la infromación de cada sensor disponible en el dispositivo y se va actualizado a medida que este cambie de valor.

## Código asociado

```csharp
public TextMeshProUGUI accelerometerText;
    public TextMeshProUGUI gyroscopeText;
    public TextMeshProUGUI gravityText;
    public TextMeshProUGUI attitudeText;
    public TextMeshProUGUI linearAccelText;
    public TextMeshProUGUI magneticText;
    public TextMeshProUGUI lightText;
    public TextMeshProUGUI pressureText;
    public TextMeshProUGUI proximityText;
    public TextMeshProUGUI humidityText;
    public TextMeshProUGUI temperatureText;
    public TextMeshProUGUI stepCounterText;`
```

Estos son los elementos públicos donde se va a arrastar los respectivos textos de la escena que van a estar asoicados con cada sensor.

Y luego activo los sensores disponibles de la siguiente manera:

```csharp
void OnEnable()
    {
        foreach (var device in InputSystem.devices)
            InputSystem.EnableDevice(device);
    }
```

Si se encuentra el sensor pues obtengo la información de la siguiente manera:
```csharp
if (AccelerometerInput.current != null)
            accelerometerText.text = "Accel: " + AccelerometerInput.current.acceleration.ReadValue();
```
Y así con todos los sensores.

## Resultado
https://github.com/user-attachments/assets/0ccc56c5-f199-428a-a44e-2017b1dcc4db

2. Crear una apk que oriente alguno de los guerreros de la práctica mirando siempre hacia el norte, avance con una aceleración proporcional a la del dispositivo y lo pare cuando el dispositivo esté fuera de un rango de latitud, longitud dado. El acelerómetro nos dará la velocidad del movimiento. A lo largo del eje z (hacia adelante y hacia atrás), se produce el movimiento inclinando el dispositivo hacia adelante y hacia atrás. Sin embargo, necesitamos invertir el valor z porque la orientación del sistema de coordenadas corresponde con el punto de vista del dispositivo. Queremos que la rotación final coincida con la orientación cuando mantenemos el dispositivo en la posición Horizontal Izquierda. Esto ocurre cuando la izquierda en la posición vertical ahora es la parte inferior. Aplicar las rotaciones con interpolación  Slerp en un quaternion.

Para conseguir este apk, yo he creado una escena, al igual que la anterior, con un canvas pero esta vez solo muestro la información del giroscopio y del acelerómetro, a su vez, he puesto un prefab de guerrero apuntado hacía el norte y un código asociado a dicho guerrero.

## Código asociado

Activo los sensores de la misma manera al igual que el ejercicio anterior.

```csharp
void OnEnable()
    {
        // Activar dispositivos si existen
        if (AccelerometerInput.current != null)
            InputSystem.EnableDevice(AccelerometerInput.current);

        if (GyroscopeInput.current != null)
            InputSystem.EnableDevice(GyroscopeInput.current);
    }
```

Y he creado 2 métodos para manejar el acelerómetro y la orientación (moverConAcelerometro y orientarAlNorte).

```csharp
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
```
Para el acelerómetro si lo detecta del dispositivo, obtenemos sus valores son ReadValue y usamos el eje y como el eje de inclinación y trasladamos el guerrero (hacía adelante o hacía atrás).

```csharp
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
```

Con el magnetómetro ocurre lo mimsmo, si se detecta leemos sus valores y proyecto en el plano XZ para obtener la dirección al norte, luego ajusto la orientación utilizando **Quaternion** y roto el guerrero.

## Resultado

https://github.com/user-attachments/assets/a852816c-15f9-45b0-ae62-5fddea02684a


