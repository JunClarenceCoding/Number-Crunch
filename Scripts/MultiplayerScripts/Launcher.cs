using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using UnityEngine.UI;
using System.Linq;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

public class Launcher : MonoBehaviourPunCallbacks
{
    public GameObject Boss1ImageInfo;
    public GameObject Boss2ImageInfo;
    public GameObject Boss1CreateRoomImageInfo;
    public GameObject Boss2CreateRoomImageInfo;
    public GameObject Boss1JoinRoomImageInfo;
    public GameObject Boss2JoinRoomImageInfo;
    public GameObject Boss1CreateRoomInfo;
    public GameObject Boss2CreateRoomInfo;
    public GameObject Boss1CreatePrivateRoom;
    public GameObject Boss2CreatePrivateRoom;
    public GameObject publicRoomTitleImage;
    public GameObject privateRoomTitleImage;
    public static Launcher Instance;
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_InputField PrivateRoomNameInputField;
    [SerializeField] TMP_InputField roomCodeInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] TMP_Text roomCodeText;
    [SerializeField] Transform roomListContent;
    [SerializeField] Transform PlayerListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] GameObject PlayerListItemPrefab;
    [SerializeField] GameObject startGameButton;
    public string selectedBoss; 
    private string generatedRoomCode;
    private Dictionary<string, string> roomCodeDictionary = new Dictionary<string, string>();
    private List<RoomInfo> currentRoomList = new List<RoomInfo>();
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public override void OnJoinedLobby()
    {
        string username = FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;
        if (FirebaseManager.Instance != null && FirebaseManager.Instance.Auth.CurrentUser != null)
        {
            string userId = FirebaseManager.Instance.Auth.CurrentUser.UserId;
            FirebaseManager.Instance.Database
                .GetReference("usernames")
                .Child(username)  
                .GetValueAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsCompleted)
                    {
                        DataSnapshot snapshot = task.Result;
                        if (snapshot.Exists)
                        {
                            PhotonNetwork.NickName = username; 
                        }
                        else
                        {
                            PhotonNetwork.NickName = "Player_" + Random.Range(0, 1000).ToString("0000");
                        }
                    }
                    else
                    {
                        PhotonNetwork.NickName = "Player_" + Random.Range(0, 1000).ToString("0000");
                    }
                    roomCodeText.text = "";
                });
        }
        else
        {
            PhotonNetwork.NickName = "Player_" + Random.Range(0, 1000).ToString("0000");
            roomCodeText.text = "";
        }
    }
    public void RefreshRoomListOnButtonClick()
    {
        PhotonNetwork.JoinLobby();
    }
    public void SelectBoss1()
    {
        selectedBoss = "Boss1";
        RefreshRoomList(); 
        MenuManager.Instance.OpenMenu("title"); 
        Boss1ImageInfo.SetActive(true);
        Boss2ImageInfo.SetActive(false);
    }
    public void SelectBoss2()
    {
        selectedBoss = "Boss2";
        RefreshRoomList(); 
        MenuManager.Instance.OpenMenu("title");
        Boss2ImageInfo.SetActive(true);
        Boss1ImageInfo.SetActive(false);
    }
    public void ShowBossImageInCreateRoomOptions()
    {
        if (selectedBoss == "Boss1")
        {
            Boss1CreateRoomImageInfo.SetActive(true);
            Boss2CreateRoomImageInfo.SetActive(false);
        }
        else if (selectedBoss == "Boss2")
        {
            Boss1CreateRoomImageInfo.SetActive(false);
            Boss2CreateRoomImageInfo.SetActive(true);
        }
    }
    public void ShowBossImageInJoinRoomOptions()
    {
        if (selectedBoss == "Boss1")
        {
            Boss1JoinRoomImageInfo.SetActive(true);
            Boss2JoinRoomImageInfo.SetActive(false);
        }
        else if (selectedBoss == "Boss2")
        {
            Boss1JoinRoomImageInfo.SetActive(false);
            Boss2JoinRoomImageInfo.SetActive(true);
        }
    }
    public void ShowBossImageInCreateRoomMenu()
    {
        if (selectedBoss == "Boss1")
        {
            Boss1CreateRoomInfo.SetActive(true);
            Boss2CreateRoomInfo.SetActive(false);
        }
        else if (selectedBoss == "Boss2")
        {
            Boss1CreateRoomInfo.SetActive(false);
            Boss2CreateRoomInfo.SetActive(true);
        }
    }
    public void ShowBossImageInCreatePrivateRoom()
    {
        if (selectedBoss == "Boss1")
        {
            Boss1CreatePrivateRoom.SetActive(true);
            Boss2CreatePrivateRoom.SetActive(false);
        }
        else if (selectedBoss == "Boss2")
        {
            Boss1CreatePrivateRoom.SetActive(false);
            Boss2CreatePrivateRoom.SetActive(true);
        }
    }
    public void CreatePublicRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        options.IsVisible = true;
        options.IsOpen = true;
        options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "BossType", selectedBoss } };
        options.CustomRoomPropertiesForLobby = new string[] { "BossType" };
        PhotonNetwork.CreateRoom(roomNameInputField.text, options);
        MenuManager.Instance.OpenMenu("loading");
        publicRoomTitleImage.SetActive(true);
        privateRoomTitleImage.SetActive(false);
        roomCodeText.text = "";
    }
    public void CreatePrivateRoom()
    {
        if (string.IsNullOrEmpty(PrivateRoomNameInputField.text))
        {
            errorText.text = "Room name cannot be empty";
            MenuManager.Instance.OpenMenu("error");
            return;
        }
        generatedRoomCode = GenerateRoomCode();
        string privateRoomName = PrivateRoomNameInputField.text;
        RoomOptions options = new RoomOptions
        {
            MaxPlayers = 4,
            IsVisible = true,
            IsOpen = true,
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "RoomCode", generatedRoomCode }, { "IsPrivate", true }, { "BossType", selectedBoss } },
            CustomRoomPropertiesForLobby = new string[] { "RoomCode", "IsPrivate", "BossType" }
        };
        PhotonNetwork.CreateRoom(privateRoomName, options);
        MenuManager.Instance.OpenMenu("loading");
        publicRoomTitleImage.SetActive(false);
        privateRoomTitleImage.SetActive(true);
        roomCodeText.text = generatedRoomCode; 
    }
    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        Player[] players = PhotonNetwork.PlayerList;
        foreach (Transform child in PlayerListContent)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(PlayerListItemPrefab, PlayerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("RoomCode", out object roomCode))
        {
            roomCodeText.text = roomCode.ToString();
        }
        else
        {
            roomCodeText.text = "";  
        }
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed: " + message;
        MenuManager.Instance.OpenMenu("error");
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
        roomCodeText.text = "";
    }
    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("title");
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        currentRoomList = roomList;
        RefreshRoomList(); 
    }
    private void RefreshRoomList()
    {
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        roomCodeDictionary.Clear();
        foreach (RoomInfo roomInfo in currentRoomList)
        {
            if (roomInfo.RemovedFromList)
                continue;

            if (roomInfo.CustomProperties.ContainsKey("RoomCode"))
            {
                string roomCode = roomInfo.CustomProperties["RoomCode"].ToString();
                roomCodeDictionary[roomCode] = roomInfo.Name;
            }
            if (roomInfo.IsVisible && (!roomInfo.CustomProperties.ContainsKey("IsPrivate") || !(bool)roomInfo.CustomProperties["IsPrivate"]))
            {
                if (roomInfo.CustomProperties.ContainsKey("BossType") && roomInfo.CustomProperties["BossType"].ToString() == selectedBoss)
                {
                    Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomInfo);
                }
            }
        }
    }
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");
        roomCodeText.text = "";
    }
    public void JoinPrivateRoom()
    {
        if (!string.IsNullOrEmpty(roomCodeInputField.text))
        {
            string roomCodeToJoin = roomCodeInputField.text;
            if (roomCodeDictionary.TryGetValue(roomCodeToJoin, out string roomName))
            {
                RoomInfo roomInfo = currentRoomList.FirstOrDefault(room => room.Name == roomName);
                if (roomInfo != null && roomInfo.CustomProperties.ContainsKey("BossType") && roomInfo.CustomProperties["BossType"].ToString() == selectedBoss)
                {
                    PhotonNetwork.JoinRoom(roomName);
                    MenuManager.Instance.OpenMenu("loading");
                }
                else
                {
                    errorText.text = "Room with the given code does not exist for the selected boss";
                    MenuManager.Instance.OpenMenu("error");
                }
            }
            else
            {
                errorText.text = "Room with the given code does not exist";
                MenuManager.Instance.OpenMenu("error");
            }
        }
        else
        {
            errorText.text = "Room code cannot be empty";
            MenuManager.Instance.OpenMenu("error");
        }
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        errorText.text = "Failed to join room: " + message;
        MenuManager.Instance.OpenMenu("error");
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerListItemPrefab, PlayerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }
    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            if (selectedBoss == "Boss1")
            {
                PhotonNetwork.LoadLevel(27);
            }
            else if (selectedBoss == "Boss2")
            {
                PhotonNetwork.LoadLevel(7);
            }
        } 
    }
    private string GenerateRoomCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, 6).Select(s => s[Random.Range(0, s.Length)]).ToArray());
    }
    public void CloseMultiplayer()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
    }
}