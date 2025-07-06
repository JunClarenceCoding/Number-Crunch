using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    private Dictionary<Item, float> itemCooldowns = new Dictionary<Item, float>();
    private Dictionary<Item, Coroutine> activeCooldownCoroutines = new Dictionary<Item, Coroutine>();  
    public GameObject inventoryUI;
    public Inventory inventory;  
    public Item redPotion;  
    public Item bluePotion;  
    public Item damagePotion;  
    public Transform inventoryPanel;  
    public GameObject inventorySlotButtonPrefab;  
    public GameObject itemDetailsPanelPrefab;
    private GameObject activeItemDetailsPanel;  
    private PlayerSoloHeallth playerComponent; 
    public GameObject notUsablePanelPrefab, DamageNotUsable, SuccessDamage, DamageAlreadyUsed; 
    private AudioManager audioManager;
    public bool isUsable = true;  
    public bool isInventoryOpen = false;
    public CooldownHandler cooldownHandler;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene.");
            return;
        }
        if (inventory == null)
        {
            inventory = FindObjectOfType<Inventory>();
        }
        playerComponent = FindObjectOfType<PlayerSoloHeallth>();
        if (playerComponent == null)
        {
            Debug.LogError("PlayerSoloHeallth component not found!");
        }
        notUsablePanelPrefab.SetActive(false);
        if (cooldownHandler == null)
        {
            cooldownHandler = FindObjectOfType<CooldownHandler>(); 
            if (cooldownHandler == null)
            {
                Debug.LogError("CooldownHandler not found in the scene!");
            }
        }
    }
    public void UpdateUI()
    {
        foreach (KeyValuePair<Item, int> item in inventory.GetItems())
        {
            if (item.Value > 0)
            {
                GameObject existingButton = FindItemButton(item.Key);
                if (existingButton == null)
                {
                    GameObject newButton = Instantiate(inventorySlotButtonPrefab, inventoryPanel);
                    TMP_Text itemName = newButton.transform.Find("ItemName")?.GetComponent<TMP_Text>();
                    TMP_Text itemCount = newButton.transform.Find("ItemCount")?.GetComponent<TMP_Text>();
                    Image itemIcon = newButton.transform.Find("ItemIcon")?.GetComponent<Image>();
                    if (itemName != null && itemCount != null && itemIcon != null)
                    {
                        if (item.Key.itemName.Contains("Red"))
                            itemName.text = "Health Potion";
                        else if (item.Key.itemName.Contains("Blue"))
                            itemName.text = "Clockstop \n Potion";
                        else if (item.Key.itemName.Contains("Damage"))
                            itemName.text = "Damage \n Boost Potion";
                        else
                            itemName.text = item.Key.itemName; 
                        itemCount.text = item.Value.ToString();
                        itemIcon.sprite = item.Key.itemIcon;
                    }
                    Button slotButton = newButton.GetComponent<Button>();
                    slotButton.onClick.AddListener(() =>
                    {
                        audioManager.clickButton();
                        ShowItemDetails(newButton.transform, item.Key);
                    });
                    TMP_Text cooldownText = newButton.transform.Find("CooldownText")?.GetComponent<TMP_Text>();
                    if (cooldownText != null)
                    {
                        cooldownText.gameObject.SetActive(false);
                    }
                    if (cooldownHandler != null && cooldownHandler.IsItemOnCooldown(item.Key))
                    {
                        cooldownHandler.StartCooldownUI(newButton, item.Key);
                    }
                }
                else
                {
                    TMP_Text itemCount = existingButton.transform.Find("ItemCount")?.GetComponent<TMP_Text>();
                    if (itemCount != null)
                    {
                        itemCount.text = item.Value.ToString();
                    }
                    TMP_Text cooldownText = existingButton.transform.Find("CooldownText")?.GetComponent<TMP_Text>();
                    if (cooldownText != null && cooldownHandler != null && cooldownHandler.IsItemOnCooldown(item.Key))
                    {
                        cooldownHandler.UpdateCooldownUI(existingButton, item.Key);
                    }
                }
            }
        }
    }
    private void ShowItemDetails(Transform slotButtonTransform, Item item)
    {
        if (activeItemDetailsPanel != null)
        {
            Destroy(activeItemDetailsPanel);
        }
        activeItemDetailsPanel = Instantiate(itemDetailsPanelPrefab, transform);
        Image detailsIcon = activeItemDetailsPanel.transform.Find("ItemIcon")?.GetComponent<Image>();
        TMP_Text detailsDescription = activeItemDetailsPanel.transform.Find("ItemDescription")?.GetComponent<TMP_Text>();
        if (detailsIcon != null)
        {
            detailsIcon.sprite = item.itemIcon;
        }
        if (detailsDescription != null)
        {
            detailsDescription.text = item.description;
        }
        Button useButton = activeItemDetailsPanel.transform.Find("UseButton")?.GetComponent<Button>();
        Button cancelButton = activeItemDetailsPanel.transform.Find("CancelButton")?.GetComponent<Button>();
        if (useButton != null)
        {
            useButton.onClick.RemoveAllListeners();
            useButton.onClick.AddListener(() =>
            {
                audioManager.clickButton();  
                UseItem(item.itemName);
            });
        }
        if (cancelButton != null)
        {
            cancelButton.onClick.RemoveAllListeners();
            cancelButton.onClick.AddListener(() =>
            {
                audioManager.clickButton(); 
                HideItemDetails();
            });
        }
        RectTransform detailsPanelRect = activeItemDetailsPanel.GetComponent<RectTransform>();
        RectTransform parentRect = transform.GetComponent<RectTransform>();
        detailsPanelRect.pivot = new Vector2(0.5f, 0f);
        float yOffset = 145f;  
        detailsPanelRect.anchoredPosition = new Vector2(0, yOffset);
    }
    public void HideItemDetails()
    {
        if (activeItemDetailsPanel != null)
        {
            Destroy(activeItemDetailsPanel);
        }
    }
    public void AddPotions(Item item, int quantity)
    {
        inventory.AddItem(item, quantity);
        UpdateUI();
    }
    public void UseItem(string itemName)
    {
        Item itemToUse = inventory.GetItemByName(itemName);
        if (itemToUse != null)
        {
            GameObject itemButton = FindItemButton(itemToUse);  
            if (isUsable && itemToUse.itemName == "Red Potion")
            {
                if (playerComponent != null)
                {
                    playerComponent.currentHealth = Mathf.Min(playerComponent.MaxHealth, playerComponent.currentHealth + 50);
                    Debug.Log("Health increased to: " + playerComponent.currentHealth);
                }
                var battleHandlers = new List<MonoBehaviour>
                {
                    FindObjectOfType<BattleTrialHandler>(),
                    FindObjectOfType<Stage1BattleHandler>(),
                    FindObjectOfType<Stage2BattleHandler>(),
                    FindObjectOfType<Stage3BattleHandler>(),
                    FindObjectOfType<Stage4BattleHandler>(),
                    FindObjectOfType<Stage5BattleHandler>(),
                    FindObjectOfType<Stage6BattleHandler>(),
                    FindObjectOfType<Stage7BattleHandler>(),
                    FindObjectOfType<Stage8BattleHandler>(),
                    FindObjectOfType<Stage9BattleHandler>(),
                    FindObjectOfType<Stage10BattleHandler>(),
                    FindObjectOfType<Stage11BattleHandler>(),
                    FindObjectOfType<Stage12BattleHandler>(),
                    FindObjectOfType<Stage13BattleHandler>(),
                    FindObjectOfType<Stage14BattleHandler>(),
                    FindObjectOfType<Stage15BattleHandler>(),
                    FindObjectOfType<Stage16BattleHandler>(),
                    FindObjectOfType<Stage17BattleHandler>(),
                    FindObjectOfType<Stage18BattleHandler>(),
                    FindObjectOfType<Stage19BattleHandler>(),
                    FindObjectOfType<Stage20BattleHandler>(),
                    FindObjectOfType<EndlessModeBattleHandler>(),
                    FindObjectOfType<Boss2BattleHandler>(),
                    FindObjectOfType<Boss1BattleHandler>()
                };
                foreach (var handler in battleHandlers)
                {
                    if (handler != null)
                    {
                        var updateMethod = handler.GetType().GetMethod("UpdateHealthUI");
                        if (updateMethod != null)
                        {
                            updateMethod.Invoke(handler, null);
                        }
                    }
                }
                inventory.DecreaseItemQuantity(itemToUse, 1);
                int remainingCount = inventory.GetItemCount(itemToUse);
                if (remainingCount <= 0)
                {
                    if (itemButton != null)
                    {
                        Destroy(itemButton);
                    }
                }
                else
                {
                    if (cooldownHandler != null)
                    {
                        cooldownHandler.StartCooldown(itemToUse, 60f);
                        Button buttonComponent = itemButton.GetComponent<Button>();
                        if (buttonComponent != null) buttonComponent.interactable = false;
                    }
                }
                HideItemDetails();
                UpdateUI();
            }
            else if (isUsable && itemToUse.itemName == "Damage Potion")
            {
                var battleHandlers = new List<MonoBehaviour>
                {
                    FindObjectOfType<BattleTrialHandler>(),
                    FindObjectOfType<Stage1BattleHandler>(),
                    FindObjectOfType<Stage2BattleHandler>(),
                    FindObjectOfType<Stage3BattleHandler>(),
                    FindObjectOfType<Stage4BattleHandler>(),
                    FindObjectOfType<Stage5BattleHandler>(),
                    FindObjectOfType<Stage6BattleHandler>(),
                    FindObjectOfType<Stage7BattleHandler>(),
                    FindObjectOfType<Stage8BattleHandler>(),
                    FindObjectOfType<Stage9BattleHandler>(),
                    FindObjectOfType<Stage10BattleHandler>(),
                    FindObjectOfType<Stage11BattleHandler>(),
                    FindObjectOfType<Stage12BattleHandler>(),
                    FindObjectOfType<Stage13BattleHandler>(),
                    FindObjectOfType<Stage14BattleHandler>(),
                    FindObjectOfType<Stage15BattleHandler>(),
                    FindObjectOfType<Stage16BattleHandler>(),
                    FindObjectOfType<Stage17BattleHandler>(),
                    FindObjectOfType<Stage18BattleHandler>(),
                    FindObjectOfType<Stage19BattleHandler>(),
                    FindObjectOfType<Stage20BattleHandler>(),
                    FindObjectOfType<EndlessModeBattleHandler>(),
                    FindObjectOfType<Boss2BattleHandler>(),
                    FindObjectOfType<Boss1BattleHandler>()
                };
                foreach (var handler in battleHandlers)
                {
                    if (handler != null)
                    {
                        var isAttackPhaseActiveMethod = handler.GetType().GetMethod("IsAttackPhaseActive");
                        var isDamagePotionUsedMethod = handler.GetType().GetMethod("IsDamagePotionUsed");
                        if (isAttackPhaseActiveMethod != null && isDamagePotionUsedMethod != null)
                        {
                            bool isAttackPhaseActive = (bool)isAttackPhaseActiveMethod.Invoke(handler, null);
                            bool isDamagePotionUsed = (bool)isDamagePotionUsedMethod.Invoke(handler, null);

                            if (isAttackPhaseActive && !isDamagePotionUsed)
                            {
                                var setDamagePotionUsedMethod = handler.GetType().GetMethod("SetDamagePotionUsed");
                                var applyDamageBoostMethod = handler.GetType().GetMethod("ApplyDamageBoost");
                                if (setDamagePotionUsedMethod != null && applyDamageBoostMethod != null)
                                {
                                    setDamagePotionUsedMethod.Invoke(handler, new object[] { true });
                                    applyDamageBoostMethod.Invoke(handler, null);
                                    SuccessDamage.SetActive(true);
                                    inventory.DecreaseItemQuantity(itemToUse, 1);
                                    int remainingCount = inventory.GetItemCount(itemToUse);
                                    if (remainingCount <= 0)
                                    {
                                        if (itemButton != null)
                                        {
                                            Destroy(itemButton);
                                        }
                                    }
                                    else
                                    {
                                        UpdateUI();
                                    }
                                }
                            }
                            else if (isDamagePotionUsed)
                            {
                                DamageAlreadyUsed.SetActive(true);
                            }
                            else
                            {
                                DamageNotUsable.SetActive(true);
                            }
                            break; 
                        }
                    }
                }
            }
            else if (isUsable && itemToUse.itemName == "Blue Potion")
            {
                var battleHandlers = new List<MonoBehaviour>
                {
                    FindObjectOfType<BattleTrialHandler>(),
                    FindObjectOfType<Stage1BattleHandler>(),
                    FindObjectOfType<Stage2BattleHandler>(),
                    FindObjectOfType<Stage3BattleHandler>(),
                    FindObjectOfType<Stage4BattleHandler>(),
                    FindObjectOfType<Stage5BattleHandler>(),
                    FindObjectOfType<Stage6BattleHandler>(),
                    FindObjectOfType<Stage7BattleHandler>(),
                    FindObjectOfType<Stage8BattleHandler>(),
                    FindObjectOfType<Stage9BattleHandler>(),
                    FindObjectOfType<Stage10BattleHandler>(),
                    FindObjectOfType<Stage11BattleHandler>(),
                    FindObjectOfType<Stage12BattleHandler>(),
                    FindObjectOfType<Stage13BattleHandler>(),
                    FindObjectOfType<Stage14BattleHandler>(),
                    FindObjectOfType<Stage15BattleHandler>(),
                    FindObjectOfType<Stage16BattleHandler>(),
                    FindObjectOfType<Stage17BattleHandler>(),
                    FindObjectOfType<Stage18BattleHandler>(),
                    FindObjectOfType<Stage19BattleHandler>(),
                    FindObjectOfType<Stage20BattleHandler>(),
                    FindObjectOfType<EndlessModeBattleHandler>(),
                    FindObjectOfType<Boss2BattleHandler>(),
                    FindObjectOfType<Boss1BattleHandler>()
                };
                foreach (var handler in battleHandlers)
                {
                    if (handler != null)
                    {
                        var stopTimerMethod = handler.GetType().GetMethod("StopTimerForSeconds");
                        if (stopTimerMethod != null)
                        {
                            stopTimerMethod.Invoke(handler, new object[] { 10f });
                        }
                    }
                }
                inventory.DecreaseItemQuantity(itemToUse, 1);
                int remainingCount = inventory.GetItemCount(itemToUse);
                if (remainingCount <= 0)
                {
                    if (itemButton != null)
                    {
                        Destroy(itemButton);
                    }
                }
                else
                {
                    if (cooldownHandler != null)
                    {
                        cooldownHandler.StartCooldown(itemToUse, 60f);
                        Button buttonComponent = itemButton.GetComponent<Button>();
                        if (buttonComponent != null) buttonComponent.interactable = false;
                    }
                }
                HideItemDetails();
                UpdateUI();
            }
            else
            {
                notUsablePanelPrefab.SetActive(true); 
            }
        }
    }
    public GameObject FindItemButton(Item item)
    {
        foreach (Transform child in inventoryPanel)
        {
            TMP_Text itemName = child.Find("ItemName")?.GetComponent<TMP_Text>();
            if (itemName != null)
            {
                string displayName = GetCustomizedName(item);
                if (itemName.text == displayName)
                {
                    return child.gameObject;
                }
            }
        }
        return null;
    }
    private string GetCustomizedName(Item item)
    {
        if (item.itemName.Contains("Red"))
            return "Health Potion";
        else if (item.itemName.Contains("Blue"))
            return "Clockstop \n Potion";
        else if (item.itemName.Contains("Damage"))
            return "Damage \n Boost Potion";
        else
            return item.itemName; 
    }
    public void CloseNotUsablePanelPrefab()
    {
        notUsablePanelPrefab.SetActive(false);
    }    
    public void AddRedPotion()
    {
        inventory.AddItem(redPotion, 1); 
        UpdateUI();  
    }
    public void AddBluePotion()
    {
        inventory.AddItem(bluePotion, 1); 
        UpdateUI();  
    }
    public void AddDamagePotion()
    {
        inventory.AddItem(damagePotion, 1); 
        UpdateUI();
    }
    public void OpenInventoryPanel()
    {
        if (inventoryUI == null)
        {
            Debug.LogError("inventoryUI is not assigned!");
            return;
        }
        isInventoryOpen = true;  
        inventoryUI.SetActive(true);
        UpdateUI();
        if (cooldownHandler == null)
        {
            Debug.LogError("cooldownHandler is not assigned!");
            return;
        }
        foreach (var item in cooldownHandler.GetCooldownItems())
        {
            GameObject itemButton = FindItemButton(item);
            if (itemButton == null)
            {
                Debug.LogWarning($"Item button for {item} not found!");
                continue;
            }
            TMP_Text cooldownText = itemButton.transform.Find("CooldownText")?.GetComponent<TMP_Text>();
            if (cooldownText == null)
            {
                Debug.LogWarning($"CooldownText not found for item: {item}");
                continue;
            }
            cooldownText.gameObject.SetActive(true);
            cooldownText.text = Mathf.Ceil(cooldownHandler.GetCooldownTime(item)).ToString() + "s";
        }
    }
    public void CloseInventoryPanel()
    {
        inventoryUI.SetActive(false);
        isInventoryOpen = false;
    }
    public void CloseDamageNotUsable()
    {
        DamageNotUsable.SetActive(false);
    }
    public void CloseSuccessDamage()
    {
        SuccessDamage.SetActive(false);
    }
    public void CloseDamageAlreadyUsed()
    {
        DamageAlreadyUsed.SetActive(false);
    }
}