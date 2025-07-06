using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using System.Collections;
using System.Threading.Tasks;

public class PlayerSoloHeallth : MonoBehaviour
{
    public int currentHealth { get; set; }
    public int MaxHealth { get; set; }
    public delegate void HealthLoadedHandler();
    public event HealthLoadedHandler OnHealthLoaded;
    public delegate void HealthUpdatedHandler(int newHealth);
    public event HealthUpdatedHandler OnHealthUpdated;
    private FirebaseApp app;
    private DatabaseReference dbRef;

    async void Start()
    {
        if (FirebaseManager.Instance.IsFirebaseInitialized())
        {
            LoadMaxHealthFromDatabase();
        }
    }
    public void LoadMaxHealthFromDatabase()
    {
        var userId = FirebaseManager.Instance.Auth.CurrentUser?.UserId;
        if (userId != null)
        {
            dbRef = FirebaseManager.Instance.Database.GetReference("users").Child(userId).Child("maxHealth");
        }
        else
        {
            Debug.LogError("No user is logged in.");
            return;
        }
        dbRef.GetValueAsync().ContinueWith(task => 
        {
            if (task.IsCompleted && task.Exception == null)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    MaxHealth = int.Parse(snapshot.Value.ToString());
                }
            }
            currentHealth = MaxHealth;
            OnHealthLoaded?.Invoke();
            OnHealthUpdated?.Invoke(currentHealth);
        });
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Player defeated");
        }

        OnHealthUpdated?.Invoke(currentHealth);
    }
    public void SetMaxHealth(int maxHealth)
    {
        MaxHealth = maxHealth;
        currentHealth = maxHealth; 
        OnHealthUpdated?.Invoke(currentHealth);
    }
}