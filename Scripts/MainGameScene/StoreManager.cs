using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public GameObject StorePanel;
    public GameObject triggerBox;
    private GameObject player;
    private PlayerControl playerController;
    private MainCharacterLoader loadCharacter; 
    
    void Start()
    {
        StartCoroutine(InitializePlayer());
    }
    private IEnumerator InitializePlayer()
    {
        loadCharacter = FindObjectOfType<MainCharacterLoader>();
        while (loadCharacter == null)
        {
            yield return null; 
            loadCharacter = FindObjectOfType<MainCharacterLoader>();
        }
        while (loadCharacter.GetInstantiatedPlayer() == null)
        {
            yield return null;
        }
        player = loadCharacter.GetInstantiatedPlayer();
        playerController = player.GetComponent<PlayerControl>();
    }
    private void SetPanelState(bool mainPanelState)
    {
        if (StorePanel != null)
        {
            StorePanel.SetActive(mainPanelState);
        }
        if (playerController != null)
        {
            playerController.enabled = !(mainPanelState);
        }
    }
    public void ShowPanel()
    {
        SetPanelState(true);
    }
    public void HidePanel()
    {
        SetPanelState(false);
    }
}