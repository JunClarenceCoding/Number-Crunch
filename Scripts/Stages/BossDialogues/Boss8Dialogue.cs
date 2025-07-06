using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Database;

public class Boss8Dialogue : MonoBehaviour
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

    [Header("Continue11")]
    public TMP_Text Dialogue11;
    public Button Continue12;

    [Header("Continue12")]
    public TMP_Text Dialogue12;
    public Button Continue13;

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

        TMP_Text[] dialogues = { startDialogue, Dialogue1, Dialogue2, Dialogue3, Dialogue4, Dialogue5, Dialogue6, Dialogue7, Dialogue8, Dialogue9, Dialogue10, Dialogue11, Dialogue12, VictoryDialogue, VictoryDialogue1,VictoryDialogue2, VictoryDialogue3, VictoryDialogue4,VictoryDialogue5, VictoryDialogue6,VictoryDialogue7, VictoryDialogue8, VictoryDialogue9};

        Button[] allButtons = { Continue1, Continue2, Continue3, Continue4, Continue5, Continue6, Continue7, Continue8, Continue9, Continue10, Continue11, Continue12, Continue13, VictoryContinue1, VictoryContinue2, VictoryContinue3, VictoryContinue4, VictoryContinue5, VictoryContinue6, VictoryContinue7, VictoryContinue8, VictoryContinue9, VictoryContinue10};

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
        SetDialogueState(false, true, startDialogue, new Button[] { Continue1 }, "I finally found you.");
        ShowCharacter(true);
    }

    public void StartDialogue1()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[0]);
        SetDialogueState(true, false, Dialogue1, new Button[] { Continue2 }, "Hmph, and who might you be?");
        ShowCharacter(false);
    }

    public void StartDialogue2()
    {
        audioManager.StopCurrentSFX();
        string username = PlayerName.text; // Retrieve the player's name from the TMP_Text field
        string dialogueText = $"My name is {username}. I'm looking for my way back home and someone has told me that you can help me.";
        SetDialogueState(false, true, Dialogue2, new Button[] { Continue3 }, dialogueText);
        ShowCharacter(true);
    }

    public void StartDialogue3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[1]);
        SetDialogueState(true, false, Dialogue3, new Button[] { Continue4 }, "And who told you that?");
        ShowCharacter(false);
    }

    public void StartDialogue4()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue4, new Button[] {Continue5}, "Zerim did. He said you might know his brother's whereabouts.");
        ShowCharacter(true);
    }

    public void StartDialogue5()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[2]);
        SetDialogueState(true, false, Dialogue5, new Button[] { Continue6 }, "Zerim? That buffoon. He really couldn't keep his mouth shut, could he?");
        ShowCharacter(false);
    }

    public void StartDialogue6()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[3]);
        SetDialogueState(true, false, Dialogue6, new Button[] { Continue7 }, "Let me guess— he didn't even tell you which brother you're looking for, did he?");
    }

    public void StartDialogue7()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue7, new Button[] {Continue8}, "N-no… He said that his brother accompanied the Powerful Solver before though.");
        ShowCharacter(true);
    }

    public void StartDialogue8()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[4]);
        SetDialogueState(true, false, Dialogue8, new Button[] { Continue9 }, "Of course, he left out the most crucial details. Both of his brothers worked with that meddlesome Solver. Honestly, one is as infuriating as the other.");
        ShowCharacter(false);
    }

    public void StartDialogue9()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue9, new Button[] {Continue10}, "Please, I just want to go back already.");
        ShowCharacter(true);
    }

    public void StartDialogue10()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[5]);
        SetDialogueState(true, false, Dialogue10, new Button[] { Continue11 }, "Well little child, answers don't come freely in this world. If you want my help, you'll have to prove your worth first.");
        ShowCharacter(false);
    }

    public void StartDialogue11()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue11, new Button[] {Continue12}, "Okay… what do I need to do?");
        ShowCharacter(true);
    }

    public void StartDialogue12()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[6]);
        SetDialogueState(true, false, Dialogue12, new Button[] { Continue13 }, "Win");
        ShowCharacter(false);
    }

    public void StartVictoryDialogue()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[7]);
        SetDialogueState(true, false, VictoryDialogue, new Button[] { VictoryContinue1 }, "Well, well, you're tougher than you look. Very well, I'll tell you what I know.");
        ShowCharacter(false);
    }

    public void StartVictoryDialogue1()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[8]);
        SetDialogueState(true, false, VictoryDialogue1, new Button[] { VictoryContinue2 }, "Zerim's brother, eh? If I had to guess, you'll want to find Nivalis first.");
    }

    public void StartVictoryDialogue2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[9]);
        SetDialogueState(true, false, VictoryDialogue2, new Button[] { VictoryContinue3 }, "He was always the Ember's shadow, following orders without question. The Solver trusted him to get things done.");
    }

    public void StartVictoryDialogue3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, VictoryDialogue3, new Button[] { VictoryContinue4 }, "Where can I find him?");
        ShowCharacter(true);
    }

    public void StartVictoryDialogue4()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[10]);
        SetDialogueState(true, false, VictoryDialogue4, new Button[] { VictoryContinue5 }, "I'm pretty sure he lives in Northmere. It'll be a long way but I'm sure you can manage.");
        ShowCharacter(false);
    }

    public void StartVictoryDialogue5()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, VictoryDialogue5, new Button[] { VictoryContinue6 }, "And what about Zerim's other brother?");
        ShowCharacter(true);
    }

    public void StartVictoryDialogue6()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[11]);
        SetDialogueState(true, false, VictoryDialogue6, new Button[] { VictoryContinue7 }, "Tsk tsk, one thing at a time, little one.");
        ShowCharacter(false);
    }

    public void StartVictoryDialogue7()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[12]);
        SetDialogueState(true, false, VictoryDialogue7, new Button[] { VictoryContinue8 }, "Focus on Nivalis first. He'll… probably have the answers you seek. Or not. Who can say?");
    }

    public void StartVictoryDialogue8()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, VictoryDialogue8, new Button[] { VictoryContinue9 }, "I feel like you're just teasing me now.");
        ShowCharacter(true);
    }

    public void StartVictoryDialogue9()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[13]);
        SetDialogueState(true, false, VictoryDialogue9, new Button[] { VictoryContinue10 }, "Maybe, maybe not. But either way, you should go on your merry way. I need to rest after playing with you for too long.");
        ShowCharacter(false);
    }

    public void ConvoEnd()
    {
        audioManager.StopCurrentSFX();
        ConvoPanel.SetActive(false);

        TMP_Text[] dialogues = { startDialogue, Dialogue1, Dialogue2, Dialogue3, Dialogue4, Dialogue5, Dialogue6, Dialogue7, Dialogue8, Dialogue9, Dialogue10, Dialogue11, Dialogue12, VictoryDialogue, VictoryDialogue1,VictoryDialogue2, VictoryDialogue3, VictoryDialogue4,VictoryDialogue5, VictoryDialogue6,VictoryDialogue7, VictoryDialogue8, VictoryDialogue9};

        Button[] allButtons = { Continue1, Continue2, Continue3, Continue4, Continue5, Continue6, Continue7, Continue8, Continue9, Continue10, Continue11, Continue12, Continue13, VictoryContinue1, VictoryContinue2, VictoryContinue3, VictoryContinue4, VictoryContinue5, VictoryContinue6, VictoryContinue7, VictoryContinue8, VictoryContinue9, VictoryContinue10};

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
