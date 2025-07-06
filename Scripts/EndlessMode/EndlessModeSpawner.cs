using System.Collections;
using UnityEngine;

public class EndlessModeSpawner : MonoBehaviour
{
    public GameObject[] monsterPrefabs;  
    public Transform spawnPoint;          
    public Vector3 customRotation = new Vector3(0, 180, 0); 
    private GameObject instantiatedMonster;
    void Start()
    {
        if (InitializeSpawner()){
            SpawnMonster();
        }
    }
    private bool InitializeSpawner()
    {
        if (monsterPrefabs == null || monsterPrefabs.Length == 0)
        {
            Debug.LogError("Monster prefabs are not assigned or the array is empty.");
            return false;
        }
        if (spawnPoint == null)
        {
            Debug.LogError("Spawn point is not assigned.");
            return false;
        }
        return true;
    }
    public GameObject SpawnMonster()
    {
        int randomMonster = Random.Range(0, monsterPrefabs.Length);
        GameObject selectedMonster = monsterPrefabs[randomMonster];
        instantiatedMonster = Instantiate(selectedMonster, spawnPoint.position, Quaternion.Euler(customRotation));
        MonsterHealthEndless monsterHealth = instantiatedMonster.GetComponent<MonsterHealthEndless>();
        if (monsterHealth != null)
        {
            monsterHealth.OnMonsterDefeated += HandleMonsterDefeat;
        }
        else
        {
            Debug.LogError("The selected monster prefab does not have a MonsterHealthEndless script attached!");
        }
        return instantiatedMonster; 
    }
    private void HandleMonsterDefeat()
    {
        if (instantiatedMonster != null)
        {
            MonsterHealthEndless monsterHealth = instantiatedMonster.GetComponent<MonsterHealthEndless>();
            if (monsterHealth != null)
            {
                monsterHealth.OnMonsterDefeated -= HandleMonsterDefeat;
            }
        }
    }
    public GameObject GetInstantiatedMonster()
    {
        return instantiatedMonster;
    }
}