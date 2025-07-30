using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Concurrent;



public class AvatarMain : MonoBehaviour
{
    public GameObject toggle1;
    public GameObject toggle2;
    public GameObject toggle3;
    public GameObject toggle4;
    public GameObject toggle5;
    public GameObject toggle6;
    public GameObject toggle7;
    public GameObject toggle8;

    public static AvatarMain Instance { get; private set; }

    public float[] percetagesActivation = new float[8];
    private List<GameObject> muscleToggles = new List<GameObject>();
    public Transform panelMonitorizacion;//contenedor
    public GameObject gradientPrefab;//prefab del gradiente
    public Font customFont;

    public void Awake()
    {
        Instance = this;
    }
    private void Start()
    {

        muscleToggles.Add(toggle1);
        muscleToggles.Add(toggle2);
        muscleToggles.Add(toggle3);
        muscleToggles.Add(toggle4);
        muscleToggles.Add(toggle5);
        muscleToggles.Add(toggle6);
        muscleToggles.Add(toggle7);
        muscleToggles.Add(toggle8);

        Display.displays[0].Activate(); // Opcional, ya está activo
        if (Display.displays.Length > 1)
        {
            Display.displays[1].Activate(); // Activa Display 2
        }
        
        UpdateMuscleToggles();
    }
    public void Update()
    {
        //1. Pasar los valores de estimulacion

        //2. Animar los musculos con los gradientes de color
        Dictionary<string, float> stimulationData = getStimulationMusclesData();
        StimulateMuscles(stimulationData);
    }


    public void UpdateMuscleActivation(float[] channelValues)
    {
        GameObject[] musclesPrefabs = GameObject.FindGameObjectsWithTag("MuscleActivationPrefab");
        for (int i = 0; i < musclesPrefabs.Length; i++)
        {
            GameObject musclePrefab = musclesPrefabs[i];
            Text muscleName = musclePrefab.GetComponentInChildren<Text>();
            MuscleActivationProgress muscleActivation = musclePrefab.GetComponent<MuscleActivationProgress>();
            
            switch (muscleName.text){
                case "Biceps":
                    muscleActivation.SetActivationPercentage(channelValues[0]);
                    break;
                case "Anterior Deltoid":
                    muscleActivation.SetActivationPercentage(channelValues[1]);
                    break;
                case "Middle Deltoid":
                    muscleActivation.SetActivationPercentage(channelValues[2]);
                    break;
                case "Middle Trapezoid":
                    muscleActivation.SetActivationPercentage(channelValues[3]);
                    break;
                case "Posterior Deltoid":
                    muscleActivation.SetActivationPercentage(channelValues[4]);
                    break;
                case "Extensor calpi ulnaris":
                    muscleActivation.SetActivationPercentage(channelValues[5]);
                    break;
                case "Bracorradial":
                    muscleActivation.SetActivationPercentage(channelValues[6]);
                    break;
                case "Digitorium Extender":
                    muscleActivation.SetActivationPercentage(channelValues[7]);
                    break;
            }
        }
    }

