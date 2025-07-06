using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Auth;
using TMPro;
using UnityEngine;
using System.Collections;
public class CurrencyManager : MonoBehaviour
{
    public TMP_Text[] currencyTexts;
    public TMP_Text[] gemTexts; 
    public GameObject panel; 
    public int currencyIncrementAmount = 1000; 
    public int gemIncrementAmount = 500; 
    private DatabaseReference databaseReference;
    private string userId;
    private RewardManager rewardsManager;
    private IEnumerator Start()
    {

        rewardsManager = FindObjectOfType<RewardManager>();
        if (rewardsManager == null)
        {
            Debug.LogError("RewardsManager instance not found!");
        }
         if (FirebaseManager.Instance != null)
        {
            yield return FirebaseManager.Instance.WaitForFirebaseInitialization();
            databaseReference = FirebaseManager.Instance.Database.RootReference;
        }
        if (FirebaseManager.Instance.IsFirebaseInitialized())
        {
            InitializeCurrencyManager();
        }
    }
    void OnEnable()
    {
        if (FirebaseManager.Instance.IsFirebaseInitialized() && FirebaseManager.Instance.Auth.CurrentUser != null)
        {
            InitializeCurrencyManager(); 
        }
        else
        {
            InitializeCurrencyManager();
        }
    }
    private void InitializeCurrencyManager()
    {
        if (FirebaseManager.Instance.Auth != null && FirebaseManager.Instance.Auth.CurrentUser != null)
        {
            databaseReference = FirebaseManager.Instance.Database.RootReference;
            userId = FirebaseManager.Instance.Auth.CurrentUser.UserId;
            UpdateCurrencyDisplay();
            UpdateGemDisplay();
        }
    }
    public void GetCurrency(System.Action<int> callback)
    {
        if (!string.IsNullOrEmpty(userId))
        {
            databaseReference.Child("users").Child(userId).Child("currency").GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && task.Result.Exists)
                {
                    int currency = int.Parse(task.Result.Value.ToString());
                    callback?.Invoke(currency);
                }
                else
                {
                    callback?.Invoke(0);
                }
            });
        }
        else
        {
            callback?.Invoke(0);
        }
    }
    public void GetGems(System.Action<int> callback)
    {
        if (!string.IsNullOrEmpty(userId))
        {
            databaseReference.Child("users").Child(userId).Child("gems").GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && task.Result.Exists)
                {
                    int gems = int.Parse(task.Result.Value.ToString());
                    callback?.Invoke(gems);
                }
                else
                {
                    callback?.Invoke(0);
                }
            });
        }
        else
        {
            callback?.Invoke(0);
        }
    }
    public void UpdateCurrency(int amount)
    {
        if (!string.IsNullOrEmpty(userId))
        {
            databaseReference.Child("users").Child(userId).Child("currency").SetValueAsync(amount).ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    UpdateCurrencyDisplay(); 
                }
            });
        }
    }
    public void UpdateGems(int amount)
    {
        if (!string.IsNullOrEmpty(userId))
        {
            databaseReference.Child("users").Child(userId).Child("gems").SetValueAsync(amount).ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    UpdateGemDisplay(); 
                }
            });
        }
    }
    public void AddCurrency()
    {
        GetCurrency(currentCurrency =>
        {
            int newCurrencyAmount = currentCurrency + currencyIncrementAmount;
            UpdateCurrency(newCurrencyAmount);
        });
    }
    public void AddGems()
    {
        GetGems(currentGems =>
        {
            int newGemAmount = currentGems + gemIncrementAmount;
            UpdateGems(newGemAmount);
        });
    }
    public void DeductCurrency()
    {
        GetCurrency(currentCurrency =>
        {
            int newCurrencyAmount = Mathf.Max(0, currentCurrency - currencyIncrementAmount);
            UpdateCurrency(newCurrencyAmount);
        });
    }
    public void DeductGems()
    {
        GetGems(currentGems =>
        {
            int newGemAmount = Mathf.Max(0, currentGems - gemIncrementAmount); 
            UpdateGems(newGemAmount);
        });
    }
    public void UpdateCurrencyDisplay()
    {
        GetCurrency(currentCurrency =>
        {
            foreach (TMP_Text text in currencyTexts)
            {
                if (text != null)
                {
                    text.text = currentCurrency.ToString();
                }
            }
        });
    }
    public void UpdateGemDisplay()
    {
        GetGems(currentGems =>
        {
            foreach (TMP_Text text in gemTexts)
            {
                if (text != null)
                {
                    text.text = currentGems.ToString();
                }
            }
        });
    }
    public void OpenPanelAndUpdateResources()
    {
        panel.SetActive(true); 
        UpdateCurrencyDisplay(); 
        UpdateGemDisplay(); 
    }
    private IEnumerator ClaimLevelUpRewardsWithUpdateDelay()
    {
        rewardsManager.ClaimLevelUpRewards();
        yield return new WaitForSeconds(0.5f);
        UpdateCurrencyDisplay();
        UpdateGemDisplay();
    }
    public void ClaimLevelUpRewards()
    {
        StartCoroutine(ClaimLevelUpRewardsWithUpdateDelay());
    }
}