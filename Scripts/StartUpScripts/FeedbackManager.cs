using Firebase.Database;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine.UI;
using System;
using Firebase.Auth;

public class FeedbackManager : MonoBehaviour
{
    public Toggle gameplayToggle, performanceToggle, SoundToggle, OtherToggle;
    public TMP_InputField emailField;
    public TMP_Text emailTitle;
    public TMP_InputField feedbackField;
    public TMP_Text statusText;
    public TMP_Text emailDisplayText; 
    public GameObject FeedbackTextPanel;
    public GameObject emailFieldContainer; 
    public Image FeedbackSuccessCheck, FeedbackRedExclamation;
    private DatabaseReference databaseReference;
    private FirebaseAuth auth; 
    public Color successColor = new Color32(0x47, 0x95, 0x69, 0xFF);  // #479569
    public Color failedColor = new Color32(0x9D, 0x2B, 0x1F, 0xFF);   // #9d2b1f
    public enum FeedbackStatus
    {
        Success,
        Failed,
    }
    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        if (FirebaseManager.Instance.IsFirebaseInitialized())
        {
            InitializeFeedbackManager();
        }
        else
        {
            FirebaseManager.Instance.OnFirebaseInitialized += InitializeFeedbackManager;
        }
        StartCoroutine(WaitForFirebaseInitialization());
    }
    IEnumerator WaitForFirebaseInitialization()
    {
        while (!FirebaseManager.Instance.IsFirebaseInitialized())
        {
            yield return null;
        }
        UpdateEmailFieldVisibility(); 
    }
    private void InitializeFeedbackManager()
    {
        databaseReference = FirebaseManager.Instance.Database.RootReference;
        FirebaseManager.Instance.OnFirebaseInitialized -= InitializeFeedbackManager;
    }
    void OnDestroy()
    {
        if (FirebaseManager.Instance != null)
        {
            FirebaseManager.Instance.OnFirebaseInitialized -= InitializeFeedbackManager;
        }
    }
    public void UpdateEmailFieldVisibility()
    {
        if (auth == null || auth.CurrentUser == null)
        {
            emailTitle.gameObject.SetActive(true);
            emailFieldContainer.SetActive(true); 
            emailDisplayText.gameObject.SetActive(false); 
        }
        else
        {
            emailTitle.gameObject.SetActive(false);  
            emailFieldContainer.SetActive(false);  
            emailDisplayText.gameObject.SetActive(true); 
            emailDisplayText.text = $"{auth.CurrentUser.Email}"; 
        }
    }
    private string GetSelectedCategories()
    {
        string selectedCategories = "";
        if (gameplayToggle.isOn) selectedCategories += "Gameplay, ";
        if (SoundToggle.isOn) selectedCategories += "Sounds, ";
        if (performanceToggle.isOn) selectedCategories += "Performance, ";
        if (OtherToggle.isOn) selectedCategories += "Others, ";
        if (selectedCategories.EndsWith(", "))
        {
            selectedCategories = selectedCategories.Substring(0, selectedCategories.Length - 2);
        }
        return selectedCategories;
    }
    public void ShowFeedbackTextPanel(string message, FeedbackStatus status, Action callback = null)
    {
        StartCoroutine(ShowFeedbackTextPanelOnMainThread(message, status, callback));
    }
    private IEnumerator ShowFeedbackTextPanelOnMainThread(string message, FeedbackStatus status, Action callback)
    {
        yield return new WaitForEndOfFrame();
        statusText.text = message;
        FeedbackSuccessCheck.gameObject.SetActive(status == FeedbackStatus.Success);
        FeedbackRedExclamation.gameObject.SetActive(status == FeedbackStatus.Failed);
        statusText.color = status == FeedbackStatus.Success ? successColor : failedColor;
        FeedbackTextPanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        FeedbackSuccessCheck.gameObject.SetActive(false);
        FeedbackRedExclamation.gameObject.SetActive(false);
        callback?.Invoke();
        FeedbackTextPanel.SetActive(false);
    }
    private bool IsInternetAvailable()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }
    public async void SubmitFeedback()
    {
        if (!FirebaseManager.Instance.IsFirebaseInitialized())
        {
            ShowFeedbackTextPanel("FIREBASE INITIALIZATION IN PROGRESS, PLEASE WAIT...", FeedbackStatus.Failed);
            return;
        }
        if (!IsInternetAvailable())
        {
            ShowFeedbackTextPanel("NO INTERNET CONNECTION. PLEASE TRY AGAIN LATER.", FeedbackStatus.Failed);
            return;
        }
        string email = auth.CurrentUser != null ? auth.CurrentUser.Email : emailField.text;
        string feedback = feedbackField.text;
        string categories = GetSelectedCategories();
        if (string.IsNullOrEmpty(feedback))
        {
            ShowFeedbackTextPanel("PLEASE PROVIDE FEEDBACK BEFORE SUBMITTING", FeedbackStatus.Failed);
            return;
        }
        else if (string.IsNullOrEmpty(email))
        {
            ShowFeedbackTextPanel("PLEASE PROVIDE EMAIL BEFORE SUBMITTING", FeedbackStatus.Failed);
            return;
        }
        string feedbackId = databaseReference.Child("feedback").Push().Key;
        FeedbackData feedbackData = new FeedbackData(email, feedback, DateTime.Now.ToString(), categories);
        try
        {
            await databaseReference.Child("feedback").Child(feedbackId).SetRawJsonValueAsync(JsonUtility.ToJson(feedbackData))
                .ContinueWith(task => {
                    if (task.IsCanceled || task.IsFaulted)
                    {
                        ShowFeedbackTextPanel("FAILED TO SUBMIT FEEDBACK", FeedbackStatus.Failed);
                        return;
                    }
                    else
                    {
                        ShowFeedbackTextPanel("SUBMITTED!", FeedbackStatus.Success);
                    }
                });
            if (emailFieldContainer.activeSelf)
            {
                emailField.text = string.Empty;
            }
            feedbackField.text = string.Empty;
            gameplayToggle.isOn = false;
            SoundToggle.isOn = false;
            performanceToggle.isOn = false;
            OtherToggle.isOn = false;
        }
        catch (Exception e)
        {
            ShowFeedbackTextPanel($"FAILED TO SUBMIT: {e.Message}", FeedbackStatus.Failed);
        }
    }
    public void OnUserLogout()
    {
        auth.SignOut();
        UpdateEmailFieldVisibility();
    }
}