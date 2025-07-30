using UnityEngine;
using TMPro;

using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopupWindow : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public TextMeshProUGUI messageText;
    public RectTransform popupRect; // El contenedor de la ventanita
    public RectTransform titleBar;
    private Vector2 originalSizePanel;
    private Vector2 originalPositionPanel;
    private Vector2 pointerOffset;
    private bool isMaximized = false;
    private RectTransform messageRectTransform;

    void Start()
    {
        originalSizePanel = popupRect.sizeDelta;
        originalPositionPanel = popupRect.anchoredPosition;
        
        
    }
    public void SetMessage(string message)
    {
        messageText.text = message;
    }

    public void ClosePopup()
    {
        Debug.Log("Botón close pulsado");
        Destroy(gameObject); // o gameObject.SetActive(false) si lo reutilizas
    }
    public void Maximize()
    {
        if (isMaximized) return;

        // Guardamos datos originales
        originalSizePanel = popupRect.sizeDelta;
        originalPositionPanel = popupRect.anchoredPosition;

        // Expandimos popup a todo el canvas
        popupRect.anchorMin = new Vector2(0, 0);
        popupRect.anchorMax = new Vector2(1, 1);
        popupRect.offsetMin = Vector2.zero;
        popupRect.offsetMax = Vector2.zero;

        messageText.fontSize = 30; // Tamaño de fuente al maximizar
        messageRectTransform = messageText.rectTransform;

        // Configurar el botón 'Close'
        GameObject closeButton = GameObject.Find("CloseButton"); // Asegúrate de que el botón tenga este nombre en la jerarquía
        if (closeButton != null)
        {
            RectTransform closeButtonRect = closeButton.GetComponent<RectTransform>();
            closeButtonRect.sizeDelta = new Vector2(120, 50);
            closeButtonRect.anchoredPosition = new Vector2(6, -175);

            TextMeshProUGUI closeButtonText = closeButton.GetComponentInChildren<TextMeshProUGUI>();
            if (closeButtonText != null)
            {
                closeButtonText.fontSize = 20;
            }
        }
        RectTransform redButton = titleBar.GetChild(0).GetComponent<RectTransform>();
        redButton.sizeDelta = new Vector2(25, 25);
        redButton.anchoredPosition = new Vector2(425, 0);
        RectTransform yellowButton = titleBar.GetChild(1).GetComponent<RectTransform>();
        yellowButton.sizeDelta = new Vector2(25, 25);
        yellowButton.anchoredPosition = new Vector2(400, 0);
        RectTransform greenButton = titleBar.GetChild(2).GetComponent<RectTransform>();
        greenButton.sizeDelta = new Vector2(25, 25);
        greenButton.anchoredPosition = new Vector2(375, 0);

        isMaximized = true;
    }

    public void Restore()
    {
        if (!isMaximized) return;
        Debug.Log("Restaurando tamaño y posición del panel.");
        // Restauramos tamaño y posición del panel
        popupRect.anchorMin = new Vector2(0.5f, 0.5f);
        popupRect.anchorMax = new Vector2(0.5f, 0.5f);
        popupRect.sizeDelta = originalSizePanel;
        popupRect.anchoredPosition = originalPositionPanel;
        
        GameObject closeButton = GameObject.Find("CloseButton"); // Asegúrate de que el botón tenga este nombre en la jerarquía
        if (closeButton != null)
        {
            RectTransform closeButtonRect = closeButton.GetComponent<RectTransform>();
            closeButtonRect.sizeDelta = new Vector2(60, 25);
            closeButtonRect.anchoredPosition = new Vector2(6, -75);

            TextMeshProUGUI closeButtonText = closeButton.GetComponentInChildren<TextMeshProUGUI>();
            if (closeButtonText != null)
            {
                closeButtonText.fontSize = 11;
            }
        }
        RectTransform redButton = titleBar.GetChild(0).GetComponent<RectTransform>();
        redButton.sizeDelta = new Vector2(15, 15);
        redButton.anchoredPosition = new Vector2(145, 0);
        RectTransform yellowButton = titleBar.GetChild(1).GetComponent<RectTransform>();
        yellowButton.sizeDelta = new Vector2(15, 15);
        yellowButton.anchoredPosition = new Vector2(130, 0);
        RectTransform greenButton = titleBar.GetChild(2).GetComponent<RectTransform>();
        greenButton.sizeDelta = new Vector2(15, 15);
        greenButton.anchoredPosition = new Vector2(115, 0);
        isMaximized = false;
    }



    // Mover ventana con titleBar
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerEnter == titleBar.gameObject)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                popupRect,
                eventData.position,
                eventData.pressEventCamera,
                out pointerOffset
            );
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerEnter == titleBar.gameObject && !isMaximized)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                popupRect.parent as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out var localPointerPosition))
            {
                popupRect.anchoredPosition = localPointerPosition - pointerOffset;
            }
        }
    }
}
