using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SecondDialogueScript : MonoBehaviour
{
    public StageDemoScene stageDemoScene;
    public AudioManager audioManager;
    public int customXRotation = 10;
    public Image playerDisplayImage; 
    public Image npcDisplayImage;  
    public Sprite boyImage; 
    public Sprite girlImage; 
    public GameObject NPCNamePanel; 
    public GameObject PlayerNamePanel; 
    public GameObject NPC, ConvoPanel;
    public TMP_Text NPCName, PlayerName;
    public NPCNavigation npcNavigation;
    public ArrowBullitin arrowBullitin;
    public ArrowTriggerBox arrowNPC;
    public GameObject triggerBox;
    public NoticeBoardTriggerBox noticeBoardTriggerBox; 
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
    private void UpdatePlayerImage()
    {
        if (loadCharacter != null)
        {
            GameObject player = loadCharacter.GetInstantiatedPlayer();

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
    private void SetDialogueState(bool npcActive, bool playerActive, TMP_Text activeDialogue, Button[] activeButtons, string dialogueText)
    {
        ConvoPanel.SetActive(true);
        NPCNamePanel.SetActive(npcActive);
        PlayerNamePanel.SetActive(playerActive);
        NPCName.gameObject.SetActive(npcActive);
        PlayerName.gameObject.SetActive(playerActive);

        TMP_Text[] dialogues = { startDialogue, Dialogue1, Dialogue2, Dialogue3};
        Button[] allButtons = { Continue1, Continue2, Continue3, Continue4};

        foreach (TMP_Text dialogue in dialogues)
        {
            dialogue.gameObject.SetActive(false);
        }
        foreach (Button button in allButtons)
        {
            button.gameObject.SetActive(false);
        }
        activeDialogue.gameObject.SetActive(true);

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
        stageDemoScene.CheckAndActivateSkipButton1();
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[0]);
        SetDialogueState(true, false, startDialogue, new Button[] { Continue1 }, "Hey there bud. I just remembered I need to do something first.");
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

        TalkCamera.transform.position = midpoint + new Vector3(8, 3, -6); 
        TalkCamera.transform.LookAt(midpoint);
        Vector3 currentRotation = TalkCamera.transform.eulerAngles;
        currentRotation.x = customXRotation;  
        TalkCamera.transform.eulerAngles = currentRotation;
        float transitionTime = 1.0f; 
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
    public void StartDialogue1()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[1]);
        SetDialogueState(true, false, Dialogue1, new Button[] { Continue2 }, "I think we actually passed by a Notice Board that contains a lot of information that can be quite useful.");
    }
    public void StartDialogue2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[2]);
        SetDialogueState(true, false, Dialogue2, new Button[] { Continue3 }, "Why not check it out first, then we can meet back here again?");
    }
    public void StartDialogue3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue3, new Button[] { Continue4 }, "Okay then.");
        ShowCharacter(true);
    }
    public void ConvoEnd()
    {
        audioManager.StopCurrentSFX();
        ConvoPanel.SetActive(false);
        NPCName.gameObject.SetActive(false);
        PlayerName.gameObject.SetActive(false);
        arrowNPC.triggerBox.gameObject.SetActive(false);
        arrowNPC.playerArrow.gameObject.SetActive(false);
        arrowBullitin.triggerBox.gameObject.SetActive(true);
        arrowBullitin.playerArrow.gameObject.SetActive(true);

        TMP_Text[] dialogues = { startDialogue, Dialogue1, Dialogue2, Dialogue3};
        Button[] allButtons = { Continue1, Continue2, Continue3, Continue4};

        foreach (TMP_Text dialogue in dialogues)
        {
            dialogue.gameObject.SetActive(false);
        }
        foreach (Button button in allButtons)
        {
            button.gameObject.SetActive(false);
        }
        if(triggerBox != null){
            triggerBox.SetActive(false);
        }
        if (noticeBoardTriggerBox != null)
        {
            noticeBoardTriggerBox.EnableTriggerBox();
        }
        StartCoroutine(SwitchToPlayerCamera());
        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }
}