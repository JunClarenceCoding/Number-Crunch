using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundManager : MonoBehaviour
{
    [SerializeField] private List<Button> buttonSet1;  // First set of buttons
    [SerializeField] private List<Button> buttonSet2;  // Second set of buttons
    private AudioManager audioManager;                 // Reference to the AudioManager

    private void Start()
    {
        // Find and assign the AudioManager in the scene
        audioManager = FindObjectOfType<AudioManager>();

        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene.");
            return;
        }

        // Add listeners to the first set of buttons to call buttonClick
        foreach (Button button in buttonSet1)
        {
            if (button != null)
            {
                button.onClick.AddListener(audioManager.clickButton);
            }
            else
            {
                Debug.LogWarning("A button in buttonSet1 is null. Please check the ButtonManager script.");
            }
        }

        // Add listeners to the second set of buttons to call buttonClick1
        foreach (Button button in buttonSet2)
        {
            if (button != null)
            {
                button.onClick.AddListener(audioManager.clickButtonNumbers);
            }
            else
            {
                Debug.LogWarning("A button in buttonSet2 is null. Please check the ButtonManager script.");
            }
        }
    }
}

