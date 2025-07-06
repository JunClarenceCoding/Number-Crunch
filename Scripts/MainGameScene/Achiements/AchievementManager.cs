using Firebase.Database;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI; 
using TMPro;

public class AchievementManager : MonoBehaviour
{
    public AudioManager audioManager;
    public List<Achievement> achievements;  
    public GameObject achievementPanel;    
    public GameObject achievementItemPrefab;
    public Transform achievementsContainer;
    private DatabaseReference dbReference; 
    private string userId;                 
    public CurrencyManager currencyManager;
    void Start()
    {
        StartCoroutine(InitializeAchievements());
    }

    private IEnumerator InitializeAchievements()
    {
        yield return FirebaseManager.Instance.WaitForFirebaseInitialization();
        dbReference = FirebaseManager.Instance.GetDatabaseReference("users");
        userId = FirebaseManager.Instance.GetUserId();
        if (!string.IsNullOrEmpty(userId))
        {
            LoadAchievementsFromFirebase();
        }
    }
    private void LoadAchievementsFromFirebase()
    {
        dbReference.Child(userId).Child("achievements").GetValueAsync().ContinueWith(task => {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (Achievement achievement in achievements)
                {
                    if (snapshot.HasChild(achievement.achievementId))
                    {
                        var achievementData = snapshot.Child(achievement.achievementId);
                        achievement.isUnlocked = (bool)achievementData.Child("isUnlocked").Value;
                        achievement.claimed = (bool)achievementData.Child("claimed").Value;
                    }
                }
                StartCoroutine(FetchPlayerStatsAndUpdateAchievements());
            }
        });
    }
    public void OpenAchievementPanel()
    {
        StartCoroutine(OpenPanelAndUpdateStats());
    }
    private IEnumerator OpenPanelAndUpdateStats()
    {
        achievementPanel.SetActive(true);  
        yield return FetchPlayerStatsAndUpdateAchievements();
    }
    private IEnumerator FetchPlayerStatsAndUpdateAchievements()
    {
        var task = dbReference.Child(userId).GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);
        if (task.IsFaulted)
        {
            Debug.LogError("Failed to retrieve stats from Firebase.");
            yield break;
        }
        DataSnapshot snapshot = task.Result;
        int currency = snapshot.HasChild("currency") ? int.Parse(snapshot.Child("currency").Value.ToString()) : 0;
        int level = snapshot.HasChild("level") ? int.Parse(snapshot.Child("level").Value.ToString()) : 1;
        int maxWave = snapshot.HasChild("maxWave") ? int.Parse(snapshot.Child("maxWave").Value.ToString()) : 0;
        int stagesReached = snapshot.HasChild("stagesReached") ? int.Parse(snapshot.Child("stagesReached").Value.ToString()) : 0;
        Debug.Log($"Fetched Currency: {currency}, Fetched Level: {level}, Max Wave: {maxWave}, Stages Reached: {stagesReached}");
        CheckAndUnlockAchievements(currency, level, maxWave, stagesReached);
        UpdateAchievementUI(currency, level, maxWave, stagesReached);
    }
    private void UpdateAchievementUI(int cachedCurrency, int cachedLevel, int cachedMaxWave, int cachedStagesReached)
    {
        foreach (Transform child in achievementsContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (Achievement achievement in achievements)
        {
            GameObject achievementItem = Instantiate(achievementItemPrefab, achievementsContainer);
            TMP_Text descriptionText = achievementItem.transform.Find("DescriptionText").GetComponent<TMP_Text>();
            TMP_Text progressText = achievementItem.transform.Find("ProgressText").GetComponent<TMP_Text>();
            TMP_Text rewardText = achievementItem.transform.Find("RewardText").GetComponent<TMP_Text>();
            Image spriteImage = achievementItem.transform.Find("AchievementSprite").GetComponent<Image>();
            descriptionText.text = achievement.description;
            int currentProgress = achievement.statType switch
            {
                StatType.Currency => cachedCurrency,
                StatType.Level => cachedLevel,
                StatType.MaxWave => cachedMaxWave,
                StatType.StagesReached => cachedStagesReached,
                _ => 0
            };
            progressText.text = $"{currentProgress}/{achievement.targetValue}";
            spriteImage.sprite = achievement.spriteType == SpriteType.TypeA ? achievement.sprite1 : achievement.sprite2;
            rewardText.text = $"{achievement.rewardAmount}";
            Button claimButton = achievementItem.transform.Find("ClaimButton").GetComponent<Button>();
            TMP_Text buttonText = claimButton.GetComponentInChildren<TMP_Text>();
            claimButton.onClick.AddListener(() => {
                audioManager.clickAchivement(); 
                ClaimReward(achievement, buttonText);
            });
            claimButton.interactable = achievement.isUnlocked && !achievement.claimed;
            buttonText.text = achievement.claimed ? "Collected" : "Collect";
        }
    }
    private void CheckAndUnlockAchievements(int currency, int level, int maxWave, int stagesReached)
    {
        foreach (Achievement achievement in achievements)
        {
            if (!achievement.isUnlocked)
            {
                switch (achievement.statType)
                {
                    case StatType.Currency:
                        if (currency >= achievement.targetValue)
                            UnlockAchievement(achievement);
                        break;
                    case StatType.Level:
                        if (level >= achievement.targetValue)
                            UnlockAchievement(achievement);
                        break;
                    case StatType.MaxWave:
                        if (maxWave >= achievement.targetValue)
                            UnlockAchievement(achievement);
                        break;
                    case StatType.StagesReached: 
                        if (stagesReached >= achievement.targetValue)
                            UnlockAchievement(achievement);
                        break;
                }
            }
        }
    }
    private void UnlockAchievement(Achievement achievement)
    {
        achievement.isUnlocked = true;
        dbReference.Child(userId).Child("achievements")
            .Child(achievement.achievementId).Child("isUnlocked").SetValueAsync(true);
        dbReference.Child(userId).Child("achievements")
            .Child(achievement.achievementId).Child("claimed").SetValueAsync(false);
        Debug.Log("Unlocked achievement: " + achievement.description);
    }
    public void ClaimReward(Achievement achievement, TMP_Text buttonText)
    {
        if (achievement.isUnlocked && !achievement.claimed)
        {
            achievement.claimed = true;
            List<RewardItem> rewards = new List<RewardItem> { achievement.reward };
            FindObjectOfType<RewardManager>().ShowRewards(rewards);
            audioManager.PlayRewardSoundWithUnmute();
            dbReference.Child(userId).Child("achievements")
                .Child(achievement.achievementId).Child("claimed").SetValueAsync(true);
            buttonText.text = "Collected";
            Debug.Log("Reward claimed for achievement: " + achievement.description);
            StartCoroutine(UpdatePlayerStatsAndUI());
        }
        else if (achievement.claimed)
        {
            Debug.Log("Achievement reward already claimed.");
        }
        else
        {
            Debug.Log("Achievement not unlocked yet.");
        }
    }
    private IEnumerator UpdatePlayerStatsAndUI()
    {
        var task = dbReference.Child(userId).GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);
        if (task.IsFaulted)
        {
            Debug.LogError("Failed to retrieve stats from Firebase.");
            yield break;
        }
        DataSnapshot snapshot = task.Result;
        int currency = snapshot.HasChild("currency") ? int.Parse(snapshot.Child("currency").Value.ToString()) : 0;
        int level = snapshot.HasChild("level") ? int.Parse(snapshot.Child("level").Value.ToString()) : 1;
        int maxWave = snapshot.HasChild("maxWave") ? int.Parse(snapshot.Child("maxWave").Value.ToString()) : 0;
        int stagesReached = snapshot.HasChild("stagesReached") ? int.Parse(snapshot.Child("stagesReached").Value.ToString()) : 0;
        Debug.Log($"Fetched Currency: {currency}, Fetched Level: {level}, Max Wave: {maxWave}, Stages Reached: {stagesReached}");
        UpdateAchievementUI(currency, level, maxWave, stagesReached);
    }
    public void CloseAchievementPanel()
    {
        achievementPanel.SetActive(false);
    }
    public void RecordCurrency()
    {
        StartCoroutine(UpdateCurrency());
    }
    private IEnumerator UpdateCurrency()
    {
        yield return new WaitForSeconds(1f);
        currencyManager.UpdateCurrencyDisplay();
        currencyManager.UpdateGemDisplay();
    }
}