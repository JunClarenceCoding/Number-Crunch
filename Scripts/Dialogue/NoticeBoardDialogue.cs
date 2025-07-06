using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoticeBoardDialogue : MonoBehaviour
{
    public StageDemoScene stageDemoScene;
    public GameObject NoticeBoardPanel;
    public TMP_Text NoticeBoardName;
    public GameObject triggerBox;
    public ArrowBullitin arrowBullitin;
    public ArrowTriggerBox arrowNPC;
    public NPCTriggerBox npcTriggerBox; 
    private GameObject player; 
    private PlayerControl playerController; 
    private LoadCharacter loadCharacter;
    public GameObject NoticeBoard;

    [Header("Start Dialogue")]
    public TMP_Text startDialogue;
    public Button option1, option2, option3;

    [Header("start option 1 dialogue1")]
    public TMP_Text option1Dialogue1;
    public Button option1Continue1;

    [Header("start option 1 dialogue2")]
    public TMP_Text option1Dialogue2;
    public Button option1Continue2;

    [Header("start option 1 dialogue3")]
    public TMP_Text option1Dialogue3;
    public Button option1Continue3;

    [Header("start option 1 dialogue4")]
    public TMP_Text option1Dialogue4;
    public Button option1Continue4;

    [Header("start option 2 dialogue1")]
    public TMP_Text option2Dialogue1;
    public Button option2Continue1;

    [Header("start option 2 dialogue2")]
    public TMP_Text option2Dialogue2;
    public Button option2Continue2;

    void Start()
    {
        StartCoroutine(InitializePlayer());
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
    }
    private void SetDialogueState(TMP_Text activeDialogue, Button[] activeButtons, string dialogueText)
    {
        NoticeBoardPanel.SetActive(true);

        TMP_Text[] dialogues = { startDialogue, option1Dialogue1, option1Dialogue2, option1Dialogue3, option1Dialogue4, option2Dialogue1, option2Dialogue2};

        Button[] allButtons = { option1, option2, option3, option1Continue1, option1Continue2, option1Continue3, option1Continue4, option2Continue1, option2Continue2};

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
        stageDemoScene.CheckAndActivateSkipButton2();
        arrowBullitin.playerArrow.gameObject.SetActive(false);
        SetDialogueState(startDialogue, new Button[] { option1, option2, option3 }, "There are many papers posted on the board. Which one would you like to read about?");

        if (playerController != null)
        {
            playerController.enabled = false;
        }
        if (player != null && NoticeBoard != null)
        {
            player.transform.LookAt(NoticeBoard.transform);
        }
    }

    //start option1 Dialogues
    public void StartOption1Dialogue1()
    {
        SetDialogueState(option1Dialogue1, new Button[] { option1Continue1 }, "Each battle consists of two repeating phases: the Attack Phase and the Defend Phase, each lasting 60 seconds.");
    }
    public void StartOption1Dialogue2()
    {
        SetDialogueState(option1Dialogue2, new Button[] { option1Continue2 }, " During these phases, the player gathers points to gain an advantage in the fight.");
    }
    public void StartOption1Dialogue3()
    {
        SetDialogueState(option1Dialogue3, new Button[] { option1Continue3 }, "In the Attack Phase, the points collected determine the damage dealt to the enemy.");
    }
    public void StartOption1Dialogue4()
    {
        SetDialogueState(option1Dialogue4, new Button[] { option1Continue4 }, "In the Defend Phase, points are used to reduce incoming enemy damage or, under certain conditions, to counterattack and deal damage to the enemy.");
    }
    public void StartOption2Dialogue1()
    {
        SetDialogueState(option2Dialogue1, new Button[] { option2Continue1 }, "A young adventurer skilled in using the power of numbers to overcome challenges and defeat enemies.");
    }
    public void StartOption2Dialogue2()
    {
        SetDialogueState(option2Dialogue2, new Button[] { option2Continue2 }, "Hailing from a mysterious place called 'NASTECH', they eventually returned there, marking the end of their journey.");
    }
    public void ConvoEnd()
    {
        NoticeBoardPanel.SetActive(false);
        arrowBullitin.triggerBox.gameObject.SetActive(false);
        arrowBullitin.playerArrow.gameObject.SetActive(false);
        arrowNPC.triggerBox.gameObject.SetActive(true);
        arrowNPC.playerArrow.gameObject.SetActive(true);

        TMP_Text[] dialogues = { startDialogue, option1Dialogue1, option1Dialogue2, option1Dialogue3, option1Dialogue4, option2Dialogue1, option2Dialogue2};

        Button[] allButtons = { option1, option2, option3, option1Continue1, option1Continue2, option1Continue3, option1Continue4, option2Continue1, option2Continue2};

        foreach (TMP_Text dialogue in dialogues)
        {
            dialogue.gameObject.SetActive(false);
        }
        foreach (Button button in allButtons)
        {
            button.gameObject.SetActive(false);
        }
        if (triggerBox != null)
        {
            triggerBox.SetActive(true); 
        }
        if (npcTriggerBox != null)
        {
            npcTriggerBox.EnableTriggerBox();
        }
        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }
}