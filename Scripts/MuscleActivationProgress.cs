using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MuscleActivationProgress  : MonoBehaviour
{
    public Image gradientImage;
    public Image arrowImage;
    public TextMeshProUGUI activationText;
    private UnityEngine.Vector2 startPositionArrow;
    private UnityEngine.Vector2 endPositionArrow;
    private UnityEngine.Vector2 startPositionText;
    private UnityEngine.Vector2 endPositionText;
     //Activation value (percentage between 0 and 1)
    [Range(0, 1)]
    public float activationPercentage = 0.0f;

    private void Start()
    {
        Debug.Log("Muscle Activation Progress. Instance inicializado correctamente.");
        RectTransform gradientRectTransform = gradientImage.rectTransform;
        float arrowOffsetY = -30f; //position the arrow below the gradiente image
        //Set initial and final position for the arrow based on gradient size
        startPositionArrow = new UnityEngine.Vector2(gradientRectTransform.rect.xMin, gradientRectTransform.anchoredPosition.y + arrowOffsetY);
        endPositionArrow = new UnityEngine.Vector2 (gradientRectTransform.rect.xMax, gradientRectTransform.anchoredPosition.y + arrowOffsetY);  

        startPositionText = startPositionArrow + new UnityEngine.Vector2(130,-15);
        endPositionText = endPositionArrow + new UnityEngine.Vector2(10,-15);
    }

    private void Update()
    {
        UpdateArrowPosition();
        UpdateActivationText();
    }

    private void UpdateArrowPosition()
    {
        // Lineal Interpolation
        UnityEngine.Vector2 newPosition = UnityEngine.Vector2.Lerp(startPositionArrow, endPositionArrow, activationPercentage);
        arrowImage.rectTransform.anchoredPosition = newPosition;
    }
    private void UpdateActivationText(){
        UnityEngine.Vector2 textPosition = UnityEngine.Vector2.Lerp(startPositionText,endPositionText,activationPercentage);
        activationText.rectTransform.anchoredPosition = textPosition;
        activationText.text = activationPercentage.ToString("P0");
        //activationText.text = (activationPercentage * 100).ToString("F0") + "%";

    }

    // This function let to change the activation percentage
    public void SetActivationPercentage(float percentage)
    {
        activationPercentage = Mathf.Clamp01(percentage); // valor esté entre 0 y 1
        Debug.Log("Muscle Activation Progress. SetActivationPercentage: " + activationPercentage);
    }

    public float GetActivationPercentage()
    {
        return activationPercentage;
    }
}
