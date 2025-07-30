using UnityEngine;

public class RotateAvatar : MonoBehaviour
{
    public float rotationSpeed = 100f; // Velocidad de rotación con teclado
    public float mouseSensitivity = 2f; // Sensibilidad del ratón

    public float zoomSpeed = 50f; // Velocidad del zoom
    public float minZoom = 5f; // Zoom mínimo (más cerca)
    public float maxZoom = 20f; // Zoom máximo (más lejos)

    void Update()
    {
        // Rotación con teclas (flechas o A/D)
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * horizontalInput * rotationSpeed * Time.deltaTime);

        // Rotación con el ratón (arrastrando horizontalmente)
        if (Input.GetMouseButton(0)) // Solo si el botón izquierdo está presionado
        {
            float mouseX = Input.GetAxis("Mouse X"); // Movimiento horizontal del ratón
            transform.Rotate(Vector3.up * mouseX * mouseSensitivity);
        }
    }
}
