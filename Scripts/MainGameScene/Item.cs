using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;  // Name of the item (e.g., "Red Potion")
    public Sprite itemIcon;  // Icon for the item
    public bool isStackable; // Whether the item is stackable
    public string description;    // Description of the item
}