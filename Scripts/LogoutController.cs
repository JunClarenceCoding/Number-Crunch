using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine.UI;

public class LogoutController : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    private FirebaseAuth auth;
    private FeedbackManager feedbackManager;
    private AudioManager audioManager;
    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene.");
            return;
        }
        feedbackManager = FindObjectOfType<FeedbackManager>();
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result == DependencyStatus.Available)
                {
                    InitializeFirebase();
                }
            }
        });
    }
    private void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
    }
    public void Logout()
    {
        if (auth != null)
        {
            auth.SignOut();
            auth = null;
            FirebaseAuth.DefaultInstance.SignOut();
            if (feedbackManager != null)
            {
                feedbackManager.OnUserLogout();
            }
            GoToScene("StartupScene");
        }
    }
    public void GoToScene(string sceneName)
    {
        loadingScreen.SetActive(true);
        slider.value = 0;
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
        yield return null;
        loadingScreen.SetActive(false);
    }
}