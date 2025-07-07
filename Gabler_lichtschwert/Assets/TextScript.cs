using UnityEngine;
using TMPro;

public class TextScript : MonoBehaviour
{
    public float lifetime = 2f;           // Dauer bis zum Verschwinden
    public float floatSpeed = 0.5f;       // Aufwärtsbewegung
    public float fadeDuration = 1f;       // Wie lange das Ausfaden dauert

    private float timer;
    private TextMeshPro text;
    private Color originalColor;
    private Transform cam;

    void Start()
    {
        text = GetComponent<TextMeshPro>();
        if (text == null)
        {
            Debug.LogError("Kein TextMeshPro-Komponente gefunden!");
            enabled = false;
            return;
        }

        cam = Camera.main.transform;
        originalColor = text.color;
        timer = 0f;
    }

    void Update()
    {
        // Kamera-Blickrichtung
        transform.LookAt(transform.position + cam.forward);

        // Nach oben bewegen
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        timer += Time.deltaTime;

        // Ausblenden starten
        if (timer >= lifetime - fadeDuration)
        {
            float fadeAmount = 1 - ((timer - (lifetime - fadeDuration)) / fadeDuration);
            Color newColor = originalColor;
            newColor.a = Mathf.Clamp01(fadeAmount);
            text.color = newColor;
        }

        // Zerstören
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
