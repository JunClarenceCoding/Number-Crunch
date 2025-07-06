using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Database;

public class Boss16Dialogue : MonoBehaviour
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

        TMP_Text[] dialogues = { startDialogue, Dialogue1, Dialogue2, Dialogue3, Dialogue4, Dialogue5, Dialogue6, Dialogue7, Dialogue8, Dialogue9, Dialogue10, Dialogue11, Dialogue12, VictoryDialogue, VictoryDialogue1,VictoryDialogue2, VictoryDialogue3, VictoryDialogue4,VictoryDialogue5, VictoryDialogue6,VictoryDialogue7, VictoryDialogue8};

        Button[] allButtons = { Continue1, Continue2, Continue3, Continue4, Continue5, Continue6, Continue7, Continue8, Continue9, Continue10, Continue11, Continue12, Continue13, VictoryContinue1, VictoryContinue2, VictoryContinue3, VictoryContinue4, VictoryContinue5, VictoryContinue6, VictoryContinue7, VictoryContinue8, VictoryContinue9};

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
        SetDialogueState(false, true, startDialogue, new Button[] { Continue1 }, "Ember?");
        ShowCharacter(true);
    }

    public void StartDialogue1()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[0]);
        SetDialogueState(true, false, Dialogue1, new Button[] { Continue2 }, "You know me?");
        ShowCharacter(false);
    }

    public void StartDialogue2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue2, new Button[] { Continue3 }, "Yes, I've met your siblings. I've been searching for you.");
        ShowCharacter(true);
    }

    public void StartDialogue3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[1]);
        SetDialogueState(true, false, Dialogue3, new Button[] { Continue4 }, "And what might a child like you need from someone like me.");
        ShowCharacter(false);
    }

    public void StartDialogue4()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue4, new Button[] {Continue5}, "I'm in the same boat as the Powerful Solver.");
        ShowCharacter(true);
    }

    public void StartDialogue5()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue5, new Button[] {Continue6}, "I want to go back home, so please help me. Please tell me where the portal is.");
    }

    public void StartDialogue6()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[2]);
        SetDialogueState(true, false, Dialogue6, new Button[] { Continue7 }, "I see. I do indeed know where the portal is, but I can't let you go there. At least not yet.");
        ShowCharacter(false);
    }

    public void StartDialogue7()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[3]);
        SetDialogueState(true, false, Dialogue7, new Button[] { Continue8 }, "You definitely have passed by my brothers and they have let you continue being capable of handling more of the challenges along the journey.");
    }

    public void StartDialogue8()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[4]);
        SetDialogueState(true, false, Dialogue8, new Button[] { Continue9}, "Tell me, how was it?");
    }

    public void StartDialogue9()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue9, new Button[] {Continue10}, "They were definitely hard but as I went through I think I'm slowly improving more and more with my ability.");
        ShowCharacter(true);
    }

    public void StartDialogue10()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[5]);
        SetDialogueState(true, false, Dialogue10, new Button[] { Continue11 }, "That does seem to be the case.");
        ShowCharacter(false);
    }

    public void StartDialogue11()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[6]);
        SetDialogueState(true, false, Dialogue11, new Button[] { Continue12 }, "Still, if you really continue towards the end where the portal is you’ll definitely encounter more challenging enemies.");
    }

    public void StartDialogue12()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[7]);
        SetDialogueState(true, false, Dialogue12, new Button[] { Continue13 }, "For now, I need you to be able to defeat me before I can freely let you pursue the gate back to your world.");
    }

    public void StartVictoryDialogue()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[8]);
        SetDialogueState(true, false, VictoryDialogue, new Button[] { VictoryContinue1 }, "Great work. It seems like you really are ready to take on more of the challenges ahead.");
        ShowCharacter(false);
    }

    public void StartVictoryDialogue1()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, VictoryDialogue1, new Button[] { VictoryContinue2 }, "Will you tell me where the portal is now?");
        ShowCharacter(true);
    }

    public void StartVictoryDialogue2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[9]);
        SetDialogueState(true, false, VictoryDialogue2, new Button[] { VictoryContinue3}, "Yes, the portal is in the Blightspire but I'm warning you, child.");
        ShowCharacter(false);
    }

    public void StartVictoryDialogue3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[10]);
        SetDialogueState(true, false, VictoryDialogue3, new Button[] { VictoryContinue4}, "The portal is being guarded by an extremely strong enemy.");
    }

    public void StartVictoryDialogue4()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[11]);
        SetDialogueState(true, false, VictoryDialogue4, new Button[] { VictoryContinue5}, "Do your best to improve your skill more than what it is now and hope you can defeat him successfully.");
    }

    public void StartVictoryDialogue5()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[12]);
        SetDialogueState(true, false, VictoryDialogue5, new Button[] { VictoryContinue6}, "Before you leave, be sure to be careful and remember what I’m about to tell you.");
    }

    public void StartVictoryDialogue6()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[13]);
        SetDialogueState(true, false, VictoryDialogue6, new Button[] { VictoryContinue7}, "Kael, a demon, can manipulate the way you see things.");
    }

    public void StartVictoryDialogue7()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[14]);
        SetDialogueState(true, false, VictoryDialogue7, new Button[] { VictoryContinue8}, "So I suggest that when things around you are getting a little too odd, then it is most likely that you are growing close to the one guarding the portal.");
    }

    public void StartVictoryDialogue8()
    {
        if (characterLoader != null)
        {
            GameObject player = characterLoader.GetInstantiatedPlayer();

            int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");

            if (selectedCharacter == 0)
            {
                StartVictoryDialogue8BoyModel();
            }
            else if (selectedCharacter == 1)
            {
                StartVictoryDialogue8GirlModel();
            }
        }
    }

    private void StartVictoryDialogue8GirlModel()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[15]);
        SetDialogueState(true, false, VictoryDialogue8, new Button[] { VictoryContinue9}, "I wish you luck, child. I hope you find your way back like Chris.");
    }

    private void StartVictoryDialogue8BoyModel()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[16]);
        SetDialogueState(true, false, VictoryDialogue8, new Button[] { VictoryContinue9}, "I wish you luck, child. I hope you find your way back like Natalie.");
    }


    public void ConvoEnd()
    {
        audioManager.StopCurrentSFX();

        ConvoPanel.SetActive(false);

        TMP_Text[] dialogues = { startDialogue, Dialogue1, Dialogue2, Dialogue3, Dialogue4, Dialogue5, Dialogue6, Dialogue7, Dialogue8, Dialogue9, Dialogue10, Dialogue11, Dialogue12, VictoryDialogue, VictoryDialogue1,VictoryDialogue2, VictoryDialogue3, VictoryDialogue4,VictoryDialogue5, VictoryDialogue6,VictoryDialogue7, VictoryDialogue8};

        Button[] allButtons = { Continue1, Continue2, Continue3, Continue4, Continue5, Continue6, Continue7, Continue8, Continue9, Continue10, Continue11, Continue12, Continue13, VictoryContinue1, VictoryContinue2, VictoryContinue3, VictoryContinue4, VictoryContinue5, VictoryContinue6, VictoryContinue7, VictoryContinue8, VictoryContinue9};

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
