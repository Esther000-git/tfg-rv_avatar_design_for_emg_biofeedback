using UnityEngine;

public class ZoomAvatar : MonoBehaviour
{
    public Camera camara; // Referencia a la cámara principal
    private float zoom = 10f;


    void Update()
    {
        if (camara.orthographic)
        {
            camara.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * zoom;// Volumen de la camara
        }
        else
        {
            camara.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * zoom; //Volumen de la camara
        }
    }
}


