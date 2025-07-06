using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
public class MainSceneDemo : MonoBehaviour
{
    public List<GameObject> DemoButtons; // List for demo buttons

    void Start()
    {
        // Wait for Firebase initialization
        StartCoroutine(WaitForFirebaseInitialization());
    }

    private IEnumerator WaitForFirebaseInitialization()
    {
        // Wait until Firebase is initialized
        yield return StartCoroutine(FirebaseManager.Instance.WaitForFirebaseInitialization());

        // Check the value of DemoPowers from Firebase after Firebase initialization
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
                    // Set the active state of the objects based on the DemoPowers value
                    if (demoPowers)
                    {
                        ActivateObjects(objectsToActivate);
                    }
                    else
                    {
                        DeactivateObjects(objectsToActivate);
                    }
                }
                else
                {
                    Debug.LogWarning("DemoPowers value is missing or invalid in the database.");
                    DeactivateObjects(objectsToActivate);
                }
            }
        });
    }

    private void ActivateObjects(List<GameObject> objectsToActivate)
    {
        foreach (GameObject obj in objectsToActivate)
        {
            obj.SetActive(true);
        }
    }

    private void DeactivateObjects(List<GameObject> objectsToActivate)
    {
        foreach (GameObject obj in objectsToActivate)
        {
            obj.SetActive(false);
        }
    }
}
