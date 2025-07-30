using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustPanelSize : MonoBehaviour
{
    public RectTransform content; // El contenedor que contiene las checkboxes.
    public RectTransform panel; // El cuadro externo que quieres ajustar.

    private void Update()
    {
        // Ajusta la altura del panel al tamaño del contenido.
        Vector2 newSize = new Vector2(panel.sizeDelta.x, content.sizeDelta.y + 20); // Añade un margen opcional.
        panel.sizeDelta = newSize;
    }
}
