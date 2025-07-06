using UnityEngine;
using TMPro;
using Firebase.Auth;
using System;
using System.Collections;
using Firebase.Database;

public class DisplayNameHandler : MonoBehaviour
{
    public TMP_Text[] displayNameTexts; 
    public GameObject panel; 
    public ClothingManager clothingManager; 
    private DatabaseReference databaseReference;
    private IEnumerator Start()
    {
         if (FirebaseManager.Instance != null)
        {
            yield return FirebaseManager.Instance.WaitForFirebaseInitialization();
            databaseReference = FirebaseManager.Instance.Database.RootReference;
        }
        if (FirebaseManager.Instance.IsFirebaseInitialized())
        {
            StartCoroutine(RetrieveAndUpdateDisplayName());
        }else{
            UpdateDisplayName("Firebase initialization failed.");
        }
    }
    public void RefreshUsernameDisplay()
    {
        StartCoroutine(RetrieveAndUpdateDisplayName());
    }
    private IEnumerator RetrieveAndUpdateDisplayName()
    {
        string userId = FirebaseManager.Instance.GetUserId();
        if (string.IsNullOrEmpty(userId))
        {
            UpdateDisplayName("No user logged in.");
            yield break;
        }
        var usernameTask = FirebaseManager.Instance.GetDatabaseReference("usernames").GetValueAsync();
        yield return new WaitUntil(() => usernameTask.IsCompleted);
        if (usernameTask.IsFaulted)
        {
            UpdateDisplayName("Error loading username.");
        }
        else
        {
            DataSnapshot snapshot = usernameTask.Result;
            string foundUsername = null;
            foreach (DataSnapshot childSnapshot in snapshot.Children)
            {
                if (childSnapshot.Value != null && childSnapshot.Value.ToString() == userId)
                {
                    foundUsername = childSnapshot.Key; 
                    break;
                }
            }
            if (!string.IsNullOrEmpty(foundUsername))
            {
                UpdateDisplayName(foundUsername);
            }
            else
            {
                UpdateDisplayName("No display name found.");
            }
        }
    }
    private void UpdateDisplayName(string displayName)
    {
        foreach (TMP_Text text in displayNameTexts)
        {
            if (text != null)
            {
                text.text = displayName;
            }
        }
    }
    public void OpenPanelAndUpdateDisplayName()
    {
        StartCoroutine(CheckIfOwnedWithDelay());
    }
    private IEnumerator CheckIfOwnedWithDelay()
    {
        panel.SetActive(true);
        yield return RetrieveAndUpdateDisplayName();
        yield return new WaitForSeconds(0.2f);
        CheckSweaterBasedOnCharacter();
    }
    public void CheckSweaterBasedOnCharacter()
    {
        int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter", 0); 

        if (selectedCharacter == 0)
        {
            clothingManager.CheckIfBoyShirt2Owned();
        }
        else if (selectedCharacter == 1)
        {
            clothingManager.CheckIfGirlSweaterOwned();
        }
        else
        {
            Debug.LogWarning("Invalid character selection. No sweater purchase available for this character.");
        }
    }
}