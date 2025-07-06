using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using Firebase.Database;
using System;
using Firebase.Extensions;

public class RewardManager : MonoBehaviour
{
    public GameObject rewardPanel;                  
    public GameObject[] rewardPrefabs;              
    public Transform rewardContainer;               
    public GameObject claimButton;                  
    public GameObject levelUpRewardPanel;           
    public Transform levelUpRewardContainer;        
    public TMP_Text levelText;                      
    public TMP_Text maxHealthText;  
    private List<RewardItem> rewards = new List<RewardItem>(); 
    private int currentXP, currentLevel, xpToNextLevel, currentCoins, currentGems;
    private int oldLevel, oldMaxHealth, currentMaxHealth;
    private AudioManager audioManager;

    private void Start()
    {
        InitializePlayerData();

        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene.");
            return;
        }
    }
    private void InitializePlayerData()
    {
        string userId = FirebaseManager.Instance.Auth.CurrentUser.UserId;
        DatabaseReference dbRef = FirebaseManager.Instance.Database.GetReference("users").Child(userId);
        dbRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && task.Result.Exists)
            {
                DataSnapshot snapshot = task.Result;
                oldLevel = snapshot.HasChild("level") ? Convert.ToInt32(snapshot.Child("level").Value) : 1;
                oldMaxHealth = snapshot.HasChild("maxHealth") ? Convert.ToInt32(snapshot.Child("maxHealth").Value) : 100;
            }
        });
    }
    public void ShowRewards(List<RewardItem> rewardsToShow)
    {
        foreach (Transform child in rewardContainer)
        {
            Destroy(child.gameObject);  
        }
        rewards = rewardsToShow;
        rewardPanel.SetActive(true);  
        claimButton.SetActive(false);
        foreach (var reward in rewards)
        {
            GameObject rewardPrefab = GetRewardPrefab(reward.rewardType);
            GameObject rewardInstance = Instantiate(rewardPrefab, rewardContainer);
            TMP_Text rewardText = rewardInstance.GetComponentInChildren<TMP_Text>();
            if (reward.rewardType == "GirlClothes" || reward.rewardType == "BoyClothes")
            {
                rewardText.text = "Nastech Uniform";
            }
            else
            {
                rewardText.text = reward.amount + " " + reward.rewardType;
            }
        }
        StartCoroutine(ShowClaimButtonAfterDelay(1.0f));  
    }
    private GameObject GetRewardPrefab(string rewardType)
    {
        switch (rewardType)
        {
            case "XP":
                return rewardPrefabs[0];  
            case "Coins":
                return rewardPrefabs[1];  
            case "Gems":
                return rewardPrefabs[2];  
            case "GirlClothes":
                return rewardPrefabs[3];
            case "BoyClothes":
                return rewardPrefabs[4];
            default:
                return rewardPrefabs[0];  
        }
    }
    private IEnumerator ShowClaimButtonAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        claimButton.SetActive(true);
    }
    public void ClaimRewards()
    {
        foreach (var reward in rewards)
        {
            if (reward.rewardType == "XP")
            {
                AddXPAndCheckLevelUp(reward.amount);
            }
            else if (reward.rewardType == "Coins")
            {
                AddCoins(reward.amount);
            }
            else if( reward.rewardType == "Gems")
            {
                AddGems(reward.amount);
            }
            else if(reward.rewardType == "GirlClothes")
            {
                SetMultipleGirlUniformsAsOwned();
            }
            else if (reward.rewardType == "BoyClothes")
            {
                SetMultipleBoyUniformsAsOwned();
            }
        }
        rewardPanel.SetActive(false);  
        ClearRewards();                
        StartCoroutine(CheckForLevelUpAfterDelay(1f));
    }
    private IEnumerator CheckForLevelUpAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        string userId = FirebaseManager.Instance.Auth.CurrentUser.UserId;
        DatabaseReference dbRef = FirebaseManager.Instance.Database.GetReference("users").Child(userId);
        dbRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && task.Result.Exists)
            {
                DataSnapshot snapshot = task.Result;
                currentLevel = snapshot.HasChild("level") ? Convert.ToInt32(snapshot.Child("level").Value) : 1;
                currentMaxHealth = snapshot.HasChild("maxHealth") ? Convert.ToInt32(snapshot.Child("maxHealth").Value) : 100;
                if (currentLevel > oldLevel) 
                {
                    ShowLevelUpRewardPanel();
                }
                oldLevel = currentLevel;
                oldMaxHealth = currentMaxHealth;
            }
        });
    }
    private void ShowLevelUpRewardPanel()
    {
        levelText.text = $"{oldLevel} → {currentLevel}";
        maxHealthText.text = $"{oldMaxHealth} → {currentMaxHealth}";
        foreach (Transform child in levelUpRewardContainer)
        {
            Destroy(child.gameObject);
        }
        AddRewardToContainer("Coins", 100, levelUpRewardContainer);
        AddRewardToContainer("Gems", 5, levelUpRewardContainer);
        levelUpRewardPanel.SetActive(true);
        audioManager.playLevelUpSound();
    }
    private void AddRewardToContainer(string rewardType, int amount, Transform container)
    {
        GameObject rewardPrefab = GetRewardPrefab(rewardType);
        GameObject rewardInstance = Instantiate(rewardPrefab, container);
        TMP_Text rewardText = rewardInstance.GetComponentInChildren<TMP_Text>();
        rewardText.text = $"{amount} {rewardType}";
    }
    public void ClaimLevelUpRewards()
    {
        AddCoins(100);
        AddGems(5);
        levelUpRewardPanel.SetActive(false);
    }
    public void TriggerLevelUpCheck()
    {
        StartCoroutine(CheckForLevelUpAfterDelay(1f));
    }
    public void SetMultipleGirlUniformsAsOwned()
    {
        string userId = FirebaseManager.Instance.Auth.CurrentUser.UserId;
        DatabaseReference girlShirtRef = FirebaseManager.Instance.Database
            .GetReference("users").Child(userId).Child("clothes").Child("tops").Child("GirlShirt2");
        DatabaseReference girlPantsRef = FirebaseManager.Instance.Database
            .GetReference("users").Child(userId).Child("clothes").Child("bottoms").Child("GirlPants2");
        DatabaseReference girlBootsRef = FirebaseManager.Instance.Database
            .GetReference("users").Child(userId).Child("clothes").Child("shoes").Child("GirlBoots2");
        girlShirtRef.Child("owned").SetValueAsync(true).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
                Debug.Log("GirlShirt2 marked as owned.");
            else
                Debug.LogError("Failed to set GirlShirt2 as owned: " + task.Exception);
        });
        girlPantsRef.Child("owned").SetValueAsync(true).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
                Debug.Log("GirlPants2 marked as owned.");
            else
                Debug.LogError("Failed to set GirlPants2 as owned: " + task.Exception);
        });
        girlBootsRef.Child("owned").SetValueAsync(true).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
                Debug.Log("GirlBoots2 marked as owned.");
            else
                Debug.LogError("Failed to set GirlBoots2 as owned: " + task.Exception);
        });
    }
    public void SetMultipleBoyUniformsAsOwned()
    {
        string userId = FirebaseManager.Instance.Auth.CurrentUser.UserId;
        DatabaseReference boyShirtRef = FirebaseManager.Instance.Database
            .GetReference("users").Child(userId).Child("clothes").Child("tops").Child("BoyShirt3");
        DatabaseReference boyPantsRef = FirebaseManager.Instance.Database
            .GetReference("users").Child(userId).Child("clothes").Child("bottoms").Child("BoyPants3");
        DatabaseReference boyBootsRef = FirebaseManager.Instance.Database
            .GetReference("users").Child(userId).Child("clothes").Child("shoes").Child("BoyBoots2");
        boyShirtRef.Child("owned").SetValueAsync(true).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
                Debug.Log("boyShirt3 marked as owned.");
            else
                Debug.LogError("Failed to set boyShirt3 as owned: " + task.Exception);
        });
        boyPantsRef.Child("owned").SetValueAsync(true).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
                Debug.Log("BoyPants3 marked as owned.");
            else
                Debug.LogError("Failed to set boyPants3 as owned: " + task.Exception);
        });
        boyBootsRef.Child("owned").SetValueAsync(true).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
                Debug.Log("BoyBoots2 marked as owned.");
            else
                Debug.LogError("Failed to set BoyBoots2 as owned: " + task.Exception);
        });
    }
    private void AddCoins(int coinAmount)
    {
        var userId = FirebaseManager.Instance.Auth.CurrentUser?.UserId;
        if (userId != null)
        {
            DatabaseReference coinsRef = FirebaseManager.Instance.Database.GetReference("users").Child(userId).Child("currency");
            coinsRef.GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted && task.Result.Exists)
                {
                    currentCoins = Convert.ToInt32(task.Result.Value);
                    currentCoins += coinAmount; 
                    SaveCoinData(coinsRef);
                }
            });
        }
    }
    private void SaveCoinData(DatabaseReference coinsRef)
    {
        coinsRef.SetValueAsync(currentCoins).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Coin data saved successfully: " + currentCoins);
            }
        });
    }

    private void AddGems(int gemsAmount)
    {
        var userId = FirebaseManager.Instance.Auth.CurrentUser?.UserId;
        if (userId != null)
        {
            DatabaseReference gemsRef = FirebaseManager.Instance.Database.GetReference("users").Child(userId).Child("gems");
            gemsRef.GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted && task.Result.Exists)
                {
                    currentGems = Convert.ToInt32(task.Result.Value);
                    currentGems += gemsAmount; 
                    SaveGemsData(gemsRef);
                }
            });
        }
    }
    private void SaveGemsData(DatabaseReference gemsRef)
    {
        gemsRef.SetValueAsync(currentGems).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Gems data saved successfully: " + currentGems);
            }
        });
    }
    private void AddXPAndCheckLevelUp(int xpAmount)
    {
        var userId = FirebaseManager.Instance.Auth.CurrentUser?.UserId;
        if (userId != null)
        {
            DatabaseReference dbRef = FirebaseManager.Instance.Database.GetReference("users").Child(userId);
            dbRef.GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted && task.Result.Exists)
                {
                    DataSnapshot snapshot = task.Result;
                    currentXP = snapshot.HasChild("xp") ? Convert.ToInt32(snapshot.Child("xp").Value) : 0;
                    currentLevel = snapshot.HasChild("level") ? Convert.ToInt32(snapshot.Child("level").Value) : 1;
                    xpToNextLevel = snapshot.HasChild("xpToNextLevel") ? Convert.ToInt32(snapshot.Child("xpToNextLevel").Value) : 1000;
                    currentXP += xpAmount;
                    while (currentXP >= xpToNextLevel)
                    {
                        currentXP -= xpToNextLevel;  
                        currentLevel++;             
                        xpToNextLevel = Mathf.FloorToInt(xpToNextLevel * 1.1f); 
                        UpdateMaxHealthInFirebase(dbRef);
                    }
                    SaveXPData(dbRef);
                }
            });
        }
    }
    private void SaveXPData(DatabaseReference dbRef)
    {
        dbRef.Child("xp").SetValueAsync(currentXP);
        dbRef.Child("level").SetValueAsync(currentLevel);
        dbRef.Child("xpToNextLevel").SetValueAsync(xpToNextLevel);
        Debug.Log("XP Data saved successfully.");
    }
    private void UpdateMaxHealthInFirebase(DatabaseReference dbRef)
    {
        dbRef.Child("maxHealth").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted && task.Result.Exists)
            {
                int currentMaxHealth = Convert.ToInt32(task.Result.Value);
                int newMaxHealth = currentMaxHealth + 100;
                dbRef.Child("maxHealth").SetValueAsync(newMaxHealth).ContinueWith(saveTask =>
                {
                    if (saveTask.IsCompleted)
                    {
                        Debug.Log("Max health updated successfully: " + newMaxHealth);
                    }
                });
            }
        });
    }
    private void ClearRewards()
    {
        foreach (Transform child in rewardContainer)
        {
            Destroy(child.gameObject);  
        }
        rewards.Clear();
    }
}