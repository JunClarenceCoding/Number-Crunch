using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class CooldownHandler : MonoBehaviour
{
    private Dictionary<Item, float> itemCooldowns = new Dictionary<Item, float>(); 
    private Dictionary<Item, Coroutine> activeCooldownCoroutines = new Dictionary<Item, Coroutine>();
    public void StartCooldown(Item item, float cooldownTime)
    {
        if (itemCooldowns.ContainsKey(item))
        {
            StopCoroutine(activeCooldownCoroutines[item]);
        }
        activeCooldownCoroutines[item] = StartCoroutine(CooldownCoroutine(item, cooldownTime));
    }
    private IEnumerator CooldownCoroutine(Item item, float cooldownTime)
    {
        itemCooldowns[item] = cooldownTime;
        InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
        GameObject itemButton = inventoryUI?.FindItemButton(item);
        TMP_Text cooldownText = itemButton?.transform.Find("CooldownText")?.GetComponent<TMP_Text>();
        Button buttonComponent = itemButton?.GetComponent<Button>();
        if (cooldownText != null)
        {
            cooldownText.gameObject.SetActive(true);
        }
        if (buttonComponent != null)
        {
            buttonComponent.interactable = false;  
        }
        while (cooldownTime > 0)
        {
            cooldownTime -= Time.deltaTime;
            itemCooldowns[item] = cooldownTime;

            if (cooldownText != null)
            {
                cooldownText.text = Mathf.Ceil(cooldownTime).ToString() + "";  
            }
            yield return null;
        }
        if (cooldownText != null)
        {
            cooldownText.gameObject.SetActive(false);  
        }

        if (buttonComponent != null)
        {
            buttonComponent.interactable = true;  
        }
        itemCooldowns.Remove(item);
        activeCooldownCoroutines.Remove(item);
    }
    public bool IsItemOnCooldown(Item item)
    {
        return itemCooldowns.ContainsKey(item);
    }
    public void UpdateCooldownUI(GameObject itemButton, Item item)
    {
        if (!itemCooldowns.ContainsKey(item))
            return;

        TMP_Text cooldownText = itemButton.transform.Find("CooldownText")?.GetComponent<TMP_Text>();
        if (cooldownText != null)
        {
            cooldownText.text = Mathf.Ceil(itemCooldowns[item]).ToString() + "";
            cooldownText.gameObject.SetActive(true);
        }
    }
    public void StartCooldownUI(GameObject itemButton, Item item)
    {
        if (!itemCooldowns.ContainsKey(item))
            return;

        TMP_Text cooldownText = itemButton.transform.Find("CooldownText")?.GetComponent<TMP_Text>();
        if (cooldownText != null)
        {
            cooldownText.gameObject.SetActive(true);
            cooldownText.text = Mathf.Ceil(itemCooldowns[item]).ToString() + "";
        }
    }
    public IEnumerable<Item> GetCooldownItems()
    {
        return itemCooldowns.Keys;
    }
    public float GetCooldownTime(Item item)
    {
        if (itemCooldowns.ContainsKey(item))
        {
            return itemCooldowns[item];
        }
        return 0f;
    }
}