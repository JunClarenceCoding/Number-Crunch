using System.Collections;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class MainGameModes : MonoBehaviour
{
    public MainCharacterLoader characterLoader;
    public MainArrowToNPC arrowToNPC;
    public GameObject NotUnlockedPanel, NotFinishTask4Panel,NotFinishTask8Panel, NotFinishTask12Panel, NotFinishTask16Panel, ChainImage;
    public GameObject MainGameModesPanel;
    public GameObject StageSelectionPanel;
    public GameObject Stage4Task, Stage8Task, Stage12Task, Stage16Task;
    public GameObject loadingScreen;
    public Slider slider;
    public Button[] stageButtons;
    private DatabaseReference databaseReference;
    private string userId;
    private FirebaseAuth auth;
    public GameObject endlessmodePanel;
    public TMP_Text maxWaveDisplay;
    public Animator endlessAnimatorAnim;
    private AudioManager audioManager;
    public Color activeColor = Color.white; 
    public Color inactiveColor = new Color(1, 1, 1, 0.5f);
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene.");
            return;
        }
        StartCoroutine(WaitForFirebaseInitialization());
    }
    private IEnumerator WaitForFirebaseInitialization()
    {
        yield return new WaitUntil(() => FirebaseManager.Instance != null && FirebaseManager.Instance.IsFirebaseInitialized());
        InitializeAndLoadData();
    }
    private void InitializeAndLoadData()
    {
        auth = FirebaseManager.Instance.Auth;
        databaseReference = FirebaseManager.Instance.Database.RootReference;
        if (auth.CurrentUser != null)
        {
            userId = auth.CurrentUser.UserId;
            LoadStageData();
            CheckStage4Cleared();
            CheckStage8Cleared();
            CheckStage12Cleared();
            CheckStage16Cleared();
            CheckStage4Status();
            CheckStage8Status();
            CheckStage12Status();
            CheckStage16Status();
        }
        else
        {
            SceneManager.LoadScene("LoginScene"); 
        }
    }
    public void ShowEndlessModeMaxWave()
    {
        if (databaseReference == null || string.IsNullOrEmpty(userId))
        {
            Debug.Log("Firebase or user ID not initialized.");
            return;
        }
        databaseReference.Child("users").Child(userId).Child("maxWave")
        .GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && task.Result.Exists)
            {
                int maxWave = int.Parse(task.Result.Value.ToString());
                StartCoroutine(DisplayEndlessModePanel(maxWave));
            }
        });
    }
    private IEnumerator DisplayEndlessModePanel(int maxWave)
    {
        endlessmodePanel.SetActive(true);
        endlessAnimatorAnim.Play("ShowMonster");
        yield return new WaitForSeconds(0.2f);
        maxWaveDisplay.text = $"{maxWave}";
    }
    public void CloseEndlessModePanel()
    {
        endlessmodePanel.SetActive(false);
    }
    private void LoadStageData()
    {
        if (databaseReference == null)
        {
            Debug.Log("Firebase is not initialized.");
            return;
        }
        databaseReference.Child("users").Child(userId).Child("stages").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && task.Result != null)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    int index = 0;
                    foreach (var stage in snapshot.Children)
                    {
                        bool cleared = bool.Parse(stage.Child("cleared").Value.ToString());
                        bool unlocked = bool.Parse(stage.Child("unlocked").Value.ToString());
                        Button button = stageButtons[index];
                        button.interactable = unlocked;
                        button.GetComponent<Image>().color = unlocked ? activeColor : inactiveColor;
                        Outline outline = button.GetComponent<Outline>();
                        if (outline == null)
                        {
                            outline = button.gameObject.AddComponent<Outline>();
                        }
                        outline.enabled = !unlocked;
                        outline.effectColor = Color.gray;
                        outline.effectDistance = new Vector2(2, 2);
                        index++;
                    }
                }
                else
                {
                    InitializeStages();
                }
            }
            else if (task.IsFaulted)
            {
                Debug.Log("Failed to load stage data: " + task.Exception?.Flatten()?.ToString());
            }
            else
            {
                Debug.Log("No data returned from Firebase. Check if the user is authenticated.");
            }
        });
    }
    private void InitializeStages()
    {
        databaseReference.Child("users").Child(userId).Child("stages").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && task.Result.Exists)
            {
                Debug.Log("Stages are already initialized. Skipping initialization.");
                return; 
            }
            Dictionary<string, object> stageData = new Dictionary<string, object>();
            for (int i = 0; i < stageButtons.Length; i++)
            {
                int stageIndex = i + 1;
                string stageKey = "stage" + stageIndex.ToString("D2");
                bool isUnlocked = (i == 0); 
                stageData[$"users/{userId}/stages/{stageKey}/unlocked"] = isUnlocked;
                stageData[$"users/{userId}/stages/{stageKey}/cleared"] = false;
            }
            databaseReference.UpdateChildrenAsync(stageData).ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    LoadStageData();
                }
            });
        });
    }
    public void GoToScene(string sceneName, int stageIndex)
    {
        if (stageButtons[stageIndex].interactable)
        {
            loadingScreen.SetActive(true);
            slider.value = 0; 
            StartCoroutine(LoadAsynchronously(sceneName));
        }
    }
    public void GoToEndlessMode(string sceneName)
    {
        loadingScreen.SetActive(true);
        slider.value = 0;
        audioManager.StopMusic();
        StartCoroutine(LoadAsynchronously(sceneName));
    }
    public void LoadStage1() => GoToScene("Stage1", 0);
    public void LoadStage2() => GoToScene("Stage2", 1);
    public void LoadStage3() => GoToScene("Stage3", 2);
    public void LoadStage4() => GoToScene("Stage4Boss", 3);
    public void LoadStage5() => GoToScene("Stage5", 4);
    public void LoadStage6() => GoToScene("Stage6", 5);
    public void LoadStage7() => GoToScene("Stage7", 6);
    public void LoadStage8() => GoToScene("Stage8Boss", 7);
    public void LoadStage9() => GoToScene("Stage9", 8);
    public void LoadStage10() => GoToScene("Stage10", 9);
    public void LoadStage11() => GoToScene("Stage11", 10);
    public void LoadStage12() => GoToScene("Stage12Boss", 11);
    public void LoadStage13() => GoToScene("Stage13", 12);
    public void LoadStage14() => GoToScene("Stage14", 13);
    public void LoadStage15() => GoToScene("Stage15", 14);
    public void LoadStage16() => GoToScene("Stage16Boss", 15);
    public void LoadStage17() => GoToScene("Stage17", 16);
    public void LoadStage18() => GoToScene("Stage18", 17);
    public void LoadStage19() => GoToScene("Stage19", 18);
    public void LoadStage20() => GoToScene("Stage20Boss", 19);
    private IEnumerator LoadAsynchronously(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        while (operation.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            yield return null;
        }
        slider.value = 1f;
        yield return new WaitForSeconds(1f);
        operation.allowSceneActivation = true;
    }
    private IEnumerator OpenGameModeWithDelay()
    {
        SetPanelState(true, false);
        yield return new WaitForSeconds(0.1f);
        CheckActivateChainEndless();
    }
    public void GoToSelectionStage() => SetPanelState(false, true);
    public void GoToMainGameModes()
    {
        StartCoroutine(OpenGameModeWithDelay());
    }
    public void ClosePanel() => SetPanelState(false, false);
    private void SetPanelState(bool mainPanelState, bool stagePanelState)
    {
        if (MainGameModesPanel != null)
        {
            MainGameModesPanel.SetActive(mainPanelState);
        }

        if (StageSelectionPanel != null)
        {
            StageSelectionPanel.SetActive(stagePanelState);
        }
    }
    public void CheckStage21Unlocked()
    {
        if (databaseReference == null || auth.CurrentUser == null)
        {
            Debug.Log("Firebase or user not initialized.");
            return;
        }
        databaseReference.Child("users").Child(userId).Child("stages").Child("stage21").Child("unlocked")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && task.Result.Exists)
                {
                    bool isUnlocked = bool.Parse(task.Result.Value.ToString());
                    if (isUnlocked)
                    {
                        ShowEndlessModeMaxWave();
                    }
                    else
                    {
                        NotUnlockedPanel.SetActive(true);
                    }
                }
            });
    }
    public void CloseNotUnlockedPanel()
    {
        NotUnlockedPanel.SetActive(false);
    }
    public void CheckStage4Cleared()
    {
        if (databaseReference == null || auth.CurrentUser == null)
        {
            Debug.Log("Firebase or user not initialized.");
            return;
        }
        databaseReference.Child("users").Child(userId).Child("stages").Child("stage04").Child("cleared")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && task.Result.Exists)
                {
                    bool isCleared = bool.Parse(task.Result.Value.ToString());

                    if (isCleared)
                    {
                        Stage4Task.gameObject.SetActive(true);
                    }
                    else
                    {
                        Stage4Task.gameObject.SetActive(false);
                    }
                }
            });
    }
    public void CheckStage8Cleared()
    {
        if (databaseReference == null || auth.CurrentUser == null)
        {
            Debug.Log("Firebase or user not initialized.");
            return;
        }
        databaseReference.Child("users").Child(userId).Child("stages").Child("stage08").Child("cleared")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && task.Result.Exists)
                {
                    bool isCleared = bool.Parse(task.Result.Value.ToString());
                    if (isCleared)
                    {
                        Stage8Task.gameObject.SetActive(true);
                    }
                    else
                    {
                        Stage8Task.gameObject.SetActive(false);
                    }
                }
            });
    }
    public void CheckStage12Cleared()
    {
        if (databaseReference == null || auth.CurrentUser == null)
        {
            Debug.Log("Firebase or user not initialized.");
            return;
        }
        databaseReference.Child("users").Child(userId).Child("stages").Child("stage12").Child("cleared")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && task.Result.Exists)
                {
                    bool isCleared = bool.Parse(task.Result.Value.ToString());
                    if (isCleared)
                    {
                        Stage12Task.gameObject.SetActive(true);
                    }
                    else
                    {
                        Stage12Task.gameObject.SetActive(false);
                    }
                }
            });
    }
    public void CheckStage16Cleared()
    {
        if (databaseReference == null || auth.CurrentUser == null)
        {
            Debug.Log("Firebase or user not initialized.");
            return;
        }
        databaseReference.Child("users").Child(userId).Child("stages").Child("stage16").Child("cleared")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && task.Result.Exists)
                {
                    bool isCleared = bool.Parse(task.Result.Value.ToString());
                    if (isCleared)
                    {
                        Stage16Task.gameObject.SetActive(true);
                    }
                    else
                    {
                        Stage16Task.gameObject.SetActive(false);
                    }
                }
            });
    }
    public void CheckStage4TaskComplete()
    {
        if (databaseReference == null || auth.CurrentUser == null)
        {
            Debug.Log("Firebase or user not initialized.");
            return;
        }
        databaseReference.Child("users").Child(userId).Child("stages").Child("stage04").Child("stage4Task")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && task.Result.Exists)
                {
                    bool isComplete = bool.Parse(task.Result.Value.ToString());
                    if (isComplete)
                    {
                        LoadStage5();
                    }
                    else
                    {
                        NotFinishTask4Panel.SetActive(true);
                    }
                }
            });
    }
    public void CheckStage8TaskComplete()
    {
        if (databaseReference == null || auth.CurrentUser == null)
        {
            Debug.Log("Firebase or user not initialized.");
            return;
        }
        databaseReference.Child("users").Child(userId).Child("stages").Child("stage08").Child("stage8Task")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && task.Result.Exists)
                {
                    bool isComplete = bool.Parse(task.Result.Value.ToString());
                    if (isComplete)
                    {
                        LoadStage9();
                    }
                    else
                    {
                        NotFinishTask8Panel.SetActive(true);
                    }
                }
            });
    }
    public void CheckStage12TaskComplete()
    {
        if (databaseReference == null || auth.CurrentUser == null)
        {
            Debug.Log("Firebase or user not initialized.");
            return;
        }
        databaseReference.Child("users").Child(userId).Child("stages").Child("stage12").Child("stage12Task")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && task.Result.Exists)
                {
                    bool isComplete = bool.Parse(task.Result.Value.ToString());
                    if (isComplete)
                    {
                        LoadStage13();
                    }
                    else
                    {
                        NotFinishTask12Panel.SetActive(true);
                    }
                }
            });
    }
    public void CheckStage16TaskComplete()
    {
        if (databaseReference == null || auth.CurrentUser == null)
        {
            Debug.Log("Firebase or user not initialized.");
            return;
        }
        databaseReference.Child("users").Child(userId).Child("stages").Child("stage16").Child("stage16Task")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && task.Result.Exists)
                {
                    bool isComplete = bool.Parse(task.Result.Value.ToString());
                    if (isComplete)
                    {
                        LoadStage17();
                    }
                    else
                    {
                        NotFinishTask16Panel.SetActive(true);
                    }
                }
            });
    }
    public void CloseNotFinishTask4Panel()
    {
        NotFinishTask4Panel.SetActive(false);
    }
    public void CloseNotFinishTask8Panel()
    {
        NotFinishTask8Panel.SetActive(false);
    }
    public void CloseNotFinishTask12Panel()
    {
        NotFinishTask12Panel.SetActive(false);
    }
    public void CloseNotFinishTask16Panel()
    {
        NotFinishTask16Panel.SetActive(false);
    }
    public void UpdateArrowBasedOnTasks()
    {
        CheckStage4();
    }
    private void CheckStage4()
    {
        databaseReference.Child("users").Child(userId).Child("stages").Child("stage04")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && task.Result.Exists)
                {
                    bool stage4Cleared = bool.Parse(task.Result.Child("cleared").Value.ToString());
                    bool stage4TaskComplete = bool.Parse(task.Result.Child("stage4Task").Value.ToString());
                    CheckStage8(stage4Cleared, stage4TaskComplete);
                }
            });
    }
    private void CheckStage8(bool stage4Cleared, bool stage4TaskComplete)
    {
        databaseReference.Child("users").Child(userId).Child("stages").Child("stage08")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && task.Result.Exists)
                {
                    bool stage8Cleared = bool.Parse(task.Result.Child("cleared").Value.ToString());
                    bool stage8TaskComplete = bool.Parse(task.Result.Child("stage8Task").Value.ToString());
                    CheckStage12(stage4Cleared, stage4TaskComplete, stage8Cleared, stage8TaskComplete);
                }
            });
    }
    private void CheckStage12(bool stage4Cleared, bool stage4TaskComplete, bool stage8Cleared, bool stage8TaskComplete)
    {
        databaseReference.Child("users").Child(userId).Child("stages").Child("stage12")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && task.Result.Exists)
                {
                    bool stage12Cleared = bool.Parse(task.Result.Child("cleared").Value.ToString());
                    bool stage12TaskComplete = bool.Parse(task.Result.Child("stage12Task").Value.ToString());
                    CheckStage16(stage4Cleared, stage4TaskComplete, stage8Cleared, stage8TaskComplete, stage12Cleared, stage12TaskComplete);
                }
            });
    }
    private void CheckStage16(bool stage4Cleared, bool stage4TaskComplete, bool stage8Cleared, bool stage8TaskComplete, bool stage12Cleared, bool stage12TaskComplete)
    {
        databaseReference.Child("users").Child(userId).Child("stages").Child("stage16")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && task.Result.Exists)
                {
                    bool stage16Cleared = bool.Parse(task.Result.Child("cleared").Value.ToString());
                    bool stage16TaskComplete = bool.Parse(task.Result.Child("stage16Task").Value.ToString());
                    SetArrowVisibility(stage4Cleared, stage4TaskComplete, stage8Cleared, stage8TaskComplete, stage12Cleared, stage12TaskComplete, stage16Cleared, stage16TaskComplete);
                }
            });
    }
    private void SetArrowVisibility(bool stage4Cleared, bool stage4TaskComplete, bool stage8Cleared, bool stage8TaskComplete, bool stage12Cleared, bool stage12TaskComplete, bool stage16Cleared, bool stage16TaskComplete)
    {
        bool arrowShouldBeVisible = false;
        if ((stage4Cleared && !stage4TaskComplete) ||
            (stage8Cleared && !stage8TaskComplete) ||
            (stage12Cleared && !stage12TaskComplete) ||
            (stage16Cleared && !stage16TaskComplete))
        {
            arrowShouldBeVisible = true; 
        }
        else{
            arrowShouldBeVisible = false; 
        }
        arrowToNPC.playerArrow.SetActive(arrowShouldBeVisible);
        Debug.Log($"Arrow Visibility Set To: {arrowShouldBeVisible}");
    }
    private void CheckStage4Status()
    {
        if (databaseReference == null || auth.CurrentUser == null) return;
        databaseReference.Child("users").Child(userId).Child("stages").Child("stage04").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && task.Result.Exists)
            {
                bool isCleared = bool.Parse(task.Result.Child("cleared").Value.ToString());
                bool isTaskComplete = bool.Parse(task.Result.Child("stage4Task").Value.ToString());
                if (isCleared && !isTaskComplete)
                {
                    characterLoader.ActivateAddArrowToPlayer();
                }
                else if (isTaskComplete)
                {
                    Debug.Log("Stage 4 task completed. Arrow will not instantiate anymore.");
                }
            }
        });
    }
    private void CheckStage8Status()
    {
        if (databaseReference == null || auth.CurrentUser == null) return;
        databaseReference.Child("users").Child(userId).Child("stages").Child("stage08").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && task.Result.Exists)
            {
                bool isCleared = bool.Parse(task.Result.Child("cleared").Value.ToString());
                bool isTaskComplete = bool.Parse(task.Result.Child("stage8Task").Value.ToString());
                if (isCleared && !isTaskComplete)
                {
                    characterLoader.ActivateAddArrowToPlayer();
                }
                else if (isTaskComplete)
                {
                    Debug.Log("Stage 8 task completed. Arrow will not instantiate anymore.");
                }
            }
        });
    }
    private void CheckStage12Status()
    {
        if (databaseReference == null || auth.CurrentUser == null) return;
        databaseReference.Child("users").Child(userId).Child("stages").Child("stage12").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && task.Result.Exists)
            {
                bool isCleared = bool.Parse(task.Result.Child("cleared").Value.ToString());
                bool isTaskComplete = bool.Parse(task.Result.Child("stage12Task").Value.ToString());
                if (isCleared && !isTaskComplete)
                {
                    characterLoader.ActivateAddArrowToPlayer();
                }
                else if (isTaskComplete)
                {
                    Debug.Log("Stage 12 task completed. Arrow will not instantiate anymore.");
                }
            }
        });
    }
    private void CheckStage16Status()
    {
        if (databaseReference == null || auth.CurrentUser == null) return;
        databaseReference.Child("users").Child(userId).Child("stages").Child("stage16").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && task.Result.Exists)
            {
                bool isCleared = bool.Parse(task.Result.Child("cleared").Value.ToString());
                bool isTaskComplete = bool.Parse(task.Result.Child("stage16Task").Value.ToString());
                if (isCleared && !isTaskComplete)
                {
                    characterLoader.ActivateAddArrowToPlayer();
                }
                else if (isTaskComplete)
                {
                    Debug.Log("Stage 16 task completed. Arrow will not instantiate anymore.");
                }
            }
        });
    }
    public void CheckActivateChainEndless()
    {
            if (databaseReference == null || auth.CurrentUser == null)
            {
                Debug.Log("Firebase or user not initialized.");
                return;
            }
            databaseReference.Child("users").Child(userId).Child("stages").Child("stage21").Child("unlocked")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && task.Result.Exists)
                {
                    bool isUnlocked = bool.Parse(task.Result.Value.ToString());
                    if (isUnlocked)
                    {
                        ChainImage.SetActive(false);
                    }
                    else{
                        ChainImage.SetActive(true);
                    }
                }
            });
    }
}