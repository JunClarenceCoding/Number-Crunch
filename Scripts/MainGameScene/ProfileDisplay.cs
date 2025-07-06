using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Database;
using Firebase.Extensions;

public class ProfileDisplay : MonoBehaviour
{
    public GameObject ProfilePanel;
    public XPManager xPManager;
    public DisplayNameHandler displayNameHandler;
    public Image profileImage; 
    public Sprite boyProfileSprite; 
    public Sprite girlProfileSprite; 
    public TMP_Text maxHealthText;
    public TMP_Text accountCreatedText;
    private MainCharacterLoader mainCharacterLoader;
    private DatabaseReference databaseReference;
    private IEnumerator Start()
    {
        mainCharacterLoader = FindObjectOfType<MainCharacterLoader>();
        if (FirebaseManager.Instance != null)
        {
            yield return FirebaseManager.Instance.WaitForFirebaseInitialization();
            databaseReference = FirebaseManager.Instance.Database.RootReference;
        }
    }
    void LoadMaxHealthData()
    {
        if (databaseReference == null)
        {
            Debug.LogError("Database reference is null. Unable to load data.");
            return;
        }
        string userId = FirebaseManager.Instance.GetUserId();
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("User ID is null or empty. Unable to load data.");
            return;
        }
        databaseReference.Child("users").Child(userId).Child("maxHealth").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result.Exists)
                {
                    long maxHealth = (long)task.Result.Value;
                    maxHealthText.text = "Health: " + maxHealth;
                }
                else
                {
                    maxHealthText.text = "Health: N/A";
                }
            }
            else
            {
                maxHealthText.text = "Health: Error";
            }
        });
    }
    void LoadAccountCreated()
    {
        if (databaseReference == null)
        {
            Debug.LogError("Database reference is null. Unable to load data.");
            return;
        }
        string userId = FirebaseManager.Instance.GetUserId();
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("User ID is null or empty. Unable to load data.");
            return;
        }
        databaseReference.Child("users").Child(userId).Child("AccountCreated").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result.Exists)
                {
                    string accountCreated = task.Result.Value.ToString();
                    string[] dateTimeParts = accountCreated.Split(' '); 
                    if (dateTimeParts.Length == 2)
                    {
                        string formattedAccountCreated = $"{dateTimeParts[0]} | {dateTimeParts[1]}";
                        accountCreatedText.text = "Account Created: " + formattedAccountCreated;
                    }
                    else
                    {
                        accountCreatedText.text = "Account Created: Invalid Format";
                    }
                }
                else
                {
                    accountCreatedText.text = "Account Created: N/A";
                }
            }
            else
            {
                accountCreatedText.text = "Account Created: Error";
            }
        });
    }
    void UpdateProfileImage()
    {
        if (mainCharacterLoader != null)
        {
            GameObject player = mainCharacterLoader.GetInstantiatedPlayer();
            int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
            if (selectedCharacter == 0)
            {
                profileImage.sprite = boyProfileSprite;
            }
            else if (selectedCharacter == 1)
            {
                profileImage.sprite = girlProfileSprite;
            }
            else
            {
                Debug.LogError("Invalid character selection.");
            }
        }
    }
    private IEnumerator RefreshData()
    {
        ProfilePanel.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        xPManager.RefreshUI();
        displayNameHandler.RefreshUsernameDisplay();
        UpdateProfileImage();
        LoadMaxHealthData();
        LoadAccountCreated();
    }
    public void OpenProfilePanel()
    {
        StartCoroutine(RefreshData());
    }
    public void CloseProfilePanel()
    {
        ProfilePanel.SetActive(false);
    }
}