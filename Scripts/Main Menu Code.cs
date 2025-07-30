using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuCode : MonoBehaviour
{
    public List<Toggle> muscleToggles; // Lista de toggles para cada músculo
    private bool hasPreviousSelection = false; // Bandera para saber si ya hubo selección
    private List<string> lastSelectedMuscles = new List<string>(); // Última selección de músculos
    public GameObject popupPrefab; // Prefab del popup
    public Transform canvasTransform; // Parent donde se instanciará el popup

    void Start()
    {

        if (MuscleSelectionManager.Instance == null || MuscleSelectionManager.Instance.selectedMuscles == null)
        {
            Debug.LogWarning("MainMenuCode: MuscleSelectionManager no está inicializado o no tiene músculos seleccionados.");
            return;
        }

        List<string> selectedMuscles = MuscleSelectionManager.Instance.selectedMuscles;

        // Verificar si ya hay músculos seleccionados previamente
        hasPreviousSelection = selectedMuscles.Count > 0;

        if (!hasPreviousSelection)
        {
            return; // No marcamos ningún toggle
        }

        // Si hay selección previa, activamos los toggles correspondientes
        foreach (Toggle toggle in muscleToggles)
        {
            Text muscleLabel = toggle.GetComponentInChildren<Text>();

            if (muscleLabel == null)
            {
                Debug.LogError("MainMenuCode: No se encontró un componente Text en el Toggle.");
                continue;
            }

            string muscleName = muscleLabel.text.Trim(); // Usamos el nombre real del músculo
            // Marcar el toggle si el músculo ya había sido seleccionado antes
            toggle.isOn = selectedMuscles.Contains(muscleName);
            
        }
 
    }

    public void Update()//DETECTA LA SELECCION Y ACTUALIZA EN TIEMPO REAL
    {
        if (HasSelectionChanged()) // Si hay un cambio en la selección de toggles
        {
             
            UpdateMuscleSelection(); // Actualizar lista de músculos seleccionados
        }
    }

    private bool HasSelectionChanged()
    {
        List<string> currentSelection = GetCurrentToggleSelection();
        return !AreListsEqual(lastSelectedMuscles, currentSelection);
    }

    private void UpdateMuscleSelection()
    {
        List<string> currentSelection = GetCurrentToggleSelection();
        MuscleSelectionManager.Instance.selectedMuscles = new List<string>(currentSelection);
        lastSelectedMuscles = new List<string>(currentSelection); // Actualizamos la selección previa
    }

    private List<string> GetCurrentToggleSelection()
    {
        List<string> selectedMuscles = new List<string>();
        foreach (Toggle toggle in muscleToggles)
        {
            if (toggle.isOn)
            {
                Text muscleLabel = toggle.GetComponentInChildren<Text>();
                if (muscleLabel != null)
                {
                    selectedMuscles.Add(muscleLabel.text.Trim());
                }
            }
        }
        return selectedMuscles;
    }

    private bool AreListsEqual(List<string> listA, List<string> listB)
    {
        if (listA.Count != listB.Count) return false;
        for (int i = 0; i < listA.Count; i++)
        {
            if (listA[i] != listB[i]) return false;
        }
        return true;
    }
    public void checkConfigurations()
    {
        if (string.IsNullOrEmpty(AvatarManager.SelectedAvatar) && MuscleSelectionManager.Instance.selectedMuscles.Count == 0)
        {
            ShowPopup("No avatar has been selected, and no muscles have been configured. Please select an avatar and the muscles you want to stimulate.");
            return;
        }
        else if (string.IsNullOrEmpty(AvatarManager.SelectedAvatar))
        {
            ShowPopup("No avatar has been selected. Please select an avatar.");
            return;
        }
        else if (MuscleSelectionManager.Instance.selectedMuscles.Count == 0)
        {
            ShowPopup("No muscles have been selected. Please select a muscle.");
            return;
        }
    }

    public void OnPlayPressed()
    {
        checkConfigurations();
        if (string.IsNullOrEmpty(AvatarManager.SelectedAvatar) || MuscleSelectionManager.Instance.selectedMuscles.Count == 0)
        {
            return; // No hacemos nada si no hay avatar o músculos seleccionados
        } 
        AvatarSelection.Instance.LoadAvatarScene();

    }

    public void ShowPopup(string message)
    {
        GameObject popup = Instantiate(popupPrefab, canvasTransform);
        popup.GetComponent<PopupWindow>().SetMessage(message); // Solo si tu script tiene ese método
    }
}
