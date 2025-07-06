using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Database;
public class UnlockStage5Dialogue : MonoBehaviour
{
    public Button skipButton;
    public AudioManager audioManager;
    public int customXRotation = 10;
    private FirebaseManager firebaseManager;
    public Stage4GameTrialHandler stage4GameTrialHandler;
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
    private bool isStage4TaskComplete = false;
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
    public Button EndChoice1;
    //choice 1 dialogue ends here
    //choice 2 dialogue starts here
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
    public Button EndChoice2;
    //choice 2 dialogue ends here
    // choice 3 dialogue starts here
    [Header("Dialogue1Choice3")]
    public TMP_Text Dialogue1Choice3;
    public Button Continue1Choice3;
    [Header("Dialogue2Choice3")]
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
    // choice 3 stop here
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
    public Button Continue29Choice3;
    [Header("Dialogue30Choice3")]
    public TMP_Text Dialogue30Choice3;
    public Button Continue30Choice3;
    // choice 3 stop here
        //choice 3 Question 3  start here
        [Header("DialogueQuestion3Choice3")]
        public TMP_Text DialogueQuestion3Choice3;
        public Button Answer1Question3, Answer2Question3, Answer3Question3;
        //answer 1 starts here
        [Header("Dialogue1 Answer1 Question3 Choice3")]
        public TMP_Text Dialogue1Answer1Question3Choice3;
        public Button Continue1Answer1Question3Choice3;
        [Header("Dialogue2 Answer1 Question3 Choice3")]
        public TMP_Text Dialogue2Answer1Question3Choice3;
        public Button Continue2Answer1Question3Choice3;
        [Header("Dialogue3 Answer1 Question3 Choice3")]
        public TMP_Text Dialogue3Answer1Question3Choice3;
        public Button Continue3Answer1Question3Choice3;
        [Header("Dialogue4 Answer1 Question3 Choice3")]
        public TMP_Text Dialogue4Answer1Question3Choice3;
        public Button Continue4Answer1Question3Choice3;
        //answer 2 starts here
        [Header("Dialogue1 Answer2 Question3 Choice3")]
        public TMP_Text Dialogue1Answer2Question3Choice3;
        public Button Continue1Answer2Question3Choice3;
        [Header("Dialogue2 Answer2 Question3 Choice3")]
        public TMP_Text Dialogue2Answer2Question3Choice3;
        public Button Continue2Answer2Question3Choice3;
        [Header("Dialogue3 Answer2 Question3 Choice3")]
        public TMP_Text Dialogue3Answer2Question3Choice3;
        public Button Continue3Answer2Question3Choice3;
        [Header("Dialogue4 Answer2 Question3 Choice3")]
        public TMP_Text Dialogue4Answer2Question3Choice3;
        public Button Continue4Answer2Question3Choice3;
        //answer 3 starts here
        [Header("Dialogue1 Answer3 Question3 Choice3")]
        public TMP_Text Dialogue1Answer3Question3Choice3;
        public Button Continue1Answer3Question3Choice3;
        [Header("Dialogue2 Answer3 Question3 Choice3")]
        public TMP_Text Dialogue2Answer3Question3Choice3;
        public Button Continue2Answer3Question3Choice3;
        [Header("Dialogue3 Answer3 Question3 Choice3")]
        public TMP_Text Dialogue3Answer3Question3Choice3;
        public Button Continue3Answer3Question3Choice3;
        [Header("Dialogue4 Answer3 Question3 Choice3")]
        public TMP_Text Dialogue4Answer3Question3Choice3;
        public Button Continue4Answer3Question3Choice3;
    [Header("Dialogue31Choice3")]
    public TMP_Text Dialogue31Choice3;
    public Button Continue31Choice3;
    [Header("Dialogue32Choice3")]
    public TMP_Text Dialogue32Choice3;
    public Button Continue32Choice3;
    [Header("Dialogue33Choice3")]
    public TMP_Text Dialogue33Choice3;
    public Button Continue33Choice3;
    [Header("Dialogue34Choice3")]
    public TMP_Text Dialogue34Choice3;
    public Button Continue34Choice3;
    [Header("Dialogue35Choice3")]
    public TMP_Text Dialogue35Choice3;
    public Button Continue35Choice3;
    [Header("Dialogue36Choice3")]
    public TMP_Text Dialogue36Choice3;
    public Button Continue36Choice3;
    [Header("Dialogue37Choice3")]
    public TMP_Text Dialogue37Choice3;
    public Button Continue37Choice3;
    [Header("Dialogue38Choice3")]
    public TMP_Text Dialogue38Choice3;
    public Button Continue38Choice3;
    [Header("Dialogue39Choice3")]
    public TMP_Text Dialogue39Choice3;
    public Button Continue39Choice3;
    [Header("Dialogue40Choice3")]
    public TMP_Text Dialogue40Choice3;
    public Button StartTrial;
    void Start()
    {
        StartCoroutine(InitializePlayer());
        characterLoader = FindObjectOfType<MainCharacterLoader>();
        firebaseManager = FindObjectOfType<FirebaseManager>();
        if(characterLoader != null)
        {
            UpdatePlayerImage();
        }
        CheckStage4TaskStatus();
    }
    private void CheckStage4TaskStatus()
    {
        string userId = firebaseManager.GetUserId();
        DatabaseReference stage4TaskRef = firebaseManager.GetDatabaseReference($"users/{userId}/stages/stage04/stage4Task");
        stage4TaskRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                isStage4TaskComplete = snapshot.Exists && (bool)snapshot.Value;
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
        TMP_Text[] dialogues = { startDialogue, Dialogue1Choice1, Dialogue2Choice1, Dialogue3Choice1, Dialogue4Choice1, Dialogue1Choice2, Dialogue2Choice2, Dialogue3Choice2, Dialogue4Choice2, Dialogue5Choice2, Dialogue6Choice2, Dialogue7Choice2, Dialogue8Choice2, Dialogue9Choice2, Dialogue10Choice2, Dialogue11Choice2, Dialogue1Choice3, Dialogue2Choice3, Dialogue3Choice3, Dialogue4Choice3, Dialogue5Choice3, Dialogue6Choice3, Dialogue7Choice3, Dialogue8Choice3, Dialogue9Choice3,DialogueQuestion1Choice3, Dialogue1Answer1Question1Choice3, Dialogue2Answer1Question1Choice3, Dialogue3Answer1Question1Choice3, Dialogue4Answer1Question1Choice3, Dialogue1Answer2Question1Choice3, Dialogue2Answer2Question1Choice3, Dialogue3Answer2Question1Choice3, Dialogue4Answer2Question1Choice3, Dialogue1Answer3Question1Choice3, Dialogue2Answer3Question1Choice3, Dialogue3Answer3Question1Choice3, Dialogue4Answer3Question1Choice3, Dialogue10Choice3, Dialogue11Choice3, Dialogue12Choice3, Dialogue13Choice3, Dialogue14Choice3, Dialogue15Choice3, Dialogue16Choice3, Dialogue17Choice3, Dialogue18Choice3, Dialogue19Choice3, Dialogue20Choice3, DialogueQuestion2Choice3, Dialogue1Answer1Question2Choice3, Dialogue2Answer1Question2Choice3, Dialogue3Answer1Question2Choice3, Dialogue4Answer1Question2Choice3, Dialogue1Answer2Question2Choice3, Dialogue2Answer2Question2Choice3, Dialogue3Answer2Question2Choice3, Dialogue4Answer2Question2Choice3, Dialogue1Answer3Question2Choice3, Dialogue2Answer3Question2Choice3, Dialogue3Answer3Question2Choice3, Dialogue4Answer3Question2Choice3,Dialogue21Choice3, Dialogue22Choice3, Dialogue23Choice3, Dialogue24Choice3, Dialogue25Choice3, Dialogue26Choice3, Dialogue27Choice3, Dialogue28Choice3,Dialogue29Choice3, Dialogue30Choice3, DialogueQuestion3Choice3, Dialogue1Answer1Question3Choice3, Dialogue2Answer1Question3Choice3,Dialogue3Answer1Question3Choice3, Dialogue4Answer1Question3Choice3, Dialogue1Answer2Question3Choice3, Dialogue2Answer2Question3Choice3, Dialogue3Answer2Question3Choice3, Dialogue4Answer2Question3Choice3, Dialogue1Answer3Question3Choice3, Dialogue2Answer3Question3Choice3, Dialogue3Answer3Question3Choice3, Dialogue4Answer3Question3Choice3, Dialogue31Choice3, Dialogue32Choice3, Dialogue33Choice3,Dialogue34Choice3, Dialogue35Choice3, Dialogue36Choice3, Dialogue37Choice3, Dialogue38Choice3, Dialogue39Choice3, Dialogue40Choice3};
        Button[] allButtons = { Choice1, Choice2, Choice3, Continue1Choice1, Continue2Choice1, Continue3Choice1, EndChoice1, Continue1Choice2, Continue2Choice2, Continue3Choice2, Continue4Choice2, Continue5Choice2, Continue6Choice2, Continue7Choice2, Continue8Choice2, Continue9Choice2, Continue10Choice2, EndChoice2, Continue1Choice3, Continue2Choice3, Continue3Choice3, Continue4Choice3, Continue5Choice3, Continue6Choice3, Continue7Choice3, Continue8Choice3, Continue9Choice3, Answer1Question1, Answer2Question1, Answer3Question1, Continue1Answer1Question1Choice3, Continue2Answer1Question1Choice3, Continue3Answer1Question1Choice3, Continue4Answer1Question1Choice3,Continue1Answer2Question1Choice3, Continue2Answer2Question1Choice3, Continue3Answer2Question1Choice3, Continue4Answer2Question1Choice3, Continue1Answer3Question1Choice3, Continue2Answer3Question1Choice3, Continue3Answer3Question1Choice3, Continue4Answer3Question1Choice3, Continue10Choice3, Continue11Choice3, Continue12Choice3, Continue13Choice3, Continue14Choice3, Continue15Choice3, Continue16Choice3, Continue17Choice3, Continue18Choice3, Continue19Choice3, Continue20Choice3, Answer1Question2, Answer2Question2, Answer3Question2, Continue1Answer1Question2Choice3, Continue2Answer1Question2Choice3, Continue3Answer1Question2Choice3, Continue4Answer1Question2Choice3,Continue1Answer2Question2Choice3, Continue2Answer2Question2Choice3, Continue3Answer2Question2Choice3, Continue4Answer2Question2Choice3, Continue1Answer3Question2Choice3, Continue2Answer3Question2Choice3, Continue3Answer3Question2Choice3, Continue4Answer3Question2Choice3, Continue21Choice3, Continue22Choice3, Continue23Choice3,Continue24Choice3, Continue25Choice3, Continue26Choice3, Continue27Choice3, Continue28Choice3, Continue29Choice3, Continue30Choice3, Answer1Question3, Answer2Question3, Answer3Question3, Continue1Answer1Question3Choice3, Continue2Answer1Question3Choice3, Continue3Answer1Question3Choice3, Continue4Answer1Question3Choice3, Continue1Answer2Question3Choice3, Continue2Answer2Question3Choice3, Continue3Answer2Question3Choice3, Continue4Answer2Question3Choice3, Continue1Answer3Question3Choice3, Continue2Answer3Question3Choice3, Continue3Answer3Question3Choice3, Continue4Answer3Question3Choice3, Continue31Choice3, Continue32Choice3, Continue33Choice3, Continue34Choice3, Continue35Choice3, Continue36Choice3, Continue37Choice3, Continue38Choice3, Continue39Choice3, StartTrial};
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
                if (button == Choice3 && isStage4TaskComplete)
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
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[0]);
        CheckStage4TaskStatus(); 
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
    //choice1 button = "nothing" for StartDialogue1
    public void StartDialogue1Choice1()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Choice1, new Button[] { Continue1Choice1 }, "Nothing, just wandering around.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Choice1()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[1]);
        SetDialogueState(true, false, Dialogue2Choice1, new Button[] { Continue2Choice1 }, "Just wondering, huh? Well, be careful because you might encounter stronger monsters as you progress.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue3Choice1()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue3Choice1, new Button[] { Continue3Choice1 }, "I'll be careful. Thanks for the warning.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue4Choice1()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[2]);
        SetDialogueState(true, false, Dialogue4Choice1, new Button[] { EndChoice1 }, "No problem. If you need anything, just ask.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    //choice 2 button = Got any advice?
    public void StartDialogue1Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Choice2, new Button[] { Continue1Choice2 }, "Got any advice for someone trying to get better at math?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[3]);
        SetDialogueState(true,false, Dialogue2Choice2, new Button[] { Continue2Choice2 }, "I see you're working on your skills. Let me give you a few tips.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue3Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[4]);
        SetDialogueState(true,false, Dialogue3Choice2, new Button[] { Continue3Choice2 }, "Well, when it comes to math, the most important thing is practice. Don't be afraid to start small and build your way up.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue4Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue4Choice2, new Button[] { Continue4Choice2 }, "Got it. I tend to rush through things sometimes.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue5Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[5]);
        SetDialogueState(true,false, Dialogue5Choice2, new Button[] { Continue5Choice2 }, "That’s common. But remember, getting comfortable with the basics makes the harder problems a lot easier down the road.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue6Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue6Choice2, new Button[] { Continue6Choice2 }, "So, focus on mastering the fundamentals first?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue7Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[6]);
        SetDialogueState(true,false, Dialogue7Choice2, new Button[] { Continue7Choice2 }, "Exactly! Once you’re quick with the basics, the more complex challenges will feel much smoother. It’s all about laying the right foundation.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue8Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue8Choice2, new Button[] { Continue8Choice2 }, "I see. I guess I need to slow down and really understand the steps.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue9Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[7]);
        SetDialogueState(true,false, Dialogue9Choice2, new Button[] { Continue9Choice2 }, "Yes, take your time with each one. Practice makes perfect, and before you know it, you'll be solving them faster than you think.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue10Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue10Choice2, new Button[] { Continue10Choice2 }, "I’ll try that. Thanks for the tips!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue11Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[8]);
        SetDialogueState(true,false, Dialogue11Choice2, new Button[] { EndChoice2 }, "No problem! Keep at it, and you’ll get stronger with each challenge.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    //choice3 = learn subtraction
    public void StartDialogue1Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Choice3, new Button[] { Continue1Choice3 }, "I need help understanding subtraction.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[9]);
        SetDialogueState(true, false, Dialogue2Choice3, new Button[] { Continue2Choice3 }, "Ah, I see you're ready to learn a bit more. Let's talk about subtraction, it's one of the most important math skills you'll use.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue3Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue3Choice3, new Button[] { Continue3Choice3 }, "I'm ready! I've always found subtraction a bit tricky.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue4Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[10]);
        SetDialogueState(true, false, Dialogue4Choice3, new Button[] { Continue4Choice3 }, "That's okay! It can seem confusing at first, but once you understand how it works, it'll be much easier.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue5Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[11]);
        SetDialogueState(true, false, Dialogue5Choice3, new Button[] { Continue5Choice3 }, "Subtraction is all about finding out how much is left when you take something away.");
    }
    public void StartDialogue6Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue6Choice3, new Button[] { Continue6Choice3 }, "So, it's like removing things?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue7Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[12]);
        SetDialogueState(true, false, Dialogue7Choice3, new Button[] { Continue7Choice3 }, "Exactly! Imagine you have 8 candies, and you give 3 to your friend. How many candies do you have left?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue8Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue8Choice3, new Button[] { Continue8Choice3 }, "Umm… 5?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue9Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[13]);
        SetDialogueState(true, false, Dialogue9Choice3, new Button[] { Continue9Choice3 }, "That's right! You started with 8, took away 3, and you're left with 5. That's subtraction in action.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    //pause the Dialogue from choice 3
    // Start Dialogue from Question1
    public void StartDialogueQuestion1Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[14]);
        SetDialogueState(true, false, DialogueQuestion1Choice3, new Button[] { Answer1Question1, Answer2Question1, Answer3Question1 }, "Let's test your skills with a quick question! If you have 15 apples and give away 7, how many apples do you have left?");
    }
    // Start Answer 1
    public void StartDialogue1Answer1Question1Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Answer1Question1Choice3, new Button[] { Continue1Answer1Question1Choice3 }, "Hmm… I think it's 9.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Answer1Question1Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[15]);
        SetDialogueState(true, false, Dialogue2Answer1Question1Choice3, new Button[] { Continue2Answer1Question1Choice3 }, "Not quite, but that's okay! Remember, subtraction is about taking away. Start with 15 and count down 7 steps. That would leave you with 8 apples.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue3Answer1Question1Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue3Answer1Question1Choice3, new Button[] { Continue3Answer1Question1Choice3 }, "Oh, I see! I forgot to count carefully.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue4Answer1Question1Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[16]);
        SetDialogueState(true, false, Dialogue4Answer1Question1Choice3, new Button[] { Continue4Answer1Question1Choice3 }, "No problem! Mistakes are how we learn. Let's keep going.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    // Start Answer 2
    public void StartDialogue1Answer2Question1Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Answer2Question1Choice3, new Button[] { Continue1Answer2Question1Choice3 }, "Hmm… I think it's 8.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Answer2Question1Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[17]);
        SetDialogueState(true, false, Dialogue2Answer2Question1Choice3, new Button[] { Continue2Answer2Question1Choice3 }, "That's correct! Great job! You started with 15 and subtracted 7, which leaves you with 8 apples.");
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
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[18]);
        SetDialogueState(true, false, Dialogue4Answer2Question1Choice3, new Button[] { Continue4Answer2Question1Choice3 }, "I can see that! You're doing great. Let's continue.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    // Start Answer 3
    public void StartDialogue1Answer3Question1Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Answer3Question1Choice3, new Button[] { Continue1Answer3Question1Choice3 }, "Hmm… I think it's 7.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Answer3Question1Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[19]);
        SetDialogueState(true, false, Dialogue2Answer3Question1Choice3, new Button[] { Continue2Answer3Question1Choice3 }, "Close, but not quite. Start with 15 and count down 7 steps. That would leave you with 8 apples.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue3Answer3Question1Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue3Answer3Question1Choice3, new Button[] { Continue3Answer3Question1Choice3 }, "Ah, I see where I went wrong. I didn't count carefully enough.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue4Answer3Question1Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[20]);
        SetDialogueState(true, false, Dialogue4Answer3Question1Choice3, new Button[] { Continue4Answer3Question1Choice3 }, "That's okay! You're learning, and that's what matters. Let's keep going.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue10Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[21]);
        SetDialogueState(true, false, Dialogue10Choice3, new Button[] { Continue10Choice3 }, "Sometimes, instead of counting down one by one, you can use shortcuts.");
    }
    public void StartDialogue11Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[22]);
        SetDialogueState(true, false, Dialogue11Choice3, new Button[] { Continue11Choice3 }, "For example, if you need to subtract 5 from 12, think of it as “what number do I need to add to 5 to make 12?”");
    }
    public void StartDialogue12Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue12Choice3, new Button[] { Continue12Choice3 }, "Hmm… I think it's 7?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue13Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[23]);
        SetDialogueState(true, false, Dialogue13Choice3, new Button[] { Continue13Choice3 }, "You got it! That's a great way to check your work too. Now, let's talk about what happens when you subtract and get a negative number.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue14Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue14Choice3, new Button[] { Continue14Choice3 }, "Negative numbers? What are those?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue15Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[24]);
        SetDialogueState(true, false, Dialogue15Choice3, new Button[] { Continue15Choice3 }, "Negative numbers are numbers smaller than zero.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue16Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[25]);
        SetDialogueState(true, false, Dialogue16Choice3, new Button[] { Continue16Choice3 }, "Sometimes, instead of counting down one by one, you can use shortcuts.");
    }
    public void StartDialogue17Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[26]);
        SetDialogueState(true, false, Dialogue17Choice3, new Button[] { Continue17Choice3 }, "Imagine this, you have 3 apples, but you promised to give 5 apples to your friends.");
    }
    public void StartDialogue18Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[27]);
        SetDialogueState(true, false, Dialogue18Choice3, new Button[] { Continue18Choice3 }, "You don't have enough, so you're “in debt” by 2 apples. That's what a negative number means, it's like you owe something.");
    }
    public void StartDialogue19Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue19Choice3, new Button[] { Continue19Choice3 }, "Oh, I think I get it. So, 3 minus 5 equals negative 2?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue20Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[28]);
        SetDialogueState(true, false, Dialogue20Choice3, new Button[] { Continue20Choice3 }, "That's it! Negative numbers show that the amount is less than zero. You can think of it as “below zero.”");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    // start question2
    public void StartDialogueQuestion2Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[29]);
        SetDialogueState(true, false, DialogueQuestion2Choice3, new Button[] { Answer1Question2, Answer2Question2, Answer3Question2 }, "Let's try another problem with negative numbers. If you start with 2, and you subtract 5, what do you get?");
    }
    // Start Answer 1
    public void StartDialogue1Answer1Question2Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Answer1Question2Choice3, new Button[] { Continue1Answer1Question2Choice3 }, "Hmm… I think it's -3.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Answer1Question2Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[30]);
        SetDialogueState(true, false, Dialogue2Answer1Question2Choice3, new Button[] { Continue2Answer1Question2Choice3 }, "That's correct! Great job! Starting with 2 and subtracting 5 puts you 3 below zero, which is -3.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue3Answer1Question2Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue3Answer1Question2Choice3, new Button[] { Continue3Answer1Question2Choice3 }, "This is starting to make more sense now.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue4Answer1Question2Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[31]);
        SetDialogueState(true, false, Dialogue4Answer1Question2Choice3, new Button[] { Continue4Answer1Question2Choice3 }, "I'm glad to hear that! Let's keep the momentum going.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    // Start Answer 2
    public void StartDialogue1Answer2Question2Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Answer2Question2Choice3, new Button[] { Continue1Answer2Question2Choice3 }, "Hmm… I think it's 3.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Answer2Question2Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[32]);
        SetDialogueState(true, false, Dialogue2Answer2Question2Choice3, new Button[] { Continue2Answer2Question2Choice3 }, "Not quite, but you're close! Start with 2 and subtract 5. Since 5 is greater than 2, you'll end up below zero, at -3.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue3Answer2Question2Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue3Answer2Question2Choice3, new Button[] { Continue3Answer2Question2Choice3 }, "Ah, okay! I see now.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue4Answer2Question2Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[33]);
        SetDialogueState(true, false, Dialogue4Answer2Question2Choice3, new Button[] { Continue4Answer2Question2Choice3 }, "That's the spirit! Let's keep practicing.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    // Start Answer 3
    public void StartDialogue1Answer3Question2Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Answer3Question2Choice3, new Button[] { Continue1Answer3Question2Choice3 }, "Hmm… I think it's 7.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Answer3Question2Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[34]);
        SetDialogueState(true, false, Dialogue2Answer3Question2Choice3, new Button[] { Continue2Answer3Question2Choice3 }, "That's a bit off. Start with 2 and subtract 5. Since 5 is greater than 2, you'll end up with -3, which is below zero.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue3Answer3Question2Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue3Answer3Question2Choice3, new Button[] { Continue3Answer3Question2Choice3 }, "Got it. I'll pay closer attention next time.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue4Answer3Question2Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[35]);
        SetDialogueState(true, false, Dialogue4Answer3Question2Choice3, new Button[] { Continue4Answer3Question2Choice3 }, "That's the way to learn! Let's move on.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue21Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[36]);
        SetDialogueState(true, false, Dialogue21Choice3, new Button[] { Continue21Choice3 }, "Now, let's talk about what happens when you subtract multiple numbers.");
    }
    public void StartDialogue22Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[37]);
        SetDialogueState(true, false, Dialogue22Choice3, new Button[] { Continue22Choice3 }, "When you have multiple numbers, you just solve them one step at a time, from left to right. Try to solve 15 minus 4 minus 6");
    }
    public void StartDialogue23Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue23Choice3, new Button[] { Continue23Choice3 }, "So, I start with 15 minus 4?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue24Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[38]);
        SetDialogueState(true, false, Dialogue24Choice3, new Button[] { Continue24Choice3 }, "Exactly! What's 15 minus 4?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue25Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue25Choice3, new Button[] { Continue25Choice3 }, "That's 11.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue26Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[39]);
        SetDialogueState(true, false, Dialogue26Choice3, new Button[] { Continue26Choice3 }, "Right! Now take 11 and subtract 6.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue27Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue27Choice3, new Button[] { Continue27Choice3 }, "11 minus 6 is 5.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue28Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[40]);
        SetDialogueState(true, false, Dialogue28Choice3, new Button[] { Continue28Choice3 }, "Perfect! So, 15 minus 4 minus 6 equals 5. Breaking it into steps makes it easier to solve.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue29Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue29Choice3, new Button[] { Continue29Choice3 }, "That's not so bad!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue30Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[41]);
        SetDialogueState(true, false, Dialogue30Choice3, new Button[] { Continue30Choice3 }, "Not at all. Just remember to go step by step, and if you're ever unsure, you can double-check your work by adding back the numbers you subtracted.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    // start question3
    public void StartDialogueQuestion3Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[42]);
        SetDialogueState(true, false, DialogueQuestion3Choice3, new Button[] { Answer1Question3, Answer2Question3, Answer3Question3 }, "Now, let's try a problem with three numbers. Imagine you have 20 candies. First, you give away 8 to your friends, and then you eat 5 yourself. How many candies do you have left?");
    }
    // Start Answer 1
    public void StartDialogue1Answer1Question3Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Answer1Question3Choice3, new Button[] { Continue1Answer1Question3Choice3 }, "Hmm… I think it's 10.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Answer1Question3Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[43]);
        SetDialogueState(true, false, Dialogue2Answer1Question3Choice3, new Button[] { Continue2Answer1Question3Choice3 }, "Not quite, but let's break it down. Start with 20 candies. If you give away 8, you're left with 12. Then you eat 5, so 12 minus 5 leaves you with 7.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue3Answer1Question3Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue3Answer1Question3Choice3, new Button[] { Continue3Answer1Question3Choice3 }, "Ah, okay! I forgot to subtract the second number.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue4Answer1Question3Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[44]);
        SetDialogueState(true, false, Dialogue4Answer1Question3Choice3, new Button[] { Continue4Answer1Question3Choice3 }, "That's okay! It can be tricky at first, but it's all about taking it step by step.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    // Start Answer 2
    public void StartDialogue1Answer2Question3Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Answer2Question3Choice3, new Button[] { Continue1Answer2Question3Choice3 }, "Hmm… I think it's 7.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Answer2Question3Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[45]);
        SetDialogueState(true, false, Dialogue2Answer2Question3Choice3, new Button[] { Continue2Answer2Question3Choice3 }, "That's correct! Great job! You started with 20 candies, and gave away 8 to your friends, leaving 12. Then you ate 5, so 12 minus 5 leaves you with 7 candies.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue3Answer2Question3Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue3Answer2Question3Choice3, new Button[] { Continue3Answer2Question3Choice3 }, "Nice! I feel like I'm getting better at this.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue4Answer2Question3Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[46]);
        SetDialogueState(true, false, Dialogue4Answer2Question3Choice3, new Button[] { Continue4Answer2Question3Choice3 }, "You definitely are! Keep practicing like this, and it'll feel easy in no time.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    // Start Answer 3
    public void StartDialogue1Answer3Question3Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Answer3Question3Choice3, new Button[] { Continue1Answer3Question3Choice3 }, "Hmm… I think it's 8.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Answer3Question3Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[47]);
        SetDialogueState(true, false, Dialogue2Answer3Question3Choice3, new Button[] { Continue2Answer3Question3Choice3 }, "Not quite, but you're close. Let's break it down. You start with 20 candies. When you give away 8, you're left with 12. Then subtract 5, and you're left with 7.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue3Answer3Question3Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue3Answer3Question3Choice3, new Button[] { Continue3Answer3Question3Choice3 }, "Oh, I missed that last step!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue4Answer3Question3Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[48]);
        SetDialogueState(true, false, Dialogue4Answer3Question3Choice3, new Button[] { Continue4Answer3Question3Choice3 }, "That's alright. Subtracting multiple numbers just takes a little practice. You're doing great!");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue31Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[49]);
        SetDialogueState(true, false, Dialogue31Choice3, new Button[] { Continue31Choice3 }, "You're doing great so far! I can see you've got a good understanding of subtraction now.");
    }
    public void StartDialogue32Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue32Choice3, new Button[] { Continue32Choice3 }, "Thanks! I feel much more confident.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue33Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[50]);
        SetDialogueState(true, false, Dialogue33Choice3, new Button[] { Continue33Choice3 }, "That’s what I like to hear! Now, it's time to put your skills to the test.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue34Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[51]);
        SetDialogueState(true, false, Dialogue34Choice3, new Button[] { Continue34Choice3 }, "You've learned about subtracting step by step, dealing with borrowing, handling negative answers, and even solving problems with multiple numbers.");
    }
    public void StartDialogue35Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue35Choice3, new Button[] { Continue35Choice3 }, "I think I'm ready!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue36Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[52]);
        SetDialogueState(true, false, Dialogue36Choice3, new Button[] { Continue36Choice3 }, "Perfect! The next part is all about solving subtraction problems on your own. Don't worry, I know you've got what it takes.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue37Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[53]);
        SetDialogueState(true, false, Dialogue37Choice3, new Button[] { Continue37Choice3 }, "Just remember the tips: take it step by step, stay calm, and double-check your answers.");
    }
    public void StartDialogue38Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue38Choice3, new Button[] { Continue38Choice3 }, "Got it. I'll do my best!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue39Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[54]);
        SetDialogueState(true, false, Dialogue39Choice3, new Button[] { Continue39Choice3 }, "I know you will. And remember, mistakes are just steps toward learning, so don't get discouraged if something feels tricky.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue40Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart1Clips[55]);
        SetDialogueState(true, false, Dialogue40Choice3, new Button[] { StartTrial }, "Good luck, and have fun solving those problems! You've got this!");
    }
    public void ConvoEnd()
    {
        audioManager.StopCurrentSFX();
        ConvoPanel.SetActive(false);
        NPCName.gameObject.SetActive(false);
        PlayerName.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        TMP_Text[] dialogues = { startDialogue, Dialogue1Choice1, Dialogue2Choice1, Dialogue3Choice1, Dialogue4Choice1, Dialogue1Choice2, Dialogue2Choice2, Dialogue3Choice2, Dialogue4Choice2, Dialogue5Choice2, Dialogue6Choice2, Dialogue7Choice2, Dialogue8Choice2, Dialogue9Choice2, Dialogue10Choice2, Dialogue11Choice2, Dialogue1Choice3, Dialogue2Choice3, Dialogue3Choice3, Dialogue4Choice3, Dialogue5Choice3, Dialogue6Choice3, Dialogue7Choice3, Dialogue8Choice3, Dialogue9Choice3,DialogueQuestion1Choice3, Dialogue1Answer1Question1Choice3, Dialogue2Answer1Question1Choice3, Dialogue3Answer1Question1Choice3, Dialogue4Answer1Question1Choice3, Dialogue1Answer2Question1Choice3, Dialogue2Answer2Question1Choice3, Dialogue3Answer2Question1Choice3, Dialogue4Answer2Question1Choice3, Dialogue1Answer3Question1Choice3, Dialogue2Answer3Question1Choice3, Dialogue3Answer3Question1Choice3, Dialogue4Answer3Question1Choice3, Dialogue10Choice3, Dialogue11Choice3, Dialogue12Choice3, Dialogue13Choice3, Dialogue14Choice3, Dialogue15Choice3, Dialogue16Choice3, Dialogue17Choice3, Dialogue18Choice3, Dialogue19Choice3, Dialogue20Choice3, DialogueQuestion2Choice3, Dialogue1Answer1Question2Choice3, Dialogue2Answer1Question2Choice3, Dialogue3Answer1Question2Choice3, Dialogue4Answer1Question2Choice3, Dialogue1Answer2Question2Choice3, Dialogue2Answer2Question2Choice3, Dialogue3Answer2Question2Choice3, Dialogue4Answer2Question2Choice3, Dialogue1Answer3Question2Choice3, Dialogue2Answer3Question2Choice3, Dialogue3Answer3Question2Choice3, Dialogue4Answer3Question2Choice3,Dialogue21Choice3, Dialogue22Choice3, Dialogue23Choice3, Dialogue24Choice3, Dialogue25Choice3, Dialogue26Choice3, Dialogue27Choice3, Dialogue28Choice3,Dialogue29Choice3, Dialogue30Choice3, DialogueQuestion3Choice3, Dialogue1Answer1Question3Choice3, Dialogue2Answer1Question3Choice3,Dialogue3Answer1Question3Choice3, Dialogue4Answer1Question3Choice3, Dialogue1Answer2Question3Choice3, Dialogue2Answer2Question3Choice3, Dialogue3Answer2Question3Choice3, Dialogue4Answer2Question3Choice3, Dialogue1Answer3Question3Choice3, Dialogue2Answer3Question3Choice3, Dialogue3Answer3Question3Choice3, Dialogue4Answer3Question3Choice3, Dialogue31Choice3, Dialogue32Choice3, Dialogue33Choice3,Dialogue34Choice3, Dialogue35Choice3, Dialogue36Choice3, Dialogue37Choice3, Dialogue38Choice3, Dialogue39Choice3, Dialogue40Choice3};
        Button[] allButtons = { Choice1, Choice2, Choice3, Continue1Choice1, Continue2Choice1, Continue3Choice1, EndChoice1, Continue1Choice2, Continue2Choice2, Continue3Choice2, Continue4Choice2, Continue5Choice2, Continue6Choice2, Continue7Choice2, Continue8Choice2, Continue9Choice2, Continue10Choice2, EndChoice2, Continue1Choice3, Continue2Choice3, Continue3Choice3, Continue4Choice3, Continue5Choice3, Continue6Choice3, Continue7Choice3, Continue8Choice3, Continue9Choice3, Answer1Question1, Answer2Question1, Answer3Question1, Continue1Answer1Question1Choice3, Continue2Answer1Question1Choice3, Continue3Answer1Question1Choice3, Continue4Answer1Question1Choice3,Continue1Answer2Question1Choice3, Continue2Answer2Question1Choice3, Continue3Answer2Question1Choice3, Continue4Answer2Question1Choice3, Continue1Answer3Question1Choice3, Continue2Answer3Question1Choice3, Continue3Answer3Question1Choice3, Continue4Answer3Question1Choice3, Continue10Choice3, Continue11Choice3, Continue12Choice3, Continue13Choice3, Continue14Choice3, Continue15Choice3, Continue16Choice3, Continue17Choice3, Continue18Choice3, Continue19Choice3, Continue20Choice3, Answer1Question2, Answer2Question2, Answer3Question2, Continue1Answer1Question2Choice3, Continue2Answer1Question2Choice3, Continue3Answer1Question2Choice3, Continue4Answer1Question2Choice3,Continue1Answer2Question2Choice3, Continue2Answer2Question2Choice3, Continue3Answer2Question2Choice3, Continue4Answer2Question2Choice3, Continue1Answer3Question2Choice3, Continue2Answer3Question2Choice3, Continue3Answer3Question2Choice3, Continue4Answer3Question2Choice3, Continue21Choice3, Continue22Choice3, Continue23Choice3,Continue24Choice3, Continue25Choice3, Continue26Choice3, Continue27Choice3, Continue28Choice3, Continue29Choice3, Continue30Choice3, Answer1Question3, Answer2Question3, Answer3Question3, Continue1Answer1Question3Choice3, Continue2Answer1Question3Choice3, Continue3Answer1Question3Choice3, Continue4Answer1Question3Choice3, Continue1Answer2Question3Choice3, Continue2Answer2Question3Choice3, Continue3Answer2Question3Choice3, Continue4Answer2Question3Choice3, Continue1Answer3Question3Choice3, Continue2Answer3Question3Choice3, Continue3Answer3Question3Choice3, Continue4Answer3Question3Choice3, Continue31Choice3, Continue32Choice3, Continue33Choice3, Continue34Choice3, Continue35Choice3, Continue36Choice3, Continue37Choice3, Continue38Choice3, Continue39Choice3, StartTrial};
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
        stage4GameTrialHandler.OpenStage4Trial();
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