    private void OnEnable()
    {
        //Solo actualiza si hay una instance de MuscleSelectionManager
        if (MuscleSelectionManager.Instance != null)
        {
            UpdateMuscleToggles();
        }
    }
    private void UpdateMuscleToggles()
    {
        List<string> selectedMuscles = MuscleSelectionManager.Instance.selectedMuscles;//consulta lista de músculos seleccionados
        
        
        for (int i = 0; i < muscleToggles.Count; i++){                                  // Recorremos cada toggle y lo mostramos solo si está en la lista de músculos seleccionados
            GameObject toggleObj = muscleToggles[i];
            if (toggleObj != null)                                              //si el objeto toggle no es nulo
            {
                
                Text toggleText = toggleObj.GetComponentInChildren<Text>();
                if (toggleText != null && selectedMuscles.Contains(toggleText.text))//si el texto del toggle no es nulo y el músculo seleccionado coincide con el texto del toggle
                {
                    
                    toggleObj.SetActive(true);                                      //activa el toggle
                    Toggle toggle = toggleObj.GetComponent<Toggle>();               //obtiene el componente que se marca y desmarca
                    toggle.isOn = false;                                            //desmarca el toggle                                                            
                    toggle.onValueChanged.RemoveAllListeners();// Limpiar listener anterior para evitar duplicados
                    
                    //crea el handler de potencial muscular
                    createMusclePotentialHandler(toggleText.text); //crea el handler de potencial muscular
                }
                else
                {
                    
                    toggleObj.SetActive(false);
                }
            }
        }
    }
    private void createMusclePotentialHandler(string muscle){
        // Aquí se puede implementar la lógica para crear el handler de potencial muscular
    
        if (gradientPrefab == null || panelMonitorizacion == null)
        {
            Debug.LogError("No se ha asignado el prefab del gradiente o el panel de monitorización.");
            return;
        }
        int childCount = panelMonitorizacion.childCount;
        GameObject muscleHandler = Instantiate(gradientPrefab, panelMonitorizacion);//instancio el prefab del gradiente en el panel de monitorizacion
        if (muscleHandler == null)
        {
            Debug.LogError("No se ha podido instanciar el prefab del gradiente.");
            return;
        }
        else
        {
            RectTransform handlerTransform = muscleHandler.GetComponent<RectTransform>();
            if (handlerTransform == null)
            {
                Debug.LogError("No se ha encontrado el componente RectTransform en el prefab del gradiente.");
                return;
            }
            //Asigno la etiqueta identificativa
            muscleHandler.tag = "MuscleActivationPrefab";
            //Calculas la posicion basada en cuantos hay
            int index = childCount;

            float startX = -295f; // Posición inicial en X
            float secondX = -100f;
            float startY = 170; // Posición inicial en Y
            float yStep = -105f; // Espacio entre elementos (ajústalo según tu diseño)

            //Calculamos las coordenadas de la posición
            float posX = (index % 2 == 0) ? startX : secondX; // Alterna entre dos posiciones en X
            float posY = startY + yStep * (index / 2); // Calcula la posición en Y según el índice
            //Asignas la pos calculada al handler
            handlerTransform.anchoredPosition = new Vector2(posX, posY); // Asigna la posición calculada

            //ajusto la posicion del gradiente
            GameObject labelObject = new GameObject("Label");               //creo un objeto de texto
            labelObject.transform.SetParent(muscleHandler.transform);       //lo asigno como hijo del objeto de gradiente
            Text label = labelObject.AddComponent<Text>();
            label.font = customFont;
            label.color = Color.black;
            label.text = muscle;
            label.fontSize = 13; // Tamaño de fuente
            label.fontStyle = FontStyle.Bold;
            label.alignment = TextAnchor.MiddleLeft;

            RectTransform labelTransform = label.GetComponent<RectTransform>();
            labelTransform.sizeDelta = new Vector2(120, 15);

            // Anclar al centro inferior del handler
            labelTransform.anchorMin = new Vector2(0, 1);
            labelTransform.anchorMax = new Vector2(0, 1);
            labelTransform.pivot = new Vector2(0.5f, 0);
            labelTransform.localPosition = new Vector3(0, 30, 0); // Ajusta la posición local para que esté centrado en el handler
        /*
                // Colocar el texto 
                if (index == 0) // Si es el primer índice
                {
                    labelTransform.localPosition = new Vector3(0, 30, 0);
                }
                else if (index == 1) // Si es el segundo índice
                {
                    labelTransform.localPosition = new Vector3(0, 10, 0);
                }
                else if (index == 2) // Si es el tercer índice
                {
                    labelTransform.anchoredPosition = new Vector2(15, -30);
                }
                else if (index == 3) // Si es el cuarto índice
                {
                    labelTransform.anchoredPosition = new Vector2(-10, -30);
                }
                else if (index == 4) // Si es el quinto índice
                {
                    labelTransform.anchoredPosition = new Vector2(15, -12);
                }
                else if (index == 5) // Si es el sexto índice
                {
                    labelTransform.anchoredPosition = new Vector2(-10,-12);
                }
                else if (index == 6) // Si es el séptimo índice
                {
                    labelTransform.anchoredPosition = new Vector2(15, 5);
                }
                else if (index == 7) // Si es el octavo índice
                {
                    labelTransform.anchoredPosition = new Vector2(15,5);
                }
                */
        }
    }
    
    
    private Dictionary<string, float> getStimulationMusclesData()
    {
        // Simulación de datos de estimulación muscular || deberia hacer que el diccionario fuera una variable global?
        Dictionary<string, float> stimulationData = new Dictionary<string, float>{};
        GameObject[] musclesPrefabs = GameObject.FindGameObjectsWithTag("MuscleActivationPrefab");//cuando se crean el prefab se le asigna esta etiqueta
        if (musclesPrefabs.Length == 0)
        {
            Debug.LogWarning("No se encontraron prefabs de activación muscular.");
            return null;
        }
        foreach (GameObject musclePrefab in musclesPrefabs)
        {
            MuscleActivationProgress muscleActivation = musclePrefab.GetComponent<MuscleActivationProgress>();
            if (muscleActivation != null)
            {
                float activationPercentage = muscleActivation.GetActivationPercentage();    // Si existe el componente, obtengo el porcentaje de activación
                Text muscleName = musclePrefab.GetComponentInChildren<Text>();
                if (muscleName != null)
                {
                    string muscleKey = muscleName.text;
                    if (stimulationData.ContainsKey(muscleKey))
                    {
                        stimulationData[muscleKey] = activationPercentage; // Sustituye el porcentaje existente
                    }
                    else
                    {
                        stimulationData.Add(muscleKey, activationPercentage); // Crea una nueva clave con el porcentaje
                    }
                }
            }
        }
        return stimulationData;
    }

