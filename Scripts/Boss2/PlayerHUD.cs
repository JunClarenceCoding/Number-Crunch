using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PlayerHUD : MonoBehaviourPunCallbacks
{
    public GameObject playerHUDPrefab;
    public Transform hudParent;
    private Dictionary<int, GameObject> playerHUDs = new Dictionary<int, GameObject>();
    private PlayerInfoManager playerInfoManager;
    void Start()
    {
        playerInfoManager = FindObjectOfType<PlayerInfoManager>();

        if (playerInfoManager == null)
        {
            Debug.LogError("PlayerInfoManager not found in the scene.");
            return;
        }
        playerInfoManager.OnPlayerInfoRetrieved += CreateOrUpdatePlayerHUDs;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            StartCoroutine(InitializePlayerHUD(player));
        }
    }
    private IEnumerator InitializePlayerHUD(Player player)
    {
        float retryDuration = 0.5f;
        int maxRetries = 10;

        for (int attempt = 0; attempt < maxRetries; attempt++)
        {
            if (playerInfoManager != null)
            {
                int level = playerInfoManager.GetPlayerLevel(player.ActorNumber);
                int maxHealth = playerInfoManager.GetPlayerMaxHealth(player.ActorNumber);

                if (level != 0 && maxHealth != 0)
                {
                    CreateOrUpdatePlayerHUDs(player.ActorNumber, level, maxHealth);
                    yield break;
                }
            }
            yield return new WaitForSeconds(retryDuration);
        }
        Debug.LogWarning("Failed to retrieve player data after multiple attempts for player: " + player.NickName);
    }
    private void CreateOrUpdatePlayerHUDs(int actorNumber, int level, int maxHealth)
    {
        if (playerHUDs.ContainsKey(actorNumber))
        {
            PlayerHUDItem hudItem = playerHUDs[actorNumber].GetComponent<PlayerHUDItem>();
            if (hudItem != null)
            {
                hudItem.SetPlayer(PhotonNetwork.CurrentRoom.GetPlayer(actorNumber).NickName, level, maxHealth);
            }
        }
        else
        {
            GameObject playerHUD = Instantiate(playerHUDPrefab, hudParent);
            playerHUDs[actorNumber] = playerHUD;
            PlayerHUDItem hudItem = playerHUD.GetComponent<PlayerHUDItem>();
            if (hudItem != null)
            {
                hudItem.SetPlayer(PhotonNetwork.CurrentRoom.GetPlayer(actorNumber).NickName, level, maxHealth);
            }
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        StartCoroutine(InitializePlayerHUD(newPlayer));
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (playerHUDs.ContainsKey(otherPlayer.ActorNumber))
        {
            Destroy(playerHUDs[otherPlayer.ActorNumber]);
            playerHUDs.Remove(otherPlayer.ActorNumber);
        }
    }
}