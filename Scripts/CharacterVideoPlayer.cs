using UnityEngine;
using UnityEngine.Video;

public class CharacterVideoPlayer : MonoBehaviour
{
    public GameObject imagedisplay;
    public VideoPlayer videoPlayer; 
    public RenderTexture videoRenderTexture; 
    public VideoClip videoForCharacter0; 
    public VideoClip videoForCharacter1; 
    public AudioManager audioManager;
    private LoadCharacter characterLoader;

    void Start()
    {
        characterLoader = FindObjectOfType<LoadCharacter>();
        if (videoPlayer == null || audioManager == null)
        {
            Debug.LogError("VideoPlayer or AudioManager not assigned!");
            return;
        }
        videoPlayer.targetTexture = videoRenderTexture;
        if (characterLoader != null)
        {
            GameObject player = characterLoader.GetInstantiatedPlayer();
            int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");

            if (selectedCharacter == 0)
            {
                PlayVideo(videoForCharacter0);
            }
            else if (selectedCharacter == 1)
            {
                PlayVideo(videoForCharacter1);
            }
        }
    }
    void PlayVideo(VideoClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("No video clip assigned for this character.");
            return;
        }

        videoPlayer.clip = clip;
        videoPlayer.loopPointReached += OnVideoEnd; 
        videoPlayer.Play();
        audioManager.musicSource.mute = true;
    }
    void OnVideoEnd(VideoPlayer vp)
    {
        audioManager.musicSource.mute = false;
        imagedisplay.SetActive(false);
        videoPlayer.loopPointReached -= OnVideoEnd;
    }
}