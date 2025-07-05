using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float radius = 2f;
    public float rotationSpeed = 90f; 
    public Transform targetObject;
    public ConnectArduino bluetoothReceiver; 

    private float smoothedValue = 0.5f; 
    private float manualAngleOffset = 0f; 
    void Update()
    {
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
                Mathf.Sin(angleRad),   // X-Achse
                Mathf.Cos(angleRad),   // Y-Achse
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
}
