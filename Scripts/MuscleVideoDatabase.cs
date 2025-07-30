using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "MuscleVideoDatabase", menuName = "Custom/Muscle Video Database")]
public class MuscleVideoDatabase : ScriptableObject
{
    public List<MuscleVideo> muscleVideos;

    [System.Serializable]
    public class MuscleVideo
    {
        public string muscleName;
        public VideoClip videoClip; // Asigna videos aquí en el inspector
    }
    void Start()
    {
        Debug.Log("Muscle Video Database. Instance inicializado correctamente.");
    }

    public VideoClip GetVideoForMuscle(string muscleName)
    {
        foreach (var muscleVideo in muscleVideos)
        {
            if (muscleVideo.muscleName == muscleName)
            {
                return muscleVideo.videoClip;
            }
        }
        return null;
    }
}
