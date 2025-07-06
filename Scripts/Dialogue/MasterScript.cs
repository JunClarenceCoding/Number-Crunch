using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MasterScript : MonoBehaviour
{
    public StageDemoScene stageDemoScene;
    public AudioManager audioManager;
    public Image playerDisplayImage; 
    public Image npcDisplayImage;  
    public Sprite boyImage; 
    public Sprite girlImage; 
    public GameObject NPCNamePanel;
    public GameObject PlayerNamePanel; 
    public GameObject NPC, ConvoPanel;
    public TMP_Text NPCName, PlayerName;
    public NPCNavigation npcNavigation;
    public TriggerBox triggerCube;
    public ArrowTriggerBox Arrow;
    public GameObject triggerBox;
    public Camera PlayerCamera, TalkCamera;
    private GameObject player; 
    private PlayerControl playerController; 
    private LoadCharacter loadCharacter;

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

    [Header("Continue14")]
    public TMP_Text Dialogue14;
    public Button Continue15;

    [Header("Continue15")]
    public TMP_Text Dialogue15;
    public Button Continue16;

    [Header("Continue16")]
    public TMP_Text Dialogue16;
    public Button Continue17;

    [Header("Continue17")]
    public TMP_Text Dialogue17;
    public Button Continue18;

    [Header("Continue18")]
    public TMP_Text Dialogue18;
    public Button Continue19;

    [Header("Continue19")]
    public TMP_Text Dialogue19;
    public Button Continue20;

    [Header("Continue20")]
    public TMP_Text Dialogue20;
    public Button Continue21;

    [Header("Continue21")]
    public TMP_Text Dialogue21;
    public Button Continue22;

    [Header("Continue22")]
    public TMP_Text Dialogue22;
    public Button Continue23;

    [Header("Continue23")]
    public TMP_Text Dialogue23;
    public Button Continue24;

    void Start()
    {
        npcNavigation.enabled = false;
        StartCoroutine(InitializePlayer());
        loadCharacter = FindObjectOfType<LoadCharacter>();
        if(loadCharacter != null)
        {
            UpdatePlayerImage();
        }
    }
    private IEnumerator InitializePlayer()
    {
        loadCharacter = FindObjectOfType<LoadCharacter>();
        while (loadCharacter == null)
        {
            yield return null; 
            loadCharacter = FindObjectOfType<LoadCharacter>();
        }

        while (loadCharacter.GetInstantiatedPlayer() == null)
        {
            yield return null;
        }
        player = loadCharacter.GetInstantiatedPlayer();
        playerController = player.GetComponent<PlayerControl>();
        TalkCamera.gameObject.SetActive(false);
    }
    private void UpdatePlayerImage()
    {
        if (loadCharacter != null)
        {
            GameObject player = loadCharacter.GetInstantiatedPlayer();

            // Check which character is selected
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

        TMP_Text[] dialogues = { startDialogue, Dialogue1, Dialogue2, Dialogue3, Dialogue4, Dialogue5, Dialogue6, Dialogue7, Dialogue8, Dialogue9, Dialogue10, Dialogue11, Dialogue12, Dialogue13, Dialogue14, Dialogue15, Dialogue16, Dialogue17, Dialogue18, Dialogue19, Dialogue20, Dialogue21, Dialogue22, Dialogue23};

        Button[] allButtons = { Continue1, Continue2, Continue3, Continue4, Continue5, Continue6, Continue7, Continue8, Continue9, Continue10, Continue11, Continue12, Continue13, Continue14, Continue15, Continue16, Continue17, Continue18, Continue19, Continue20, Continue21, Continue22, Continue23, Continue24};

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
                button.gameObject.SetActive(true); 
            }
        });
    }
    public void ConvoStart()
    {
        stageDemoScene.CheckAndActivateSkipButton();
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[0]);
        SetDialogueState(true, false, startDialogue, new Button[] { Continue1 }, "Welcome to the town, we should be safe here.");
        ShowCharacter(false); 
        StartCoroutine(SwitchToTalkCamera());
        if (playerController != null)
        {
            playerController.enabled = false;
        }
    }
    private IEnumerator SwitchToTalkCamera()
    {
        Vector3 midpoint = (player.transform.position + NPC.transform.position) / 2f;
        TalkCamera.transform.position = midpoint + new Vector3(-8, 3, -3); 
        TalkCamera.transform.LookAt(midpoint);
        float transitionTime = 0f; 
        float elapsedTime = 0f;
        while (elapsedTime < transitionTime)
        {
            PlayerCamera.gameObject.SetActive(false);
            TalkCamera.gameObject.SetActive(true);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        PlayerCamera.gameObject.SetActive(false);
        TalkCamera.gameObject.SetActive(true);
    }
    public void StartDialogue1()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1, new Button[] { Continue2 }, "Thank you for saving me.");
        ShowCharacter(true);
    }
    public void StartDialogue2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[1]);
        SetDialogueState(true, false, Dialogue2, new Button[] { Continue3 }, "No need to thank me, I'm glad you're alright.");
        ShowCharacter(false);
    }
    public void StartDialogue3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[2]);
        SetDialogueState(true, false, Dialogue3, new Button[] { Continue4 }, "Anyway, what were you doing in the forest? Alone even.");
    }
    public void StartDialogue4()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue4, new Button[] { Continue5 }, "I don't know. I was in my school when I suddenly saw a bright light.");
        ShowCharacter(true);
    }
    public void StartDialogue5()
    {
        SetDialogueState(false, true, Dialogue5, new Button[] { Continue6 }, "The next thing I know, I'm in a forest and even my clothes are different.");
    }
    public void StartDialogue6()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[3]);
        SetDialogueState(true, false, Dialogue6, new Button[] { Continue7 }, "A sudden bright light? Say, what's this… school of yours called?");
        ShowCharacter(false);
    }
    public void StartDialogue7()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue7, new Button[] { Continue8 }, "It's the National Academy of Science and Technology. NASTECH for short.");
        ShowCharacter(true);
    }
    public void StartDialogue8()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[4]);
        SetDialogueState(true, false, Dialogue8, new Button[] { Continue9 }, "NASTECH…");
        ShowCharacter(false);
    }
    public void StartDialogue9()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[5]);
        SetDialogueState(true, false, Dialogue9, new Button[] { Continue10 }, "Oh, I see. So you're similar to the Powerful Solver.");
    }
    public void StartDialogue10()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue10, new Button[] { Continue11 }, "The Powerful Solver?");
        ShowCharacter(true);
    }
    public void StartDialogue11()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[6]);
        SetDialogueState(true, false, Dialogue11, new Button[] { Continue12 }, "Yeah, they're known for being extremely strong despite their young age.");
        ShowCharacter(false);
    }
    public void StartDialogue12()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[7]);
        SetDialogueState(true, false, Dialogue12, new Button[] { Continue13 }, "Do you remember when I had blocked the slime's attack for you?");
    }
    public void StartDialogue13()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[8]);
        SetDialogueState(true, false, Dialogue13, new Button[] { Continue14 }, "I had to quickly summon the power of numbers to be able to do that.");
    }
    public void StartDialogue14()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[9]);
        SetDialogueState(true, false, Dialogue14, new Button[] { Continue15 }, "Now the Powerful Solver is able to solve math problems quickly and with just their mind.");
    }
    public void StartDialogue15()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[10]);
        SetDialogueState(true, false, Dialogue15, new Button[] { Continue16 }, "I mean, if that's not impressive I don't know what is.");
    }
    public void StartDialogue16()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[11]);
        SetDialogueState(true, false, Dialogue16, new Button[] { Continue17 }, "Aside from that, it was said that they came from this mysterious NASTECH and wanted to go back.");
    }
    public void StartDialogue17()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue17, new Button[] { Continue18 }, "Can you tell me how I can meet them?");
        ShowCharacter(true);
    }
    public void StartDialogue18()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[12]);
        SetDialogueState(true, false, Dialogue18, new Button[] { Continue19 }, "I'm sorry, I'm afraid you can't. From what I know, they're gone and went back to where you guys came from.");
        ShowCharacter(false);
    }
    public void StartDialogue19()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue19, new Button[] { Continue20 }, "Oh…");
        ShowCharacter(true);
    }
    public void StartDialogue20()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[13]);
        SetDialogueState(true, false, Dialogue20, new Button[] { Continue21 }, "Don't worry, not all hope is gone.");
        ShowCharacter(false);
    }
    public void StartDialogue21()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[14]);
        SetDialogueState(true, false, Dialogue21, new Button[] { Continue22 }, "If you want to go home, I'm sure the Fighter Association knows something about how to do so.");
    }
    public void StartDialogue22()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[15]);
        SetDialogueState(true, false, Dialogue22, new Button[] { Continue23 }, "That's where the Powerful Solver associated themself while they were here..");
    }
    public void StartDialogue23()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[16]);
        SetDialogueState(true, false, Dialogue23, new Button[] { Continue24}, "Let's go there and ask around.");
    }
    public void ConvoEnd()
    {
        audioManager.StopCurrentSFX();
        ConvoPanel.SetActive(false);
        NPCName.gameObject.SetActive(false);
        PlayerName.gameObject.SetActive(false);
        Arrow.playerArrow.gameObject.SetActive(true);
        triggerCube.trigger.gameObject.SetActive(false);

        TMP_Text[] dialogues = { startDialogue, Dialogue1, Dialogue2, Dialogue3, Dialogue4, Dialogue5, Dialogue6, Dialogue7, Dialogue8, Dialogue9, Dialogue10, Dialogue11, Dialogue12, Dialogue13, Dialogue14, Dialogue15, Dialogue16, Dialogue17, Dialogue18, Dialogue19, Dialogue20, Dialogue21, Dialogue22, Dialogue23};

        Button[] allButtons = { Continue1, Continue2, Continue3, Continue4, Continue5, Continue6, Continue7, Continue8, Continue9, Continue10, Continue11, Continue12, Continue13, Continue14, Continue15, Continue16, Continue17, Continue18, Continue19, Continue20, Continue21, Continue22, Continue23, Continue24};

        foreach (TMP_Text dialogue in dialogues)
        {
            dialogue.gameObject.SetActive(false);
        }

        foreach (Button button in allButtons)
        {
            button.gameObject.SetActive(false);
        }

        npcNavigation.OnPressedFollow();
        npcNavigation.enabled = true;
        if (triggerBox != null)
        {
            triggerBox.SetActive(false);
        }
        StartCoroutine(SwitchToPlayerCamera());
        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }
    private IEnumerator SwitchToPlayerCamera()
    {
        float transitionTime = 1.0f; 
        float elapsedTime = 0f;

        while (elapsedTime < transitionTime)
        {
            TalkCamera.gameObject.SetActive(false);
            PlayerCamera.gameObject.SetActive(true);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        TalkCamera.gameObject.SetActive(false);
        PlayerCamera.gameObject.SetActive(true);
    }
}