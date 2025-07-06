using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine.UI;
public class ClothingManager : MonoBehaviour
{
    private FirebaseAuth auth;
    private FirebaseDatabase database;
    public Button boyShirt2Button;
    public GameObject OwnedImage;
    private void Start()
    {
        auth = FirebaseManager.Instance.Auth;
        database = FirebaseManager.Instance.Database;
    }
    public void SetBoyShirt2AsOwned()
    {
        string userId = auth.CurrentUser.UserId;
        DatabaseReference clothesRef = database.GetReference("users").Child(userId).Child("clothes").Child("tops").Child("BoySweater");
        clothesRef.Child("owned").SetValueAsync(true).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                boyShirt2Button.interactable = false;
                OwnedImage.SetActive(true);
            }
        });
    }
    public void CheckIfBoyShirt2Owned()
    {
        string userId = auth.CurrentUser.UserId;
        DatabaseReference clothesRef = database.GetReference("users").Child(userId).Child("clothes").Child("tops").Child("BoySweater");
        clothesRef.Child("owned").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists && snapshot.Value != null && (bool)snapshot.Value == true)
                {
                    boyShirt2Button.interactable = false;
                    OwnedImage.SetActive(true);
                }
                else
                {
                    boyShirt2Button.interactable = true;
                    OwnedImage.SetActive(false);
                }
            }
        });
    }
    public void SetGirlSweaterAsOwned()
    {
        string userId = auth.CurrentUser.UserId;
        DatabaseReference clothesRef = database.GetReference("users").Child(userId).Child("clothes").Child("tops").Child("GirlSweater");
        clothesRef.Child("owned").SetValueAsync(true).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                boyShirt2Button.interactable = false;
                OwnedImage.SetActive(true);
            }
        });
    }
    public void CheckIfGirlSweaterOwned()
    {
        string userId = auth.CurrentUser.UserId;
        DatabaseReference clothesRef = database.GetReference("users").Child(userId).Child("clothes").Child("tops").Child("GirlSweater");
        clothesRef.Child("owned").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists && snapshot.Value != null && (bool)snapshot.Value == true)
                {
                    boyShirt2Button.interactable = false;
                    OwnedImage.SetActive(true);
                }
                else
                {
                    boyShirt2Button.interactable = true;
                    OwnedImage.SetActive(false);
                }
            }
        });
    }
}