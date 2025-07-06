using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class BattleTrialCharacterLoader : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    public Transform spawnPoint; 
    public Vector3 characterScale = new Vector3(3, 3, 3); 
    public Vector3 customRotation = new Vector3(0, 180, 0); 
    private GameObject instantiatedPlayer;

    void Start()
    {
        if (FirebaseManager.Instance.IsFirebaseInitialized())
        {
            FirebaseManager.Instance.GetEquippedClothes(SetClothesForCharacter);
        }
        if (FindObjectOfType<PlayerControl>() != null)
        {
            instantiatedPlayer = FindObjectOfType<PlayerControl>().gameObject;
            instantiatedPlayer.transform.position = spawnPoint.position;
            instantiatedPlayer.transform.localScale = characterScale;
            instantiatedPlayer.transform.rotation = Quaternion.Euler(customRotation);
            return;
        }

        int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
        if (selectedCharacter < 0 || selectedCharacter >= characterPrefabs.Length)
        {
            Debug.LogError("Selected character index is out of range. Please check the PlayerPrefs setting.");
            return;
        }
        GameObject prefab = characterPrefabs[selectedCharacter];
        instantiatedPlayer = Instantiate(prefab, spawnPoint.position, Quaternion.Euler(customRotation));
        instantiatedPlayer.tag = "Player";
        instantiatedPlayer.transform.localScale = characterScale;
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
    public GameObject GetInstantiatedPlayer()
    {
        return instantiatedPlayer;
    }
}