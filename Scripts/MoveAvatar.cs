using UnityEngine;

public class MoveAvatar : MonoBehaviour
{
    public float moveSpeed = 0.5f; // Velocidad del movimiento

    private Vector3 lastMousePosition;

    void Update()
    {
        // Detecta si el botón izquierdo del ratón está presionado
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;

            // Convertimos el movimiento de pantalla en movimiento en el mundo (ejes X e Y)
            Vector3 move = new Vector3(delta.x, delta.y, 0f) * moveSpeed * Time.deltaTime;

            // Puedes ajustar aquí si quieres que el movimiento sea local o global
            transform.Translate(move, Space.World);

            lastMousePosition = Input.mousePosition;
        }
    }
}
