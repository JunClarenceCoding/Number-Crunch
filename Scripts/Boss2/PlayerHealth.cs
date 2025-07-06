using UnityEngine;
using Photon.Pun;
using Firebase.Database;
using System.Threading.Tasks;

public class PlayerHealth : MonoBehaviourPunCallbacks
{
    public int maxHealth { get; set; }
    public int currentHealth { get; set; }

    private Boss2BattleHandler boss2BattleHandler;
    private PlayerHUD playerHUD;
    private DatabaseReference dbRef;

    // Define a delegate and an event for when health is loaded
    public delegate void HealthLoadedHandler();
    public event HealthLoadedHandler OnHealthLoaded;

    void Start()
    {
        playerHUD = FindObjectOfType<PlayerHUD>();
        boss2BattleHandler = FindObjectOfType<Boss2BattleHandler>();

        if (photonView.IsMine)
        {
            // Load health data from Firebase for the local player
            LoadMaxHealthFromFirebase();
        }
    }

    private async void LoadMaxHealthFromFirebase()
    {
        if (FirebaseManager.Instance.IsFirebaseInitialized())
        {
            var userId = FirebaseManager.Instance.Auth.CurrentUser?.UserId;
            if (userId != null)
            {
                dbRef = FirebaseManager.Instance.Database.GetReference("users").Child(userId).Child("maxHealth");

                Debug.Log("Attempting to load maxHealth from Firebase...");
                
                DataSnapshot snapshot = await dbRef.GetValueAsync();
                if (snapshot.Exists)
                {
                    maxHealth = int.Parse(snapshot.Value.ToString());
                    currentHealth = maxHealth; // Initialize current health
                    Debug.Log("Firebase MaxHealth Loaded: " + maxHealth);

                    // Notify that health has been loaded
                    OnHealthLoaded?.Invoke();

                  
                }
                else
                {
                    Debug.LogError("No health data found in Firebase.");
                }
            }
            else
            {
                Debug.LogError("No user logged in.");
            }
        }
        else
        {
            Debug.LogError("Firebase is not initialized.");
        }
    }

    

    public void SetMaxHealth(int maxHealth)
    {
        Debug.Log("Setting MaxHealth to: " + maxHealth);
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;  // Reset current health to max health if needed

        // Update HUDs after setting the health
        // playerHUD?.UpdatePlayerHealth(photonView.Owner, currentHealth);
        boss2BattleHandler?.UpdatePlayerHealthText();
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        if (!photonView.IsMine) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // playerHUD?.UpdatePlayerHealth(photonView.Owner, currentHealth);
        boss2BattleHandler?.UpdatePlayerHealthText();

        photonView.RPC("SyncHealth", RpcTarget.All, currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    [PunRPC]
    public void SyncHealth(int health)
    {
        currentHealth = health;

        if (!photonView.IsMine)
        {
            // playerHUD?.UpdatePlayerHealth(photonView.Owner, currentHealth);
        }
        // No need to update localPlayerHUD anymore
    }

    void Die()
    {
        // Handle player death
        Debug.Log("You are defeated!");
    }
}
