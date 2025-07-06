using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Database;
public class UnlockStage17Dialogue : MonoBehaviour
{
    public Button skipButton;
    public AudioManager audioManager;
    public int customXRotation = 10;
    private FirebaseManager firebaseManager;
    public Stage16GameTrialHandler stage16GameTrialHandler;
    public Animator npcAnimator;
    public Image playerDisplayImage; 
    public Image npcDisplayImage;  
    public Sprite boyImage; 
    public Sprite girlImage; 
    public GameObject NPCNamePanel;
    public GameObject PlayerNamePanel;
    public GameObject NPC, ConvoPanel;
    public TMP_Text NPCName, PlayerName;
    public Camera PlayerCamera, TalkCamera;
    private GameObject player; 
    private PlayerControl playerController;
    private MainCharacterLoader characterLoader;
    private bool isStage16TaskComplete = false;
    [Header("Start Dialogue")]
    public TMP_Text startDialogue;
    public Button Choice1, Choice2, Choice3;
    [Header("Dialogue1Choice1")]
    public TMP_Text Dialogue1Choice1;
    public Button Continue1Choice1;
    [Header("Dialogue2Choice1")]
    public TMP_Text Dialogue2Choice1;
    public Button Continue2Choice1;
    [Header("Dialogue3Choice1")]
    public TMP_Text Dialogue3Choice1;
    public Button Continue3Choice1;
    [Header("Dialogue4Choice1")]
    public TMP_Text Dialogue4Choice1;
    public Button Continue4Choice1;
    [Header("Dialogue5Choice1")]
    public TMP_Text Dialogue5Choice1;
    public Button Continue5Choice1;
    [Header("Dialogue6Choice1")]
    public TMP_Text Dialogue6Choice1;
    public Button Continue6Choice1;
    [Header("Dialogue7Choice1")]
    public TMP_Text Dialogue7Choice1;
    public Button Continue7Choice1;
    [Header("Dialogue8Choice1")]
    public TMP_Text Dialogue8Choice1;
    public Button Continue8Choice1;
    [Header("Dialogue9Choice1")]
    public TMP_Text Dialogue9Choice1;
    public Button Continue9Choice1;
    [Header("Dialogue10Choice1")]
    public TMP_Text Dialogue10Choice1;
    public Button Continue10Choice1;
    [Header("Dialogue11Choice1")]
    public TMP_Text Dialogue11Choice1;
    public Button Continue11Choice1;
    [Header("Dialogue12Choice1")]
    public TMP_Text Dialogue12Choice1; 
    public Button EndChoice1;
    [Header("Dialogue1Choice2")]
    public TMP_Text Dialogue1Choice2;
    public Button Continue1Choice2;
    [Header("Dialogue2Choice2")]
    public TMP_Text Dialogue2Choice2;
    public Button Continue2Choice2;
    [Header("Dialogue3Choice2")]
    public TMP_Text Dialogue3Choice2;
    public Button Continue3Choice2;
    [Header("Dialogue4Choice2")]
    public TMP_Text Dialogue4Choice2;
    public Button Continue4Choice2;
    [Header("Dialogue5Choice2")]
    public TMP_Text Dialogue5Choice2;
    public Button Continue5Choice2;
    [Header("Dialogue6Choice2")]
    public TMP_Text Dialogue6Choice2;
    public Button Continue6Choice2;
    [Header("Dialogue7Choice2")]
    public TMP_Text Dialogue7Choice2;
    public Button Continue7Choice2;
    [Header("Dialogue8Choice2")]
    public TMP_Text Dialogue8Choice2;
    public Button Continue8Choice2;
    [Header("Dialogue9Choice2")]
    public TMP_Text Dialogue9Choice2;
    public Button Continue9Choice2;
    [Header("Dialogue10Choice2")]
    public TMP_Text Dialogue10Choice2;
    public Button Continue10Choice2;
    [Header("Dialogue11Choice2")]
    public TMP_Text Dialogue11Choice2;
    public Button Continue11Choice2;
    [Header("Dialogue12Choice2")]
    public TMP_Text Dialogue12Choice2;
    public Button Continue12Choice2;
    [Header("Dialogue13Choice2")]
    public TMP_Text Dialogue13Choice2;
    public Button Continue13Choice2;
    [Header("Dialogue14Choice2")]
    public TMP_Text Dialogue14Choice2;
    public Button Continue14Choice2;
    [Header("Dialogue15Choice2")]
    public TMP_Text Dialogue15Choice2;
    public Button Continue15Choice2;
    [Header("Dialogue16Choice2")]
    public TMP_Text Dialogue16Choice2;
    public Button Continue16Choice2;
    [Header("Dialogue17Choice2")]
    public TMP_Text Dialogue17Choice2;
    public Button Continue17Choice2;
    [Header("Dialogue18Choice2")]
    public TMP_Text Dialogue18Choice2;
    public Button Continue18Choice2;
    [Header("Dialogue19Choice2")]
    public TMP_Text Dialogue19Choice2;
    public Button Continue19Choice2;
    [Header("Dialogue20Choice2")]
    public TMP_Text Dialogue20Choice2;
    public Button Continue20Choice2;
    [Header("Dialogue21Choice2")]
    public TMP_Text Dialogue21Choice2;
    public Button EndChoice2;
    [Header("Dialogue1Choice3")]
    public TMP_Text Dialogue1Choice3;
    public Button Continue1Choice3;
    [Header("Dialogue2Choice2")]
    public TMP_Text Dialogue2Choice3;
    public Button Continue2Choice3;
    [Header("Dialogue3Choice3")]
    public TMP_Text Dialogue3Choice3;
    public Button Continue3Choice3;
    [Header("Dialogue4Choice3")]
    public TMP_Text Dialogue4Choice3;
    public Button Continue4Choice3;
    [Header("Dialogue5Choice3")]
    public TMP_Text Dialogue5Choice3;
    public Button Continue5Choice3;
    [Header("Dialogue6Choice3")]
    public TMP_Text Dialogue6Choice3;
    public Button Continue6Choice3;
    [Header("Dialogue7Choice3")]
    public TMP_Text Dialogue7Choice3;
    public Button Continue7Choice3;
    [Header("Dialogue8Choice3")]
    public TMP_Text Dialogue8Choice3;
    public Button Continue8Choice3;
    [Header("Dialogue9Choice3")]
    public TMP_Text Dialogue9Choice3;
    public Button Continue9Choice3;
    [Header("Dialogue10Choice3")]
    public TMP_Text Dialogue10Choice3;
    public Button Continue10Choice3;
    [Header("Dialogue11Choice3")]
    public TMP_Text Dialogue11Choice3;
    public Button Continue11Choice3;
    [Header("Dialogue12Choice3")]
    public TMP_Text Dialogue12Choice3;
    public Button Continue12Choice3;
    [Header("Dialogue13Choice3")]
    public TMP_Text Dialogue13Choice3;
    public Button Continue13Choice3;
    [Header("Dialogue14Choice3")]
    public TMP_Text Dialogue14Choice3;
    public Button Continue14Choice3;
    [Header("Dialogue15Choice3")]
    public TMP_Text Dialogue15Choice3;
    public Button Continue15Choice3;
    [Header("Dialogue16Choice3")]
    public TMP_Text Dialogue16Choice3;
    public Button Continue16Choice3;
    [Header("Dialogue17Choice3")]
    public TMP_Text Dialogue17Choice3;
    public Button Continue17Choice3;
    [Header("Dialogue18Choice3")]
    public TMP_Text Dialogue18Choice3;
    public Button Continue18Choice3;
    [Header("Dialogue19Choice3")]
    public TMP_Text Dialogue19Choice3;
    public Button Continue19Choice3;
    [Header("Dialogue20Choice3")]
    public TMP_Text Dialogue20Choice3;
    public Button Continue20Choice3;
    [Header("Dialogue21Choice3")]
    public TMP_Text Dialogue21Choice3;
    public Button Continue21Choice3;
    [Header("Dialogue22Choice3")]
    public TMP_Text Dialogue22Choice3;
    public Button Continue22Choice3;
    [Header("Dialogue23Choice3")]
    public TMP_Text Dialogue23Choice3;
    public Button Continue23Choice3;
        [Header("DialogueQuestion1Choice3")]
        public TMP_Text DialogueQuestion1Choice3;
        public Button Answer1Question1, Answer2Question1, Answer3Question1;
        [Header("Dialogue1 Answer1 Question1 Choice3")]
        public TMP_Text Dialogue1Answer1Question1Choice3;
        public Button Continue1Answer1Question1Choice3;
        [Header("Dialogue2 Answer1 Question1 Choice3")]
        public TMP_Text Dialogue2Answer1Question1Choice3;
        public Button Continue2Answer1Question1Choice3;
        [Header("Dialogue1 Answer2 Question1 Choice3")]
        public TMP_Text Dialogue1Answer2Question1Choice3;
        public Button Continue1Answer2Question1Choice3;
        [Header("Dialogue2 Answer2 Question1 Choice3")]
        public TMP_Text Dialogue2Answer2Question1Choice3;
        public Button Continue2Answer2Question1Choice3;
        [Header("Dialogue3 Answer2 Question1 Choice3")]
        public TMP_Text Dialogue3Answer2Question1Choice3;
        public Button Continue3Answer2Question1Choice3;
        [Header("Dialogue4 Answer2 Question1 Choice3")]
        public TMP_Text Dialogue4Answer2Question1Choice3;
        public Button Continue4Answer2Question1Choice3;
        [Header("Dialogue1 Answer3 Question1 Choice3")]
        public TMP_Text Dialogue1Answer3Question1Choice3;
        public Button Continue1Answer3Question1Choice3;
        [Header("Dialogue2 Answer3 Question1 Choice3")]
        public TMP_Text Dialogue2Answer3Question1Choice3;
        public Button Continue2Answer3Question1Choice3;
        [Header("Dialogue3 Answer3 Question1 Choice3")]
        public TMP_Text Dialogue3Answer3Question1Choice3;
        public Button Continue3Answer3Question1Choice3;
        [Header("Dialogue4 Answer3 Question1 Choice3")]
        public TMP_Text Dialogue4Answer3Question1Choice3;
        public Button Continue4Answer3Question1Choice3;
    [Header("Dialogue24Choice3")]
    public TMP_Text Dialogue24Choice3;
    public Button Continue24Choice3;
    [Header("Dialogue25Choice3")]
    public TMP_Text Dialogue25Choice3;
    public Button Continue25Choice3;
    [Header("Dialogue26Choice3")]
    public TMP_Text Dialogue26Choice3;
    public Button EndChoice3;
    void Start()
    {
        StartCoroutine(InitializePlayer());
        characterLoader = FindObjectOfType<MainCharacterLoader>();
        firebaseManager = FindObjectOfType<FirebaseManager>();
        if(characterLoader != null)
        {
            UpdatePlayerImage();
        }
        CheckStage16TaskStatus();
    }
    private void CheckStage16TaskStatus()
    {
        string userId = firebaseManager.GetUserId();
        DatabaseReference stage16TaskRef = firebaseManager.GetDatabaseReference($"users/{userId}/stages/stage16/stage16Task");
        stage16TaskRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                isStage16TaskComplete = snapshot.Exists && (bool)snapshot.Value;
            }
        });
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
    private IEnumerator InitializePlayer()
    {
        characterLoader = FindObjectOfType<MainCharacterLoader>();
        while (characterLoader == null)
        {
            yield return null; 
            characterLoader= FindObjectOfType<MainCharacterLoader>();
        }
        while (characterLoader.GetInstantiatedPlayer() == null)
        {
            yield return null;
        }
        player = characterLoader.GetInstantiatedPlayer();
        playerController = player.GetComponent<PlayerControl>();
        TalkCamera.gameObject.SetActive(false);
    }
    private IEnumerator SwitchToTalkCamera()
    {
        Vector3 midpoint = (player.transform.position + NPC.transform.position) / 2f;
        TalkCamera.transform.position = midpoint + new Vector3(-8, 3, -6); 
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
    private void SetDialogueState(bool npcActive, bool playerActive, TMP_Text activeDialogue, Button[] activeButtons, string dialogueText)
    {
        ConvoPanel.SetActive(true);
        NPCNamePanel.SetActive(npcActive);
        PlayerNamePanel.SetActive(playerActive);
        NPCName.gameObject.SetActive(npcActive);
        PlayerName.gameObject.SetActive(playerActive);
        TMP_Text[] dialogues = { startDialogue, Dialogue1Choice1, Dialogue2Choice1, Dialogue3Choice1, Dialogue4Choice1, Dialogue5Choice1, Dialogue6Choice1, Dialogue7Choice1, Dialogue8Choice1,Dialogue9Choice1, Dialogue10Choice1, Dialogue11Choice1,Dialogue12Choice1, Dialogue1Choice2, Dialogue2Choice2, Dialogue3Choice2, Dialogue4Choice2,Dialogue5Choice2, Dialogue6Choice2, Dialogue7Choice2, Dialogue8Choice2, Dialogue9Choice2, Dialogue10Choice2,Dialogue11Choice2, Dialogue12Choice2, Dialogue13Choice2, Dialogue14Choice2, Dialogue15Choice2, Dialogue16Choice2,Dialogue17Choice2, Dialogue18Choice2, Dialogue19Choice2, Dialogue20Choice2, Dialogue21Choice2, Dialogue1Choice3, Dialogue2Choice3, Dialogue3Choice3, Dialogue4Choice3, Dialogue5Choice3, Dialogue6Choice3, Dialogue7Choice3, Dialogue8Choice3, Dialogue9Choice3, Dialogue10Choice3, Dialogue11Choice3, Dialogue12Choice3, Dialogue13Choice3, Dialogue14Choice3, Dialogue15Choice3, Dialogue16Choice3, Dialogue17Choice3, Dialogue18Choice3, Dialogue19Choice3, Dialogue20Choice3, Dialogue21Choice3, Dialogue22Choice3, Dialogue23Choice3, Dialogue24Choice3, Dialogue25Choice3, Dialogue26Choice3 };
        Button[] allButtons = { Choice1, Choice2, Choice3, Continue1Choice1, Continue2Choice1, Continue3Choice1, Continue4Choice1, Continue5Choice1, Continue6Choice1, Continue7Choice1, Continue8Choice1, Continue9Choice1, Continue10Choice1, Continue11Choice1, EndChoice1, Continue1Choice2, Continue2Choice2, Continue3Choice2, Continue4Choice2,Continue5Choice2, Continue6Choice2, Continue7Choice2, Continue8Choice2, Continue9Choice2, Continue10Choice2,Continue11Choice2, Continue12Choice2, Continue13Choice2, Continue14Choice2, Continue15Choice2,Continue16Choice2,Continue17Choice2, Continue18Choice2, Continue19Choice2, Continue20Choice2, EndChoice2, Continue1Choice3, Continue2Choice3, Continue3Choice3, Continue4Choice3, Continue5Choice3, Continue6Choice3, Continue7Choice3, Continue8Choice3, Continue9Choice3 ,Continue10Choice3 ,Continue11Choice3 ,Continue12Choice3, Continue13Choice3, Continue14Choice3, Continue15Choice3, Continue16Choice3, Continue17Choice3, Continue18Choice3, Continue19Choice3, Continue20Choice3, Continue21Choice3, Continue22Choice3, Continue23Choice3, Continue24Choice3, Continue25Choice3, EndChoice3 };
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
                if (button == Choice3 && isStage16TaskComplete)
                {
                    button.gameObject.SetActive(false); 
                }
                else
                {
                    button.gameObject.SetActive(true);
                }
            }
        });
    }
    private IEnumerator SmoothLookAt(Transform source, Transform target, float duration)
    {
        Quaternion startRotation = source.rotation;
        Quaternion endRotation = Quaternion.LookRotation(target.position - source.position);
        float elapsed = 0f;
        while (elapsed < duration)
        {
            source.rotation = Quaternion.Slerp(startRotation, endRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        source.rotation = endRotation;
    }
    public void ConvoStart()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[0]);
        CheckStage16TaskStatus(); 
        DisplayPlayerName();
        SetDialogueState(true, false, startDialogue, new Button[] { Choice1, Choice2, Choice3 }, "Hello, What can I do for you?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
        StartCoroutine(SwitchToTalkCamera());
        StartCoroutine(SmoothLookAt(NPC.transform, player.transform, 0.5f));
        StartCoroutine(SmoothLookAt(player.transform, NPC.transform, 0.5f));
        if (playerController != null)
        {
            playerController.enabled = false;
        }
    }
    public void StartDialogue1Choice1()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Choice1, new Button[] { Continue1Choice1 }, "I'm just looking to learn something new. Got any fun facts to share?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Choice1()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[1]);
        SetDialogueState(true, false, Dialogue2Choice1, new Button[] { Continue2Choice1 }, "Fun facts? Sure! Let's talk about shapes, do you know how many sides a square has?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue3Choice1()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue3Choice1, new Button[] { Continue3Choice1 }, "Hmm, I think it has 4 sides!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue4Choice1()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[2]);
        SetDialogueState(true, false, Dialogue4Choice1, new Button[] { Continue4Choice1 }, "Exactly! A square has 4 sides, and all the sides are the same length. It's like a box with equal sides all around.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue5Choice1()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue5Choice1, new Button[] { Continue5Choice1 }, "Oh, cool! What about a triangle?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue6Choice1()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[3]);
        SetDialogueState(true, false, Dialogue6Choice1, new Button[] { Continue6Choice1 }, "Great question! A triangle has 3 sides. It can look different, like a pointy mountain or a flat-bottomed shape, but it always has 3 sides.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue7Choice1()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue7Choice1, new Button[] { Continue7Choice1 }, "So, squares have 4 sides, and triangles have 3 sides. What about a circle?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue8Choice1()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[4]);
        SetDialogueState(true, false, Dialogue8Choice1, new Button[] { Continue8Choice1 }, "Good thinking! A circle is unique. It doesn't have sides; it's just one continuous round shape, like a wheel or a clock!");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue9Choice1()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue9Choice1, new Button[] { Continue9Choice1 }, "I get it! So squares, triangles, and circles are the basics?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue10Choice1()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[5]);
        SetDialogueState(true, false, Dialogue10Choice1, new Button[] { Continue10Choice1 }, "That's right! These are the most common shapes you'll spot, but there are many more out there to explore.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue11Choice1()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue11Choice1, new Button[] { Continue11Choice1 }, "I'll keep an eye out for shapes around me! Thanks!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue12Choice1()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[6]);
        SetDialogueState(true, false, Dialogue12Choice1, new Button[] { EndChoice1 }, "You're welcome! Shapes are everywhere, have fun spotting them!");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue1Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Choice2, new Button[] { Continue1Choice2}, "I want to sharpen my math skills. Can you help me practice?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[7]);
        SetDialogueState(true, false, Dialogue2Choice2, new Button[] { Continue2Choice2 }, "Of course! Let's start with solving problems using the order of operations.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue3Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue3Choice2, new Button[] { Continue3Choice2}, "Order of operations? What's that?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue4Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[8]);
        SetDialogueState(true, false, Dialogue4Choice2, new Button[] { Continue4Choice2 }, "It's like following a recipe in the right order. When solving a problem with multiple operations, we use the PEMDAS rule.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue5Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[9]);
        SetDialogueState(true, false, Dialogue5Choice2, new Button[] { Continue5Choice2}, "It stands for Parenthesis, Exponent, Multiplication, Division, Addition, and Subtraction, but don't worry, we're just focusing on the four basic ones: multiplication, division, addition, and subtraction.");
    }
    public void StartDialogue6Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue6Choice2, new Button[] { Continue6Choice2}, "Oh, so I solve multiplication first, then division, then addition, and finally subtraction?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue7Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[10]);
        SetDialogueState(true, false, Dialogue7Choice2, new Button[] { Continue7Choice2 }, "Close! You solve multiplication and division as you find them, moving left to right. Then do addition and subtraction, also from left to right.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue8Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue8Choice2, new Button[] { Continue8Choice2}, "Got it! Can we try one?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue9Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[11]);
        SetDialogueState(true, false, Dialogue9Choice2, new Button[] { Continue9Choice2 }, "Sure! Here's a problem: 8 + 6 x 2 - 4. Which operation do you do first?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue10Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue10Choice2, new Button[] { Continue10Choice2}, "Multiplication comes first, right? So, 6 x 2 equals 12.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue11Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[12]);
        SetDialogueState(true, false, Dialogue11Choice2, new Button[] { Continue11Choice2 }, "Exactly! Now replace 6 x 2 with 12. What does the problem look like now?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue12Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue12Choice2, new Button[] { Continue12Choice2}, "It's 8 + 12 - 4.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue13Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[13]);
        SetDialogueState(true, false, Dialogue13Choice2, new Button[] { Continue13Choice2 }, "Great! What's next?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue14Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue14Choice2, new Button[] { Continue14Choice2}, "Addition. 8 + 12 equals 20.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue15Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[14]);
        SetDialogueState(true, false, Dialogue15Choice2, new Button[] { Continue15Choice2 }, "Awesome! What's the final step?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue16Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue16Choice2, new Button[] { Continue16Choice2}, "Subtract 4 from 20. That's 16!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue17Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[15]);
        SetDialogueState(true, false, Dialogue17Choice2, new Button[] { Continue17Choice2 }, "Perfect! The answer is 16. You're doing great!");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue18Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue18Choice2, new Button[] { Continue18Choice2}, "That was fun!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue19Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[16]);
        SetDialogueState(true, false, Dialogue19Choice2, new Button[] { Continue19Choice2 }, "I'm glad you enjoyed it! Here's a tip: when solving these problems, take it one step at a time. Write down each step so you don't miss anything, and double-check your work to build your confidence.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue20Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue20Choice2, new Button[] { Continue20Choice2}, "That's a good idea!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue21Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[17]);
        SetDialogueState(true, false, Dialogue21Choice2, new Button[] { EndChoice2 }, "It sure is! And remember, math is like a puzzle—each piece fits together to give you the big picture. Keep practicing, and you'll be solving even trickier problems with ease. You've got this!");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue1Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Choice3, new Button[] { Continue1Choice3 }, "I want to learn more about solving math problems with different operations, like multiplication, division, addition, and subtraction.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[18]);
        SetDialogueState(true, false, Dialogue2Choice3, new Button[] { Continue2Choice3 }, "Ah, a great choice! Have you heard of the PEMDAS rule? It's a simple way to solve problems that have all these operations together.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue3Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue3Choice3, new Button[] { Continue3Choice3 }, "I've heard of it, but I don't really understand how it works.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue4Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[19]);
        SetDialogueState(true, false, Dialogue4Choice3, new Button[] { Continue4Choice3 }, "No worries! Let me explain. PEMDAS tells us the order in which to solve problems. For now, let's focus on the important part.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue5Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[20]);
        SetDialogueState(true, false, Dialogue5Choice3, new Button[] { Continue5Choice3 }, "Multiplication and Division come first (from left to right) and then Addition and Subtraction come next (also from left to right).");
    }
    public void StartDialogue6Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue6Choice3, new Button[] { Continue6Choice3 }, "So, I solve multiplication and division first, then addition and subtraction?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue7Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[21]);
        SetDialogueState(true, false, Dialogue7Choice3, new Button[] { Continue7Choice3 }, "Exactly! Let's try an example together. Solve this step by step: 6 + 4 x 2");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue8Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue8Choice3, new Button[] { Continue8Choice3 }, "Hmm... Should I do 6 + 4 first?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue9Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[22]);
        SetDialogueState(true, false, Dialogue9Choice3, new Button[] { Continue9Choice3 }, "Not quite. Remember, multiplication comes before addition. So first, solve 4 x 2. What's that?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue10Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue10Choice3, new Button[] { Continue10Choice3 }, "That's 8!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue11Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[23]);
        SetDialogueState(true, false, Dialogue11Choice3, new Button[] { Continue11Choice3 }, "Great! Now replace 4 x 2 with 8, so the problem becomes: 6 + 8. What's the answer?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue12Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue12Choice3, new Button[] { Continue12Choice3 }, "That's 14!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue13Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[24]);
        SetDialogueState(true, false, Dialogue13Choice3, new Button[] { Continue13Choice3 }, "Perfect! You just used PEMDAS! Want to try another one?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue14Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue14Choice3, new Button[] { Continue14Choice3 }, "Yes, please.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue15Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[25]);
        SetDialogueState(true, false, Dialogue15Choice3, new Button[] { Continue15Choice3 }, "Here's the problem: 12 ÷ 3 x 2 + 5");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue16Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue16Choice3, new Button[] { Continue16Choice3 }, "Okay... What do I solve first?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue17Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[26]);
        SetDialogueState(true, false, Dialogue17Choice3, new Button[] { Continue17Choice3 }, "Start with multiplication or division, whichever comes first from left to right. So, solve 12 ÷ 3 first. What's that?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue18Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue18Choice3, new Button[] { Continue18Choice3 }, "That's 4!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue19Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[27]);
        SetDialogueState(true, false, Dialogue19Choice3, new Button[] { Continue19Choice3 }, "Good! Now the problem becomes: 4 x 2 + 5. What's next?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue20Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue20Choice3, new Button[] { Continue20Choice3 }, "I do 4 x 2, which is 8.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue21Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[28]);
        SetDialogueState(true, false, Dialogue21Choice3, new Button[] { Continue21Choice3 }, "Right again! Now it's just 8 + 5. What's the answer?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue22Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue22Choice3, new Button[] { Continue22Choice3 }, "That's 13!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue23Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[29]);
        SetDialogueState(true, false, Dialogue23Choice3, new Button[] { Continue23Choice3 }, "Excellent work! Now let's try a test problem.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogueQuestion1Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[30]);
        SetDialogueState(true, false, DialogueQuestion1Choice3, new Button[] { Answer1Question1, Answer2Question1, Answer3Question1 }, "Solve this: 10 - 3 x 2 + 8 ÷ 4");
    }
    public void StartDialogue1Answer1Question1Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Answer1Question1Choice3, new Button[] { Continue1Answer1Question1Choice3 }, "I think it's 6.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Answer1Question1Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[31]);
        SetDialogueState(true, false, Dialogue2Answer1Question1Choice3, new Button[] { Continue2Answer1Question1Choice3 }, "That's absolutely right! Let's break it down: First, do 3 x 2, which is 6. Then do 8 ÷ 4, which is 2. Now the problem becomes: 10 - 6 + 2. Solve from left to right: 10 - 6 = 4, then 4 + 2 = 6. Great job!");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue1Answer2Question1Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Answer2Question1Choice3, new Button[] { Continue1Answer2Question1Choice3 }, "Is it 10?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Answer2Question1Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[32]);
        SetDialogueState(true, false, Dialogue2Answer2Question1Choice3, new Button[] { Continue2Answer2Question1Choice3 }, "Not quite, but let's break it down. Start with 3 x 2, which is 6, and 8 ÷ 4, which is 2. That makes the problem: 10 - 6 + 2. What does that equal?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue3Answer2Question1Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue3Answer2Question1Choice3, new Button[] { Continue3Answer2Question1Choice3 }, "Oh, it's 6!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue4Answer2Question1Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[33]);
        SetDialogueState(true, false, Dialogue4Answer2Question1Choice3, new Button[] { Continue4Answer2Question1Choice3 }, "Exactly! Just remember to follow the PEMDAS rule step by step.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue1Answer3Question1Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Answer3Question1Choice3, new Button[] { Continue1Answer3Question1Choice3 }, "Is it 4?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Answer3Question1Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[34]);
        SetDialogueState(true, false, Dialogue2Answer3Question1Choice3, new Button[] { Continue2Answer3Question1Choice3 }, "Almost! Let's review: First, solve 3 x 2 (6) and 8 ÷ 4 (2). That makes the problem: 10 - 6 + 2. Solve it step by step: 10 - 6 = 4, then 4 + 2 = 6.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue3Answer3Question1Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue3Answer3Question1Choice3, new Button[] { Continue3Answer3Question1Choice3 }, "Got it now! The answer is 6.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue4Answer3Question1Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[35]);
        SetDialogueState(true, false, Dialogue4Answer3Question1Choice3, new Button[] { Continue4Answer3Question1Choice3 }, "Great! Mistakes help us learn!");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue24Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[36]);
        SetDialogueState(true, false, Dialogue24Choice3, new Button[] { Continue24Choice3 }, "Now that you understand PEMDAS, here's one last tip, always take your time, and solve step by step. Following the rules will make everything much easier!");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue25Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue25Choice3, new Button[] { Continue25Choice3 }, "Thanks! I feel more confident about solving problems now.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue26Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[37]);
        SetDialogueState(true, false, Dialogue26Choice3, new Button[] { EndChoice3 }, "You're welcome! you've got this!");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void ConvoEnd()
    {
        audioManager.StopCurrentSFX();
        ConvoPanel.SetActive(false);
        NPCName.gameObject.SetActive(false);
        PlayerName.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        TMP_Text[] dialogues = { startDialogue, Dialogue1Choice1, Dialogue2Choice1, Dialogue3Choice1, Dialogue4Choice1, Dialogue5Choice1, Dialogue6Choice1, Dialogue7Choice1, Dialogue8Choice1,Dialogue9Choice1, Dialogue10Choice1, Dialogue11Choice1,Dialogue12Choice1, Dialogue1Choice2, Dialogue2Choice2, Dialogue3Choice2, Dialogue4Choice2,Dialogue5Choice2, Dialogue6Choice2, Dialogue7Choice2, Dialogue8Choice2, Dialogue9Choice2, Dialogue10Choice2,Dialogue11Choice2, Dialogue12Choice2, Dialogue13Choice2, Dialogue14Choice2, Dialogue15Choice2, Dialogue16Choice2,Dialogue17Choice2, Dialogue18Choice2, Dialogue19Choice2, Dialogue20Choice2, Dialogue21Choice2, Dialogue1Choice3, Dialogue2Choice3, Dialogue3Choice3, Dialogue4Choice3, Dialogue5Choice3, Dialogue6Choice3, Dialogue7Choice3, Dialogue8Choice3, Dialogue9Choice3, Dialogue10Choice3, Dialogue11Choice3, Dialogue12Choice3, Dialogue13Choice3, Dialogue14Choice3, Dialogue15Choice3, Dialogue16Choice3, Dialogue17Choice3, Dialogue18Choice3, Dialogue19Choice3, Dialogue20Choice3, Dialogue21Choice3, Dialogue22Choice3, Dialogue23Choice3, Dialogue24Choice3, Dialogue25Choice3, Dialogue26Choice3 };
        Button[] allButtons = { Choice1, Choice2, Choice3, Continue1Choice1, Continue2Choice1, Continue3Choice1, Continue4Choice1, Continue5Choice1, Continue6Choice1, Continue7Choice1, Continue8Choice1, Continue9Choice1, Continue10Choice1, Continue11Choice1, EndChoice1, Continue1Choice2, Continue2Choice2, Continue3Choice2, Continue4Choice2,Continue5Choice2, Continue6Choice2, Continue7Choice2, Continue8Choice2, Continue9Choice2, Continue10Choice2,Continue11Choice2, Continue12Choice2, Continue13Choice2, Continue14Choice2, Continue15Choice2,Continue16Choice2,Continue17Choice2, Continue18Choice2, Continue19Choice2, Continue20Choice2, EndChoice2, Continue1Choice3, Continue2Choice3, Continue3Choice3, Continue4Choice3, Continue5Choice3, Continue6Choice3, Continue7Choice3, Continue8Choice3, Continue9Choice3 ,Continue10Choice3 ,Continue11Choice3 ,Continue12Choice3, Continue13Choice3, Continue14Choice3, Continue15Choice3, Continue16Choice3, Continue17Choice3, Continue18Choice3, Continue19Choice3, Continue20Choice3, Continue21Choice3, Continue22Choice3, Continue23Choice3, Continue24Choice3, Continue25Choice3, EndChoice3 };
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
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartTrialGame()
    {
        ConvoEnd();
        stage16GameTrialHandler.OpenStage16Trial();
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
                    string username = childSnapshot.Key;
                    string storedUserId = childSnapshot.Value.ToString(); 
                    if (storedUserId == userId)
                    {
                        PlayerName.text = username; 
                        userFound = true;
                        break;
                    }
                }
                if (!userFound)
                {
                    PlayerName.text = "Player"; 
                }
            }
            else
            {
                PlayerName.text = "Error"; 
            }
        });
    }
    public void activateSkipButton()
    {
        skipButton.gameObject.SetActive(true);
    }
}