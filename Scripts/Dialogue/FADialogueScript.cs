using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine.SceneManagement;
using Firebase.Extensions;
using Firebase;
using System.Collections.Generic;

public class FADialogueScript: MonoBehaviour
{
    public Image playerDisplayImage; 
    public Image npcDisplayImage;  
    public Sprite boyImage; 
    public Sprite girlImage;
     public GameObject loadingScreen;
    public Slider slider;
    public ArrowFA arrowFa;
    public TMP_Text TextLogUsername;
    public GameObject FAPanel;
    public GameObject triggerBox;
    private GameObject player;
    private PlayerControl playerController; 
    private LoadCharacter loadCharacter;

    [Header("Start Dialogue")]
    public TMP_Text startDialogue;
    public Button Continue1;

    [Header("Continue1")]
    public TMP_Text Dialogue1;
    public Button Continue2;

    [Header("Username Input")]
    public TMP_Text Dialogue2;
    public TMP_InputField usernameInputField;
    public Button SubmitUsernameButton;

    [Header("Feedback Dialogue")]
    public TMP_Text FeedbackDialogue;
    public Button Continue3;

    [Header("Continue Dialogue3")]
    public TMP_Text Dialogue3;
    public Button Continue4;
    
    [Header("ContinueDialogue4")]
    public TMP_Text Dialogue4;
    public Button Continue5;

    [Header("ContinueDialogue5")]
    public TMP_Text Dialogue5;
    public Button Continue6;

    [Header("ContinueDialogue6")]
    public TMP_Text Dialogue6;
    public Button Continue7;

    [Header("ContinueDialogue7")]
    public TMP_Text Dialogue7;
    public Button Continue8;

    [Header("ContinueDialogue8")]
    public TMP_Text Dialogue8;
    public Button Continue9;

    [Header("ContinueDialogue9")]
    public TMP_Text Dialogue9;
    public Button Continue10;

    [Header("ContinueDialogue10")]
    public TMP_Text Dialogue10;
    public Button Continue11;

    [Header("ContinueDialogue11")]
    public TMP_Text Dialogue11;
    public Button Continue12;

    [Header("ContinueDialogue12")]
    public TMP_Text Dialogue12;
    public Button Continue13;

    [Header("ContinueDialogue13")]
    public TMP_Text Dialogue13;
    public Button Continue14;

    [Header("ContinueDialogue14")]
    public TMP_Text Dialogue14;
    public Button Continue15;

    [Header("ContinueDialogue15")]
    public TMP_Text Dialogue15;
    public Button Continue16;

    [Header("ContinueDialogue16")]
    public TMP_Text Dialogue16;
    public Button Continue17;

    [Header("ContinueDialogue17")]
    public TMP_Text Dialogue17;
    public Button Continue18;

    [Header("ContinueDialogue18")]
    public TMP_Text Dialogue18;
    public Button Continue19;

    [Header("ContinueDialogue19")]
    public TMP_Text Dialogue19;
    public Button Continue20;

    [Header("ContinueDialogue20")]
    public TMP_Text Dialogue20;
    public Button proceedToBattleButton;

