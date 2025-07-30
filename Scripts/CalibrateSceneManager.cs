using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CalibrateSceneManager : MonoBehaviour
{
    public GameObject popupPrefab; // Prefab del popup
    public Transform canvasTransform; // Parent donde se instanciará el popup
    private void Start(){
        Debug.Log("Calibrate Scene Manager. Instance inicializado correctamente.");
        
    }
    public void LoadCalibrateScene()
    {
        //se comprueba si hay musculos seleccionados
        if (MuscleSelectionManager.Instance != null && MuscleSelectionManager.Instance.selectedMuscles != null){

            List<string> selectedMuscles = MuscleSelectionManager.Instance.selectedMuscles;
            Debug.Log($"Músculos seleccionados: {string.Join(", ", selectedMuscles)}");

            bool hasMuscles = selectedMuscles.Count > 0;
            
            bool hasAvatar = !string.IsNullOrEmpty(AvatarManager.SelectedAvatar);
            
            if (hasMuscles && hasAvatar) {
                
                SceneManager.LoadScene("CalibrateScene");
            }
            else if (!hasMuscles && !hasAvatar) {
                ShowPopup("No muscles and no avatar have been selected. Please select both.");
                return;
            }
            else if (!hasMuscles) {
                ShowPopup("No muscles have been selected. Please select a muscle.");
                return;
            }
            else if (!hasAvatar) {
                ShowPopup("No avatar has been selected. Please choose an avatar.");
                return;
            }
        }
        else{
            ShowPopup("La instancia de la escena o la lista de músculos no están inicializados.");
            
        }
    }

    public void LoadStimulationScene()
    {
         
        SceneManager.LoadScene("StimulationScene");
    }

    public void ShowPopup(string message)
    {
        GameObject popup = Instantiate(popupPrefab, canvasTransform);
        popup.GetComponent<PopupWindow>().SetMessage(message); // Solo si tu script tiene ese método
    }

}
