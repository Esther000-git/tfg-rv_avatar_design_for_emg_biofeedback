using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitGame : MonoBehaviour{
    private List<string> selectedMuscles;
    private Toggle[] toggles;

    private void Start()
    {
        if (MuscleSelectionManager.Instance != null)
        {
            selectedMuscles = MuscleSelectionManager.Instance.selectedMuscles;
            toggles = MuscleSelectionManager.Instance.toggles;
        }
        else
        {
            Debug.LogError("MuscleSelectionManager.Instance es NULL. Asegúrate de que está en la escena.");
        }
    }

        
    public void QuitGame()//Método para salir del juego
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//si estamos en el editor, detiene la ejecucion
        #else
            //si estamos en una construcción del juego sale de la aplicacion
            Application.Quit();
        #endif
        
    }
    public void LoadMainMenu(){
      //  MuscleSelectionManager.Instance.SaveMuscleSelection();  // Guarda la selección actual
        SceneManager.LoadScene("MainMenu");
        
    }
    public void LoadCalibrate(){
       // MuscleSelectionManager.Instance.SaveMuscleSelection(); // Guarda la selección actual
        SceneManager.LoadScene("CalibrateScene");
    }


  
}
