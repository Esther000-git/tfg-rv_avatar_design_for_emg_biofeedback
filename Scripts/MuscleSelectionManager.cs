using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuscleSelectionManager : MonoBehaviour
{
    public static MuscleSelectionManager Instance { get; private set; }
    
    public Toggle[] toggles;
    public List<string> selectedMuscles = new List<string>();
    public Dictionary<string, float> muscleMaxPotentials = new Dictionary<string, float>();
    

    private void Start()
    {
        
        // Initialize muscle  
        InitializeToggles();
    }
    private void Awake()
{
    if (Instance != null && Instance != this)
    {
        Destroy(gameObject);  // Si ya existe una instancia, destrúyela.
    }
    else
    {
        Instance = this;  // Si no, asigna esta instancia.
        DontDestroyOnLoad(gameObject);  // Previene que el objeto se destruya al cambiar de escena.
    }
}


    private void InitializeToggles()
    {
        toggles = FindObjectsOfType<Toggle>();
        foreach (Toggle toggle in toggles)
        {
            Text muscleName = toggle.GetComponentInChildren<Text>();
            if (muscleName == null)
            {
                Debug.LogError("MuscleSelectionManager. InitializeToggles. No se encontró el componente Text en el toggle.");
                return;
            }
            string muscleText = muscleName.text.Trim();
            toggle.isOn = selectedMuscles.Contains(muscleText);
            toggle.onValueChanged.AddListener((isSelected) => ToggleMuscleSelection(muscleText , isSelected));
        }
    }

    public void ToggleMuscleSelection(string muscleName, bool isSelected)
    {
        if (isSelected)                                 //if the muscle is selected
        {
            if (!selectedMuscles.Contains(muscleName))  //if the muscle is not in the list
            {
                selectedMuscles.Add(muscleName);        //add the muscle to the list
                muscleMaxPotentials[muscleName] = 0f;
                
            }
        }
        else
        {
            selectedMuscles.Remove(muscleName);         //remove the muscle from the list
            muscleMaxPotentials.Remove(muscleName);    //remove the muscle from the dictionary
        }
    }
    public List<string> GetMuscles(){
        return selectedMuscles;
    }

    public void RestoreMuscleSelection()            //restore the muscle selection
    {
        toggles = FindObjectsOfType<Toggle>();      //find the toggles
        
        foreach (Toggle toggle in toggles)          //for each toggle
        {
            Text muscleName = toggle.GetComponentInChildren<Text>(); //get the text of the toggle
            bool isSelected = selectedMuscles.Contains(muscleName.text);    //check if the muscle is selected
            
            toggle.gameObject.SetActive(isSelected);                    //set the toggle active  
            toggle.isOn = false;                                        //set the toggle on
            Debug.Log("Muscle Selection Manager. RestoreMuscleSelection. Muscle: " + toggle.name + " Selected: " + isSelected);
        }
    }

    public void SetMuscleMaxPotential(string muscle, float value)
    {
        if (muscleMaxPotentials.ContainsKey(muscle))
        {
            muscleMaxPotentials[muscle] = value;
        }
    }

    public float GetMuscleMaxPotential(string muscleName)
    {
        return muscleMaxPotentials.TryGetValue(muscleName, out float value) ? value : 0f;
    }

}
