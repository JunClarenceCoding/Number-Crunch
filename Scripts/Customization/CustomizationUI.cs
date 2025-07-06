using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Auth;
using Firebase.Database;

public class CustomizationUI : MonoBehaviour
{
    public Button ownedButton, TopButton, BottomButton, ShoesButton;
    public GameObject ownedPanel, TopPanel, BottomPanel, ShoesPanel;
    public GameObject clothesButtonPrefab; 
    public GameObject equipPanelPrefab;  
    public Transform panelsFolder;
    private GameObject activeEquipPanel;
     public Transform ownedContainer, topContainer, bottomContainer, shoesContainer; 
    public GameObject notOwnedClothesPanel;
    public GameObject notOwnedClothesPanel1; 
    public TMP_Text ownedTextButton;
    public TMP_Text TopTextButton;
    public TMP_Text BottomTextButton;
    public TMP_Text ShoesTextButton;
    private Color activeTextColor = new Color32(255, 153, 163, 255);  // #ff99a3
    private Color inactiveTextColor = new Color32(255, 255, 255, 255);  // #ffffff
    private bool isInitialized = false; 
    public CustomizationManager characterLoader;
    public MainCharacterLoader mainCharacterLoader;
    private AudioManager audioManager;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene.");
            return;
        }
    }

    public void InitializeUI()
    {
        if (!isInitialized)
        {

            characterLoader = FindObjectOfType<CustomizationManager>();
            ownedButton.onClick.AddListener(() => DisplayClothes("owned"));
            TopButton.onClick.AddListener(() => DisplayClothes("tops"));
            BottomButton.onClick.AddListener(() => DisplayClothes("bottoms"));
            ShoesButton.onClick.AddListener(() => DisplayClothes("shoes"));
            ownedButton.onClick.AddListener(ActivateOwnedPanel);
            TopButton.onClick.AddListener(ActivateTopPanel);
            BottomButton.onClick.AddListener(ActivateBottomPanel);
            ShoesButton.onClick.AddListener(ActivateShoesPanel);
            ActivateOwnedPanel();
            DisplayClothes("owned");
            ownedTextButton.text = "Owned";
            TopTextButton.text = "Top";
            BottomTextButton.text = "Bottom";
            ShoesTextButton.text = "Shoes";
            isInitialized = true; 
        }
    }
    private void DisplayClothes(string category)
    {
        ClearPanel(category); 

        if (category == "owned")
        {
            FirebaseManager.Instance.GetOwnedClothes(clothesData =>
            {
                foreach (var clothesCategory in clothesData)
                {
                    foreach (var item in clothesCategory.Value)
                    {
                        if (item.Value)  
                        {
                            if (CanCharacterWearItem(item.Key))
                            {
                                InstantiateClothesButton(item.Key, "owned", item.Value);  
                            }
                            else
                            {
                                Debug.LogWarning($"Item {item.Key} cannot be worn by this character.");
                            }
                        }
                    }
                }
            });
        }
        else
        {
            FirebaseManager.Instance.GetAllClothes(clothesData =>
            {
                if (clothesData.ContainsKey(category))
                {
                    foreach (var item in clothesData[category])
                    {
                        if (CanCharacterWearItem(item.Key))
                        {
                            InstantiateClothesButton(item.Key, category, item.Value); 
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"Category {category} not found!");
                }
            });
        }
    }
    private bool CanCharacterWearItem(string itemName)
    {
        if (characterLoader == null || characterLoader.GetInstantiatedPlayer() == null)
        {
            Debug.LogError("Instantiated player not found.");
            return false;
        }
        GameObject instantiatedPlayer = characterLoader.GetInstantiatedPlayer();
        Transform clothingSlot = instantiatedPlayer.transform.Find(itemName);
        if (clothingSlot != null)
        {
            return true; 
        }

        return false; 
    }
    private void ClearPanel(string category)
    {
        Transform container = GetContainerByCategory(category);
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
    }
    private Transform GetContainerByCategory(string category)
    {
        switch (category)
        {
            case "tops": return topContainer;
            case "bottoms": return bottomContainer;
            case "shoes": return shoesContainer;
            default: return ownedContainer;
        }
    }
    private string GetDisplayName(string itemName)
    {
        if (itemName.Contains("Sweater"))
        {
            return "Sweater";
        }
        else if (itemName.Contains("BoyShirt1"))
        {
            return "Vest";
        }
        else if (itemName.Contains("GirlShirt1"))
        {
            return "Small Cloak";
        }
        else if (itemName.Contains("BoyPants1"))
        {
            return "Trouser";
        }
        else if (itemName.Contains("GirlPants1"))
        {
            return "Skirt";
        }
        else if (itemName.Contains("Boots1"))
        {
            return "Boots";
        }
        else if (itemName.Contains("BoyShirt3") || itemName.Contains("GirlShirt2"))
        {
            return "NASTECH Shirt";
        }else if (itemName.Contains("Boots2"))
        {
            return "NASTECH Shoes";
        }
        else if (itemName.Contains("BoyPants3"))
        {
            return "NASTECH Trouser";
        }  
        else if (itemName.Contains("GirlPants2"))
        {
            return "NASTECH Skirt";
        }       

        return itemName;
    }
    private GameObject InstantiateClothesButton(string itemName, string category, bool isOwned)
    {
        GameObject button = Instantiate(clothesButtonPrefab);
        Transform container = GetContainerByCategory(category);
        button.transform.SetParent(container, false);
        TMP_Text itemText = button.GetComponentInChildren<TMP_Text>();
        itemText.text = GetDisplayName(itemName);
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null && !isOwned)
        {
            buttonImage.color = new Color(0.5f, 0.5f, 0.5f, 1f); 
        }
        Transform clothingImageTransform = button.transform.Find("ClothingImage");
        if (clothingImageTransform != null)
        {
            Image itemImage = clothingImageTransform.GetComponent<Image>();
            Sprite clothingSprite = FirebaseManager.Instance.GetClothingSprite(itemName);
            if (clothingSprite != null)
            {
                itemImage.sprite = clothingSprite;
                if (!isOwned)
                {
                    itemImage.color = new Color(0.5f, 0.5f, 0.5f, 1f); 
                }
            }
            else
            {
                Debug.LogWarning($"Sprite for {itemName} not found!");
            }
        }
        Button buttonComponent = button.GetComponent<Button>();
        if (buttonComponent != null)
        {
            buttonComponent.onClick.AddListener(() =>
            {
                audioManager.clickButton();

                if (isOwned)
                {
                    ShowEquipPanel(button.transform, category, itemName);
                }
                else
                {
                    if (itemName.Contains("Sweater"))
                    {
                        ShowNotOwnedPanel();
                    }
                    else if (itemName.Contains("Boots2") || itemName.Contains("BoyPants3") ||
                            itemName.Contains("BoyShirt3") || itemName.Contains("GirlPants2") ||
                            itemName.Contains("GirlShirt2"))
                    {
                        ShowNotOwnedPanel1();  
                    }
                    else
                    {
                        ShowNotOwnedPanel();  
                    }
                }
            });
        }
        return button;
    }
    private void ShowNotOwnedPanel()
    {
        notOwnedClothesPanel.SetActive(true);
    }
    public void CloseNotOwnedPanel()
    {
        notOwnedClothesPanel.SetActive(false);
    }
    private void ShowNotOwnedPanel1()
    {
        notOwnedClothesPanel1.SetActive(true);
    }
    public void CloseNotOwnedPanel1()
    {
        notOwnedClothesPanel1.SetActive(false);
    }
    private void ActivateOwnedPanel()
    {
        SetActivePanel(ownedPanel, ownedTextButton);
    }
    private void ActivateTopPanel()
    {
        SetActivePanel(TopPanel, TopTextButton);
    }
    private void ActivateBottomPanel()
    {
        SetActivePanel(BottomPanel, BottomTextButton);
    }
    private void ActivateShoesPanel()
    {
        SetActivePanel(ShoesPanel, ShoesTextButton);
    }
    private void SetActivePanel(GameObject activePanel, TMP_Text activeText)
    {
        ownedPanel.SetActive(false);
        TopPanel.SetActive(false);
        BottomPanel.SetActive(false);
        ShoesPanel.SetActive(false);
        activePanel.SetActive(true);
        ownedTextButton.color = inactiveTextColor;
        TopTextButton.color = inactiveTextColor;
        BottomTextButton.color = inactiveTextColor;
        ShoesTextButton.color = inactiveTextColor;
        activeText.color = activeTextColor;
    }
    public void ShowEquipPanel(Transform selectedItemTransform, string category, string itemName)
    {
        if (activeEquipPanel != null)
        {
            Destroy(activeEquipPanel);
        }
        activeEquipPanel = Instantiate(equipPanelPrefab, panelsFolder);
        Button equipButton = activeEquipPanel.transform.Find("EquipButton")?.GetComponent<Button>();
        Button cancelButton = activeEquipPanel.transform.Find("CancelButton")?.GetComponent<Button>();
        if (equipButton != null)
        {
            equipButton.onClick.RemoveAllListeners();
        if (category == "owned")
        {
            equipButton.onClick.AddListener(() =>
            {
                audioManager.clickButton(); 
                EquipClothing(itemName);
            });
        }
        else
        {
             equipButton.onClick.AddListener(() =>
            {
                audioManager.clickButton(); 
                EquipItem(category, itemName); 
            });
        }
        }
        if (cancelButton != null)
        {
            cancelButton.onClick.RemoveAllListeners();
           cancelButton.onClick.AddListener(() =>
        {
            audioManager.clickButton(); 
            CancelEquip(); 
        });
        }
        RectTransform panelRect = activeEquipPanel.GetComponent<RectTransform>();
        panelRect.pivot = new Vector2(0.5f, 0f);
        panelRect.anchoredPosition = new Vector2(0, 145f); 
    }
    public void CancelEquip()
    {
        HideEquipPanel();
    }
    private void HideEquipPanel()
    {
        if (activeEquipPanel != null)
        {
            Destroy(activeEquipPanel);
        }
    }
    public void EquipItem(string category, string newItemName)
    {
        string userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        DatabaseReference categoryRef = FirebaseDatabase.DefaultInstance.GetReference($"users/{userId}/clothes/{category}");
        categoryRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted && task.Result.Exists)
            {
                DataSnapshot snapshot = task.Result;
                bool unequippedAnyItem = false;
                foreach (DataSnapshot item in snapshot.Children)
                {
                    string itemName = item.Key;
                    bool isEquipped = item.Child("equipped").Value is bool equipped && equipped;
                    Debug.Log($"Item: {itemName}, Equipped: {isEquipped}"); // Log item states
                    if (isEquipped)
                    {
                        categoryRef.Child(itemName).Child("equipped").SetValueAsync(false)
                            .ContinueWith(unequipTask =>
                            {
                                if (unequipTask.IsCompleted)
                                {
                                    Debug.Log($"Unequipped {itemName} in {category}.");
                                }
                            });
                        unequippedAnyItem = true; 
                    }
                }
                if (!unequippedAnyItem)
                {
                    Debug.Log($"No items were equipped in {category} to unequip.");
                }
                categoryRef.Child(newItemName).Child("equipped").SetValueAsync(true)
                    .ContinueWith(equipTask =>
                    {
                        if (equipTask.IsCompleted)
                        {
                            Debug.Log($"Equipped {newItemName} in {category}.");
                            characterLoader.EquipClothesForCharacter();
                            mainCharacterLoader.EquipClothesForCharacter();
                        }
                    });
            }
            else
            {
                Debug.LogError($"Snapshot does not exist for category: {category}");
            }
        });
        HideEquipPanel(); 
    }
    public void EquipClothing(string itemName)
    {
        string userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        string category = GetCategory(itemName);
        if (category == null)
        {
            Debug.LogError($"Item category not found for: {itemName}");
            return;
        }
        DatabaseReference categoryRef = FirebaseDatabase.DefaultInstance.GetReference($"users/{userId}/clothes/{category}");
        categoryRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted && task.Result.Exists)
            {
                DataSnapshot snapshot = task.Result;
                bool unequippedAnyItem = false;
                foreach (DataSnapshot item in snapshot.Children)
                {
                    string currentItem = item.Key;
                    bool isEquipped = item.Child("equipped").Value is bool equipped && equipped;
                    if (isEquipped && currentItem != itemName) 
                    {
                        categoryRef.Child(currentItem).Child("equipped").SetValueAsync(false)
                            .ContinueWith(unequipTask =>
                            {
                                if (unequipTask.IsCompleted)
                                {
                                    Debug.Log($"Unequipped {currentItem} in {category}.");
                                }
                            });
                        unequippedAnyItem = true;
                    }
                }
                if (!unequippedAnyItem)
                {
                    Debug.Log("No previously equipped item to unequip.");
                }
                categoryRef.Child(itemName).Child("equipped").SetValueAsync(true)
                    .ContinueWith(equipTask =>
                    {
                        if (equipTask.IsCompleted)
                        {
                            Debug.Log($"Equipped item: {itemName}");
                            characterLoader.EquipClothesForCharacter();
                            mainCharacterLoader.EquipClothesForCharacter();
                        }
                    });
            }
            else
            {
                Debug.LogError($"Snapshot does not exist for category: {category}");
            }
        });
        HideEquipPanel();  
    }
    private void UpdateOtherItemsEquippedStatus(string category, string equippedItemName, string userId)
    {
        DatabaseReference categoryRef = FirebaseDatabase.DefaultInstance.GetReference($"users/{userId}/clothes/{category}");

        categoryRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted && task.Result.Exists)
            {
                foreach (var item in task.Result.Children)
                {
                    string itemName = item.Key;
                    if (itemName != equippedItemName)
                    {
                        DatabaseReference itemRef = categoryRef.Child(itemName);
                        itemRef.Child("equipped").SetValueAsync(false);
                    }
                }
            }
        });
    }
    private string GetCategory(string itemName)
    {
        if (itemName.StartsWith("shirt") || itemName.Contains("Shirt") || itemName.Contains("Sweater"))
            return "tops";
        else if (itemName.StartsWith("pants") || itemName.Contains("Pants"))
            return "bottoms";
        else if (itemName.Contains("Boots")|| itemName.Contains("Shoes"))
            return "shoes";
        return null; 
    }
}