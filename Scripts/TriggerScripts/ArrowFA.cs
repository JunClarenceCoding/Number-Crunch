using UnityEngine;

public class ArrowFA : MonoBehaviour
{
    public GameObject triggerBox;
    public GameObject gameObjectLook;    
    public GameObject playerArrow;      
    public GameObject lookAtTarget;      
    private GameObject arrowChild;       

    void Start()
    {
        triggerBox.gameObject.SetActive(false);
        playerArrow.gameObject.SetActive(false);
        if (playerArrow != null)
        {
            arrowChild = playerArrow.transform.Find("ArrowChildObjectName")?.gameObject;
            if (arrowChild == null)
            {
                Debug.LogWarning("Arrow child object not found in the prefab.");
            }
        }
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            Transform arrowTransform = col.transform.Find("ArrowPrefabName");
            if (arrowTransform != null)
            {
                playerArrow = arrowTransform.gameObject;
                playerArrow.SetActive(true);
                arrowChild = playerArrow.transform.Find("ArrowChildObjectName")?.gameObject;
                UpdateArrowDirection();
            }
        }
    }
    void OnTriggerStay(Collider col)
    {
        if (playerArrow != null && playerArrow.activeInHierarchy)
        {
            UpdateArrowDirection();
        }
    }
    void UpdateArrowDirection()
    {
        if (playerArrow != null && lookAtTarget != null)
        {
            playerArrow.transform.LookAt(lookAtTarget.transform.position);
            playerArrow.transform.Rotate(0, 180, 0);
            Vector3 arrowRotation = playerArrow.transform.eulerAngles;
            arrowRotation.x = 0;
            arrowRotation.z = 0;
            playerArrow.transform.eulerAngles = arrowRotation;
        }
    }
    void LateUpdate()
    {
        UpdateArrowDirection();
    }
}