using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AccountDeletionManager : MonoBehaviour
{
    private FirebaseManager firebaseManager;
    public GameObject loadingScreen;
    public Slider slider;
    private FeedbackManager feedbackManager;

    private void Start()
    {
        firebaseManager = FirebaseManager.Instance;
        feedbackManager = FindObjectOfType<FeedbackManager>();
        StartCoroutine(WaitForFirebaseInitialization());
    }
    private IEnumerator WaitForFirebaseInitialization()
    {
        yield return firebaseManager.WaitForFirebaseInitialization();
    }
    public void DeleteAccount()
    {
        if (!firebaseManager.IsFirebaseInitialized())
        {
            Debug.LogError("Firebase is not initialized yet.");
            return;
        }
        FirebaseAuth auth = firebaseManager.Auth;
        DatabaseReference database = firebaseManager.Database.RootReference;
        FirebaseUser currentUser = auth.CurrentUser;
        if (currentUser == null)
        {
            Debug.LogError("No user is currently logged in.");
            return;
        }
        string userId = firebaseManager.GetUserId();
        GoToScene("StartupScene");
        database.Child("usernames").OrderByValue().EqualTo(userId).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result.Exists)
                {
                    foreach (var childSnapshot in task.Result.Children)
                    {
                        string username = childSnapshot.Key;
                        database.Child("usernames").Child(username).RemoveValueAsync().ContinueWith(deleteUsernameTask =>
                        {
                            if (deleteUsernameTask.IsCompleted)
                            {
                                DeleteUserDataAndAccount(database, userId, currentUser, auth);
                            }
                        });
                    }
                }
                else
                {
                    DeleteUserDataAndAccount(database, userId, currentUser, auth);
                }
            }
        });
    }
    private void DeleteUserDataAndAccount(DatabaseReference database, string userId, FirebaseUser currentUser, FirebaseAuth auth)
    {
        database.Child("users").Child(userId).RemoveValueAsync().ContinueWith(deleteUserTask =>
        {
            if (deleteUserTask.IsCompleted)
            {
                currentUser.DeleteAsync().ContinueWith(deleteAccountTask =>
                {
                    if (deleteAccountTask.IsCompleted)
                    {
                        auth.SignOut();
                    }
                });
            }
        });
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