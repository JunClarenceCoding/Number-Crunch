using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Database;

public class UnlockStage13Dialogue : MonoBehaviour
{
    public Button skipButton;
    public AudioManager audioManager;
    public int customXRotation = 10;
    private FirebaseManager firebaseManager;
    public Stage12GameTrialHandler stage12GameTrialHandler;
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
    private bool isStage12TaskComplete = false;
    [Header("Start Dialogue")]
    public TMP_Text startDialogue;
    public Button Choice1, Choice2, Choice3;
    //For choice 1 only
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
    public Button Continue12Choice1;
    [Header("Dialogue13Choice1")]
    public TMP_Text Dialogue13Choice1;
    public Button EndChoice1;
     //For choice 2 only
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
    //For choice 3 only
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
    // choice 3 stop here
        //choice 3 Question 1  start here
        [Header("DialogueQuestion1Choice3")]
        public TMP_Text DialogueQuestion1Choice3;
        public Button Answer1Question1, Answer2Question1, Answer3Question1;
        //answer 1 starts here
        [Header("Dialogue1 Answer1 Question1 Choice3")]
        public TMP_Text Dialogue1Answer1Question1Choice3;
        public Button Continue1Answer1Question1Choice3;
        [Header("Dialogue2 Answer1 Question1 Choice3")]
        public TMP_Text Dialogue2Answer1Question1Choice3;
        public Button Continue2Answer1Question1Choice3;
        [Header("Dialogue3 Answer1 Question1 Choice3")]
        public TMP_Text Dialogue3Answer1Question1Choice3;
        public Button Continue3Answer1Question1Choice3;
        [Header("Dialogue4 Answer1 Question1 Choice3")]
        public TMP_Text Dialogue4Answer1Question1Choice3;
        public Button Continue4Answer1Question1Choice3;
        //answer 2 starts here
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
        //answer 3 starts here
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
        //choice 3 Question 2  start here
        [Header("DialogueQuestion2Choice3")]
        public TMP_Text DialogueQuestion2Choice3;
        public Button Answer1Question2, Answer2Question2, Answer3Question2;
        //answer 1 starts here
        [Header("Dialogue1 Answer1 Question2 Choice3")]
        public TMP_Text Dialogue1Answer1Question2Choice3;
        public Button Continue1Answer1Question2Choice3;
        [Header("Dialogue2 Answer1 Question2 Choice3")]
        public TMP_Text Dialogue2Answer1Question2Choice3;
        public Button Continue2Answer1Question2Choice3;
        [Header("Dialogue3 Answer1 Question2 Choice3")]
        public TMP_Text Dialogue3Answer1Question2Choice3;
        public Button Continue3Answer1Question2Choice3;
        [Header("Dialogue4 Answer1 Question2 Choice3")]
        public TMP_Text Dialogue4Answer1Question2Choice3;
        public Button Continue4Answer1Question2Choice3;
        //answer 2 starts here
        [Header("Dialogue1 Answer2 Question2 Choice3")]
        public TMP_Text Dialogue1Answer2Question2Choice3;
        public Button Continue1Answer2Question2Choice3;
        [Header("Dialogue2 Answer2 Question2 Choice3")]
        public TMP_Text Dialogue2Answer2Question2Choice3;
        public Button Continue2Answer2Question2Choice3;
        [Header("Dialogue3 Answer2 Question2 Choice3")]
        public TMP_Text Dialogue3Answer2Question2Choice3;
        public Button Continue3Answer2Question2Choice3;
        [Header("Dialogue4 Answer2 Question2 Choice3")]
        public TMP_Text Dialogue4Answer2Question2Choice3;
        public Button Continue4Answer2Question2Choice3;
        //answer 3 starts here
        [Header("Dialogue1 Answer3 Question2 Choice3")]
        public TMP_Text Dialogue1Answer3Question2Choice3;
        public Button Continue1Answer3Question2Choice3;
        [Header("Dialogue2 Answer3 Question2 Choice3")]
        public TMP_Text Dialogue2Answer3Question2Choice3;
        public Button Continue2Answer3Question2Choice3;
        [Header("Dialogue3 Answer3 Question2 Choice3")]
        public TMP_Text Dialogue3Answer3Question2Choice3;
        public Button Continue3Answer3Question2Choice3;
        [Header("Dialogue4 Answer3 Question2 Choice3")]
        public TMP_Text Dialogue4Answer3Question2Choice3;
        public Button Continue4Answer3Question2Choice3;
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
    [Header("Dialogue24Choice3")]
    public TMP_Text Dialogue24Choice3;
    public Button Continue24Choice3;
    [Header("Dialogue25Choice3")]
    public TMP_Text Dialogue25Choice3;
    public Button Continue25Choice3;
    [Header("Dialogue26Choice3")]
    public TMP_Text Dialogue26Choice3;
    public Button Continue26Choice3;
    [Header("Dialogue27Choice3")]
    public TMP_Text Dialogue27Choice3;
    public Button Continue27Choice3;
    [Header("Dialogue28Choice3")]
    public TMP_Text Dialogue28Choice3;
    public Button Continue28Choice3;
    [Header("Dialogue29Choice3")]
    public TMP_Text Dialogue29Choice3;
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
        CheckStage12TaskStatus();
    }
    private void CheckStage12TaskStatus()
    {
        string userId = firebaseManager.GetUserId();
        DatabaseReference stage4TaskRef = firebaseManager.GetDatabaseReference($"users/{userId}/stages/stage12/stage12Task");
        stage4TaskRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                isStage12TaskComplete = snapshot.Exists && (bool)snapshot.Value;
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
    private void SetDialogueState(bool npcActive, bool playerActive, TMP_Text activeDialogue, Button[] activeButtons, string dialogueText)
    {
        ConvoPanel.SetActive(true);
        NPCNamePanel.SetActive(npcActive);
        PlayerNamePanel.SetActive(playerActive);
        NPCName.gameObject.SetActive(npcActive);
        PlayerName.gameObject.SetActive(playerActive);
        TMP_Text[] dialogues = { startDialogue, Dialogue1Choice1, Dialogue2Choice1, Dialogue3Choice1, Dialogue4Choice1, Dialogue5Choice1, Dialogue6Choice1, Dialogue7Choice1, Dialogue8Choice1,Dialogue9Choice1, Dialogue10Choice1, Dialogue11Choice1,Dialogue12Choice1, Dialogue13Choice1, Dialogue1Choice2, Dialogue2Choice2, Dialogue3Choice2, Dialogue4Choice2,Dialogue5Choice2, Dialogue6Choice2, Dialogue7Choice2, Dialogue8Choice2, Dialogue9Choice2, Dialogue10Choice2,Dialogue11Choice2, Dialogue12Choice2, Dialogue13Choice2, Dialogue14Choice2, Dialogue15Choice2, Dialogue16Choice2,Dialogue17Choice2, Dialogue18Choice2, Dialogue19Choice2, Dialogue20Choice2, Dialogue21Choice2, Dialogue1Choice3, Dialogue2Choice3, Dialogue3Choice3, Dialogue4Choice3, Dialogue5Choice3, Dialogue6Choice3, Dialogue7Choice3, Dialogue8Choice3, DialogueQuestion1Choice3, Dialogue1Answer1Question1Choice3, Dialogue2Answer1Question1Choice3, Dialogue3Answer1Question1Choice3, Dialogue4Answer1Question1Choice3, Dialogue1Answer2Question1Choice3, Dialogue2Answer2Question1Choice3, Dialogue3Answer2Question1Choice3, Dialogue4Answer2Question1Choice3, Dialogue1Answer3Question1Choice3, Dialogue2Answer3Question1Choice3, Dialogue3Answer3Question1Choice3, Dialogue4Answer3Question1Choice3, DialogueQuestion2Choice3, Dialogue1Answer1Question2Choice3, Dialogue2Answer1Question2Choice3, Dialogue3Answer1Question2Choice3, Dialogue4Answer1Question2Choice3, Dialogue1Answer2Question2Choice3, Dialogue2Answer2Question2Choice3, Dialogue3Answer2Question2Choice3, Dialogue4Answer2Question2Choice3, Dialogue1Answer3Question2Choice3, Dialogue2Answer3Question2Choice3, Dialogue3Answer3Question2Choice3, Dialogue4Answer3Question2Choice3, Dialogue9Choice3, Dialogue10Choice3, Dialogue11Choice3, Dialogue12Choice3, Dialogue13Choice3, Dialogue14Choice3, Dialogue15Choice3, Dialogue16Choice3, Dialogue17Choice3, Dialogue18Choice3, Dialogue19Choice3, Dialogue20Choice3, Dialogue21Choice3, Dialogue22Choice3, Dialogue23Choice3, Dialogue24Choice3, Dialogue25Choice3, Dialogue26Choice3, Dialogue27Choice3, Dialogue28Choice3,Dialogue29Choice3 };
        Button[] allButtons = { Choice1, Choice2, Choice3, Continue1Choice1, Continue2Choice1, Continue3Choice1, Continue4Choice1, Continue5Choice1, Continue6Choice1, Continue7Choice1, Continue8Choice1, Continue9Choice1, Continue10Choice1, Continue11Choice1, Continue12Choice1, EndChoice1, Continue1Choice2, Continue2Choice2, Continue3Choice2, Continue4Choice2,Continue5Choice2, Continue6Choice2, Continue7Choice2, Continue8Choice2, Continue9Choice2, Continue10Choice2,Continue11Choice2, Continue12Choice2, Continue13Choice2, Continue14Choice2, Continue15Choice2,Continue16Choice2,Continue17Choice2, Continue18Choice2, Continue19Choice2, Continue20Choice2, EndChoice2, Continue1Choice3, Continue2Choice3, Continue3Choice3, Continue4Choice3, Continue5Choice3, Continue6Choice3, Continue7Choice3, Continue8Choice3, Answer1Question1, Answer2Question1, Answer3Question1, Continue1Answer1Question1Choice3, Continue2Answer1Question1Choice3, Continue3Answer1Question1Choice3, Continue4Answer1Question1Choice3,Continue1Answer2Question1Choice3, Continue2Answer2Question1Choice3, Continue3Answer2Question1Choice3, Continue4Answer2Question1Choice3, Continue1Answer3Question1Choice3, Continue2Answer3Question1Choice3, Continue3Answer3Question1Choice3, Continue4Answer3Question1Choice3, Answer1Question2, Answer2Question2, Answer3Question2, Continue1Answer1Question2Choice3, Continue2Answer1Question2Choice3, Continue3Answer1Question2Choice3, Continue4Answer1Question2Choice3,Continue1Answer2Question2Choice3, Continue2Answer2Question2Choice3, Continue3Answer2Question2Choice3, Continue4Answer2Question2Choice3, Continue1Answer3Question2Choice3, Continue2Answer3Question2Choice3, Continue3Answer3Question2Choice3, Continue4Answer3Question2Choice3, Continue9Choice3 ,Continue10Choice3 ,Continue11Choice3 ,Continue12Choice3, Continue13Choice3, Continue14Choice3, Continue15Choice3, Continue16Choice3, Continue17Choice3, Continue18Choice3, Continue19Choice3, Continue20Choice3, Continue21Choice3, Continue22Choice3, Continue23Choice3,Continue24Choice3, Continue25Choice3, Continue26Choice3, Continue27Choice3, Continue28Choice3, EndChoice3 };
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
                if (button == Choice3 && isStage12TaskComplete)
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
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[0]);
        CheckStage12TaskStatus(); 
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
    //choice1 button = "Just exploring" 
    public void StartDialogue1Choice1()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Choice1, new Button[] { Continue1Choice1 }, "I'm just exploring and learning new things.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Choice1()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[1]);
        SetDialogueState(true, false, Dialogue2Choice1, new Button[] { Continue2Choice1 }, "Speaking of learning, Did you know that math can be found all around us? For example, have you ever wondered why we have numbers like 0?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue3Choice1()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue3Choice1, new Button[] { Continue3Choice1 }, "Hmm, I never really thought about that. Why do we need 0?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue4Choice1()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[2]);
        SetDialogueState(true, false, Dialogue4Choice1, new Button[] { Continue4Choice1 }, "Great question! Zero is a very special number. It's not a number you can count with, but it helps us understand when there’s nothing there.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue5Choice1()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[3]);
        SetDialogueState(true, false, Dialogue5Choice1, new Button[] { Continue5Choice1 }, "Imagine if you had 5 apples, and you ate all of them. How many apples do you have now?");
    }
    public void StartDialogue6Choice1()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue6Choice1, new Button[] { Continue6Choice1 }, "I have none!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue7Choice1()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[4]);
        SetDialogueState(true, false, Dialogue7Choice1, new Button[] { Continue7Choice1 }, "Exactly! That's where zero comes in. It helps us say nothing or none. Without zero, we wouldn't have a way to show that something is completely gone");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue8Choice1()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue8Choice1, new Button[] { Continue8Choice1 }, "Oh, I see! Zero helps us show when there's nothing.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue9Choice1()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[5]);
        SetDialogueState(true, false, Dialogue9Choice1, new Button[] { Continue9Choice1 }, "Yes! And did you know that zero is used in many things, like in a clock or even in computers? Without zero, counting would be a lot harder!");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue10Choice1()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue10Choice1, new Button[] { Continue10Choice1 }, "Wow! Zero is super important, huh?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue11Choice1()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[6]);
        SetDialogueState(true, false, Dialogue11Choice1, new Button[] { Continue11Choice1 }, "It sure is! So next time you see a zero, remember—it helps us count and understand everything around us better!");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue12Choice1()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue12Choice1, new Button[] { Continue12Choice1 }, "Thanks for teaching me about zero!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue13Choice1()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[7]);
        SetDialogueState(true, false, Dialogue13Choice1, new Button[] { EndChoice1 }, "You're welcome! Keep exploring, and you'll find even more cool things about math!");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    //choice2 button = "I need help" 
    public void StartDialogue1Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Choice2, new Button[] { Continue1Choice2}, "I want to get better at division. Can you help?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[8]);
        SetDialogueState(true, false, Dialogue2Choice2, new Button[] { Continue2Choice2 }, "Absolutely! Division can be a lot of fun once you know some simple tricks. Here's a tip to get started, think of division as sharing or grouping.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue3Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue3Choice2, new Button[] { Continue3Choice2}, "Sharing? What do you mean?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue4Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[9]);
        SetDialogueState(true, false, Dialogue4Choice2, new Button[] { Continue4Choice2 }, "Let's say you have 12 candies and 3 friends. If you want to share the candies equally, how many would each friend get?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue5Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue5Choice2, new Button[] { Continue5Choice2}, "Hmm, 12 divided by 3... that's 4, right?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue6Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[10]);
        SetDialogueState(true, false, Dialogue6Choice2, new Button[] { Continue6Choice2 }, "Exactly! You're sharing the candies into 3 groups, so each group gets 4 candies. That's how division works, splitting things evenly.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue7Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue7Choice2, new Button[] { Continue7Choice2}, "Oh, that makes sense! Do you have any other tips?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue8Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[11]);
        SetDialogueState(true, false, Dialogue8Choice2, new Button[] { Continue8Choice2 }, "Sure! Another helpful tip is to use multiplication to check your answers. For example, if you think 12 ÷ 3 = 4, you can multiply 4 by 3 to see if it equals 12.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue9Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue9Choice2, new Button[] { Continue9Choice2}, "Oh! So division and multiplication are connected?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue10Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[12]);
        SetDialogueState(true, false, Dialogue10Choice2, new Button[] { Continue10Choice2 }, "Exactly! They're like best friends, they help each other out. If you ever feel stuck with a division problem, try thinking about the multiplication fact that matches it.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue11Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue11Choice2, new Button[] { Continue11Choice2}, "That's cool! What if the numbers are bigger?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue12Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[13]);
        SetDialogueState(true, false, Dialogue12Choice2, new Button[] { Continue12Choice2 }, "For bigger numbers, you can break the problem into smaller steps. Let's say you're solving 84 ÷ 6.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue13Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[14]);
        SetDialogueState(true, false, Dialogue13Choice2, new Button[] { Continue13Choice2 }, "You might not know the answer right away, but if you know 6 X 10 = 60, you can subtract 60 from 84. What's left?");
    }
    public void StartDialogue14Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue14Choice2, new Button[] { Continue14Choice2}, "Hmm, 84 minus 60 is 24.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue15Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[15]);
        SetDialogueState(true, false, Dialogue15Choice2, new Button[] { Continue15Choice2 }, "Great! Now divide 24 by 6.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue16Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue16Choice2, new Button[] { Continue16Choice2}, "That's 4!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue17Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[16]);
        SetDialogueState(true, false, Dialogue17Choice2, new Button[] { Continue17Choice2 }, "Exactly! So 84 ÷ 6 = 14. You just broke it into two steps to make it easier!");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue18Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue18Choice2, new Button[] { Continue18Choice2}, "That's a smart trick. Thanks for the tips!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue19Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[17]);
        SetDialogueState(true, false, Dialogue19Choice2, new Button[] { Continue19Choice2 }, "You're welcome! Remember, division is all about breaking things into smaller parts. Practice a little every day, and you'll be a division expert in no time!");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue20Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue20Choice2, new Button[] { Continue20Choice2}, "I'll do that. Thanks again!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue21Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[18]);
        SetDialogueState(true, false, Dialogue21Choice2, new Button[] { EndChoice2 }, "Anytime! Keep up the great work!");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
     //choice3 button = "I want to learn" 
    public void StartDialogue1Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Choice3, new Button[] { Continue1Choice3 }, "I want to learn about division.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[19]);
        SetDialogueState(true, false, Dialogue2Choice3, new Button[] { Continue2Choice3 }, "That's great! Division is a really useful skill, and I'm here to help you understand it. Let's get started. Division is like sharing things equally among groups.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue3Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue3Choice3, new Button[] { Continue3Choice3 }, "Sharing? How does that work?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue4Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[20]);
        SetDialogueState(true, false, Dialogue4Choice3, new Button[] { Continue4Choice3 }, "Imagine you have 6 cookies, and you want to share them equally with 2 friends. How many cookies would each friend get?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue5Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue5Choice3, new Button[] { Continue5Choice3 }, "Hmm… 3?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue6Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[21]);
        SetDialogueState(true, false, Dialogue6Choice3, new Button[] { Continue6Choice3 }, "That's exactly right! When you divide 6 by 2, it means you're splitting 6 into 2 equal parts, and each part has 3 cookies.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue7Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue7Choice3, new Button[] { Continue7Choice3 }, "Oh, so division is about splitting into equal groups?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue8Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[22]);
        SetDialogueState(true, false, Dialogue8Choice3, new Button[] { Continue8Choice3 }, "That's right! Division is the opposite of multiplication. Instead of finding the total, you're finding out how many are in each group or how many groups you can make.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    // Start Dialogue from Question1
    public void StartDialogueQuestion1Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[23]);
        SetDialogueState(true, false, DialogueQuestion1Choice3, new Button[] { Answer1Question1, Answer2Question1, Answer3Question1 }, "Let's try a quick example. If you have 12 pencils and want to share them equally among 4 friends, how many pencils does each friend get?");
    }
    // Start Answer 1
    public void StartDialogue1Answer1Question1Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Answer1Question1Choice3, new Button[] { Continue1Answer1Question1Choice3 }, "Is it 2 pencils?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Answer1Question1Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[24]);
        SetDialogueState(true, false, Dialogue2Answer1Question1Choice3, new Button[] { Continue2Answer1Question1Choice3 }, "Not quite! If you divide 12 pencils among 4 friends, you're splitting them into 4 equal groups. Think of it as subtracting 4 repeatedly: 12 - 4 - 4 - 4. Does that leave 2 in each group?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue3Answer1Question1Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue3Answer1Question1Choice3, new Button[] { Continue3Answer1Question1Choice3 }, "Oh, no, that makes 3!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue4Answer1Question1Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[25]);
        SetDialogueState(true, false, Dialogue4Answer1Question1Choice3, new Button[] { Continue4Answer1Question1Choice3 }, "Exactly! Each friend gets 3 pencils. 12 divided by 4 equals 3. Let's continue.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    // Start Answer 2
    public void StartDialogue1Answer2Question1Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Answer2Question1Choice3, new Button[] { Continue1Answer2Question1Choice3 }, "Is it 3 pencils?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Answer2Question1Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[26]);
        SetDialogueState(true, false, Dialogue2Answer2Question1Choice3, new Button[] { Continue2Answer2Question1Choice3 }, "That's correct! 12 divided by 4 equals 3. Each friend gets 3 pencils. Great job!");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue3Answer2Question1Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue3Answer2Question1Choice3, new Button[] { Continue3Answer2Question1Choice3 }, "Nice! I feel like I'm getting it.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue4Answer2Question1Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[27]);
        SetDialogueState(true, false, Dialogue4Answer2Question1Choice3, new Button[] { Continue4Answer2Question1Choice3 }, "I can see that! You're doing great. Let's continue.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    // Start Answer 3
    public void StartDialogue1Answer3Question1Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Answer3Question1Choice3, new Button[] { Continue1Answer3Question1Choice3 }, "Is it 4 pencils?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Answer3Question1Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[28]);
        SetDialogueState(true, false, Dialogue2Answer3Question1Choice3, new Button[] { Continue2Answer3Question1Choice3 }, "Not quite. If you divide 12 pencils among 4 friends, you're splitting them into 4 equal groups. Each group would have 3 pencils, not 4.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue3Answer3Question1Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue3Answer3Question1Choice3, new Button[] { Continue3Answer3Question1Choice3 }, "Oh, I see!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue4Answer3Question1Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[29]);
        SetDialogueState(true, false, Dialogue4Answer3Question1Choice3, new Button[] { Continue4Answer3Question1Choice3 }, "Exactly! Now, let's try something trickier.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    // start question2
    public void StartDialogueQuestion2Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[30]);
        SetDialogueState(true, false, DialogueQuestion2Choice3, new Button[] { Answer1Question2, Answer2Question2, Answer3Question2 }, "If you have 18 pieces of candy and want to split them equally among 6 kids, how many pieces does each child get?");
    }
    // Start Answer 1
    public void StartDialogue1Answer1Question2Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Answer1Question2Choice3, new Button[] { Continue1Answer1Question2Choice3 }, "Is it 2?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Answer1Question2Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[31]);
        SetDialogueState(true, false, Dialogue2Answer1Question2Choice3, new Button[] { Continue2Answer1Question2Choice3 }, "Not quite. Try dividing 18 into 6 equal groups. What's 18 divided by 6?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue3Answer1Question2Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue3Answer1Question2Choice3, new Button[] { Continue3Answer1Question2Choice3 }, "Oh, it's 3!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue4Answer1Question2Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[32]);
        SetDialogueState(true, false, Dialogue4Answer1Question2Choice3, new Button[] { Continue4Answer1Question2Choice3 }, "That's right! Each child gets 3 pieces.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    // Start Answer 2
    public void StartDialogue1Answer2Question2Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Answer2Question2Choice3, new Button[] { Continue1Answer2Question2Choice3 }, "It's 3");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Answer2Question2Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[33]);
        SetDialogueState(true, false, Dialogue2Answer2Question2Choice3, new Button[] { Continue2Answer2Question2Choice3 }, "Correct! 18 divided by 6 equals 3. You're getting the hang of this!");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue3Answer2Question2Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue3Answer2Question2Choice3, new Button[] { Continue3Answer2Question2Choice3 }, "This is starting to make more sense now.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue4Answer2Question2Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[34]);
        SetDialogueState(true, false, Dialogue4Answer2Question2Choice3, new Button[] { Continue4Answer2Question2Choice3 }, "I'm glad to hear that!");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    // Start Answer 3
    public void StartDialogue1Answer3Question2Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Answer3Question2Choice3, new Button[] { Continue1Answer3Question2Choice3 }, "Is it 4?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Answer3Question2Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[35]);
        SetDialogueState(true, false, Dialogue2Answer3Question2Choice3, new Button[] { Continue2Answer3Question2Choice3 }, "Close, but not quite. If you divide 18 into 6 equal groups, there would be 3 pieces in each group, not 4.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue3Answer3Question2Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue3Answer3Question2Choice3, new Button[] { Continue3Answer3Question2Choice3 }, " Got it now, it's 3!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue4Answer3Question2Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[36]);
        SetDialogueState(true, false, Dialogue4Answer3Question2Choice3, new Button[] { Continue4Answer3Question2Choice3 }, "Perfect!");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue9Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[37]);
        SetDialogueState(true, false, Dialogue9Choice3, new Button[] { Continue9Choice3 }, "Now, what if you have to divide more than two numbers, like 12 ÷ 2 ÷ 3?");
    }
    public void StartDialogue10Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue10Choice3, new Button[] { Continue10Choice3 }, "How does that work?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue11Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[38]);
        SetDialogueState(true, false, Dialogue11Choice3, new Button[] { Continue11Choice3 }, "Easy! You solve it step by step, just like multiplication. First, divide 12 by 2. What's 12 ÷ 2?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue12Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue12Choice3, new Button[] { Continue12Choice3 }, "That's 6!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue13Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[39]);
        SetDialogueState(true, false, Dialogue13Choice3, new Button[] { Continue13Choice3 }, "Correct! Now take that 6 and divide it by the next number, which is 3. What's 6 ÷ 3?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue14Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue14Choice3, new Button[] { Continue14Choice3 }, "That2s 2!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue15Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[40]);
        SetDialogueState(true, false, Dialogue15Choice3, new Button[] { Continue15Choice3 }, "Exactly! So, 12 ÷ 2 ÷ 3 equals 2. The trick is to go step by step.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue16Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue16Choice3, new Button[] { Continue16Choice3 }, "Oh, I see! Just divide two numbers at a time.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue17Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[41]);
        SetDialogueState(true, false, Dialogue17Choice3, new Button[] { Continue17Choice3 }, "That's it! Now let's talk about special cases. What happens when you divide a number by 1 or by itself?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue18Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue18Choice3, new Button[] { Continue18Choice3 }, "Hmm… I'm not sure.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue19Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[42]);
        SetDialogueState(true, false, Dialogue19Choice3, new Button[] { Continue19Choice3 }, "If you divide any number by 1, the answer is always the same number. For example, 8 ÷ 1 equals…?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue20Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue20Choice3, new Button[] { Continue20Choice3 }, "8?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue21Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[43]);
        SetDialogueState(true, false, Dialogue21Choice3, new Button[] { Continue21Choice3 }, "Exactly! Dividing by 1 doesn't change the number. It's like saying, “I have one group of 8.”");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue22Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue22Choice3, new Button[] { Continue22Choice3 }, "That's simple!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue23Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[44]);
        SetDialogueState(true, false, Dialogue23Choice3, new Button[] { Continue23Choice3 }, "It is! Now, what if you divide a number by itself, like 8 ÷ 8?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue24Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue24Choice3, new Button[] { Continue24Choice3 }, "Umm… is it 1?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue25Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[45]);
        SetDialogueState(true, false, Dialogue25Choice3, new Button[] { Continue25Choice3 }, "That's correct! Dividing a number by itself always equals 1. It's like saying, “I have 8 cookies, and I'm splitting them into 8 groups.” Each group gets 1 cookie.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue26Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue26Choice3, new Button[] { Continue26Choice3 }, "Oh, that makes sense!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue27Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[46]);
        SetDialogueState(true, false, Dialogue27Choice3, new Button[] { Continue27Choice3 }, "You've got it! Division is all about understanding these patterns. Now, are you ready to try some division challenges in the game?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue28Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue28Choice3, new Button[] { Continue28Choice3 }, "I'm ready! Thanks for teaching me!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue29Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart3Clips[47]);
        SetDialogueState(true, false, Dialogue29Choice3, new Button[] { EndChoice3 }, "You're welcome! You're going to do great. Remember, step by step, and you've got this!");
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
        TMP_Text[] dialogues = { startDialogue, Dialogue1Choice1, Dialogue2Choice1, Dialogue3Choice1, Dialogue4Choice1, Dialogue5Choice1, Dialogue6Choice1, Dialogue7Choice1, Dialogue8Choice1,Dialogue9Choice1, Dialogue10Choice1, Dialogue11Choice1,Dialogue12Choice1, Dialogue13Choice1, Dialogue1Choice2, Dialogue2Choice2, Dialogue3Choice2, Dialogue4Choice2,Dialogue5Choice2, Dialogue6Choice2, Dialogue7Choice2, Dialogue8Choice2, Dialogue9Choice2, Dialogue10Choice2,Dialogue11Choice2, Dialogue12Choice2, Dialogue13Choice2, Dialogue14Choice2, Dialogue15Choice2, Dialogue16Choice2,Dialogue17Choice2, Dialogue18Choice2, Dialogue19Choice2, Dialogue20Choice2, Dialogue21Choice2, Dialogue1Choice3, Dialogue2Choice3, Dialogue3Choice3, Dialogue4Choice3, Dialogue5Choice3, Dialogue6Choice3, Dialogue7Choice3, Dialogue8Choice3, DialogueQuestion1Choice3, Dialogue1Answer1Question1Choice3, Dialogue2Answer1Question1Choice3, Dialogue3Answer1Question1Choice3, Dialogue4Answer1Question1Choice3, Dialogue1Answer2Question1Choice3, Dialogue2Answer2Question1Choice3, Dialogue3Answer2Question1Choice3, Dialogue4Answer2Question1Choice3, Dialogue1Answer3Question1Choice3, Dialogue2Answer3Question1Choice3, Dialogue3Answer3Question1Choice3, Dialogue4Answer3Question1Choice3, DialogueQuestion2Choice3, Dialogue1Answer1Question2Choice3, Dialogue2Answer1Question2Choice3, Dialogue3Answer1Question2Choice3, Dialogue4Answer1Question2Choice3, Dialogue1Answer2Question2Choice3, Dialogue2Answer2Question2Choice3, Dialogue3Answer2Question2Choice3, Dialogue4Answer2Question2Choice3, Dialogue1Answer3Question2Choice3, Dialogue2Answer3Question2Choice3, Dialogue3Answer3Question2Choice3, Dialogue4Answer3Question2Choice3, Dialogue9Choice3, Dialogue10Choice3, Dialogue11Choice3, Dialogue12Choice3, Dialogue13Choice3, Dialogue14Choice3, Dialogue15Choice3, Dialogue16Choice3, Dialogue17Choice3, Dialogue18Choice3, Dialogue19Choice3, Dialogue20Choice3, Dialogue21Choice3, Dialogue22Choice3, Dialogue23Choice3, Dialogue24Choice3, Dialogue25Choice3, Dialogue26Choice3, Dialogue27Choice3, Dialogue28Choice3,Dialogue29Choice3 };
        Button[] allButtons = { Choice1, Choice2, Choice3, Continue1Choice1, Continue2Choice1, Continue3Choice1, Continue4Choice1, Continue5Choice1, Continue6Choice1, Continue7Choice1, Continue8Choice1, Continue9Choice1, Continue10Choice1, Continue11Choice1, Continue12Choice1, EndChoice1, Continue1Choice2, Continue2Choice2, Continue3Choice2, Continue4Choice2,Continue5Choice2, Continue6Choice2, Continue7Choice2, Continue8Choice2, Continue9Choice2, Continue10Choice2,Continue11Choice2, Continue12Choice2, Continue13Choice2, Continue14Choice2, Continue15Choice2,Continue16Choice2,Continue17Choice2, Continue18Choice2, Continue19Choice2, Continue20Choice2, EndChoice2, Continue1Choice3, Continue2Choice3, Continue3Choice3, Continue4Choice3, Continue5Choice3, Continue6Choice3, Continue7Choice3, Continue8Choice3, Answer1Question1, Answer2Question1, Answer3Question1, Continue1Answer1Question1Choice3, Continue2Answer1Question1Choice3, Continue3Answer1Question1Choice3, Continue4Answer1Question1Choice3,Continue1Answer2Question1Choice3, Continue2Answer2Question1Choice3, Continue3Answer2Question1Choice3, Continue4Answer2Question1Choice3, Continue1Answer3Question1Choice3, Continue2Answer3Question1Choice3, Continue3Answer3Question1Choice3, Continue4Answer3Question1Choice3, Answer1Question2, Answer2Question2, Answer3Question2, Continue1Answer1Question2Choice3, Continue2Answer1Question2Choice3, Continue3Answer1Question2Choice3, Continue4Answer1Question2Choice3,Continue1Answer2Question2Choice3, Continue2Answer2Question2Choice3, Continue3Answer2Question2Choice3, Continue4Answer2Question2Choice3, Continue1Answer3Question2Choice3, Continue2Answer3Question2Choice3, Continue3Answer3Question2Choice3, Continue4Answer3Question2Choice3, Continue9Choice3 ,Continue10Choice3 ,Continue11Choice3 ,Continue12Choice3, Continue13Choice3, Continue14Choice3, Continue15Choice3, Continue16Choice3, Continue17Choice3, Continue18Choice3, Continue19Choice3, Continue20Choice3, Continue21Choice3, Continue22Choice3, Continue23Choice3,Continue24Choice3, Continue25Choice3, Continue26Choice3, Continue27Choice3, Continue28Choice3, EndChoice3 };
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
        stage12GameTrialHandler.OpenStage12Trial();
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