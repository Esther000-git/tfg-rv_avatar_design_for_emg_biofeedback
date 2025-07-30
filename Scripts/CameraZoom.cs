using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Transform target;
    public float zoomSpeed = 10f;
    public float minZoom = 0.5f;
    public float maxZoom = 15f;

    private float distance;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Falta asignar el avatar como objetivo de la cámara.");
            return;
        }

        distance = Vector3.Distance(transform.position, target.position);
    }

    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            distance -= scrollInput * zoomSpeed * Time.deltaTime;
            distance = Mathf.Clamp(distance, minZoom, maxZoom);
            Vector3 direction = (transform.position - target.position).normalized;
            transform.position = target.position + direction * distance;
        }
    }
}


