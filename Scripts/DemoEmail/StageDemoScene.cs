using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.UI;

public class StageDemoScene : MonoBehaviour
{
    public List<GameObject> DemoButtons;
    public Button skipButton1, skipButton2, skipButton3, skipButton4, skipButton5;

    void Start()
    {
        StartCoroutine(WaitForFirebaseInitialization());
    }
    private IEnumerator WaitForFirebaseInitialization()
    {
        yield return StartCoroutine(FirebaseManager.Instance.WaitForFirebaseInitialization());
        CheckDemoPowers(DemoButtons);
    }
    private void CheckDemoPowers(List<GameObject> objectsToActivate)
    {
        string userId = FirebaseManager.Instance.GetUserId(); // Get the current user's ID
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("User is not logged in.");
            return;
        }

        // Get the database reference for DemoPowers
        DatabaseReference demoPowersRef = FirebaseManager.Instance.GetDatabaseReference($"users/{userId}/DemoPowers");

        // Fetch the value from the database
        demoPowersRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to retrieve DemoPowers from Firebase.");
                return;
            }
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists && bool.TryParse(snapshot.Value.ToString(), out bool demoPowers))
                {
                    if (demoPowers)
                    {
                        foreach (GameObject obj in objectsToActivate)
                        {
                            obj.SetActive(true);
                        }
                    }
                    else
                    {
                        foreach (GameObject obj in objectsToActivate)
                        {
                            obj.SetActive(false);
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("DemoPowers value is missing or invalid in the database.");
                }
            }
        });
    }

    public void CheckAndActivateSkipButton()
    {
        string userId = FirebaseManager.Instance.GetUserId(); 
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("User is not logged in.");
            return;
        }
        DatabaseReference demoPowersRef = FirebaseManager.Instance.GetDatabaseReference($"users/{userId}/DemoPowers");

        demoPowersRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to retrieve DemoPowers from Firebase.");
                return;
            }
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists && bool.TryParse(snapshot.Value.ToString(), out bool demoPowers))
                {
                    if (demoPowers && skipButton1 != null)
                    {
                        skipButton1.gameObject.SetActive(true);
                    }
                    else
                    {
                        if (skipButton1 != null)
                        {
                            skipButton1.gameObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("DemoPowers value is missing or invalid in the database.");
                }
            }
        });
    }
    public void CheckAndActivateSkipButton1()
    {
        string userId = FirebaseManager.Instance.GetUserId(); 
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("User is not logged in.");
            return;
        }
        DatabaseReference demoPowersRef = FirebaseManager.Instance.GetDatabaseReference($"users/{userId}/DemoPowers");

        demoPowersRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to retrieve DemoPowers from Firebase.");
                return;
            }
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists && bool.TryParse(snapshot.Value.ToString(), out bool demoPowers))
                {
                    if (demoPowers && skipButton2 != null)
                    {
                        skipButton2.gameObject.SetActive(true);
                    }
                    else
                    {
                        if (skipButton2 != null)
                        {
                            skipButton2.gameObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("DemoPowers value is missing or invalid in the database.");
                }
            }
        });
    }
    public void CheckAndActivateSkipButton2()
    {
        string userId = FirebaseManager.Instance.GetUserId(); 
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("User is not logged in.");
            return;
        }
        DatabaseReference demoPowersRef = FirebaseManager.Instance.GetDatabaseReference($"users/{userId}/DemoPowers");

        demoPowersRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to retrieve DemoPowers from Firebase.");
                return;
            }
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists && bool.TryParse(snapshot.Value.ToString(), out bool demoPowers))
                {
                    if (demoPowers && skipButton3 != null)
                    {
                        skipButton3.gameObject.SetActive(true);
                    }
                    else
                    {
                        if (skipButton3 != null)
                        {
                            skipButton3.gameObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("DemoPowers value is missing or invalid in the database.");
                }
            }
        });
    }
    public void CheckAndActivateSkipButton3()
    {
        string userId = FirebaseManager.Instance.GetUserId(); 
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("User is not logged in.");
            return;
        }
        DatabaseReference demoPowersRef = FirebaseManager.Instance.GetDatabaseReference($"users/{userId}/DemoPowers");

        demoPowersRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to retrieve DemoPowers from Firebase.");
                return;
            }
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists && bool.TryParse(snapshot.Value.ToString(), out bool demoPowers))
                {
                    if (demoPowers && skipButton4 != null)
                    {
                        skipButton4.gameObject.SetActive(true);
                    }
                    else
                    {
                        if (skipButton4 != null)
                        {
                            skipButton4.gameObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("DemoPowers value is missing or invalid in the database.");
                }
            }
        });
    }
    public void CheckAndActivateSkipButton4()
    {
        string userId = FirebaseManager.Instance.GetUserId(); 
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("User is not logged in.");
            return;
        }
        DatabaseReference demoPowersRef = FirebaseManager.Instance.GetDatabaseReference($"users/{userId}/DemoPowers");

        demoPowersRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to retrieve DemoPowers from Firebase.");
                return;
            }
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists && bool.TryParse(snapshot.Value.ToString(), out bool demoPowers))
                {
                    if (demoPowers && skipButton5 != null)
                    {
                        skipButton5.gameObject.SetActive(true);
                    }
                    else
                    {
                        if (skipButton5 != null)
                        {
                            skipButton5.gameObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("DemoPowers value is missing or invalid in the database.");
                }
            }
        });
    }
}