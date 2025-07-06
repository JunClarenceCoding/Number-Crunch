using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AI;
using System.Collections.Generic;

public class CustomizationManager : MonoBehaviour
{
    public GameObject LightForCustomizaiton;
    public Image BgCus;
    public GameObject[] characterPrefabs; 
    public Transform customizationSpawnPoint;
    public Camera PlayerCamera; 
    public Camera CustomizationCamera; 
    public RawImage customizationRawImage; 
    public RenderTexture customizationRenderTexture;
    public float rotationSpeed = 2f; 
    private GameObject instantiatedCharacter; 
    public CustomizationUI customizationUI;

    void Start()
    {
        if (FirebaseManager.Instance.IsFirebaseInitialized())
        {
            FirebaseManager.Instance.GetEquippedClothes(SetClothesForCharacter);
        }
        customizationRawImage.texture = customizationRenderTexture;
        CustomizationCamera.gameObject.SetActive(false);
        LightForCustomizaiton.SetActive(false);
        BgCus.gameObject.SetActive(false);
    }
    public void OpenCustomization()
    {
        if (FirebaseManager.Instance.IsFirebaseInitialized())
        {
            FirebaseManager.Instance.GetEquippedClothes(SetClothesForCharacter);
        }
        BgCus.gameObject.SetActive(true);
        customizationUI.InitializeUI(); 
        PlayerCamera.gameObject.SetActive(false);
        LightForCustomizaiton.SetActive(true);
        CustomizationCamera.gameObject.SetActive(true);
        int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
        if (instantiatedCharacter == null)
        {
            GameObject prefab = characterPrefabs[selectedCharacter];
            instantiatedCharacter = Instantiate(prefab, customizationSpawnPoint.position, Quaternion.identity);
            instantiatedCharacter.transform.localScale = new Vector3(1, 1, 1); 
            Rigidbody rb = instantiatedCharacter.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true; 
            }
            NavMeshAgent agent = instantiatedCharacter.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.enabled = false; 
            }
        }
    }
    void SetClothesForCharacter(Dictionary<string, Dictionary<string, bool>> clothesData)
    {
        foreach (var category in clothesData)
        {
            foreach (var item in category.Value)
            {
                Transform clothingItem = instantiatedCharacter.transform.Find(item.Key);
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
    public void CloseCustomization()
    {
        if (instantiatedCharacter != null)
        {
            Destroy(instantiatedCharacter);
        }
        CustomizationCamera.gameObject.SetActive(false);
        LightForCustomizaiton.SetActive(false);
        BgCus.gameObject.SetActive(false);
        PlayerCamera.gameObject.SetActive(true);
    }
    void Update()
    {
        if (instantiatedCharacter != null && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (RectTransformUtility.RectangleContainsScreenPoint(customizationRawImage.rectTransform, touch.position, null))
            {
                if (touch.phase == TouchPhase.Moved)
                {
                    float rotationAmount = touch.deltaPosition.x * rotationSpeed * Time.deltaTime;
                    instantiatedCharacter.transform.Rotate(0, -rotationAmount, 0);
                }
            }
        }
    }   
    public GameObject GetInstantiatedPlayer()
    {
        return instantiatedCharacter;
    }
    public void EquipClothesForCharacter()
    {
        if (FirebaseManager.Instance.IsFirebaseInitialized())
        {
            FirebaseManager.Instance.GetEquippedClothes(SetClothesForCharacter);
        }
    }
}