using UnityEngine;
using TMPro;
using Cinemachine;
using System.Collections.Generic;

public class MainCharacterLoader : MonoBehaviour
{
    public GameObject arrowPrefab;
    public GameObject[] characterPrefabs;
    public Transform spawnPoint; 
    public Vector3 characterScale = new Vector3(3, 3, 3); 
    public Joystick joystick;
    public Camera PlayerCamera; 
    public RectTransform rotationPanel; 
    private CinemachineVirtualCamera cinemachineCamera;
    private GameObject instantiatedPlayer;
    private GameObject arrowInstance;
    private float rotationSpeed = 0.2f; 
    private float smoothRotationSpeed = 5.0f;
    private Vector3 cameraTargetEulerAngles;
    private bool isRotationEnabled = true;

    void Start()
    {
        SetJoystickAlwaysVisible();
        if (FirebaseManager.Instance.IsFirebaseInitialized())
        {
            FirebaseManager.Instance.GetEquippedClothes(SetClothesForCharacter);
        }
        PlayerControl existingPlayerController = FindObjectOfType<PlayerControl>();
        if (existingPlayerController != null)
        {
            instantiatedPlayer = existingPlayerController.gameObject;
            instantiatedPlayer.transform.position = spawnPoint.position;
            instantiatedPlayer.transform.localScale = characterScale;
            AssignJoystickAndCamera(existingPlayerController);
            return;
        }
        int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
        if (selectedCharacter < 0 || selectedCharacter >= characterPrefabs.Length)
        {
            Debug.LogError("Selected character index is out of range. Please check the PlayerPrefs setting.");
            return;
        }
        GameObject prefab = characterPrefabs[selectedCharacter];
        instantiatedPlayer = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        instantiatedPlayer.tag = "Player";
        instantiatedPlayer.transform.localScale = characterScale;
        PlayerControl newPlayerController = instantiatedPlayer.GetComponent<PlayerControl>();
        if (newPlayerController != null)
        {
            AssignJoystickAndCamera(newPlayerController);
        }
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
    private void AssignJoystickAndCamera(PlayerControl playerController)
    {
        playerController.SetJoystick(joystick);
        playerController.SetPlayerCamera(PlayerCamera);
        cinemachineCamera = PlayerCamera.GetComponent<CinemachineVirtualCamera>();
        if (cinemachineCamera != null)
        {
            cinemachineCamera.Follow = instantiatedPlayer.transform;
        }
    }
    void Update()
    {
        if (isRotationEnabled && Input.touchCount > 0)
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
            ResetJoystickPosition(); 
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
    public void EnableCameraRotation()
    {
        isRotationEnabled = true;
    }
    public void DisableCameraRotation()
    {
        isRotationEnabled = false;
    }
    public GameObject GetInstantiatedPlayer()
    {
        return instantiatedPlayer;
    }
    public void EquipClothesForCharacter()
    {
        if (FirebaseManager.Instance.IsFirebaseInitialized())
        {
            FirebaseManager.Instance.GetEquippedClothes(SetClothesForCharacter);
        }
    }
    private void AddArrowToPlayer(GameObject player)
    {
        arrowInstance = Instantiate(arrowPrefab, player.transform);
        arrowInstance.name = "ArrowPrefabName";
    }
    public void ActivateAddArrowToPlayer()
    {
        AddArrowToPlayer(instantiatedPlayer);
    }
    public void DestroyArrowFromPlayer()
    {
        if (arrowInstance != null)
        {
            Destroy(arrowInstance);
            arrowInstance = null; 
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
    void ResetJoystickPosition()
    {
        if (joystick != null && joystick.transform != null)
        {
            RectTransform joystickTransform = joystick.GetComponent<RectTransform>();
            joystickTransform.anchoredPosition = Vector2.zero; 
        }
    }
}