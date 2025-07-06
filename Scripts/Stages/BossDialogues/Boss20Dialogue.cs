using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Database;

public class Boss20Dialogue : MonoBehaviour
{
    public AudioManager audioManager;
    private FirebaseManager firebaseManager;
    public Image playerDisplayImage; 
    public Image npcDisplayImage;  
    public Sprite boyImage; 
    public Sprite girlImage; 
    public GameObject NPCNamePanel;
    public GameObject PlayerNamePanel;
    public GameObject ConvoPanel;
    public TMP_Text NPCName, PlayerName;
    private BattleTrialCharacterLoader characterLoader;

    [Header("Start Dialogue")]
    public TMP_Text startDialogue;
    public Button Continue1;

    [Header("Continue1")]
    public TMP_Text Dialogue1;
    public Button Continue2;

    [Header("Continue2")]
    public TMP_Text Dialogue2;
    public Button Continue3;

    [Header("Continue3")]
    public TMP_Text Dialogue3;
    public Button Continue4;

    [Header("Continue4")]
    public TMP_Text Dialogue4;
    public Button Continue5;

    [Header("Continue5")]
    public TMP_Text Dialogue5;
    public Button Continue6;

    [Header("Continue6")]
    public TMP_Text Dialogue6;
    public Button Continue7;

    [Header("Continue7")]
    public TMP_Text Dialogue7;
    public Button Continue8;

    [Header("Continue8")]
    public TMP_Text Dialogue8;
    public Button Continue9;

    [Header("Continue9")]
    public TMP_Text Dialogue9;
    public Button Continue10;

    [Header("Continue10")]
    public TMP_Text Dialogue10;
    public Button Continue11;

    void Start()
    {
        
        characterLoader = FindObjectOfType<BattleTrialCharacterLoader>();
        firebaseManager = FindObjectOfType<FirebaseManager>();
        if(characterLoader != null)
        {
            UpdatePlayerImage();
        }

        DisplayPlayerName();
    }

    private void UpdatePlayerImage()
    {
        if (characterLoader != null)
        {
            GameObject player = characterLoader.GetInstantiatedPlayer();

            int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");

            if (selectedCharacter == 0)
            {
                playerDisplayImage.sprite = boyImage; 
            }
            else if (selectedCharacter == 1)
            {
                playerDisplayImage.sprite = girlImage; 
            }
        }
    }

    private void ShowCharacter(bool isPlayerSpeaking)
    {
        playerDisplayImage.gameObject.SetActive(isPlayerSpeaking);
        npcDisplayImage.gameObject.SetActive(!isPlayerSpeaking);
    }

    private void SetDialogueState(bool npcActive, bool playerActive, TMP_Text activeDialogue, Button[] activeButtons, string dialogueText)
    {
        ConvoPanel.SetActive(true);

        NPCNamePanel.SetActive(npcActive);
        PlayerNamePanel.SetActive(playerActive);

        NPCName.gameObject.SetActive(npcActive);
        PlayerName.gameObject.SetActive(playerActive);

        TMP_Text[] dialogues = { startDialogue, Dialogue1, Dialogue2, Dialogue3, Dialogue4, Dialogue5, Dialogue6, Dialogue7, Dialogue8, Dialogue9, Dialogue10};

        Button[] allButtons = { Continue1, Continue2, Continue3, Continue4, Continue5, Continue6, Continue7, Continue8, Continue9, Continue10, Continue11};

        foreach (TMP_Text dialogue in dialogues)
        {
            dialogue.gameObject.SetActive(false);
        }

        foreach (Button button in allButtons)
        {
            button.gameObject.SetActive(false);
        }

        activeDialogue.gameObject.SetActive(true);

        // Start typewriter effect and enable buttons after it's done
        activeDialogue.GetComponent<TypeWriterEffect>().StartTypewriterEffect(dialogueText, () =>
        {
            foreach (Button button in activeButtons)
            {
                button.gameObject.SetActive(true); // Enable buttons after typing is complete
            }
        });
    }

