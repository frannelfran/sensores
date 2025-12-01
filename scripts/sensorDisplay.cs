using UnityEngine;
using UnityEngine.InputSystem;
using TMPro; // Importante para usar TextMeshProUGUI

// Alias para evitar ambigüedad con clases del viejo sistema
using AccelerometerInput = UnityEngine.InputSystem.Accelerometer;
using GyroscopeInput = UnityEngine.InputSystem.Gyroscope;
using GravitySensorInput = UnityEngine.InputSystem.GravitySensor;
using AttitudeSensorInput = UnityEngine.InputSystem.AttitudeSensor;
using LinearAccelerationInput = UnityEngine.InputSystem.LinearAccelerationSensor;
using MagneticFieldInput = UnityEngine.InputSystem.MagneticFieldSensor;
using LightSensorInput = UnityEngine.InputSystem.LightSensor;
using PressureSensorInput = UnityEngine.InputSystem.PressureSensor;
using ProximitySensorInput = UnityEngine.InputSystem.ProximitySensor;
using HumiditySensorInput = UnityEngine.InputSystem.HumiditySensor;
using AmbientTemperatureInput = UnityEngine.InputSystem.AmbientTemperatureSensor;
using StepCounterInput = UnityEngine.InputSystem.StepCounter;

public class SensorDisplay : MonoBehaviour
{
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
    public TextMeshProUGUI stepCounterText;

    void OnEnable()
    {
        foreach (var device in InputSystem.devices)
            InputSystem.EnableDevice(device);
    }

    void Update()
    {
        if (AccelerometerInput.current != null)
            accelerometerText.text = "Accel: " + AccelerometerInput.current.acceleration.ReadValue();

        if (GyroscopeInput.current != null)
            gyroscopeText.text = "Gyro: " + GyroscopeInput.current.angularVelocity.ReadValue();

        if (GravitySensorInput.current != null)
            gravityText.text = "Gravity: " + GravitySensorInput.current.gravity.ReadValue();

        if (AttitudeSensorInput.current != null)
            attitudeText.text = "Attitude: " + AttitudeSensorInput.current.attitude.ReadValue();

        if (LinearAccelerationInput.current != null)
            linearAccelText.text = "Lin Accel: " + LinearAccelerationInput.current.acceleration.ReadValue();

        if (MagneticFieldInput.current != null)
            magneticText.text = "Magnetic: " + MagneticFieldInput.current.magneticField.ReadValue();

        if (LightSensorInput.current != null)
            lightText.text = "Light: " + LightSensorInput.current.lightLevel.ReadValue();

        if (PressureSensorInput.current != null)
            pressureText.text = "Pressure: " + PressureSensorInput.current.atmosphericPressure.ReadValue();

        if (ProximitySensorInput.current != null)
            proximityText.text = "Proximity: " + ProximitySensorInput.current.distance.ReadValue();

        // Humedad 
        humidityText.text = HumiditySensorInput.current != null
            ? "Humidity: " + HumiditySensorInput.current.relativeHumidity.ReadValue() + "%"
            : "Humidity: No detectado";

        // Temperatura
        temperatureText.text = AmbientTemperatureInput.current != null
            ? "Temp: " + AmbientTemperatureInput.current.ambientTemperature.ReadValue() + " °C"
            : "Temp: No detectado";

        if (StepCounterInput.current != null)
            stepCounterText.text = "Steps: " + StepCounterInput.current.stepCounter.ReadValue();
    }

    void OnDisable()
    {
        foreach (var device in InputSystem.devices)
            InputSystem.DisableDevice(device);
    }
}