    private string getColorFromPercentage(float percentage)
    {
        float normalizedPercentage = Mathf.Clamp(percentage, 0f, 1f);
        Color color = Color.Lerp(Color.green, Color.red, normalizedPercentage);
        return UnityEngine.ColorUtility.ToHtmlStringRGB(color);
    }
    private void StimulateMuscles(Dictionary<string, float> stimulationData)
    {
        Debug.Log("Estimulación muscular: " + string.Join(", ", stimulationData));
        if (stimulationData == null || stimulationData.Count == 0)
        {
            Debug.LogWarning("No hay datos de estimulación muscular.");
            return;
        }
        foreach (var muscle in stimulationData)
        {
            string muscleName = muscle.Key;
            float activationPercentage = muscle.Value;

            GameObject[] muscles = GameObject.FindGameObjectsWithTag(muscleName);
            if (muscles.Length > 0)
            {
                string hexColor = "#" + getColorFromPercentage(activationPercentage); // "#RRGGBB"

                if (UnityEngine.ColorUtility.TryParseHtmlString(hexColor, out Color color))
                {
                    foreach (GameObject muscleObj in muscles)//fibras del biceps
                    {
                        MeshRenderer renderer = muscleObj.GetComponent<MeshRenderer>();
                        if (renderer != null)
                        {
                            // Cambia directamente el color base del material original
                            if (renderer.material.HasProperty("_BaseColor")) // URP
                            {
                                renderer.material.SetColor("_BaseColor", color);
                            }
                            else if (renderer.material.HasProperty("_Color")) // Shader estándar
                            {
                                renderer.material.color = color;
                            }
                            else
                            {
                                Debug.LogWarning($"Material del músculo {muscleName} no tiene propiedad _BaseColor ni _Color.");
                            }
                        }
                        else
                        {
                            Debug.LogWarning($"El músculo {muscleName} no tiene Renderer.");
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"Color no válido: {hexColor} para el músculo {muscleName}");
                }
            }
            else
            {
                Debug.LogWarning($"No se encontró el músculo {muscleName}");
            }
        }
    }
}