    public void ConvoStart()
    {
        DisplayPlayerName();
        SetDialogueState(false, true, startDialogue, new Button[] { Continue1 }, "Why does it feel like… I'm back in my own world but not at the same time. Is this the power that Ember has warned me about?");
        ShowCharacter(true);
    }

    public void StartDialogue1()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[0]);
        SetDialogueState(true, false, Dialogue1, new Button[] { Continue2 }, "That's no fun. Sounds like someone has spoiled the fun for me.");
        ShowCharacter(false);
    }

    public void StartDialogue2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue2, new Button[] { Continue3 }, "Who are you?");
        ShowCharacter(true);
    }

    public void StartDialogue3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[1]);
        SetDialogueState(true, false, Dialogue3, new Button[] { Continue4 }, "Hehehe want to take a guess?");
        ShowCharacter(false);
    }

    public void StartDialogue4()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue4, new Button[] {Continue5}, "You must be Kael.");
        ShowCharacter(true);
    }

    public void StartDialogue5()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[2]);
        SetDialogueState(true, false, Dialogue5, new Button[] { Continue6 }, "Well well well! It seems like my popularity has increased, even a child knows of me now. Go ahead child, cower in fear.");
        ShowCharacter(false);
    }

    public void StartDialogue6()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue6, new Button[] {Continue7}, "Aren't you a child as well? You're a baby even.");
        ShowCharacter(true);
    }

    public void StartDialogue7()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[3]);
        SetDialogueState(true, false, Dialogue7, new Button[] { Continue8 }, "Hmph, you mortals and your nonsense! I may be in the form of an infant but I'm sure I have lived way longer than you.");
        ShowCharacter(false);
    }

    public void StartDialogue8()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue8, new Button[] {Continue9}, "Then, am I supposed to call you Grandpa Kael then?");
        ShowCharacter(true);
    }

    public void StartDialogue9()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue9, new Button[] {Continue10}, "Grandpa Kael, could you please let me go through the portal? I want to go back home.");
    }

    public void StartDialogue10()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[4]);
        SetDialogueState(true, false, Dialogue10, new Button[] { Continue11 }, "Silence! I am not your grandpa! You are really annoying. As your punishment, you can't go near the portal and I'll make you play with me until I'm satisfied!");
        ShowCharacter(false);
    }

    public void ConvoEnd()
    {
        audioManager.StopCurrentSFX();
        ConvoPanel.SetActive(false);

        TMP_Text[] dialogues = { startDialogue, Dialogue1, Dialogue2, Dialogue3, Dialogue4, Dialogue5, Dialogue6, Dialogue7, Dialogue8, Dialogue9, Dialogue10};

        Button[] allButtons = { Continue1, Continue2, Continue3, Continue4, Continue5, Continue6, Continue7, Continue8, Continue9, Continue10, Continue11};

        foreach (TMP_Text dialogue in dialogues)
        {
            dialogue.gameObject.SetActive(false);
        }

        foreach (Button button in allButtons)
        {
            button.gameObject.SetActive(false);
        }   
    }

    private void DisplayPlayerName()
    {
        string userId = firebaseManager.GetUserId(); 
        DatabaseReference usernamesRef = firebaseManager.GetDatabaseReference("usernames");

        usernamesRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                bool userFound = false;

                foreach (DataSnapshot childSnapshot in snapshot.Children)
                {
                    string username = childSnapshot.Key; // Get the username
                    string storedUserId = childSnapshot.Value.ToString(); // Get the userID

                    if (storedUserId == userId)
                    {
                        PlayerName.text = username; // Set the matching username to the TMP_Text field
                        userFound = true;
                        break;
                    }
                }

                if (!userFound)
                {
                    Debug.LogWarning("User ID not found in the 'usernames' node.");
                    PlayerName.text = "Player"; 
                }
            }
            else
            {
                Debug.Log("Failed to retrieve usernames: " + task.Exception);
                PlayerName.text = "Error"; // Optional fallback
            }
        });
    }
}