    private AudioManager audioManager;
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene.");
            return;
        }
        StartCoroutine(WaitForFirebaseInitialization());

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
    private IEnumerator WaitForFirebaseInitialization()
    {
        yield return new WaitUntil(() => FirebaseManager.Instance.IsFirebaseInitialized());
        TextLogUsername.text = "Firebase initialized successfully.";
        StartCoroutine(InitializePlayer());
        SubmitUsernameButton.onClick.AddListener(SubmitUsername);
        Continue1.onClick.AddListener(StartDialogue1);
        Continue2.onClick.AddListener(StartDialogue2);
        proceedToBattleButton.onClick.AddListener(() => GoToScene("BattleTrial"));
        CheckInternetConnectivity();
        MonitorFirebaseConnection(); 
    }
    void CheckInternetConnectivity()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.LogError("No internet connection.");
        }
        else
        {
            Debug.Log("Internet connection available.");
        }
    }
    private void MonitorFirebaseConnection()
    {
        var connectedRef = FirebaseDatabase.DefaultInstance.GetReference(".info/connected");
        connectedRef.ValueChanged += (object sender, ValueChangedEventArgs args) =>
        {
            if (args.DatabaseError != null)
            {
                Debug.LogError(args.DatabaseError.Message);
                TextLogUsername.text = "" + args.DatabaseError.Message;
                return;
            }
            bool isConnected = (bool)args.Snapshot.Value;
            if (isConnected)
            {
                TextLogUsername.text = "Firebase is connected" ;
            }
            else
            {
                TextLogUsername.text = "Firebase is not connected";
            }
        };
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
        if (FAPanel != null) 
            FAPanel.SetActive(true);

        TMP_Text[] dialogues = 
        {
            startDialogue, Dialogue1, Dialogue2, FeedbackDialogue, Dialogue3, Dialogue4, Dialogue5, Dialogue6, Dialogue7, Dialogue8, Dialogue9, Dialogue10, Dialogue11, Dialogue12, Dialogue13, Dialogue14, Dialogue15, Dialogue16, Dialogue17, Dialogue18, Dialogue19, Dialogue20  
        };
        
        Button[] allButtons = 
        {
            Continue1, Continue2, SubmitUsernameButton, Continue3, Continue4,  Continue5, Continue6, Continue7, Continue8, Continue9, Continue10, Continue11, Continue12, Continue13, Continue14, Continue15, Continue16, Continue17, Continue18, Continue19, Continue20, proceedToBattleButton 
        };

        foreach (TMP_Text dialogue in dialogues)
        {
            if (dialogue != null) 
                dialogue.gameObject.SetActive(false);
        }
        foreach (Button button in allButtons)
        {
            if (button != null) 
                button.gameObject.SetActive(false);
        }
        if (activeDialogue != null)
        {
            activeDialogue.gameObject.SetActive(true);
            activeDialogue.GetComponent<TypeWriterEffect>()?.StartTypewriterEffect(dialogueText, () =>
            {
                foreach (Button button in activeButtons)
                {
                    if (button != null) 
                        button.gameObject.SetActive(true);
                }
            });
        }
    }
    public void ConvoStart()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[0]);
        arrowFa.triggerBox.gameObject.SetActive(false);
        arrowFa.playerArrow.gameObject.SetActive(false);
        SetDialogueState(startDialogue, new Button[] { Continue1 }, "This is the Fighter Association. Now that we're here, I just realized I still don't know your name and I haven't given you mine. My name is Roger.");
        ShowCharacter(false);
        if (playerController != null)
        {
            playerController.enabled = false;
        }
    }
    public void StartDialogue1()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[1]);
        SetDialogueState(Dialogue1, new Button[] { Continue2 }, "So, what should I call you?");
    }
    public void StartDialogue2()
    {
        SetDialogueState(Dialogue2, new Button[] { SubmitUsernameButton }, "Enter your username here:");
        usernameInputField.gameObject.SetActive(true); // Show the input field
    }
    public void SubmitUsername()
    {
        string username = usernameInputField.text;

        // Check if username exceeds the maximum length
        if (username.Length > 12)
        {
            SetDialogueState(Dialogue2, new Button[] { SubmitUsernameButton }, "Username too long, please try again.");
            return;
        }
        if (!string.IsNullOrEmpty(username))
        {
            TextLogUsername.text = "You entered: " + username;
            StartCoroutine(CheckUsernameExists(username));
        }
        else
        {
            SetDialogueState(FeedbackDialogue, new Button[] { Continue2 }, "Username cannot be empty. Please enter a valid username.");
            usernameInputField.gameObject.SetActive(false); 
        }
    }
    private IEnumerator CheckUsernameExists(string username)
    {
        TextLogUsername.text = "Checking if username exists: " + username;

        var dbReference = FirebaseDatabase.DefaultInstance.GetReference("usernames");
        var task = dbReference.GetValueAsync();
        float timeout = 10f; 
        float timer = 0f;

        while (!task.IsCompleted && timer < timeout)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        if (timer >= timeout)
        {
            TextLogUsername.text = "Request timed out.";
            SetDialogueState(Dialogue2, new Button[] { SubmitUsernameButton }, "Request timed out. Please try again.");
            yield break;
        }
        if (task.Exception != null)
        {
            TextLogUsername.text = $"Error checking username: {task.Exception}";
            SetDialogueState(Dialogue2, new Button[] { SubmitUsernameButton }, "Error checking username. Please try again.");
        }
        else if (task.Result != null && task.Result.Value != null)
        {
            var userId = FirebaseAuth.DefaultInstance.CurrentUser?.UserId;

            if (string.IsNullOrEmpty(userId))
            {
                TextLogUsername.text = "No user signed in.";
                SetDialogueState(Dialogue2, new Button[] { SubmitUsernameButton }, "No user signed in. Please sign in and try again.");
                yield break;
            }

            bool usernameExists = false;

            foreach (var child in (Dictionary<string, object>)task.Result.Value)
            {
                if (child.Key == username)
                {
                    if (child.Value.ToString() != userId)
                    {
                        usernameExists = true;
                        break;
                    }
                }
            }
            if (usernameExists)
            {
                TextLogUsername.text = "Username already exists. Please choose another.";
                SetDialogueState(Dialogue2, new Button[] { SubmitUsernameButton }, "Username already taken. Please choose a different one.");
            }
            else
            {
                TextLogUsername.text = "Username available: " + username;
                StartCoroutine(UpdateUsername(username));
            }
        }
        else{
            TextLogUsername.text = "Username available: " + username;
            StartCoroutine(UpdateUsername(username));
        }
    }

    private IEnumerator UpdateExistingUsername(string existingUsername, string newUsername)
    {
        var dbReference = FirebaseDatabase.DefaultInstance.GetReference("usernames");

        var removeTask = dbReference.Child(existingUsername).RemoveValueAsync();
        yield return new WaitUntil(() => removeTask.IsCompleted);

        if (removeTask.Exception != null)
        {
            TextLogUsername.text = $"Failed to remove old username: {removeTask.Exception}";
            SetDialogueState(Dialogue2, new Button[] { SubmitUsernameButton }, "Failed to update username. Please try again.");
            yield break;
        }

        // Add the new username
        var userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        var setTask = dbReference.Child(newUsername).SetValueAsync(userId);
        yield return new WaitUntil(() => setTask.IsCompleted);

        if (setTask.Exception != null)
        {
            TextLogUsername.text = "Failed to update database: " + setTask.Exception;
            SetDialogueState(Dialogue2, new Button[] { SubmitUsernameButton }, "Failed to update database. Please try again.");
        }
        else
        {
            audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[2]);
            usernameInputField.gameObject.SetActive(false);
            TextLogUsername.text = "Username updated successfully!";
            SetDialogueState(FeedbackDialogue, new Button[] { Continue3 }, $"Neat name! Well, it's great to know you {newUsername}.");
        }
    }

    private IEnumerator UpdateUsername(string username)
    {
        var auth = FirebaseAuth.DefaultInstance;
        var user = auth.CurrentUser;

        if (user != null)
        {
            var profile = new UserProfile { DisplayName = username };
            var profileTask = user.UpdateUserProfileAsync(profile);

            yield return new WaitUntil(() => profileTask.IsCompleted);

            if (profileTask.Exception != null)
            {
                TextLogUsername.text = $"Failed to update username: {profileTask.Exception}";
                SetDialogueState(Dialogue2, new Button[] { SubmitUsernameButton }, "Failed to set username. Please try again.");
            }
            else
            {
                var dbReference = FirebaseDatabase.DefaultInstance.GetReference("usernames").Child(username);
                var setTask = dbReference.SetValueAsync(user.UserId);

                yield return new WaitUntil(() => setTask.IsCompleted);

                if (setTask.Exception != null)
                {
                    TextLogUsername.text = "Failed to update database: " + setTask.Exception;
                    SetDialogueState(Dialogue2, new Button[] { SubmitUsernameButton }, "Failed to update database. Please try again.");
                }
                else
                {
                    audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[2]);
                    usernameInputField.gameObject.SetActive(false);
                    TextLogUsername.text = "Username updated successfully!";
                    SetDialogueState(FeedbackDialogue, new Button[] { Continue3 }, $"Neat name! Well, it's great to know you {username}.");
                }
            }
        }
        else
        {
            TextLogUsername.text = "No user signed in.";
            SetDialogueState(Dialogue2, new Button[] { SubmitUsernameButton }, "No user signed in. Please sign in and try again.");
        }
    }
    public void StartDialogue3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[3]);
        SetDialogueState(Dialogue3, new Button[] { Continue4 }, "So I've asked around and it's both bad news and good news.");
    }
    public void StartDialogue4()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[4]);
        SetDialogueState(Dialogue4, new Button[] { Continue5 }, "The good news is that apparently there is a portal that can bring you back.");
    }
    public void StartDialogue5()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[5]);
        SetDialogueState(Dialogue5, new Button[] { Continue6 }, "It seemed like the Powerful Solver often talked about it before.");
    }
    public void StartDialogue6()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[6]);
        SetDialogueState(Dialogue6, new Button[] { Continue7 }, "The bad news though is that no one knows exactly where the portal is.");
    }
    public void StartDialogue7()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[7]);
        SetDialogueState(Dialogue7, new Button[] { Continue8 }, "Fortunately, I still found some clues after some more searching.");
    }
    public void StartDialogue8()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[8]);
        SetDialogueState(Dialogue8, new Button[] { Continue9 }, "Apparently there is a wizard living in a desert called Zerim.");
    }
    public void StartDialogue9()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[9]);
        SetDialogueState(Dialogue9, new Button[] { Continue10 }, "I heard the Powerful Solver seeked before about the same matter. Maybe they can help you.");
    }
    public void StartDialogue10()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[10]);
        SetDialogueState(Dialogue10, new Button[] { Continue11 }, "You'll have to begin your journey from here on out to try and get back home.");
    }
    public void StartDialogue11()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[11]);
        SetDialogueState(Dialogue11, new Button[] { Continue12 }, "So before I let you scurry off, let me show you how to fend for yourself.");
    }
    public void StartDialogue12()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[12]);
        SetDialogueState(Dialogue12, new Button[] { Continue13 }, "You look like a smart kid, I'm assuming you know how to do addition?");
    }
    public void StartDialogue13()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[13]);
        SetDialogueState(Dialogue13, new Button[] { Continue14}, "Still, it can't hurt to do a little refresh, can it?");
    }
    public void StartDialogue14()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[14]);
        SetDialogueState(Dialogue14, new Button[] { Continue15 }, "Addition is one of the building blocks of math, and it's something we use every day, whether we're counting our coins or figuring out how many apples are in the basket.");
    }
    public void StartDialogue15()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[15]);
        SetDialogueState(Dialogue15, new Button[] { Continue16 }, "It's simple, really! You just take one number, and add it to another.");
    }
    public void StartDialogue16()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[16]);
        SetDialogueState(Dialogue16, new Button[] { Continue17 }, "For example, if you have 3 apples and someone gives you 2 more, how many do you have now?");
    }
    public void StartDialogue17()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[17]);
        SetDialogueState(Dialogue17, new Button[] { Continue18 }, "That's right, 3 plus 2 equals 5! Think of it like putting things together");
    }
    public void StartDialogue18()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[18]);
        SetDialogueState(Dialogue18, new Button[] { Continue19 }, "Each number you add is like another puzzle piece. The more pieces you add, the bigger the picture gets!");
    }
    public void StartDialogue19()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[19]);
        SetDialogueState(Dialogue19, new Button[] { proceedToBattleButton }, "Well, that should be it. How about trying a trial battle on your own?");
    }
    public void GoToScene(string sceneName)
    {
        loadingScreen.SetActive(true);
        slider.value = 0;
        audioManager.StopMusic();
        StartCoroutine(LoadAsynchronously(sceneName));
    }
    IEnumerator LoadAsynchronously(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false; 
        float targetProgress = 0f;
        while (operation.progress < 0.9f)
        {
            targetProgress = Mathf.Clamp01(operation.progress / 0.9f);
            while (slider.value < targetProgress)
            {
                slider.value = Mathf.MoveTowards(slider.value, targetProgress, Time.deltaTime * 0.5f); 
                yield return null; 
            }
            yield return null;
        }
        targetProgress = 1f;
        while (slider.value < targetProgress)
        {
            slider.value = Mathf.MoveTowards(slider.value, targetProgress, Time.deltaTime * 0.5f); 
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        operation.allowSceneActivation = true;
    }
    public void ConvoEnd()
    {
        audioManager.StopCurrentSFX();
        if (FAPanel != null) FAPanel.SetActive(false);

        TMP_Text[] dialogues = 
        {
            startDialogue, Dialogue1, Dialogue2, FeedbackDialogue, Dialogue3, Dialogue4, Dialogue5, Dialogue6, Dialogue7, Dialogue8, Dialogue9, Dialogue10, Dialogue11, Dialogue12, Dialogue13, Dialogue14, Dialogue15, Dialogue16, Dialogue17, Dialogue18, Dialogue19, Dialogue20  
        };
        
        Button[] allButtons = 
        {
            Continue1, Continue2, SubmitUsernameButton, Continue3, Continue4,  Continue5, Continue6, Continue7, Continue8, Continue9, Continue10, Continue11, Continue12, Continue13, Continue14, Continue15, Continue16, Continue17, Continue18, Continue19, Continue20, proceedToBattleButton 
        };

        foreach (TMP_Text dialogue in dialogues)
        {
            if (dialogue != null) dialogue.gameObject.SetActive(false);
        }
        foreach (Button button in allButtons)
        {
            if (button != null) button.gameObject.SetActive(false);
        }
        if (usernameInputField != null) usernameInputField.gameObject.SetActive(false); 
        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }
}