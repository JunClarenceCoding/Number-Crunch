using Firebase.Database;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine.UI;
using System;

public class FeedbackMainMenu : MonoBehaviour
{
    public Button submitButton;
    public Toggle gameplayToggle, performanceToggle, soundToggle, otherToggle;
    public TMP_InputField emailField;
    public TMP_InputField feedbackField;
    public TMP_Text statusText;
    public GameObject feedbackTextPanel;
    public Image feedbackSuccessCheck, feedbackRedExclamation;
    private DatabaseReference databaseReference;
    private FirebaseManager firebaseManager;
    public Color successColor = new Color32(0x47, 0x95, 0x69, 0xFF);
    public Color failedColor = new Color32(0x9D, 0x2B, 0x1F, 0xFF);

    public enum FeedbackStatus
    {
        Success,
        Failed,
    }
    private void Start()
    {
        StartCoroutine(InitializeFirebaseManager());
    }

    private IEnumerator InitializeFirebaseManager()
    {
        firebaseManager = FirebaseManager.Instance;

        // Wait until FirebaseManager is fully initialized
        while (firebaseManager == null || !firebaseManager.IsFirebaseInitialized())
        {
            yield return new WaitForSeconds(0.5f);
            firebaseManager = FirebaseManager.Instance;
        }
        databaseReference = firebaseManager.Database.RootReference;
    }

    private string GetSelectedCategories()
    {
        string selectedCategories = "";
        if (gameplayToggle.isOn) selectedCategories += "Gameplay, ";
        if (soundToggle.isOn) selectedCategories += "Sounds, ";
        if (performanceToggle.isOn) selectedCategories += "Performance, ";
        if (otherToggle.isOn) selectedCategories += "Others, ";
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
        yield return null;
        statusText.text = message;
        feedbackSuccessCheck.gameObject.SetActive(status == FeedbackStatus.Success);
        feedbackRedExclamation.gameObject.SetActive(status == FeedbackStatus.Failed);
        statusText.color = status == FeedbackStatus.Success ? successColor : failedColor;
        feedbackTextPanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        feedbackSuccessCheck.gameObject.SetActive(false);
        feedbackRedExclamation.gameObject.SetActive(false);
        callback?.Invoke();
        feedbackTextPanel.SetActive(false);
    }
    private bool IsInternetAvailable()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }
    public async void SubmitFeedback()
    {
        if (databaseReference == null)
        {
            ShowFeedbackTextPanel("FAILED TO CONNECT TO DATABASE", FeedbackStatus.Failed);
            return;
        }
        string email = emailField.text;
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
        if (!IsInternetAvailable())
        {
            ShowFeedbackTextPanel("NO INTERNET CONNECTION. PLEASE TRY AGAIN LATER.", FeedbackStatus.Failed);
            return;
        }
        submitButton.interactable = false;
        ShowFeedbackTextPanel("SUBMITTING FEEDBACK...", FeedbackStatus.Success);
        string feedbackId = databaseReference.Child("feedback").Push().Key;
        FeedbackData feedbackData = new FeedbackData(email, feedback, DateTime.Now.ToString(), categories);
        try
        {
            var task = databaseReference.Child("feedback").Child(feedbackId).SetRawJsonValueAsync(JsonUtility.ToJson(feedbackData));
            await task;
            if (task.IsCanceled || task.IsFaulted)
            {
                ShowFeedbackTextPanel("FAILED TO SUBMIT FEEDBACK", FeedbackStatus.Failed);
            }
            else
            {
                ShowFeedbackTextPanel("SUBMITTED!", FeedbackStatus.Success);
            }
            emailField.text = string.Empty;
            feedbackField.text = string.Empty;
            gameplayToggle.isOn = false;
            soundToggle.isOn = false;
            performanceToggle.isOn = false;
            otherToggle.isOn = false;
        }
        catch (Exception e)
        {
            ShowFeedbackTextPanel($"FAILED TO SUBMIT: {e.Message}", FeedbackStatus.Failed);
        }
        finally
        {
            submitButton.interactable = true;
        }
    }
}

[System.Serializable]
public class FeedbackData
{
    public string email;
    public string feedback;
    public string timestamp;
    public string categories;

    public FeedbackData(string email, string feedback, string timestamp, string categories)
    {
        this.email = email;
        this.feedback = feedback;
        this.timestamp = timestamp;
        this.categories = categories;
    }
}