using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Database;

public class Boss12Dialogue : MonoBehaviour
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

    [Header("Continue13")]
    public TMP_Text Dialogue13;
    public Button Continue14;

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

        TMP_Text[] dialogues = { startDialogue, Dialogue1, Dialogue2, Dialogue3, Dialogue4, Dialogue5, Dialogue6, Dialogue7, Dialogue8, Dialogue9, Dialogue10, Dialogue11, Dialogue12, Dialogue13, VictoryDialogue, VictoryDialogue1,VictoryDialogue2, VictoryDialogue3, VictoryDialogue4,VictoryDialogue5, VictoryDialogue6,VictoryDialogue7, VictoryDialogue8, VictoryDialogue9, VictoryDialogue10};

        Button[] allButtons = { Continue1, Continue2, Continue3, Continue4, Continue5, Continue6, Continue7, Continue8, Continue9, Continue10, Continue11, Continue12, Continue13, Continue14, VictoryContinue1, VictoryContinue2, VictoryContinue3, VictoryContinue4, VictoryContinue5, VictoryContinue6, VictoryContinue7, VictoryContinue8, VictoryContinue9, VictoryContinue10, VictoryContinue11};

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
        SetDialogueState(false, true, startDialogue, new Button[] { Continue1 }, "You look like Zerim, could you be Nivalis?");
        ShowCharacter(true);
    }

    public void StartDialogue1()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[0]);
        SetDialogueState(true, false, Dialogue1, new Button[] { Continue2 }, "Yes, why are you looking for me?");
        ShowCharacter(false);
    }

    public void StartDialogue2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue2, new Button[] { Continue3 }, "I need to know, did you accompany the Powerful Solver towards the portal back to their home?");
        ShowCharacter(true);
    }

    public void StartDialogue3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[1]);
        SetDialogueState(true, false, Dialogue3, new Button[] { Continue4 }, "No. The one you're looking for is Ember. I'll be taking my leave now.");
        ShowCharacter(false);
    }

    public void StartDialogue4()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue4, new Button[] {Continue5}, "W-what? W-wait, can you at least tell me where he is?");
        ShowCharacter(true);
    }

    public void StartDialogue5()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[2]);
        SetDialogueState(true, false, Dialogue5, new Button[] { Continue6 }, "No");
        ShowCharacter(false);
    }

    public void StartDialogue6()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue6, new Button[] {Continue7}, "Why not?");
        ShowCharacter(true);
    }

    public void StartDialogue7()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[3]);
        SetDialogueState(true, false, Dialogue7, new Button[] { Continue8 }, "I don't owe you answers. I don't even know you.");
        ShowCharacter(false);
    }

    public void StartDialogue8()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue8, new Button[] {Continue9}, "Please, I'm just trying to find my way back home.");
        ShowCharacter(true);
    }

    public void StartDialogue9()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[4]);
        SetDialogueState(true, false, Dialogue9, new Button[] { Continue10 }, "That is not my problem. My time is precious, child. I've entertained your questions long enough.");
        ShowCharacter(false);
    }

    public void StartDialogue10()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue10, new Button[] {Continue11}, "But if you just—");
        ShowCharacter(true);
    }

    public void StartDialogue11()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[5]);
        SetDialogueState(true, false, Dialogue11, new Button[] { Continue12 }, "Child, this will be my last warning. If you do not leave me alone, I will make you.");
        ShowCharacter(false);
    }

    public void StartDialogue12()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue12, new Button[] {Continue13}, "No, I refuse. I just want to go home.");
        ShowCharacter(true);
    }

    public void StartDialogue13()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[6]);
        SetDialogueState(true, false, Dialogue13, new Button[] { Continue14 }, "Fine, if you cannot withstand this storm, you have no place seeking Ember— or anyone else.");
        ShowCharacter(false);
    }

    public void StartVictoryDialogue()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[7]);
        SetDialogueState(true, false, VictoryDialogue, new Button[] { VictoryContinue1 }, "Hmph. You're stronger than you look. I didn't expect you to last this long.");
        ShowCharacter(false);
    }

    public void StartVictoryDialogue1()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, VictoryDialogue1, new Button[] { VictoryContinue2 }, "I told you… I'm not leaving until you help me.");
        ShowCharacter(true);
    }

    public void StartVictoryDialogue2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[8]);
        SetDialogueState(true, false, VictoryDialogue2, new Button[] { VictoryContinue3 }, "Determined, aren't you? That's rare in these lands. Most would've fled or frozen by now.");
        ShowCharacter(false);
    }

    public void StartVictoryDialogue3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[9]);
        SetDialogueState(true, false, VictoryDialogue3, new Button[] { VictoryContinue4 }, "Very well. You've proven you're not just another person pursuing this journey on a whim.");
    }

    public void StartVictoryDialogue4()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[10]);
        SetDialogueState(true, false, VictoryDialogue4, new Button[] { VictoryContinue5 }, "Ember resides in the Rustwater, beyond the mountains to the north.");
    }

    public void StartVictoryDialogue5()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[11]);
        SetDialogueState(true, false, VictoryDialogue5, new Button[] { VictoryContinue6 }, "You'll find him there… if the cold doesn't claim you first.");
    }

    public void StartVictoryDialogue6()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, VictoryDialogue6, new Button[] { VictoryContinue7 }, "Thank you.");
        ShowCharacter(true);
    }

    public void StartVictoryDialogue7()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[12]);
        SetDialogueState(true, false, VictoryDialogue7, new Button[] { VictoryContinue8 }, "Don't thank me. If you fail to reach him, it will be as though this conversation never happened");
        ShowCharacter(false);
    }

    public void StartVictoryDialogue8()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, VictoryDialogue8, new Button[] { VictoryContinue9 }, "Wait… why did you fight me? Why not just tell me from the start?");
        ShowCharacter(true);
    }

    public void StartVictoryDialogue9()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[13]);
        SetDialogueState(true, false, VictoryDialogue9, new Button[] { VictoryContinue10 }, "If you couldn't survive my magic, you'd have no chance against what lies ahead. Consider this a test… and a lesson.");
        ShowCharacter(false);
    }

    public void StartVictoryDialogue10()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[14]);
        SetDialogueState(true, false, VictoryDialogue10, new Button[] { VictoryContinue11 }, "Stay out of trouble, child. Not everyone will be as merciful as I am.");
    }

    public void ConvoEnd()
    {
        audioManager.StopCurrentSFX();
        ConvoPanel.SetActive(false);

        TMP_Text[] dialogues = { startDialogue, Dialogue1, Dialogue2, Dialogue3, Dialogue4, Dialogue5, Dialogue6, Dialogue7, Dialogue8, Dialogue9, Dialogue10, Dialogue11, Dialogue12, Dialogue13, VictoryDialogue, VictoryDialogue1,VictoryDialogue2, VictoryDialogue3, VictoryDialogue4,VictoryDialogue5, VictoryDialogue6,VictoryDialogue7, VictoryDialogue8, VictoryDialogue9, VictoryDialogue10};

        Button[] allButtons = { Continue1, Continue2, Continue3, Continue4, Continue5, Continue6, Continue7, Continue8, Continue9, Continue10, Continue11, Continue12, Continue13, Continue14, VictoryContinue1, VictoryContinue2, VictoryContinue3, VictoryContinue4, VictoryContinue5, VictoryContinue6, VictoryContinue7, VictoryContinue8, VictoryContinue9, VictoryContinue10, VictoryContinue11};

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
