using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Photon.Pun;
using Photon.Realtime;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;
public class PlayerInfoManager : MonoBehaviour
{
    private DatabaseReference dbRef;
    private ConcurrentDictionary<int, (int level, int maxHealth)> playerInfoData = new ConcurrentDictionary<int, (int, int)>();
    private ConcurrentDictionary<int, string> playerUserIdMapping = new ConcurrentDictionary<int, string>();
    public delegate void PlayerInfoRetrieved(int actorNumber, int level, int maxHealth);
    public event PlayerInfoRetrieved OnPlayerInfoRetrieved;
    async void Start()
    {
        try
        {
            if (FirebaseManager.Instance.IsFirebaseInitialized())
            {
                await PopulatePlayerUserIdMapping();
                await FetchAllPlayersInfo();
            }
            else
            {
                Debug.LogError("Firebase is not initialized.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"An error occurred in Start: {ex}");
        }
    }
    private async Task PopulatePlayerUserIdMapping()
    {
        List<Task> tasks = new List<Task>();
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            Task fetchTask = GetUserIdForPlayer(player);
            tasks.Add(fetchTask);
        }
        await Task.WhenAll(tasks);
    }
    private async Task GetUserIdForPlayer(Player player)
    {
        try
        {
            string nickname = player.NickName;
            DatabaseReference usernameRef = FirebaseManager.Instance.Database.GetReference("usernames").Child(nickname);
            var snapshot = await usernameRef.GetValueAsync();
            if (snapshot.Exists)
            {
                string userId = snapshot.Value.ToString();
                playerUserIdMapping[player.ActorNumber] = userId;
            }
            else
            {
                Debug.LogWarning($"No UserID found for player {player.NickName}");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error retrieving UserID for player {player.NickName}: {ex}");
        }
    }
    private async Task FetchAllPlayersInfo()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (playerUserIdMapping.TryGetValue(player.ActorNumber, out string userId))
            {
                await FetchPlayerInfo(player, userId);
                await Task.Delay(100); 
            }
        }
    }
    private async Task FetchPlayerInfo(Player player, string userId)
    {
        try
        {
            dbRef = FirebaseManager.Instance.Database.GetReference("users").Child(userId);
            var snapshot = await dbRef.GetValueAsync();
            if (snapshot.Exists && snapshot.HasChild("level") && snapshot.HasChild("maxHealth"))
            {
                int level = int.Parse(snapshot.Child("level").Value.ToString());
                int maxHealth = int.Parse(snapshot.Child("maxHealth").Value.ToString());

                playerInfoData[player.ActorNumber] = (level, maxHealth);

                OnPlayerInfoRetrieved?.Invoke(player.ActorNumber, level, maxHealth);
            }
            else
            {
                Debug.LogWarning($"Player info not found for {player.NickName}");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error retrieving player info for {player.NickName}: {ex}");
        }
    }
    public int GetPlayerLevel(int actorNumber)
    {
        return playerInfoData.TryGetValue(actorNumber, out var infoData) ? infoData.level : 0;
    }
    public int GetPlayerMaxHealth(int actorNumber)
    {
        return playerInfoData.TryGetValue(actorNumber, out var infoData) ? infoData.maxHealth : 0;
    }
}