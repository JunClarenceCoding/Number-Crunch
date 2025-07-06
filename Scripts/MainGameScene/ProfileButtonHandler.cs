using UnityEngine;
using UnityEngine.UI;

public class ProfileButtonHandler : MonoBehaviour
{
    public Image profileImage; // Reference to the Image component in the UI
    public Sprite boyProfileSprite; // Sprite for the boy character
    public Sprite girlProfileSprite; // Sprite for the girl character

    private MainCharacterLoader mainCharacterLoader;

    void Start()
    {
        // Find the MainCharacterLoader script in the scene to access the instantiated player info
        mainCharacterLoader = FindObjectOfType<MainCharacterLoader>();

        // Update the profile button based on the instantiated character
        UpdateProfileImage();
    }

    void UpdateProfileImage()
    {
        if (mainCharacterLoader != null)
        {
            GameObject player = mainCharacterLoader.GetInstantiatedPlayer();

            // Check which character is instantiated by accessing the selectedCharacter index from PlayerPrefs
            int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");

            // If the first character (boy) is selected, display the boy's profile sprite
            if (selectedCharacter == 0)
            {
                profileImage.sprite = boyProfileSprite;
            }
            // If the second character (girl) is selected, display the girl's profile sprite
            else if (selectedCharacter == 1)
            {
                profileImage.sprite = girlProfileSprite;
            }
            else
            {
                Debug.LogError("Invalid character selection.");
            }
        }
    }
}
