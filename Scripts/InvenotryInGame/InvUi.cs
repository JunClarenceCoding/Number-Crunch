using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvUi : MonoBehaviour
{
    public GameObject TheInventory;

    public void OpenInventoryPanel()
    {
        TheInventory.SetActive(true);
    }

    public void CloseInventoryPanel()
    {
        TheInventory.SetActive(false);
    }
}
