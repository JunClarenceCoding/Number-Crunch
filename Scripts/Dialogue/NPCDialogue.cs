using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialogue : MonoBehaviour
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
    public ArrowTriggerBox arrowNPC;
    public ArrowFA arrowFA;
    public TMP_Text NPCName, PlayerName;
    public Camera PlayerCamera, TalkCamera;
    public NPCTriggerBox npcTriggerBox; 
    public FATriggerBox faTriggerBox; 
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

        TMP_Text[] dialogues = { startDialogue, Dialogue1, Dialogue2, Dialogue3 };
        Button[] allButtons = { Continue1, Continue2, Continue3, Continue4 };

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
        stageDemoScene.CheckAndActivateSkipButton3();
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[0]);
        SetDialogueState(true, false, startDialogue, new Button[] { Continue1 }, "Oh there you are. How was the reading?");
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
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1, new Button[] { Continue2 }, "It was okay.");
        ShowCharacter(true);
    }
    public void StartDialogue2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[1]);
        SetDialogueState(true, false, Dialogue2, new Button[] { Continue3 }, "Well, I'm not really sure if you actually read or not but you can always come back to it if you want to. Let's head inside then.");
        ShowCharacter(false);
    }
    public void StartDialogue3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[2]);
        SetDialogueState(true, false, Dialogue3, new Button[] { Continue4 }, "After you.");
    }
    public void ConvoEnd()
    {
        audioManager.StopCurrentSFX();
        ConvoPanel.SetActive(false);
        NPCName.gameObject.SetActive(false);
        PlayerName.gameObject.SetActive(false);
        arrowNPC.triggerBox.gameObject.SetActive(false);
        arrowNPC.playerArrow.gameObject.SetActive(false);
        arrowFA.triggerBox.gameObject.SetActive(true);
        arrowFA.playerArrow.gameObject.SetActive(true);

        TMP_Text[] dialogues = { startDialogue, Dialogue1, Dialogue2, Dialogue3 };
        Button[] allButtons = { Continue1, Continue2, Continue3, Continue4 };

        foreach (TMP_Text dialogue in dialogues)
        {
            dialogue.gameObject.SetActive(false);
        }
        foreach (Button button in allButtons)
        {
            button.gameObject.SetActive(false);
        }
        StartCoroutine(SwitchToPlayerCamera());

        if (playerController != null)
        {
            playerController.enabled = true;
        }
        if (npcTriggerBox != null)
        {
            npcTriggerBox.DeactivateTriggerBox();
        }
        if (faTriggerBox != null)
        {
            faTriggerBox.EnableTriggerBox();
        }
    }
}