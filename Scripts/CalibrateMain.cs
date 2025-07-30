using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CalibrateMain : MonoBehaviour
{
    public List<GameObject> toggles = new List<GameObject>(); // Lista de toggles para cada músculo
    public List<string> calibrateMusclesList = new List<string>(); //Lista de nombre de músculos seleccionados para calibrar
    public static CalibrateMain Instance { get; private set; } // Instancia única de CalibrateMain

    private void Awake()
    {

        if (Instance != null && Instance != this) // Si ya existe una instancia, destrúyela
        {
            Debug.LogWarning("Multiple instances of CalibrateMain found. Destroying this instance.");
            Destroy(Instance);
            return;
        }
        Instance = this; // Asigna la instancia actual
        DontDestroyOnLoad(this.gameObject); // No destruir al cargar nuevas escenas
    }

    private void Start()
    {

        // Verifica si algún toggle está nulo al momento de añadirlos
        for (int i = 0; i < toggles.Count; i++)
        {
            if (toggles[i] == null)
            {
                Debug.LogError("El toggle " + (i + 1) + " es nulo. Asegúrate de asignar todos los toggles correctamente en el Inspector.");
            }
        }
        // Añade los toggles a la lista de toggles


        if (MuscleSelectionManager.Instance != null)        //si existe la instancia de MuscleSelectionManager pide llenar los toggles con los musculos seleccionados
        {
            PopulateMuscleToggles();
        }
        else
        {
            Debug.LogError("MuscleSelectionManager.Instance no encontrado.");
        }
    }
    //Cuando este objeto se activa en la escena vuelva a cargar los toggles.
    // No pasa nada por tener despues el Update, OnEnable se ejecuta cada vez que el GameObject se activa no solo al arrancar la escena.
    // Es util iporque se garantiza que si algo desactiva reactica EL PANEL DE TOGGLES los toggles se reconfiguran
    // No afecta negativamente a muscleForDisplayer porque tu Update es independiente y trabaja en tiempo real con lo que el usuario selecciona
    // solo de la seleccion actual no interfiere. 
    private void OnEnable()
    {
        PopulateMuscleToggles();
    }
    private void PopulateMuscleToggles()// Método para llenar los toggles con los músculos seleccionados
    {
        List<string> selectedMuscles = MuscleSelectionManager.Instance.selectedMuscles;

        // Recorre la lista de toggles de muscle y los activa o desactiva según la selección de músculos
        foreach (GameObject toggle in toggles)
        {

            if (toggle != null)
            {
                if (selectedMuscles.Contains(toggle.GetComponentInChildren<Text>().text))
                {
                    toggle.SetActive(true);
                    toggle.GetComponent<Toggle>().isOn = false;

                }
                else
                {
                    toggle.SetActive(false);
                }

            }
        }
    }
    private void Update()
    {
        UpdateMusclesForDisplayer(); // Actualiza la lista de músculos para el VideoDisplayerManager
    }
    private void UpdateMusclesForDisplayer()
    {
        calibrateMusclesList.Clear(); // Limpiamos la lista antes de llenarla
        foreach (GameObject toggle in toggles)
        {
            if (toggle == null) // Comprobamos si el toggle es nulo
            {
                Debug.LogError("Toggle es nulo-nulo. Asegúrate de que todos los toggles están asignados correctamente.");
                continue; // Saltamos a la siguiente iteración si el toggle es nulo
            }
            if (toggle.activeSelf) // Solo añadimos los toggles activos y seleccionados
            {
                Toggle toggleComponent = toggle.GetComponent<Toggle>();
                if (toggleComponent == null) // Comprobamos si el componente Toggle es nulo
                {
                    Debug.LogError("No se encontró el componente Toggle en el objeto: " + toggle.name);
                    continue; // Saltamos a la siguiente iteración si el componente Toggle es nulo
                }
                if (toggleComponent.isOn)
                {
                    string muscleName = toggle.GetComponentInChildren<Text>().text;
                    if (muscleName != null)
                    {
                        calibrateMusclesList.Add(muscleName);
                        Debug.Log("Músculo añadido a la lista de calibración: " + muscleName);
                    }
                }

            }
        }
        Debug.Log("Músculos para VideoDisplayerManager: " + string.Join(", ", calibrateMusclesList));
    }
    public List<string> GetCalibrateMusclesList()
    {
        return calibrateMusclesList;
    }
}
/*
selectedMuscles -> Muscles selected by the user in the MuscleSelectionManager
calibrate
*/
