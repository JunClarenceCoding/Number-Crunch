using UnityEngine;

public class AudioTest : MonoBehaviour
{
    public AudioSource audioSource;

    void Update()
    {
        // Play the audio when space key is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioSource.Play();
        }
    }
}