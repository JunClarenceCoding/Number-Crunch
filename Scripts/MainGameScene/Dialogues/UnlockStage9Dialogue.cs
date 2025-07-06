using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Database;
public class UnlockStage9Dialogue : MonoBehaviour
{
    public Button skipButton;
    public AudioManager audioManager;
    public int customXRotation = 10;
    private FirebaseManager firebaseManager;
    public Stage8GameTrialHandler stage8GameTrialHandler;
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
    private bool isStage8TaskComplete = false;
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
    public Button EndChoice1;
    //choice 1 dialogue ends here
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
    public Button Continue21Choice2;
    [Header("Dialogue22Choice2")]
    public TMP_Text Dialogue22Choice2;
    public Button Continue22Choice2;
    [Header("Dialogue23Choice2")]
    public TMP_Text Dialogue23Choice2;
    public Button Continue23Choice2;
    [Header("Dialogue24Choice2")]
    public TMP_Text Dialogue24Choice2;
    public Button Continue24Choice2;
    [Header("Dialogue25Choice2")]
    public TMP_Text Dialogue25Choice2;
    public Button Continue25Choice2;
    [Header("Dialogue26Choice2")]
    public TMP_Text Dialogue26Choice2;
    public Button Continue26Choice2;
    [Header("Dialogue27Choice2")]
    public TMP_Text Dialogue27Choice2;
    public Button Continue27Choice2;
    [Header("Dialogue28Choice2")]
    public TMP_Text Dialogue28Choice2;
    public Button Continue28Choice2;
    [Header("Dialogue29Choice2")]
    public TMP_Text Dialogue29Choice2;
    public Button EndChoice2;
    //choice 2 dialogue ends here
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
    [Header("Dialogue15Choice3")]
    public TMP_Text Dialogue15Choice3;
    public Button Continue15Choice3;
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
    public Button Continue29Choice3;
    [Header("Dialogue30Choice3")]
    public TMP_Text Dialogue30Choice3;
    public Button Continue30Choice3;
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
        CheckStage8TaskStatus();
    }
    private void CheckStage8TaskStatus()
    {
        string userId = firebaseManager.GetUserId();
        DatabaseReference stage4TaskRef = firebaseManager.GetDatabaseReference($"users/{userId}/stages/stage08/stage8Task");
        stage4TaskRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                isStage8TaskComplete = snapshot.Exists && (bool)snapshot.Value;
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
        TMP_Text[] dialogues = { startDialogue, Dialogue1Choice1, Dialogue2Choice1, Dialogue3Choice1, Dialogue4Choice1, Dialogue5Choice1, Dialogue6Choice1, Dialogue7Choice1, Dialogue8Choice1,Dialogue9Choice1, Dialogue10Choice1, Dialogue11Choice1, Dialogue1Choice2, Dialogue2Choice2, Dialogue3Choice2, Dialogue4Choice2,Dialogue5Choice2, Dialogue6Choice2, Dialogue7Choice2, Dialogue8Choice2, Dialogue9Choice2, Dialogue10Choice2,Dialogue11Choice2, Dialogue12Choice2, Dialogue13Choice2, Dialogue14Choice2, Dialogue15Choice2, Dialogue16Choice2,Dialogue17Choice2, Dialogue18Choice2, Dialogue19Choice2, Dialogue20Choice2, Dialogue21Choice2, Dialogue22Choice2, Dialogue23Choice2, Dialogue24Choice2, Dialogue25Choice2, Dialogue26Choice2, Dialogue27Choice2, Dialogue28Choice2, Dialogue29Choice2, Dialogue1Choice3, Dialogue2Choice3, Dialogue3Choice3, Dialogue4Choice3, Dialogue5Choice3, Dialogue6Choice3, Dialogue7Choice3, Dialogue8Choice3, Dialogue9Choice3, Dialogue10Choice3, Dialogue11Choice3, Dialogue12Choice3, Dialogue13Choice3, Dialogue14Choice3, DialogueQuestion1Choice3, Dialogue1Answer1Question1Choice3, Dialogue2Answer1Question1Choice3, Dialogue3Answer1Question1Choice3, Dialogue4Answer1Question1Choice3, Dialogue1Answer2Question1Choice3, Dialogue2Answer2Question1Choice3, Dialogue3Answer2Question1Choice3, Dialogue4Answer2Question1Choice3, Dialogue1Answer3Question1Choice3, Dialogue2Answer3Question1Choice3, Dialogue3Answer3Question1Choice3, Dialogue4Answer3Question1Choice3, Dialogue15Choice3, DialogueQuestion2Choice3, Dialogue1Answer1Question2Choice3, Dialogue2Answer1Question2Choice3, Dialogue3Answer1Question2Choice3, Dialogue4Answer1Question2Choice3, Dialogue1Answer2Question2Choice3, Dialogue2Answer2Question2Choice3, Dialogue3Answer2Question2Choice3, Dialogue4Answer2Question2Choice3, Dialogue1Answer3Question2Choice3, Dialogue2Answer3Question2Choice3, Dialogue3Answer3Question2Choice3, Dialogue4Answer3Question2Choice3, Dialogue16Choice3, Dialogue17Choice3, Dialogue18Choice3, Dialogue19Choice3, Dialogue20Choice3, Dialogue21Choice3, Dialogue22Choice3, Dialogue23Choice3, Dialogue24Choice3, Dialogue25Choice3, Dialogue26Choice3, Dialogue27Choice3, Dialogue28Choice3,Dialogue29Choice3, Dialogue30Choice3, Dialogue31Choice3, Dialogue32Choice3, Dialogue33Choice3,Dialogue34Choice3, Dialogue35Choice3, Dialogue36Choice3, Dialogue37Choice3, Dialogue38Choice3 };
        Button[] allButtons = { Choice1, Choice2, Choice3, Continue1Choice1, Continue2Choice1, Continue3Choice1, Continue4Choice1, Continue5Choice1, Continue6Choice1, Continue7Choice1, Continue8Choice1, Continue9Choice1, Continue10Choice1, EndChoice1, Continue1Choice2, Continue2Choice2, Continue3Choice2, Continue4Choice2,Continue5Choice2, Continue6Choice2, Continue7Choice2, Continue8Choice2, Continue9Choice2, Continue10Choice2,Continue11Choice2, Continue12Choice2, Continue13Choice2, Continue14Choice2, Continue15Choice2,Continue16Choice2,Continue17Choice2, Continue18Choice2, Continue19Choice2, Continue20Choice2, Continue21Choice2, Continue22Choice2, Continue23Choice2, Continue24Choice2, Continue25Choice2, Continue26Choice2, Continue27Choice2, Continue28Choice2, EndChoice2, Continue1Choice3, Continue2Choice3, Continue3Choice3, Continue4Choice3, Continue5Choice3, Continue6Choice3, Continue7Choice3, Continue8Choice3 ,Continue9Choice3 ,Continue10Choice3 ,Continue11Choice3 ,Continue12Choice3, Continue13Choice3, Continue14Choice3, Answer1Question1, Answer2Question1, Answer3Question1, Continue1Answer1Question1Choice3, Continue2Answer1Question1Choice3, Continue3Answer1Question1Choice3, Continue4Answer1Question1Choice3,Continue1Answer2Question1Choice3, Continue2Answer2Question1Choice3, Continue3Answer2Question1Choice3, Continue4Answer2Question1Choice3, Continue1Answer3Question1Choice3, Continue2Answer3Question1Choice3, Continue3Answer3Question1Choice3, Continue4Answer3Question1Choice3, Continue15Choice3, Answer1Question2, Answer2Question2, Answer3Question2, Continue1Answer1Question2Choice3, Continue2Answer1Question2Choice3, Continue3Answer1Question2Choice3, Continue4Answer1Question2Choice3,Continue1Answer2Question2Choice3, Continue2Answer2Question2Choice3, Continue3Answer2Question2Choice3, Continue4Answer2Question2Choice3, Continue1Answer3Question2Choice3, Continue2Answer3Question2Choice3, Continue3Answer3Question2Choice3, Continue4Answer3Question2Choice3, Continue16Choice3, Continue17Choice3, Continue18Choice3, Continue19Choice3, Continue20Choice3, Continue21Choice3, Continue22Choice3, Continue23Choice3,Continue24Choice3, Continue25Choice3, Continue26Choice3, Continue27Choice3, Continue28Choice3, Continue29Choice3, Continue30Choice3, Continue31Choice3, Continue32Choice3, Continue33Choice3, Continue34Choice3, Continue35Choice3, Continue36Choice3, Continue37Choice3, EndChoice3};
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
                if (button == Choice3 && isStage8TaskComplete)
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
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[0]);
        CheckStage8TaskStatus(); 
        DisplayPlayerName();
        SetDialogueState(true, false, startDialogue, new Button[] { Choice1, Choice2, Choice3 }, "Hello There! What brings you here today?");
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
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[1]);
        SetDialogueState(true, false, Dialogue2Choice1, new Button[] { Continue2Choice1 }, "Speaking of learning, have you ever wondered why the sky changes colors during the day?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue3Choice1()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue3Choice1, new Button[] { Continue3Choice1 }, "Hmm, I've noticed it, but I've never really thought about why.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue4Choice1()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[2]);
        SetDialogueState(true, false, Dialogue4Choice1, new Button[] { Continue4Choice1 }, "It's because of the sunlight scattering in the atmosphere. During the day, the sun is high, and its light scatters in all directions, which makes the sky look blue.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue5Choice1()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue5Choice1, new Button[] { Continue5Choice1 }, "Oh, that's cool! What about during sunset?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue6Choice1()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[3]);
        SetDialogueState(true, false, Dialogue6Choice1, new Button[] { Continue6Choice1 }, "Good question! At sunset, the sun is lower in the sky, so the light has to pass through more atmosphere.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue7Choice1()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[4]);
        SetDialogueState(true, false, Dialogue7Choice1, new Button[] { Continue7Choice1 }, "The blue light scatters out, leaving the reds and oranges for us to see. It's like nature's own painting!");
    }
    public void StartDialogue8Choice1()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue8Choice1, new Button[] { Continue8Choice1 }, "That's amazing! I'll have to pay more attention next time I watch a sunset.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue9Choice1()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[5]);
        SetDialogueState(true, false, Dialogue9Choice1, new Button[] { Continue9Choice1 }, "You should! It's one of the most beautiful reminders of how fascinating our world is.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue10Choice1()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[6]);
        SetDialogueState(true, false, Dialogue10Choice1, new Button[] { Continue10Choice1 }, "Well, enjoy your journey, and remember to keep asking questions—you never know what you'll discover!");
    }
    public void StartDialogue11Choice1()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue11Choice1, new Button[] { EndChoice1 }, "Thanks, I will!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    //choice2 button = "I need tips" 
    public void StartDialogue1Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Choice2, new Button[] { Continue1Choice2}, "Oh, I'm just trying to get better at math. Multiplication, in particular, has been giving me some trouble.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[7]);
        SetDialogueState(true, false, Dialogue2Choice2, new Button[] { Continue2Choice2 }, "Ah, multiplication! It's an important skill, but once you understand the basics, it's like second nature.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue3Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[8]);
        SetDialogueState(true, false, Dialogue3Choice2, new Button[] { Continue3Choice2 }, "Let me give you a few tips to help boost your confidence.");
    }
    public void StartDialogue4Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue4Choice2, new Button[] { Continue4Choice2}, "I'd really appreciate that.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue5Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[9]);
        SetDialogueState(true, false, Dialogue5Choice2, new Button[] { Continue5Choice2 }, "Alright! First, remember that multiplication is just repeated addition.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue6Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[10]);
        SetDialogueState(true, false, Dialogue6Choice2, new Button[] { Continue6Choice2 }, "For example, if you multiply 3 by 4, it means you’re adding 3 to itself four times. So, 3 + 3 + 3 + 3 equals 12.");
    }
    public void StartDialogue7Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue7Choice2, new Button[] { Continue7Choice2}, "Oh, so that's what multiplication really means!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue8Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[11]);
        SetDialogueState(true, false, Dialogue8Choice2, new Button[] { Continue8Choice2 }, "Exactly. Here's a little trick to make it easier.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue9Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[12]);
        SetDialogueState(true, false, Dialogue9Choice2, new Button[] { Continue9Choice2 }, "Start with smaller numbers you already know and build from there. Like, what's 5 times 2?");
    }
    public void StartDialogue10Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue10Choice2, new Button[] { Continue10Choice2}, "That's 10.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue11Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[13]);
        SetDialogueState(true, false, Dialogue11Choice2, new Button[] { Continue11Choice2 }, "Perfect! Now, if you know that 5 times 2 is 10, then you can figure out 5 times 4 by doubling that. What's double 10?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue12Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue12Choice2, new Button[] { Continue12Choice2}, "That's 20!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue13Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[14]);
        SetDialogueState(true, false, Dialogue13Choice2, new Button[] { Continue13Choice2 }, "Right again! Breaking things down into smaller steps helps make bigger problems feel less intimidating.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue14Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue14Choice2, new Button[] { Continue14Choice2}, "That's a smart way to look at it.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue15Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[15]);
        SetDialogueState(true, false, Dialogue15Choice2, new Button[] { Continue15Choice2 }, "Another tip, get comfortable with the times table for 1, 2, 5, and 10 first. They're like building blocks for bigger numbers.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue16Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[16]);
        SetDialogueState(true, false, Dialogue16Choice2, new Button[] { Continue16Choice2 }, "Also, remember this: any number multiplied by 1 stays the same, and any number multiplied by 0 is always 0.");
    }
    public void StartDialogue17Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue17Choice2, new Button[] { Continue17Choice2}, "So, like 7 times 1 is 7, and 7 times 0 is 0?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue18Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[17]);
        SetDialogueState(true, false, Dialogue18Choice2, new Button[] { Continue18Choice2 }, "Exactly! Those are rules you can always rely on. And here's a confidence booster...");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue19Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[18]);
        SetDialogueState(true, false, Dialogue19Choice2, new Button[] { Continue19Choice2 }, "You already know more multiplication than you think. For example, if you can count by twos or fives, you're already multiplying.");
    }
    public void StartDialogue20Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue20Choice2, new Button[] { Continue20Choice2}, "I guess I do know more than I realized.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue21Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[19]);
        SetDialogueState(true, false, Dialogue21Choice2, new Button[] { Continue21Choice2 }, "See? You've got this! Multiplication is just about practice and spotting patterns.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue22Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[20]);
        SetDialogueState(true, false, Dialogue22Choice2, new Button[] { Continue22Choice2 }, "The more you practice, the faster and more confident you'll become.");
    }
    public void StartDialogue23Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue23Choice2, new Button[] { Continue23Choice2}, "Thanks for the tips. I feel like I can handle it better now!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue24Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[21]);
        SetDialogueState(true, false, Dialogue24Choice2, new Button[] { Continue24Choice2 }, "You're welcome! Just take it one step at a time, and soon you'll be multiplying with ease.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue25Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[22]);
        SetDialogueState(true, false, Dialogue25Choice2, new Button[] { Continue25Choice2 }, "Every little bit of practice brings you closer to mastering it.");
    }
    public void StartDialogue26Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue26Choice2, new Button[] { Continue26Choice2}, "I'll definitely keep practicing!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue27Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[23]);
        SetDialogueState(true, false, Dialogue27Choice2, new Button[] { Continue27Choice2 }, "That's the spirit! Remember, mistakes are just steps toward learning. Stay determined, and multiplication will be your ally in no time.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue28Choice2()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue28Choice2, new Button[] { Continue28Choice2}, "Thanks for the help!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue29Choice2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[24]);
        SetDialogueState(true, false, Dialogue29Choice2, new Button[] { EndChoice2 }, "You're welcome!");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    //choice3 button = "I want to learn" 
    public void StartDialogue1Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Choice3, new Button[] { Continue1Choice3 }, "I want to learn multiplication.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[25]);
        SetDialogueState(true, false, Dialogue2Choice3, new Button[] { Continue2Choice3 }, "Ah, I see you're ready to learn something new! Lets dive into multiplication, it's like supercharged addition.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue3Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue3Choice3, new Button[] { Continue3Choice3 }, "Supercharged addition? What does that mean?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue4Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[26]);
        SetDialogueState(true, false, Dialogue4Choice3, new Button[] { Continue4Choice3 }, "Well, multiplication is a quick way of adding the same number multiple times.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue5Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[27]);
        SetDialogueState(true, false, Dialogue5Choice3, new Button[] { Continue5Choice3 }, "For example, if you have 3 groups of 4 apples, instead of adding 4 + 4 + 4, you can just multiply 3 by 4.");
    }
    public void StartDialogue6Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue6Choice3, new Button[] { Continue6Choice3 }, "Oh, so 3 times 4 is the same as 4 + 4 + 4?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue7Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[28]);
        SetDialogueState(true, false, Dialogue7Choice3, new Button[] { Continue7Choice3 }, "Exactly! And it works the other way around too. You could also say 4 times 3, which means you have 4 groups with 3 apples in each.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue8Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[29]);
        SetDialogueState(true, false, Dialogue8Choice3, new Button[] { Continue8Choice3 }, "Multiplication doesn't care about the order, it always gives the same answer.");
    }
    public void StartDialogue9Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue9Choice3, new Button[] { Continue9Choice3 }, "That's cool! So, what's the answer for 3 times 4?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue10Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[30]);
        SetDialogueState(true, false, Dialogue10Choice3, new Button[] { Continue10Choice3 }, "Let's figure it out. Imagine 3 groups of 4 apples. How many apples do you have in total?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue11Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue11Choice3, new Button[] { Continue11Choice3 }, "Um… 12?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue12Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[31]);
        SetDialogueState(true, false, Dialogue12Choice3, new Button[] { Continue12Choice3 }, "That's correct! 3 times 4 equals 12. See? Multiplication is all about grouping numbers together.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue13Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue13Choice3, new Button[] { Continue13Choice3 }, "I think I get it. So, multiplication is like shortcuts for adding.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue14Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[32]);
        SetDialogueState(true, false, Dialogue14Choice3, new Button[] { Continue14Choice3 }, "That's exactly right! Now let's try a quick problem. Here's the question.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    // Start Dialogue from Question1
    public void StartDialogueQuestion1Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[33]);
        SetDialogueState(true, false, DialogueQuestion1Choice3, new Button[] { Answer1Question1, Answer2Question1, Answer3Question1 }, "You have 5 baskets, and each basket holds 2 oranges. How many oranges do you have in total?");
    }
    // Start Answer 1
    public void StartDialogue1Answer1Question1Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Answer1Question1Choice3, new Button[] { Continue1Answer1Question1Choice3 }, "Is it 7?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Answer1Question1Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[34]);
        SetDialogueState(true, false, Dialogue2Answer1Question1Choice3, new Button[] { Continue2Answer1Question1Choice3 }, "Not quite! Remember, multiplication means we're finding the total when you have 5 groups of 2. Try thinking of it as adding 2 five times: 2 + 2 + 2 + 2 + 2. Does that add up to 7?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue3Answer1Question1Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue3Answer1Question1Choice3, new Button[] { Continue3Answer1Question1Choice3 }, "Oh, no, that's 10!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue4Answer1Question1Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[35]);
        SetDialogueState(true, false, Dialogue4Answer1Question1Choice3, new Button[] { Continue4Answer1Question1Choice3 }, "That's it! So, 5 times 2 equals 10. Great job figuring it out! Let's keep going.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    // Start Answer 2
    public void StartDialogue1Answer2Question1Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Answer2Question1Choice3, new Button[] { Continue1Answer2Question1Choice3 }, "I think it's 10.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Answer2Question1Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[59]);
        SetDialogueState(true, false, Dialogue2Answer2Question1Choice3, new Button[] { Continue2Answer2Question1Choice3 }, "That's absolutely correct! 5 times 2 equals 10. Think of it like this: if you have 5 groups of 2, you're adding 2 five times. That makes 10 oranges.");
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
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[36]);
        SetDialogueState(true, false, Dialogue4Answer2Question1Choice3, new Button[] { Continue4Answer2Question1Choice3 }, "I can see that! You're doing great. Let's continue.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    // Start Answer 3
    public void StartDialogue1Answer3Question1Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Answer3Question1Choice3, new Button[] { Continue1Answer3Question1Choice3 }, "Is it 12?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Answer3Question1Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[37]);
        SetDialogueState(true, false, Dialogue2Answer3Question1Choice3, new Button[] { Continue2Answer3Question1Choice3 }, "Not quite. Let's think about it again. If you have 5 baskets and each basket holds 2 oranges, it's like adding 2 five times: 2 + 2 + 2 + 2 + 2. What's that?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue3Answer3Question1Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue3Answer3Question1Choice3, new Button[] { Continue3Answer3Question1Choice3 }, "Oh, it's 10!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue4Answer3Question1Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[38]);
        SetDialogueState(true, false, Dialogue4Answer3Question1Choice3, new Button[] { Continue4Answer3Question1Choice3 }, "Exactly! Multiplication is about grouping numbers, so 5 times 2 equals 10. You've got it!");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue15Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[39]);
        SetDialogueState(true, false, Dialogue15Choice3, new Button[] { Continue15Choice3 }, "Now that you understand the basics, let's try a slightly harder one.");
    }
    // start question2
    public void StartDialogueQuestion2Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[40]);
        SetDialogueState(true, false, DialogueQuestion2Choice3, new Button[] { Answer1Question2, Answer2Question2, Answer3Question2 }, "If a bakery makes 3 trays of cookies and each tray has 8 cookies, how many cookies are there in total?");
    }
    // Start Answer 1
    public void StartDialogue1Answer1Question2Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Answer1Question2Choice3, new Button[] { Continue1Answer1Question2Choice3 }, "I think it's 24.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Answer1Question2Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[41]);
        SetDialogueState(true, false, Dialogue2Answer1Question2Choice3, new Button[] { Continue2Answer1Question2Choice3 }, "That's absolutely right! 3 trays with 8 cookies each means you’re adding 8 three times: 8 + 8 + 8. Multiplication is just a shortcut for this, so 3 times 8 equals 24. Great job!");
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
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[42]);
        SetDialogueState(true, false, Dialogue4Answer1Question2Choice3, new Button[] { Continue4Answer1Question2Choice3 }, "I'm glad to hear that! Let's keep the momentum going.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    // Start Answer 2
    public void StartDialogue1Answer2Question2Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Answer2Question2Choice3, new Button[] { Continue1Answer2Question2Choice3 }, "Is it 16?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Answer2Question2Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[43]);
        SetDialogueState(true, false, Dialogue2Answer2Question2Choice3, new Button[] { Continue2Answer2Question2Choice3 }, "Not quite, but that's okay! Let's think about it again. If you have 3 trays of cookies, and each tray has 8 cookies, you're adding 8 three times: 8 + 8 + 8. What does that add up to?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue3Answer2Question2Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue3Answer2Question2Choice3, new Button[] { Continue3Answer2Question2Choice3 }, "Oh, it's 24!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue4Answer2Question2Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[44]);
        SetDialogueState(true, false, Dialogue4Answer2Question2Choice3, new Button[] { Continue4Answer2Question2Choice3 }, "Exactly! 3 times 8 equals 24. You've got it now!");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    // Start Answer 3
    public void StartDialogue1Answer3Question2Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue1Answer3Question2Choice3, new Button[] { Continue1Answer3Question2Choice3 }, "Is it 32?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue2Answer3Question2Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[45]);
        SetDialogueState(true, false, Dialogue2Answer3Question2Choice3, new Button[] { Continue2Answer3Question2Choice3 }, "Let's break it down. You have 3 trays, and each tray has 8 cookies. Think of it like adding 8 three times: 8 + 8 + 8. What does that add up to?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue3Answer3Question2Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue3Answer3Question2Choice3, new Button[] { Continue3Answer3Question2Choice3 }, "Oh, it's 24!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue4Answer3Question2Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[46]);
        SetDialogueState(true, false, Dialogue4Answer3Question2Choice3, new Button[] { Continue4Answer3Question2Choice3 }, "That's right! Multiplication helps us find totals quickly. 3 times 8 equals 24. Great job figuring it out!");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue16Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[47]);
        SetDialogueState(true, false, Dialogue16Choice3, new Button[] { Continue16Choice3 }, "Now that you've got the hang of it, let's talk about multiplying more than two numbers. For example, let's solve 2 x 3 x 4.");
    }
    public void StartDialogue17Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue17Choice3, new Button[] { Continue17Choice3 }, "How do I do that?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue18Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[48]);
        SetDialogueState(true, false, Dialogue18Choice3, new Button[] { Continue18Choice3 }, "Easy! Start by multiplying two of the numbers first. Let's do 2 times 3. What's 2 times 3?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue19Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue19Choice3, new Button[] { Continue19Choice3 }, "That's 6!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue20Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[49]);
        SetDialogueState(true, false, Dialogue20Choice3, new Button[] { Continue20Choice3 }, "Correct! Now, take that 6 and multiply it by the next number, which is 4. What's 6 times 4?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue21Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue21Choice3, new Button[] { Continue21Choice3 }, "That's 24!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue22Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[50]);
        SetDialogueState(true, false, Dialogue22Choice3, new Button[] { Continue22Choice3 }, "You've got it! So, 2 x 3 x 4 equals 24. The trick is to go step by step, solving two numbers at a time.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue23Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue23Choice3, new Button[] { Continue23Choice3 }, "Oh, I see! Just multiply two numbers at a time.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue24Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[51]);
        SetDialogueState(true, false, Dialogue24Choice3, new Button[] { Continue24Choice3 }, "Exactly. Now let's talk about a special rule. What happens when you multiply by 1 or 0?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue25Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue25Choice3, new Button[] { Continue25Choice3 }, "Hmm, I'm not sure.");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue26Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[52]);
        SetDialogueState(true, false, Dialogue26Choice3, new Button[] { Continue26Choice3 }, "Let's start with multiplying by 1. If you multiply any number by 1, the answer is always the same number. For example, 5 times 1 equals?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue27Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue27Choice3, new Button[] { Continue27Choice3 }, "5?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue28Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[53]);
        SetDialogueState(true, false, Dialogue28Choice3, new Button[] { Continue28Choice3 }, "That's correct! Multiplying by 1 doesn't change the number. It's like saying, “I have one group of something.”");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue29Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue29Choice3, new Button[] { Continue29Choice3 }, "Oh, that's simple!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue30Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[54]);
        SetDialogueState(true, false, Dialogue30Choice3, new Button[] { Continue30Choice3 }, "It is! Now, what about multiplying by 0?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue31Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue31Choice3, new Button[] { Continue31Choice3 }, "Hmm... Does it make everything 0?");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue32Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[55]);
        SetDialogueState(true, false, Dialogue32Choice3, new Button[] { Continue32Choice3 }, "Exactly! If you multiply any number by 0, the answer is always 0. For example, 7 times 0 equals?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue33Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue33Choice3, new Button[] { Continue33Choice3 }, "0!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue34Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[56]);
        SetDialogueState(true, false, Dialogue34Choice3, new Button[] { Continue34Choice3 }, "That's right! Multiplying by 0 means you have nothing at all. It's like having zero groups of something.");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue35Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue35Choice3, new Button[] { Continue35Choice3 }, "That makes sense!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue36Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[57]);
        SetDialogueState(true, false, Dialogue36Choice3, new Button[] { Continue36Choice3 }, "You're doing great! Multiplication is all about understanding the rules and practicing. Now, I think you're ready to solve on your own. Are you ready?");
        ShowCharacter(false);
        npcAnimator.SetBool("isTalking", true);
    }
    public void StartDialogue37Choice3()
    {
        audioManager.StopCurrentSFX();
        SetDialogueState(false, true, Dialogue37Choice3, new Button[] { Continue37Choice3 }, "I'm ready! Thanks for teaching me!");
        ShowCharacter(true);
        npcAnimator.SetBool("isTalking", false);
    }
    public void StartDialogue38Choice3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart2Clips[58]);
        SetDialogueState(true, false, Dialogue38Choice3, new Button[] { EndChoice3 }, "You're welcome! Good luck");
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
        TMP_Text[] dialogues = { startDialogue, Dialogue1Choice1, Dialogue2Choice1, Dialogue3Choice1, Dialogue4Choice1, Dialogue5Choice1, Dialogue6Choice1, Dialogue7Choice1, Dialogue8Choice1,Dialogue9Choice1, Dialogue10Choice1, Dialogue11Choice1, Dialogue1Choice2, Dialogue2Choice2, Dialogue3Choice2, Dialogue4Choice2,Dialogue5Choice2, Dialogue6Choice2, Dialogue7Choice2, Dialogue8Choice2, Dialogue9Choice2, Dialogue10Choice2,Dialogue11Choice2, Dialogue12Choice2, Dialogue13Choice2, Dialogue14Choice2, Dialogue15Choice2, Dialogue16Choice2,Dialogue17Choice2, Dialogue18Choice2, Dialogue19Choice2, Dialogue20Choice2, Dialogue21Choice2, Dialogue22Choice2, Dialogue23Choice2, Dialogue24Choice2, Dialogue25Choice2, Dialogue26Choice2, Dialogue27Choice2, Dialogue28Choice2, Dialogue29Choice2, Dialogue1Choice3, Dialogue2Choice3, Dialogue3Choice3, Dialogue4Choice3, Dialogue5Choice3, Dialogue6Choice3, Dialogue7Choice3, Dialogue8Choice3, Dialogue9Choice3, Dialogue10Choice3, Dialogue11Choice3, Dialogue12Choice3, Dialogue13Choice3, Dialogue14Choice3, DialogueQuestion1Choice3, Dialogue1Answer1Question1Choice3, Dialogue2Answer1Question1Choice3, Dialogue3Answer1Question1Choice3, Dialogue4Answer1Question1Choice3, Dialogue1Answer2Question1Choice3, Dialogue2Answer2Question1Choice3, Dialogue3Answer2Question1Choice3, Dialogue4Answer2Question1Choice3, Dialogue1Answer3Question1Choice3, Dialogue2Answer3Question1Choice3, Dialogue3Answer3Question1Choice3, Dialogue4Answer3Question1Choice3, Dialogue15Choice3, DialogueQuestion2Choice3, Dialogue1Answer1Question2Choice3, Dialogue2Answer1Question2Choice3, Dialogue3Answer1Question2Choice3, Dialogue4Answer1Question2Choice3, Dialogue1Answer2Question2Choice3, Dialogue2Answer2Question2Choice3, Dialogue3Answer2Question2Choice3, Dialogue4Answer2Question2Choice3, Dialogue1Answer3Question2Choice3, Dialogue2Answer3Question2Choice3, Dialogue3Answer3Question2Choice3, Dialogue4Answer3Question2Choice3, Dialogue16Choice3, Dialogue17Choice3, Dialogue18Choice3, Dialogue19Choice3, Dialogue20Choice3, Dialogue21Choice3, Dialogue22Choice3, Dialogue23Choice3, Dialogue24Choice3, Dialogue25Choice3, Dialogue26Choice3, Dialogue27Choice3, Dialogue28Choice3,Dialogue29Choice3, Dialogue30Choice3, Dialogue31Choice3, Dialogue32Choice3, Dialogue33Choice3,Dialogue34Choice3, Dialogue35Choice3, Dialogue36Choice3, Dialogue37Choice3, Dialogue38Choice3 };
        Button[] allButtons = { Choice1, Choice2, Choice3, Continue1Choice1, Continue2Choice1, Continue3Choice1, Continue1Choice1, Continue2Choice1, Continue3Choice1, Continue1Choice1, Continue2Choice1, Continue3Choice1, Continue1Choice1, EndChoice1, Continue1Choice2, Continue2Choice2, Continue3Choice2, Continue4Choice2,Continue5Choice2, Continue6Choice2, Continue7Choice2, Continue8Choice2, Continue9Choice2, Continue10Choice2,Continue11Choice2, Continue12Choice2, Continue13Choice2, Continue14Choice2, Continue15Choice2,Continue16Choice2,Continue17Choice2, Continue18Choice2, Continue19Choice2, Continue20Choice2, Continue21Choice2, Continue22Choice2, Continue23Choice2, Continue24Choice2, Continue25Choice2, Continue26Choice2, Continue27Choice2, Continue28Choice2, EndChoice2, Continue1Choice3, Continue2Choice3, Continue3Choice3, Continue4Choice3, Continue5Choice3, Continue6Choice3, Continue7Choice3, Continue8Choice3 ,Continue9Choice3 ,Continue10Choice3 ,Continue11Choice3 ,Continue12Choice3, Continue13Choice3, Continue14Choice3, Answer1Question1, Answer2Question1, Answer3Question1, Continue1Answer1Question1Choice3, Continue2Answer1Question1Choice3, Continue3Answer1Question1Choice3, Continue4Answer1Question1Choice3,Continue1Answer2Question1Choice3, Continue2Answer2Question1Choice3, Continue3Answer2Question1Choice3, Continue4Answer2Question1Choice3, Continue1Answer3Question1Choice3, Continue2Answer3Question1Choice3, Continue3Answer3Question1Choice3, Continue4Answer3Question1Choice3, Continue15Choice3, Answer1Question2, Answer2Question2, Answer3Question2, Continue1Answer1Question2Choice3, Continue2Answer1Question2Choice3, Continue3Answer1Question2Choice3, Continue4Answer1Question2Choice3,Continue1Answer2Question2Choice3, Continue2Answer2Question2Choice3, Continue3Answer2Question2Choice3, Continue4Answer2Question2Choice3, Continue1Answer3Question2Choice3, Continue2Answer3Question2Choice3, Continue3Answer3Question2Choice3, Continue4Answer3Question2Choice3, Continue16Choice3, Continue17Choice3, Continue18Choice3, Continue19Choice3, Continue20Choice3, Continue21Choice3, Continue22Choice3, Continue23Choice3,Continue24Choice3, Continue25Choice3, Continue26Choice3, Continue27Choice3, Continue28Choice3, Continue29Choice3, Continue30Choice3, Continue31Choice3, Continue32Choice3, Continue33Choice3, Continue34Choice3, Continue35Choice3, Continue36Choice3, Continue37Choice3, EndChoice3};
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
        stage8GameTrialHandler.OpenStage8Trial();
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