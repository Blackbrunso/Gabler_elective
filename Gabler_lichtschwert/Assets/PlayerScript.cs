using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;                // Aktuelle Geschwindigkeit
    private float zielspeed;               // Zielgeschwindigkeit
    public float maxSpeed = 10f;           // Obergrenze der Geschwindigkeit
    public float acceleration = 0.5f;      // Wie schnell man schneller wird

    public float radius = 2f;
    public float rotationSpeed = 90f;
    public Transform targetObject;
    public ConnectArduino bluetoothReceiver;
    private bool busted= false;
    private float smoothedValue = 0.5f;
    private float manualAngleOffset = 0f;

    private void Start()
    {
        zielspeed = speed;
    }

    void Update()
    {
        // Automatisch beschleunigen, aber nicht über maxSpeed
        if (speed < maxSpeed)
        {
            speed += acceleration * Time.deltaTime;
            speed = Mathf.Min(speed, maxSpeed); // Begrenzung auf maxSpeed
        }

        if(busted)
        {
            if (zielspeed < speed)
            {
                speed -= 1f * Time.deltaTime;
            }
            else if (zielspeed > speed)
            {
                speed += 1f * Time.deltaTime;
            }
        }
        

        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (bluetoothReceiver != null && targetObject != null)
        {
            float input = 0f;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                input = -1f;
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                input = 1f;

            manualAngleOffset += input * rotationSpeed * Time.deltaTime;
            manualAngleOffset = Mathf.Clamp(manualAngleOffset, -90f, 90f);

            float rawValue = bluetoothReceiver.RotationValue;
            smoothedValue = Mathf.Lerp(smoothedValue, rawValue, Time.deltaTime * 5f);

            float angleDeg = Mathf.Lerp(-90f, 90f, 1 - smoothedValue) + manualAngleOffset;
            angleDeg = Mathf.Clamp(angleDeg, -90f, 90f);
            float angleRad = angleDeg * Mathf.Deg2Rad;

            Vector3 offset = new Vector3(
                Mathf.Sin(angleRad),
                Mathf.Cos(angleRad),
                0f
            ) * radius;

            Vector3 targetPosition = transform.position + offset;
            targetObject.position = targetPosition;

            Vector3 direction = (targetObject.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            targetObject.rotation = Quaternion.Slerp(
                targetObject.rotation,
                targetRotation,
                Time.deltaTime * 5f
            );
        }
    }

    public void SetSpeed(float x)
    {
        busted = true;
        zielspeed = Mathf.Clamp(x, 0f, maxSpeed); // optional: absichern
    }
}
