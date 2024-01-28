using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VidPlayerScript : MonoBehaviour
{
    public string videoFileName;
    private bool isPlaybackEnded;
    private VideoPlayer videoPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += (_) => isPlaybackEnded = true;
        PlayVideo();
    }


    public void PlayVideo()
    {
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
        Debug.Log(videoPath);
        videoPlayer.url = videoPath;
        videoPlayer.Play();
    }

    public void Update()
    {
        if (isPlaybackEnded && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("VisualNovelScene");
        }
    }
}
