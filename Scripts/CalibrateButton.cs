using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CalibrateButton : MonoBehaviour
{
    public List<GameObject> toggles;
    private List<string> toCalibrateMuscles;
    private void Awake()
    {
        if (CalibrateMain.Instance != null)
        {
            
            //toggles = CalibrateMain.Instance.toggles; // Lista de toggles
            toCalibrateMuscles = CalibrateMain.Instance.calibrateMusclesList; // Lista de músculos a calibrar
        }
        else
        {
            Debug.LogError("CalibrateMain.Instance no encontrado.");
        }
    }
    public void OnCalibrateButtonClicked()
    {
        Debug.Log("Calibrate Button Clicked");

        SaveTogglesSelected();
    }
    

    public void LoadStimulationScene()
    {
        Debug.Log("Músculos guardados en CalibrateMain: " + string.Join(", ", CalibrateMain.Instance.calibrateMusclesList));

        Debug.Log("Cargando la escena de estimulación...");
        SceneManager.LoadScene("StimulationScene");
    }
    private void SaveTogglesSelected()
    {
        
        if (toggles == null || toggles.Count == 0) // Check if toggles list is empty or null
        {
            Debug.LogError("CalibrateButton. SaveTogglesSelected. No se encontraron toggles.");
            return;
        }
        if (toCalibrateMuscles == null || toCalibrateMuscles.Count == 0)
        {
            Debug.Log("No hay músculos seleccionados para calibrar.");
            
        return;
        }

        foreach (GameObject toggle in toggles)
        {
            Toggle toggleComponent = toggle.GetComponent<Toggle>();
            Text muscleName = toggle.GetComponentInChildren<Text>();
            if (muscleName == null)
            {
                Debug.LogError("MuscleSelectionManager. InitializeToggles. No se encontró el componente Text en el toggle.");
                return;
            }
            string muscleText = muscleName.text.Trim();
            toggleComponent.isOn = toCalibrateMuscles.Contains(muscleText);
            toggleComponent.onValueChanged.AddListener((isSelected) => ToggleMuscleSelection(muscleText , isSelected));
        }
        Debug.Log("Toggles seleccionados: " + string.Join(", ", toCalibrateMuscles));
    }

    public void ToggleMuscleSelection(string muscleName, bool isSelected)
    {
        if (isSelected)                                 //if the muscle is selected
        {
            if (!toCalibrateMuscles.Contains(muscleName))  //if the muscle is not in the list
            {
                toCalibrateMuscles.Add(muscleName);        //add the muscle to the list
                
            }
        }
        else
        {
            toCalibrateMuscles.Remove(muscleName);         //remove the muscle from the list
            
        }
    }
}
