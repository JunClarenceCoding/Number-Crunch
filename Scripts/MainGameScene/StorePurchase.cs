using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StorePurchase : MonoBehaviour
{
    public AudioManager audioManager;
    public Inventory inventory; 
    public Item redPotion;    
    public Item bluePotion;
    public Item damagePotion;
    public CurrencyManager currencyManager; 
    public InventoryUI inventoryUI; 
    public StoreUIManager storeUIManager;
    public ClothingManager clothingManager; 
    public void BuyPotion(int potionPrice, Item potionItem, int potionQuantity, Action closeConfirmPanel)
    {
        inventory.EnsureInventoryIsLoaded(() =>
        {
            currencyManager.GetCurrency(currentCurrency =>
            {
                if (currentCurrency >= potionPrice)
                {
                    int newCurrencyAmount = currentCurrency - potionPrice;
                    currencyManager.UpdateCurrency(newCurrencyAmount);
                    StartCoroutine(TemporaryOpenInventoryAndUpdate(potionItem, potionQuantity));
                    currencyManager.UpdateCurrencyDisplay();
                    storeUIManager.OpenPurchaseSuccessful();
                    audioManager.successPurchase();
                    closeConfirmPanel?.Invoke();
                }else{
                    storeUIManager.OpenNotEnoughTokens();
                    closeConfirmPanel?.Invoke();
                }
            });
        });
    }
    public IEnumerator TemporaryOpenInventoryAndUpdate(Item potionItem, int potionQuantity)
    {
        inventoryUI.inventoryUI.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        inventoryUI.AddPotions(potionItem, potionQuantity);
        yield return new WaitForSeconds(0.1f);
        inventoryUI.inventoryUI.SetActive(false);
    }
    public void BuyCurrencyWithGems(int gemCost, int currencyToAdd, Action closeConfirmPanel)
    {
        inventory.EnsureInventoryIsLoaded(() =>
        {
            currencyManager.GetGems(currentGems =>
            {
                if (currentGems >= gemCost)
                {
                    int newGemAmount = currentGems - gemCost;
                    currencyManager.UpdateGems(newGemAmount);
                    currencyManager.GetCurrency(currentCurrency =>
                    {
                        int newCurrencyAmount = currentCurrency + currencyToAdd;
                        currencyManager.UpdateCurrency(newCurrencyAmount);
                        currencyManager.UpdateCurrencyDisplay();
                        storeUIManager.OpenPurchaseSuccessful();
                        audioManager.successPurchase();
                        closeConfirmPanel?.Invoke();
                    });
                }
                else
                {
                    storeUIManager.OpenNotEnoughGems();
                    closeConfirmPanel?.Invoke();
                }
            });
        });
    }
    public void BuyPotionBundle(int bundlePrice, int redPotionQuantity, int bluePotionQuantity, int damagePotionQuantity, Action closeConfirmPanel)
    {
        inventory.EnsureInventoryIsLoaded(() =>
        {
            currencyManager.GetCurrency(currentCurrency =>
            {
                if (currentCurrency >= bundlePrice)
                {
                    int newCurrencyAmount = currentCurrency - bundlePrice;
                    currencyManager.UpdateCurrency(newCurrencyAmount);
                    StartCoroutine(TemporaryOpenInventoryAndUpdate(redPotion, redPotionQuantity));
                    StartCoroutine(TemporaryOpenInventoryAndUpdate(bluePotion, bluePotionQuantity));
                    StartCoroutine(TemporaryOpenInventoryAndUpdate(damagePotion, damagePotionQuantity));
                    currencyManager.UpdateCurrencyDisplay();
                    storeUIManager.OpenPurchaseSuccessful();
                    audioManager.successPurchase();
                    closeConfirmPanel?.Invoke();
                }else{
                    storeUIManager.OpenNotEnoughTokens();
                    closeConfirmPanel?.Invoke();
                }
            });
        });
    }
    public void BuyPotionBundleX3()
    {
        int bundlePrice = 3900; 
        int redPotionQuantity = 10;
        int bluePotionQuantity = 12;
        int damagePotionQuantity = 10;
        BuyPotionBundle(bundlePrice, redPotionQuantity, bluePotionQuantity, damagePotionQuantity, storeUIManager.CloseConfirmBundle1);
    }
    public void BuyRedPotionx1()
    {
        BuyPotion(100, redPotion, 1, storeUIManager.CloseConfirmX1RP);
    }
    public void BuyRedPotionx5()
    {
        BuyPotion(450, redPotion, 5, storeUIManager.CloseConfirmX5RP);
    }
    public void BuyRedPotionx10()
    {
        BuyPotion(800, redPotion, 10, storeUIManager.CloseConfirmX10RP);
    }
    public void BuyBluePotionx1()
    {
        BuyPotion(150, bluePotion, 1, storeUIManager.CloseConfirmX1BP);
    }
    public void BuyBluePotionx5()
    {
        BuyPotion(700, bluePotion, 5, storeUIManager.CloseConfirmX5BP);
    }
    public void BuyBluePotionx10()
    {
        BuyPotion(1300, bluePotion, 10, storeUIManager.CloseConfirmX10BP);
    }
    public void BuyDamagePotionx1()
    {
        BuyPotion(185, damagePotion, 1, storeUIManager.CloseConfirmX1DP);
    }
    public void BuyDamagePotionx5()
    {
        BuyPotion(900, damagePotion, 5, storeUIManager.CloseConfirmX5DP);
    }
    public void BuyDamagePotionx10()
    {
        BuyPotion(1700, damagePotion, 10, storeUIManager.CloseConfirmX10DP);
    }
    public void Buy150CurrencyWith5Gems()
    {
        BuyCurrencyWithGems(5, 150, storeUIManager.CloseConfirmX150Tokens);
    }
    public void Buy400CurrencyWith12Gems()
    {
        BuyCurrencyWithGems(12, 400, storeUIManager.CloseConfirmX400Tokens);
    }
    public void Buy800CurrencyWith22Gems()
    {
        BuyCurrencyWithGems(22, 900, storeUIManager.CloseConfirmX800Tokens);
    }
    public void Buy2000CurrencyWith50Gems()
    {
        BuyCurrencyWithGems(50, 2000, storeUIManager.CloseConfirmX2000Tokens);
    }
    public void Buy5000CurrencyWith110Gems()
    {
        BuyCurrencyWithGems(110, 5000, storeUIManager.CloseConfirmX5000Tokens);
    }
    public void BuySweater()
    {
        int shirtPrice = 4000; 
        inventory.EnsureInventoryIsLoaded(() =>
        {
            currencyManager.GetCurrency(currentCurrency =>
            {
                if (currentCurrency >= shirtPrice)
                {
                    int newCurrencyAmount = currentCurrency - shirtPrice;
                    currencyManager.UpdateCurrency(newCurrencyAmount);
                    currencyManager.UpdateCurrencyDisplay();
                    clothingManager.SetBoyShirt2AsOwned();
                    storeUIManager.OpenPurchaseSuccessful();
                    audioManager.successPurchase();
                    storeUIManager.CloseConfirmSweater();
                }
                else
                {
                    storeUIManager.OpenNotEnoughTokens();
                    storeUIManager.CloseConfirmSweater();
                }
            });
        });
    }
    public void BuyGirlSweater()
    {
        int shirtPrice = 4000;
        inventory.EnsureInventoryIsLoaded(() =>
        {
            currencyManager.GetCurrency(currentCurrency =>
            {
                if (currentCurrency >= shirtPrice)
                {
                    int newCurrencyAmount = currentCurrency - shirtPrice;
                    currencyManager.UpdateCurrency(newCurrencyAmount);
                    currencyManager.UpdateCurrencyDisplay();
                    clothingManager.SetGirlSweaterAsOwned();
                    storeUIManager.OpenPurchaseSuccessful();
                    audioManager.successPurchase();
                    storeUIManager.CloseConfirmSweater();
                }
                else
                {
                    storeUIManager.OpenNotEnoughTokens();
                    storeUIManager.CloseConfirmSweater();
                }
            });
        });
    }
    public void BuySweaterBasedOnCharacter()
    {
        int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter", 0); 
        if (selectedCharacter == 0)
        {
            BuySweater();
        }
        else if (selectedCharacter == 1)
        {
            BuyGirlSweater();
        }
    }
}