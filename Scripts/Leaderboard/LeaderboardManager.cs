using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Database;
using Firebase.Extensions;
using System.Linq;

public class LeaderboardManager : MonoBehaviour
{
    public Button waveButton, levelButton;
    public GameObject LeaderboardPanel, wavePanel, levelPanel;
    public GameObject LeaderboardWaveEntry, LeaderboardLevelEntry; 
    public Transform waveContainer, levelContainer; 
    public TMP_Text waveTextButton, levelTextButton;
    public Sprite[] rankSprites; 
    private Image waveButtonImage, levelButtonImage;
    private Color activeTextColor = new Color32(255, 255, 255, 255);  
    private Color inactiveTextColor = new Color32(128, 126, 120, 255); 
    private bool isInitialized = false;
    private void Start()
    {
        LeaderboardPanel.SetActive(false); 
        InitializeUI();
    }
    public void InitializeUI()
    {
        if (!isInitialized)
        {
            waveButtonImage = waveButton.GetComponent<Image>();
            levelButtonImage = levelButton.GetComponent<Image>();
            waveButton.onClick.AddListener(ActivateWavePanel);
            levelButton.onClick.AddListener(ActivateLevelPanel);
            ActivateLevelPanel(); 
            waveTextButton.text = "WAVE";
            levelTextButton.text = "LEVEL";
            isInitialized = true;
        }
    }
    public void OpenLeaderboardPanel()
    {
        LeaderboardPanel.SetActive(true); 
        Debug.Log("Leaderboard panel opened.");
        PopulateLevelLeaderboard();
    }
    public void CloseLeaderboardPanel()
    {
        LeaderboardPanel.SetActive(false);
    }
    private void ActivateWavePanel()
    {
        SetActivePanel(wavePanel, waveTextButton, waveButtonImage);
        PopulateWaveLeaderboard(); 
    }
    private void ActivateLevelPanel()
    {
        SetActivePanel(levelPanel, levelTextButton, levelButtonImage);
        PopulateLevelLeaderboard(); 
    }
    private void SetActivePanel(GameObject activePanel, TMP_Text activeText, Image activeButtonImage)
    {
        wavePanel.SetActive(false); 
        levelPanel.SetActive(false);
        activePanel.SetActive(true); 
        waveTextButton.color = inactiveTextColor;
        levelTextButton.color = inactiveTextColor;
        activeText.color = activeTextColor;
        waveButtonImage.enabled = false;
        levelButtonImage.enabled = false;
        activeButtonImage.enabled = true; 
    }
    private void FetchLeaderboardData(string leaderboardType)
    {
        FirebaseManager.Instance.Database.GetReference("users").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.ChildrenCount > 0)
                {
                    Dictionary<string, UserData> usersDict = new Dictionary<string, UserData>();
                    foreach (var user in snapshot.Children)
                    {
                        string userId = user.Key;
                        if (user.Child("level").Value != null && user.Child("maxWave").Value != null)
                        {
                            int level = int.Parse(user.Child("level").Value.ToString());
                            int maxWave = int.Parse(user.Child("maxWave").Value.ToString());
                            usersDict[userId] = new UserData(userId, level, maxWave);
                        }
                    }
                    FetchUsernames(usersDict, leaderboardType);
                }
            }
        });
    }
    private void PopulateWaveLeaderboard()
    {
        ClearContainer(waveContainer);
        FetchLeaderboardData("wave");
    }
    private void PopulateLevelLeaderboard()
    {
        ClearContainer(levelContainer);
        FetchLeaderboardData("level");
    }
    private void ClearContainer(Transform container)
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
    }
    private Sprite GetRankSprite(int rank)
    {
        if (rank > 0 && rank <= rankSprites.Length)
        {
            return rankSprites[rank - 1]; 
        }
        return null;
    }
    public class UserData
    {
        public string UserId { get; private set; }
        public int Level { get; private set; }
        public int MaxWave { get; private set; }
        public UserData(string userId, int level, int maxWave)
        {
            UserId = userId;
            Level = level;
            MaxWave = maxWave;
        }
    }
    private void FetchUsernames(Dictionary<string, UserData> usersDict, string leaderboardType)
    {
        FirebaseManager.Instance.Database.GetReference("usernames").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Dictionary<string, string> userIdToUsername = new Dictionary<string, string>();
                foreach (var userEntry in snapshot.Children)
                {
                    string username = userEntry.Key;
                    string userId = userEntry.Value.ToString();

                    if (usersDict.ContainsKey(userId))
                    {
                        userIdToUsername[userId] = username;
                    }
                }
                List<UserData> rankedUsers = usersDict.Values.ToList();
                if (leaderboardType == "wave")
                {
                    rankedUsers.Sort((x, y) => y.MaxWave.CompareTo(x.MaxWave));
                }
                else if (leaderboardType == "level")
                {
                    rankedUsers.Sort((x, y) => y.Level.CompareTo(x.Level));
                }
                int maxEntries = 20; 
                for (int i = 0; i < Mathf.Min(rankedUsers.Count, maxEntries); i++)
                {
                    UserData userData = rankedUsers[i];
                    string username = userIdToUsername[userData.UserId];
                    if (leaderboardType == "wave")
                    {
                        CreateWaveLeaderboardEntry(username, userData.MaxWave, i + 1);
                    }
                    else if (leaderboardType == "level")
                    {
                        CreateLevelLeaderboardEntry(username, userData.Level, i + 1);
                    }
                }
            }
        });
    }
    private void CreateWaveLeaderboardEntry(string userName, int maxWave, int rank)
    {
        GameObject waveEntry = Instantiate(LeaderboardWaveEntry, waveContainer);
        TMP_Text waveUsernameText = waveEntry.transform.Find("UsernameText")?.GetComponent<TMP_Text>();
        TMP_Text waveMaxWaveText = waveEntry.transform.Find("MaxWaveText")?.GetComponent<TMP_Text>();
        Image waveRankImage = waveEntry.transform.Find("RankImage")?.GetComponent<Image>();
        if (waveUsernameText != null) waveUsernameText.text = userName;
        if (waveMaxWaveText != null) waveMaxWaveText.text = maxWave.ToString();
        if (waveRankImage != null) waveRankImage.sprite = GetRankSprite(rank);
    }
    private void CreateLevelLeaderboardEntry(string userName, int level, int rank)
    {
        GameObject levelEntry = Instantiate(LeaderboardLevelEntry, levelContainer);
        TMP_Text levelUsernameText = levelEntry.transform.Find("UsernameText")?.GetComponent<TMP_Text>();
        TMP_Text levelLevelText = levelEntry.transform.Find("LevelText")?.GetComponent<TMP_Text>();
        Image levelRankImage = levelEntry.transform.Find("RankImage")?.GetComponent<Image>();
        if (levelUsernameText != null) levelUsernameText.text = userName;
        if (levelLevelText != null) levelLevelText.text = level.ToString();
        if (levelRankImage != null) levelRankImage.sprite = GetRankSprite(rank);
    }
}