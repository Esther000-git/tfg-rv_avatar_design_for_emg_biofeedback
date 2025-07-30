using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena
using UnityEngine.UI; // Necesario para trabajar con UI

public class AvatarSelection : MonoBehaviour
{
    public static AvatarSelection Instance;
    // Métodos para seleccionar el avatar
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
           
        }
        else
        {
            Destroy(gameObject); // Destruir duplicados
        }
    }
    
    public void SelectWoman()
    {
        AvatarManager.SelectedAvatar = "woman";
         
        
    }

    public void SelectGirl()
    {
        AvatarManager.SelectedAvatar = "girl";
         
       
    }

    public void SelectBoy()
    {
        AvatarManager.SelectedAvatar = "boy";
         
       
    }

    public void SelectMan()
    {
        AvatarManager.SelectedAvatar = "man";
         
        
    }

    public void LoadAvatarScene()
    {
        // Avatar Scene es el nombre que llevara la scena del avatar
         
        SceneManager.LoadScene("AvatarScene");
        //si tienen que pasar mas cosas al cambiar de escena se programa aqui
        
    }
}