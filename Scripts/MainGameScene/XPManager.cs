using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections;

public class XPManager : MonoBehaviour
{
    public Slider[] xpSliders;      
    public TMP_Text[] xpTexts;      
    public TMP_Text[] levelTexts; 
    public GameObject panel;    
    private FirebaseAuth auth;
    private DatabaseReference databaseReference;
    private string userId;
    private int currentXP;
    private int currentLevel;
    private int xpToNextLevel;
    private RewardManager rewardsManager;
    private IEnumerator Start()
    {
        rewardsManager = FindObjectOfType<RewardManager>();
        if (rewardsManager == null)
        {
            Debug.LogError("RewardsManager instance not found!");
        }
         if (FirebaseManager.Instance != null)
        {
            yield return FirebaseManager.Instance.WaitForFirebaseInitialization();
            databaseReference = FirebaseManager.Instance.Database.RootReference;
        }
        if (FirebaseManager.Instance.IsFirebaseInitialized())
        {
            InitializeXPManager();
        }
    }

    void OnEnable()
    {
        if (FirebaseManager.Instance.IsFirebaseInitialized() && auth != null && auth.CurrentUser != null)
        {
            LoadXPData(); 
        }
        else
        {
            InitializeXPManager();
        }
    }
    void InitializeXPManager()
    {
        auth = FirebaseManager.Instance.Auth;
        if (auth.CurrentUser != null)
        {
            userId = auth.CurrentUser.UserId;
            databaseReference = FirebaseManager.Instance.Database.GetReference("users").Child(userId);
            LoadXPData();
        }
    }
    void LoadXPData()
    {
        if (databaseReference == null)
        {
            Debug.LogError("Database reference is null. Unable to load data.");
            return;
        }
        databaseReference.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && task.Result.Exists)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.HasChild("xp"))
                    currentXP = int.Parse(snapshot.Child("xp").Value.ToString());
                if (snapshot.HasChild("level"))
                    currentLevel = int.Parse(snapshot.Child("level").Value.ToString());
                if (snapshot.HasChild("xpToNextLevel"))
                    xpToNextLevel = int.Parse(snapshot.Child("xpToNextLevel").Value.ToString());
                UpdateXPUI();
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("Failed to load XP data from Firebase: " + task.Exception?.Flatten()?.ToString());
            }
            else
            {
                Debug.LogWarning("No data found for XP.");
            }
        });
    }
    public void RefreshUI()
    {
        UpdateXPUI();
    }
    void UpdateXPUI()
    {
        foreach (Slider slider in xpSliders)
        {
            if (slider != null)
            {
                slider.maxValue = xpToNextLevel;
                slider.value = currentXP;
            }
        }
        foreach (TMP_Text xpText in xpTexts)
        {
            if (xpText != null)
            {
                xpText.text = currentXP + "/" + xpToNextLevel + " EXP";
            }
        }
        foreach (TMP_Text levelText in levelTexts)
        {
            if (levelText != null)
            {
                levelText.text = "" + currentLevel;
            }
        }
    }
    public void AddXP(int amount)
    {
        currentXP += amount;

        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
        SaveXPData();
        UpdateXPUI(); 
    }
    void LevelUp()
    {
        currentXP -= xpToNextLevel; 
        currentLevel++;
        xpToNextLevel = Mathf.FloorToInt(xpToNextLevel * 1.1f);
        UpdateMaxHealthInFirebase();
        UpdateXPUI();
        if (rewardsManager != null)
        {
            rewardsManager.TriggerLevelUpCheck();
        }
    }
    void UpdateMaxHealthInFirebase()
    {
        if (databaseReference == null)
        {
            Debug.LogError("Database reference is null. Cannot update maxHealth.");
            return;
        }
        databaseReference.Child("maxHealth").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && task.Result.Exists)
            {
                int currentMaxHealth = int.Parse(task.Result.Value.ToString());
                int newMaxHealth = currentMaxHealth + 100;
                databaseReference.Child("maxHealth").SetValueAsync(newMaxHealth).ContinueWithOnMainThread(saveTask =>
                {
                    if (saveTask.IsCompleted)
                    {
                        Debug.Log("maxHealth updated successfully: " + newMaxHealth);
                    }
                    else
                    {
                        Debug.LogError("Failed to update maxHealth: " + saveTask.Exception);
                    }
                });
            }
            else
            {
                Debug.LogWarning("Failed to retrieve maxHealth from Firebase, setting default value of 600.");
            }
        });
    }
    void SaveXPData()
    {
        if (databaseReference == null)
        {
            Debug.LogError("Database reference is null. Cannot save data.");
            return;
        }
        databaseReference.Child("xp").SetValueAsync(currentXP);
        databaseReference.Child("level").SetValueAsync(currentLevel);
        databaseReference.Child("xpToNextLevel").SetValueAsync(xpToNextLevel);
    }
    public void OpenPanelAndUpdateXP()
    {
        if (panel != null)
        {
            panel.SetActive(true); 
            UpdateXPUI(); 
        }
    }
}