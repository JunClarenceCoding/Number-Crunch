using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
public class Boss2CharacterLoader : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    public Transform primarySpawnPoint;
    public Vector3 characterScale = new Vector3(3, 3, 3);
    public Vector3 customRotation = new Vector3(0, 180, 0);
    private GameObject instantiatedPlayer;
    void Start()
    {
        if (FirebaseManager.Instance.IsFirebaseInitialized())
        {
            FirebaseManager.Instance.GetEquippedClothes(SetClothesForCharacter);
        }

        InstantiateLocalPlayer();
        Debug.Log("Player instantiation process initiated for local player.");
        Rigidbody rb = instantiatedPlayer.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; 
        }

        NavMeshAgent agent = instantiatedPlayer.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = false; 
        }
    }
    void InstantiateLocalPlayer()
    {
        int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter", 0);
        if (selectedCharacter < 0 || selectedCharacter >= characterPrefabs.Length)
        {
            Debug.LogError("Selected character index is out of range.");
            return;
        }
        GameObject prefab = characterPrefabs[selectedCharacter];
        if (prefab == null)
        {
            Debug.LogError("Selected character prefab is null.");
            return;
        }
        instantiatedPlayer = Instantiate(prefab, primarySpawnPoint.position, Quaternion.Euler(customRotation));
        instantiatedPlayer.name = $"Player_{Random.Range(1000, 9999)}"; // Random name for debugging, if needed
        instantiatedPlayer.transform.localScale = characterScale;
        instantiatedPlayer.tag = "Player";
        Debug.Log($"Local player instantiated at primary spawn point: {instantiatedPlayer.name}");
    }
    private void SetClothesForCharacter(Dictionary<string, Dictionary<string, bool>> clothesData)
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
    public GameObject GetInstantiatedPlayer()
    {
        return instantiatedPlayer;
    }
}