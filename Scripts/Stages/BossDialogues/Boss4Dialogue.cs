using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Database;

public class Boss4Dialogue : MonoBehaviour
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

    [Header("StartVictoryDialogue")]
    public TMP_Text VictoryDialogue;
    public Button VictoryContinue1;

    [Header("StartVictoryDialogue1")]
    public TMP_Text VictoryDialogue1;
    public Button VictoryContinue2;

    [Header("StartVictoryDialogue2")]
    public TMP_Text VictoryDialogue2;
    public Button VictoryContinue3;

    [Header("StartVictoryDialogue3")]
    public TMP_Text VictoryDialogue3;
    public Button VictoryContinue4;

    [Header("StartVictoryDialogue4")]
    public TMP_Text VictoryDialogue4;
    public Button VictoryContinue5;

    [Header("StartVictoryDialogue5")]
    public TMP_Text VictoryDialogue5;
    public Button VictoryContinue6;

    [Header("StartVictoryDialogue6")]
    public TMP_Text VictoryDialogue6;
    public Button VictoryContinue7;

    [Header("StartVictoryDialogue7")]
    public TMP_Text VictoryDialogue7;
    public Button VictoryContinue8;

    [Header("StartVictoryDialogue8")]
    public TMP_Text VictoryDialogue8;
    public Button VictoryContinue9;

    [Header("StartVictoryDialogue9")]
    public TMP_Text VictoryDialogue9;
    public Button VictoryContinue10;

    [Header("StartVictoryDialogue10")]
    public TMP_Text VictoryDialogue10;
    public Button VictoryContinue11;

    

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

        TMP_Text[] dialogues = { startDialogue, Dialogue1, Dialogue2, Dialogue3, Dialogue4, Dialogue5, Dialogue6, Dialogue7, Dialogue8, Dialogue9, Dialogue10, VictoryDialogue, VictoryDialogue1,VictoryDialogue2, VictoryDialogue3, VictoryDialogue4,VictoryDialogue5, VictoryDialogue6,VictoryDialogue7, VictoryDialogue8, VictoryDialogue9, VictoryDialogue10};

        Button[] allButtons = { Continue1, Continue2, Continue3, Continue4, Continue5, Continue6, Continue7, Continue8, Continue9, Continue10, Continue11, VictoryContinue1, VictoryContinue2, VictoryContinue3, VictoryContinue4, VictoryContinue5, VictoryContinue6, VictoryContinue7, VictoryContinue8, VictoryContinue9, VictoryContinue10, VictoryContinue11};

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
        SetDialogueState(false, true, startDialogue, new Button[] { Continue1 }, "Excuse me, are you Zerim?");
        ShowCharacter(true);
    }

    public void StartDialogue1()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[0]);
        SetDialogueState(true, false, Dialogue1, new Button[] { Continue2 }, "I am, and why might you want to know?");
        ShowCharacter(false);
    }

    public void StartDialogue2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue2, new Button[] {Continue3}, "Do you by any chance know someone who's called the “Powerful Solver”?");
        ShowCharacter(true);
    }

    public void StartDialogue3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[1]);
        SetDialogueState(true, false, Dialogue3, new Button[] { Continue4 }, "Oh? It's been a while since I've heard that name. And why might you be asking?");
        ShowCharacter(false);
    }

    public void StartDialogue4()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue4, new Button[] {Continue5}, "I heard that the Powerful Solver went to you to ask about a way back home. I want to go back too.");
        ShowCharacter(true);
    }

    public void StartDialogue5()
    {
        if (characterLoader != null)
        {
            GameObject player = characterLoader.GetInstantiatedPlayer();

            int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");

            if (selectedCharacter == 0)
            {
                StartDialogue5BoyModel();
            }
            else if (selectedCharacter == 1)
            {
                StartDialogue5GirlModel();
            }
        }
    }

    private void StartDialogue5GirlModel()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[2]);
        SetDialogueState(true, false, Dialogue5, new Button[] { Continue6 }, "So, you're the same as him? You're not from this world and you want to go back.");
        ShowCharacter(false);
    }

    private void StartDialogue5BoyModel()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[3]);
        SetDialogueState(true, false, Dialogue5, new Button[] { Continue6 }, "So, you're the same as her? You're not from this world and you want to go back.");
        ShowCharacter(false);
    }

    public void StartDialogue6()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue6, new Button[] {Continue7}, "That's right.");
        ShowCharacter(true);
    }

    public void StartDialogue7()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[4]);
        SetDialogueState(true, false, Dialogue7, new Button[] { Continue8 }, "Well sure, I can tell you more of what I know.");
        ShowCharacter(false);
    }

    public void StartDialogue8()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue8, new Button[] {Continue9}, "Really? Thank you!");
        ShowCharacter(true);
    }

    public void StartDialogue9()
    {
        if (characterLoader != null)
        {
            GameObject player = characterLoader.GetInstantiatedPlayer();

            int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");

            if (selectedCharacter == 0)
            {
                StartDialogue9BoyModel();
            }
            else if (selectedCharacter == 1)
            {
                StartDialogue9GirlModel();
            }
        }
    }

    private void StartDialogue9GirlModel()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[5]);
        SetDialogueState(true, false, Dialogue9, new Button[] { Continue10 }, "But you'll have to prove your skill to me first. Chr– I mean, the Powerful Solver is one bright kid and you too need to show some skill.");
        ShowCharacter(false);
    }

    private void StartDialogue9BoyModel()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[6]);
        SetDialogueState(true, false, Dialogue9, new Button[] { Continue10 }, "But you'll have to prove your skill to me first. Na– I mean, the Powerful Solver is one bright kid and you too need to show some skill.");
        ShowCharacter(false);
    }

    public void StartDialogue10()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[7]);
        SetDialogueState(true, false, Dialogue10, new Button[] { Continue11 }, "You should probably stop this journey of yours if you can't pass me.");
    }

    public void StartVictoryDialogue()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[8]);
        SetDialogueState(true, false, VictoryDialogue, new Button[] { VictoryContinue1 }, "Well well well, I'll admit my defeat. You at least have the basic amount of skill and determination to continue after all.");
        ShowCharacter(false);
    }

    public void StartVictoryDialogue1()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[9]);
        SetDialogueState(true, false, VictoryDialogue1, new Button[] { VictoryContinue2 }, "A deal is a deal, I'll tell you what I know in how you can go back home.");
    }

    public void StartVictoryDialogue2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[10]);
        SetDialogueState(true, false, VictoryDialogue2, new Button[] { VictoryContinue3 }, "I actually don't know much. My brother is the one who actually accompanied the “Powerful Solver” before.");
    }

    public void StartVictoryDialogue3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[11]);
        SetDialogueState(true, false, VictoryDialogue3, new Button[] { VictoryContinue4 }, "Neither do I know where my brother really is. Still, I can somehow offer some information that might lead to his whereabouts.");
    }

    public void StartVictoryDialogue4()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[12]);
        SetDialogueState(true, false, VictoryDialogue4, new Button[] { VictoryContinue5 }, "You can try looking for Nyx. He's a pretty sassy but helpful cat if you're able to tame him or get him to help somehow.");
    }

    public void StartVictoryDialogue5()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, VictoryDialogue5, new Button[] { VictoryContinue6 }, "A cat? How will a cat help me find your brother?");
        ShowCharacter(true);
    }

    public void StartVictoryDialogue6()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[13]);
        SetDialogueState(true, false, VictoryDialogue6, new Button[] { VictoryContinue7 }, "Oh right, the Powerful Solver has the same reaction before. This cat talks, so that should be more than enough to ease your worry.");
        ShowCharacter(false);
    }

    public void StartVictoryDialogue7()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[14]);
        SetDialogueState(true, false, VictoryDialogue7, new Button[] { VictoryContinue8 }, " It seems like you’ll need to continue on with your journey. You already know my name but I don't know yours.");
    }

    public void StartVictoryDialogue8()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[15]);
        SetDialogueState(true, false, VictoryDialogue8, new Button[] { VictoryContinue9 }, "I might as well learn about it, it's not everyday someone can meet someone similar to the Powerful Solver or someone who might stand on equal footing with them.");
    }

    public void StartVictoryDialogue9()
    {
        audioManager.StopCurrentSFX();
        string username = PlayerName.text; // Retrieve the player's name from the TMP_Text field
        string dialogueText = $"It's {username}.";
        SetDialogueState(false, true, VictoryDialogue9, new Button[] { VictoryContinue10 }, dialogueText);
        ShowCharacter(true);
    }

    public void StartVictoryDialogue10()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[16]);
        string username = PlayerName.text; // Retrieve the player's name from the TMP_Text field
        string dialogueText = $"{username}, I'll try to remember it. Well, good luck with your journey.";
        SetDialogueState(true, false, VictoryDialogue10, new Button[] { VictoryContinue11 }, dialogueText);
        ShowCharacter(false);
    }

    public void ConvoEnd()
    {
        audioManager.StopCurrentSFX();
        ConvoPanel.SetActive(false);

        TMP_Text[] dialogues = { startDialogue, Dialogue1, Dialogue2, Dialogue3, Dialogue4, Dialogue5, Dialogue6, Dialogue7, Dialogue8, Dialogue9, Dialogue10, VictoryDialogue, VictoryDialogue1,VictoryDialogue2, VictoryDialogue3, VictoryDialogue4,VictoryDialogue5, VictoryDialogue6,VictoryDialogue7, VictoryDialogue8, VictoryDialogue9, VictoryDialogue10};

        Button[] allButtons = { Continue1, Continue2, Continue3, Continue4, Continue5, Continue6, Continue7, Continue8, Continue9, Continue10, Continue11, VictoryContinue1, VictoryContinue2, VictoryContinue3, VictoryContinue4, VictoryContinue5, VictoryContinue6, VictoryContinue7, VictoryContinue8, VictoryContinue9, VictoryContinue10, VictoryContinue11};

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
