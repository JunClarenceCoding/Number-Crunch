using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Photon.Pun;
using Photon.Realtime;
using System.Threading.Tasks;

public class PlayerHealthManager : MonoBehaviour
{
    private DatabaseReference dbRef;

    // Dictionary to hold player health information
    private Dictionary<int, (int currentHealth, int maxHealth)> playerHealthData = new Dictionary<int, (int, int)>();

    // Mapping from actor number to user ID
    private Dictionary<int, string> playerUserIdMapping = new Dictionary<int, string>();

    public delegate void PlayerHealthRetrieved(int actorNumber, int health, int maxHealth);
    public event PlayerHealthRetrieved OnPlayerHealthRetrieved;

    async void Start()
    {
        Debug.Log("ETO YUN Checking Firebase initialization...");
        if (FirebaseManager.Instance.IsFirebaseInitialized())
        {
            Debug.Log("ETO YUN Firebase initialized successfully.");
            
            // Populate playerUserIdMapping and wait for it to complete
            await PopulatePlayerUserIdMapping();
            
            // Fetch health data for all players
            FetchAllPlayersHealth();
        }
        else
        {
            Debug.LogError("ETO YUN Firebase is not initialized.");
        }
    }

    private async Task PopulatePlayerUserIdMapping()
    {
        List<Task> tasks = new List<Task>();

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            // Fetch the userID from Firebase using the player's nickname
            Task fetchTask = GetUserIdForPlayer(player);
            tasks.Add(fetchTask);
        }

        // Wait for all Firebase fetches to complete
        await Task.WhenAll(tasks);
    }


    private async Task GetUserIdForPlayer(Player player)
    {
        string nickname = player.NickName; // Get the player's nickname
        DatabaseReference usernameRef = FirebaseManager.Instance.Database.GetReference("usernames").Child(nickname);

        await usernameRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted && task.Exception == null)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    string userId = snapshot.Value.ToString(); // Get the userID from the database
                    playerUserIdMapping[player.ActorNumber] = userId; // Store the userID in the mapping
                    Debug.Log($"ETO YUN  Mapped player {player.NickName} to UserID: {userId}");
                }
                else
                {
                    Debug.LogWarning($"ETO YUN  No UserID found for player {player.NickName}");
                }
            }
            else
            {
                Debug.LogError($"ETO YUN  Error retrieving UserID for player {player.NickName}: {task.Exception}");
            }
        });
    }


    private void FetchAllPlayersHealth()
    {
        Debug.Log($"ETO YUN Number of players in the room: {PhotonNetwork.PlayerList.Length}");
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (playerUserIdMapping.TryGetValue(player.ActorNumber, out string userId))
            {
                Debug.Log($"ETO YUN Fetching health for player: {player.NickName} (ActorNumber: {player.ActorNumber}), UserID: {userId}");
                dbRef = FirebaseManager.Instance.Database.GetReference("users").Child(userId).Child("maxHealth");

                dbRef.GetValueAsync().ContinueWith(task =>
                {
                    if (task.IsCompleted && task.Exception == null)
                    {
                        DataSnapshot snapshot = task.Result;

                        if (snapshot.Exists)
                        {
                            Debug.Log($"ETO YUN  Snapshot Value: {snapshot.Value}");
                            if (int.TryParse(snapshot.Value.ToString(), out int maxHealth))
                            {
                                int currentHealth = maxHealth;
                                Debug.Log($"ETO YUN  Retrieved health for player {player.NickName}: MaxHealth = {maxHealth}, CurrentHealth = {currentHealth}");

                                // Store health data in the dictionary
                                playerHealthData[player.ActorNumber] = (currentHealth, maxHealth);

                                // Invoke the event to update the HUD
                                Debug.Log($"ETO YUN  Invoking health update for player {player.NickName} (ActorNumber: {player.ActorNumber})");
                                OnPlayerHealthRetrieved?.Invoke(player.ActorNumber, currentHealth, maxHealth);
                            }
                            else
                            {
                                Debug.LogWarning($"ETO YUN  Failed to parse maxHealth for player {player.NickName}. Value: {snapshot.Value}");
                            }
                        }
                        else
                        {
                            Debug.LogWarning($"ETO YUN  Health data not found for player {player.NickName}");
                        }
                    }
                    else
                    {
                        Debug.LogError($"ETO YUN  Failed to retrieve health for player {player.NickName}: {task.Exception}");
                    }
                });
            }
            else
            {
                Debug.LogError($"ETO YUN UserID not found for player {player.NickName} (ActorNumber: {player.ActorNumber})");
            }
        }
    }

    // Method to get current health for a player
    public int GetPlayerCurrentHealth(int actorNumber)
    {
        if (playerHealthData.TryGetValue(actorNumber, out var healthData))
        {
            Debug.Log($"ETO YUN Current health for player (ActorNumber: {actorNumber}): {healthData.currentHealth}");
            return healthData.currentHealth;
        }
        Debug.LogWarning($"ETO YUN  No current health data found for player (ActorNumber: {actorNumber})");
        return 0; // Or some default value if the actorNumber is not found
    }

    // Method to get max health for a player
    public int GetPlayerMaxHealth(int actorNumber)
    {
        if (playerHealthData.TryGetValue(actorNumber, out var healthData))
        {
            Debug.Log($"ETO YUN Max health for player (ActorNumber: {actorNumber}): {healthData.maxHealth}");
            return healthData.maxHealth;
        }
        Debug.LogWarning($"ETO YUN  No max health data found for player (ActorNumber: {actorNumber})");
        return 0; // Or some default value if the actorNumber is not found
    }
}
