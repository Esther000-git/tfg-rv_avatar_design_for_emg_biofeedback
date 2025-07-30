using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;

public class VideoController : MonoBehaviour
{
    public VideoPlayer mainVideoPlayer;
    public Button[] thumbnailButtons;
    public GameObject popupPrefab;
    public Transform canvasTransform;
    
    private Dictionary<string, string> muscleToVideo = new Dictionary<string, string>();
    
    private bool isPlaying = false;
    private bool isPaused = false;
    private Button buttonInPlayback = null;

    void Start()
    {
        // Diccionario músculo -> vídeo
        muscleToVideo["Biceps"] = "biceps.mp4";
        muscleToVideo["Anterior Deltoid"] = "deltoides_anterior.mp4";
        muscleToVideo["Middle Deltoid"] = "deltoides_medio.mp4";
        muscleToVideo["Posterior Deltoid"] = "deltoides_posterior.mp4";
        muscleToVideo["Middle Trapezoid"] = "trapecio_medio.mp4";
        muscleToVideo["Extensor calpi ulnaris"] = "extensor_carpo.mp4";
        muscleToVideo["Bracorradial"] = "braquiorradial.mp4";
        muscleToVideo["Digitorium Extensor"] = "extensor_digitorium.mp4";

        
        foreach (Button btn in thumbnailButtons)
        {
            string muscleTag = btn.tag;
            Debug.Log($"Activando botón para: {muscleTag}");
            btn.gameObject.SetActive(true);
            string tag = muscleTag; // Capturamos el tag en una variable local para el listener
            btn.onClick.AddListener(() => TryPlayVideo(tag));
        }
    }

    void TryPlayVideo(string muscle)
    {
        if (muscleToVideo.ContainsKey(muscle))
        {
            string fullPath = System.IO.Path.Combine(Application.streamingAssetsPath, "videos", muscleToVideo[muscle]);
            mainVideoPlayer.url = fullPath;
            //mainVideoPlayer.audioOutputMode = VideoAudioOutputMode.None;
            Debug.Log($"Reproduciendo vídeo para: {muscle}");

            // Importante: desvincula eventos anteriores para evitar múltiples callbacks
            mainVideoPlayer.prepareCompleted -= OnVideoPrepared;
            mainVideoPlayer.prepareCompleted += OnVideoPrepared;

            // Preparar antes de reproducir
            mainVideoPlayer.Prepare();
        }
        else
        {
            Debug.Log($"Músculo no permitido o sin vídeo: {muscle}");
        }

        void OnVideoPrepared(VideoPlayer vp)
        {
            vp.Play();
            Debug.Log("Vídeo preparado y reproduciéndose.");
        }
    }

    public void LoadAvatarScene()
    {
        ShowPopup("A continuación podrás ver el avatar en 3D");
        StartCoroutine(PauseBeforeSceneLoad());

        IEnumerator PauseBeforeSceneLoad()
        {
            yield return new WaitForSeconds(3f);
        }
        SceneManager.LoadScene("AvatarScene");
    }

    public void OnVideoEnd(VideoPlayer vp)
    {
        isPlaying = false;

        if (buttonInPlayback != null)
        {
            ColorBlock cb = buttonInPlayback.colors;
            cb.normalColor = Color.white;
            buttonInPlayback.colors = cb;
            buttonInPlayback = null;
        }
    }

    public void ReplayCurrentVideo()
    {
        if (!isPlaying && !string.IsNullOrEmpty(mainVideoPlayer.url))
        {
            mainVideoPlayer.Play();
            isPlaying = true;
        }
    }

    public void PauseResume()
    {
        if (isPaused)
        {
            mainVideoPlayer.Play();
            isPaused = false;
            isPlaying = true;
        }
    }

    public void PauseVideo()
    {
        mainVideoPlayer.Stop();
        isPlaying = false;
        isPaused = false;
    }

    public void QuitVideo()
    {
        mainVideoPlayer.Stop();
        isPlaying = false;
        isPaused = false;
        ShowPopup("¿Estás seguro de que quieres finalizar el vídeo?");
    }

    private void ShowPopup(string message)
    {
        GameObject popup = Instantiate(popupPrefab, canvasTransform);
        popup.GetComponent<PopupWindow>().SetMessage(message);
    }
}
