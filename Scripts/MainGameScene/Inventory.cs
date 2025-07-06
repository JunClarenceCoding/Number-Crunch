using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using Firebase.Auth;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Item redPotion;
    public Item bluePotion;
    public Item damagePotion;
    private DatabaseReference dbReference;
    private Dictionary<Item, int> items = new Dictionary<Item, int>();

    private void Start()
    {
        if (FirebaseManager.Instance.IsFirebaseInitialized())
        {
            InitializeInventory();
        }
    }
    private void InitializeInventory()
    {
        string userId = FirebaseManager.Instance.Auth.CurrentUser.UserId;
        dbReference = FirebaseManager.Instance.Database.GetReference("users").Child(userId).Child("inventory");
        LoadInventoryFromDatabase();
    }
    public void EnsureInventoryIsLoaded(System.Action onInventoryLoaded = null)
    {
        if (items.Count == 0)
        {
            LoadInventoryFromDatabase(); 
            if (onInventoryLoaded != null)
            {
                onInventoryLoaded();
            }
        }
        else
        {
            if (onInventoryLoaded != null)
            {
                onInventoryLoaded();
            }
        }
    }
    public Dictionary<Item, int> GetItems()
    {
        return items;
    }
    public void AddItem(Item item, int quantity)
    {
        if (item.isStackable && items.ContainsKey(item))
        {
            items[item] += quantity;
        }
        else
        {
            items[item] = quantity;
        }
        UpdateItemInFirebase(item, items[item]);
    }
    public void DecreaseItemQuantity(Item item, int amount)
    {
        if (items.ContainsKey(item))
        {
            items[item] -= amount;

            if (items[item] <= 0)
            {
                items[item] = 0;  
            }
            UpdateItemInFirebase(item, items[item]);
        }
    }
    private void UpdateItemInFirebase(Item item, int quantity)
    {
        dbReference.Child(item.itemName).SetValueAsync(quantity).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log(item.itemName + " updated in Firebase with quantity: " + quantity);
            }
        });
    }
    private void LoadInventoryFromDatabase()
    {
        dbReference.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot itemSnapshot in snapshot.Children)
                {
                    string itemName = itemSnapshot.Key;
                    int itemQuantity = int.Parse(itemSnapshot.Value.ToString());
                    Item item = FindItemByName(itemName);
                    if (item != null)
                    {
                        items[item] = itemQuantity;
                    }
                }
                FindObjectOfType<InventoryUI>().UpdateUI();
            }
        });
    }
    private Item FindItemByName(string itemName)
    {
        if (itemName == redPotion.itemName) return redPotion;
        if (itemName == bluePotion.itemName) return bluePotion;
        if (itemName == damagePotion.itemName) return damagePotion;
        return null;
    }
    public Item GetItemByName(string itemName)
    {
        return FindItemByName(itemName);
    }
    public int GetItemCount(Item item)
    {
        if (items.ContainsKey(item))
        {
            return items[item];
        }
        return 0;
    }
}