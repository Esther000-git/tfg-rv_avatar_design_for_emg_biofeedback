using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarSceneManager : MonoBehaviour
{
    // Referencias a los objetos que representarán los avatares
    public GameObject womanAvatar;
    public GameObject girlAvatar;
    public GameObject boyAvatar;
    public GameObject manAvatar;
    


    private void Start()
    {
        
        ShowSelectedAvatar();//call the selectioned avatar
        
        
    }

    public void ShowSelectedAvatar()
    {
        // Ocultar todos 
        womanAvatar.SetActive(false);
        girlAvatar.SetActive(false);
        boyAvatar.SetActive(false);
        manAvatar.SetActive(false);
    //activa el avatar que se le haya indicado por la variable

        switch (AvatarManager.SelectedAvatar){
            case "woman":
                womanAvatar.SetActive(true);
                break;
            case "girl":
                girlAvatar.SetActive(true);
                break;
            case "boy":
                boyAvatar.SetActive(true);
                break;
            case "man":
                manAvatar.SetActive(true);
                break;
            default:
                Debug.LogWarning("No hay avatar seleccionado.");
                break;
        }
    }
}


