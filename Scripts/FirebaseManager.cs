using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions; 
using System;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;
    public FirebaseAuth Auth { get; private set; }
    public FirebaseDatabase Database { get; private set; }
    private FirebaseApp app;
    private bool firebaseInitialized = false;
    public event Action OnFirebaseInitialized;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
            StartCoroutine(InitializeFirebase());
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    private IEnumerator InitializeFirebase()
    {
        var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();
        yield return new WaitUntil(() => dependencyTask.IsCompleted);

        var dependencyStatus = dependencyTask.Result;
        if (dependencyStatus == DependencyStatus.Available)
        {
            firebaseInitialized = true;
            app = FirebaseApp.DefaultInstance;
            Auth = FirebaseAuth.DefaultInstance;
            Database = FirebaseDatabase.DefaultInstance;
            Debug.Log("Firebase Initialized Successfully.");
            OnFirebaseInitialized?.Invoke();
            CheckUserAuth();
        }
    }
    public string GetUserId()
    {
        return Auth.CurrentUser != null ? Auth.CurrentUser.UserId : null;
    }
    public DatabaseReference GetDatabaseReference(string path)
    {
        return Database.GetReference(path);
    }
    public bool IsFirebaseInitialized() => firebaseInitialized;
    public IEnumerator WaitForFirebaseInitialization(){
        yield return new WaitUntil(() => firebaseInitialized == true);
    }
    private void CheckUserAuth(){
        if (Auth.CurrentUser != null)
        {
            Debug.Log($"User is already signed in: {Auth.CurrentUser.DisplayName}");
        }else{
            Debug.Log("No user signed in.");
        }
    }
    public void GetEquippedClothes(Action<Dictionary<string, Dictionary<string, bool>>> onClothesDataRetrieved)
    {
        string userId = GetUserId(); // Get the current user ID
        DatabaseReference userRef = Database.GetReference("users").Child(userId).Child("clothes");
        userRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error retrieving clothes data.");
                return;
            }
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    Dictionary<string, Dictionary<string, bool>> clothesData = new Dictionary<string, Dictionary<string, bool>>();
                    foreach (var clothesCategory in snapshot.Children)
                    {
                        Dictionary<string, bool> items = new Dictionary<string, bool>();
                        foreach (var item in clothesCategory.Children)
                        {
                            bool isEquipped = bool.Parse(item.Child("equipped").Value.ToString());
                            items.Add(item.Key, isEquipped);
                        }
                        clothesData.Add(clothesCategory.Key, items);
                    }
                    onClothesDataRetrieved?.Invoke(clothesData);
                }
            }
        });
    }
    public void GetOwnedClothes(Action<Dictionary<string, Dictionary<string, bool>>> onClothesDataRetrieved)
    {
        string userId = GetUserId();
        DatabaseReference userRef = Database.GetReference("users").Child(userId).Child("clothes");
        userRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error retrieving clothes data.");
                return;
            }
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    Dictionary<string, Dictionary<string, bool>> clothesData = new Dictionary<string, Dictionary<string, bool>>();
                    foreach (var clothesCategory in snapshot.Children)
                    {
                        Dictionary<string, bool> items = new Dictionary<string, bool>();
                        foreach (var item in clothesCategory.Children)
                        {
                            var ownedSnapshot = item.Child("owned");
                            if (ownedSnapshot.Exists)
                            {
                                bool isOwned = bool.Parse(ownedSnapshot.Value.ToString());
                                if (isOwned)
                                {
                                    items.Add(item.Key, isOwned);
                                }
                            }
                        }
                        if (items.Count > 0)
                        {
                            clothesData.Add(clothesCategory.Key, items); 
                        }
                    }
                    onClothesDataRetrieved?.Invoke(clothesData);
                }
            }
        });
    }
    public Sprite GetClothingSprite(string clothingName)
    {
        Sprite clothingSprite = Resources.Load<Sprite>($"ClothesImages/{clothingName}");
        if (clothingSprite == null)
        {
            Debug.LogError($"Clothing image for {clothingName} not found in Resources.");
        }
        return clothingSprite;
    }
    public void UpdateStage4Task(bool taskCompleted)
    {
        string userId = GetUserId(); 
        DatabaseReference userRef = Database.GetReference("users").Child(userId).Child("stages").Child("stage04");
        userRef.Child("stage4Task").SetValueAsync(taskCompleted).ContinueWithOnMainThread(task => 
        {
            if (task.IsCompleted)
            {
                Debug.Log("Stage4 task updated successfully.");
            }
        });
    }
    public void UpdateStage8Task(bool taskCompleted)
    {
        string userId = GetUserId();
        DatabaseReference userRef = Database.GetReference("users").Child(userId).Child("stages").Child("stage08");
        userRef.Child("stage8Task").SetValueAsync(taskCompleted).ContinueWithOnMainThread(task => 
        {
            if (task.IsCompleted)
            {
                Debug.Log("Stage8 task updated successfully.");
            }
        });
    }
    public void UpdateStage12Task(bool taskCompleted)
    {
        string userId = GetUserId(); 
        DatabaseReference userRef = Database.GetReference("users").Child(userId).Child("stages").Child("stage12");

        userRef.Child("stage12Task").SetValueAsync(taskCompleted).ContinueWithOnMainThread(task => 
        {
            if (task.IsCompleted)
            {
                Debug.Log("Stage12 task updated successfully.");
            }
        });
    }
    public void UpdateStage16Task(bool taskCompleted)
    {
        string userId = GetUserId();
        DatabaseReference userRef = Database.GetReference("users").Child(userId).Child("stages").Child("stage16");

        userRef.Child("stage16Task").SetValueAsync(taskCompleted).ContinueWithOnMainThread(task => 
        {
            if (task.IsCompleted)
            {
                Debug.Log("Stage16 task updated successfully.");
            }
        });
    }
    public void GetAllClothes(Action<Dictionary<string, Dictionary<string, bool>>> onClothesDataRetrieved)
    {
        string userId = GetUserId();
        DatabaseReference userRef = Database.GetReference("users").Child(userId).Child("clothes");
        userRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error retrieving clothes data.");
                return;
            }
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    Dictionary<string, Dictionary<string, bool>> clothesData = new Dictionary<string, Dictionary<string, bool>>();
                    foreach (var clothesCategory in snapshot.Children)
                    {
                        Dictionary<string, bool> items = new Dictionary<string, bool>();
                        foreach (var item in clothesCategory.Children)
                        {
                            var ownedSnapshot = item.Child("owned");
                            if (ownedSnapshot.Exists)
                            {
                                bool isOwned = bool.Parse(ownedSnapshot.Value.ToString());
                                items.Add(item.Key, isOwned);
                            }
                        }
                        if (items.Count > 0)
                        {
                            clothesData.Add(clothesCategory.Key, items);
                        }
                    }
                    onClothesDataRetrieved?.Invoke(clothesData);
                }
            }
        });
    }
}