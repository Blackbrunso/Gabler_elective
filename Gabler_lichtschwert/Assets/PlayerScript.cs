using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float radius = 2f;
    public Transform targetObject; // Das leere GameObject über dem Player
    public ConnectArduino bluetoothReceiver; // Referenz zum Bluetooth-Script

    private float smoothedValue = 0.5f; // Startwert mittig

    void Update()
    {
        // Player bewegt sich konstant vorwärts
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (bluetoothReceiver != null && targetObject != null)
        {
            // Eingabewert glätten (zwischen 0 und 1)
            float rawValue = bluetoothReceiver.RotationValue;
            smoothedValue = Mathf.Lerp(smoothedValue, rawValue, Time.deltaTime * 5f);

            // Umrechnen auf Winkel
            float angleDeg = Mathf.Lerp(-90f, 90f, 1-smoothedValue);
            float angleRad = angleDeg * Mathf.Deg2Rad;

            // Offset auf Halbkreis berechnen (X oben, Y Höhe)
            Vector3 offset = new Vector3(
                Mathf.Sin(angleRad),   // X-Achse
                Mathf.Cos(angleRad),   // Y-Achse
                0f
            ) * radius;

            // Neue Position
            Vector3 targetPosition = transform.position + offset;
            targetObject.position = targetPosition;

            // Rotation vom Mittelpunkt nach außen
            Vector3 direction = (targetObject.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            targetObject.rotation = Quaternion.Slerp(
                targetObject.rotation,
                targetRotation,
                Time.deltaTime * 5f
            );
        }
    }
}
