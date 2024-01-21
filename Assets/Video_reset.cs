using UnityEngine;
using UnityEngine.Video;

public class Video_reset : MonoBehaviour
{
    public GameObject ReflectionProbe;
    public VideoPlayer videoPlayer; // Die VideoPlayer-Komponente

    private void Start()
    {
        // Deaktiviere VideoPlayer am Anfang
        videoPlayer.Stop();
        videoPlayer.enabled = false;
    }

    private void Update()
    {
        if (ReflectionProbe != null && !ReflectionProbe.activeSelf)
        {
            // Aktiviere den VideoPlayer
            videoPlayer.enabled = true;

            if (!videoPlayer.isPlaying)
            {
                // Starte das Video
                videoPlayer.Play();
            }
        }
        else
        {
            // Deaktiviere VideoPlayer wenn Zielobjekt nicht aktiv ist
            videoPlayer.Stop();
            videoPlayer.enabled = false;
        }
    }
}