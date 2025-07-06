using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class LoadCharacter : MonoBehaviour
{   
    public GameObject[] characterPrefabs;
    public Transform spawnPoint;
    public GameObject arrowPrefab; 
    public Vector3 characterScale = new Vector3(3, 3, 3); 
    public Joystick joystick;
    public Camera PlayerCamera; 
    public Camera TalkCamera;   
    public RectTransform rotationPanel; 
    private CinemachineVirtualCamera cinemachineCamera;
    private GameObject instantiatedPlayer;
    private float rotationSpeed = 0.2f; 
    private float smoothRotationSpeed = 5.0f; 
    private Vector3 cameraTargetEulerAngles; 

    void Start()
    {
        SetJoystickAlwaysVisible();
        if (FirebaseManager.Instance.IsFirebaseInitialized())
        {
            FirebaseManager.Instance.GetEquippedClothes(SetClothesForCharacter);
        }
        cinemachineCamera = PlayerCamera.GetComponent<CinemachineVirtualCamera>();
        if (cinemachineCamera == null)
        {
            Debug.LogError("CinemachineVirtualCamera component not found on PlayerCamera");
            return;
        }

        int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
        GameObject prefab = characterPrefabs[selectedCharacter];
        instantiatedPlayer = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        AddArrowToPlayer(instantiatedPlayer);
        instantiatedPlayer.tag = "Player";
         
        PlayerControl playerController = instantiatedPlayer.GetComponent<PlayerControl>();
        if (playerController != null)
        {
            playerController.SetJoystick(joystick);  
            playerController.SetPlayerCamera(PlayerCamera);  
        } 
        cinemachineCamera.Follow = instantiatedPlayer.transform; 
        instantiatedPlayer.transform.localScale = characterScale; 
        cameraTargetEulerAngles = PlayerCamera.transform.eulerAngles;
    }
    void SetClothesForCharacter(Dictionary<string, Dictionary<string, bool>> clothesData)
    {
        foreach (var category in clothesData)
        {
            foreach (var item in category.Value)
            {
                Transform clothingItem = instantiatedPlayer.transform.Find(item.Key);
                if (clothingItem != null)
                {
                    clothingItem.gameObject.SetActive(item.Value); 
                }
                else
                {
                    Debug.Log($"Clothing item {item.Key} not found on character prefab.");
                }
            }
        }
    }
    void AddArrowToPlayer(GameObject player)
    {
        GameObject arrowInstance = Instantiate(arrowPrefab, player.transform);
        arrowInstance.name = "ArrowPrefabName"; // Set the name explicitly to match
    }
    public GameObject GetInstantiatedPlayer()
    {
        return instantiatedPlayer;
    }
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (RectTransformUtility.RectangleContainsScreenPoint(rotationPanel, touch.position, null))
            {
                if (touch.phase == TouchPhase.Moved)
                {
                    float rotationAmount = touch.deltaPosition.x * rotationSpeed;
                    cameraTargetEulerAngles.y += rotationAmount;
                }
            }
        }
        if (Input.touchCount == 0 && !Input.GetMouseButton(0)) 
        {
            SetJoystickAlwaysVisible();
        }
    }
    void LateUpdate()
    {
        Quaternion targetRotation = Quaternion.Euler(cameraTargetEulerAngles);
        PlayerCamera.transform.rotation = Quaternion.Slerp(PlayerCamera.transform.rotation, targetRotation, Time.deltaTime * smoothRotationSpeed);

        if (cinemachineCamera != null && instantiatedPlayer != null)
        {
            cinemachineCamera.Follow = instantiatedPlayer.transform;
        }
    }
    void SetJoystickAlwaysVisible()
    {
        if (joystick != null)
        {
            Transform joystickTransform = joystick.transform;
            foreach (Transform child in joystickTransform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}