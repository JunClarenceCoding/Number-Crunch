using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBoxToNPC : MonoBehaviour
{
    public MainArrowToNPC arrowToNPC;
    public MainGameModes mainGameModes;
    public GameObject ActionButton;
    public GameObject NoticeBoardPanel;
    void Start()
    {
        ActionButton.SetActive(false);
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            arrowToNPC.playerArrow.gameObject.SetActive(false);
            ActionChecker();
        }
    }
    void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            ActionChecker();
        }
    }
    void ActionChecker()
    {
        if (!NoticeBoardPanel.activeInHierarchy)
        {
            ActionButton.SetActive(true);
        }
        else
        {
            ActionButton.SetActive(false);
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            mainGameModes.UpdateArrowBasedOnTasks();
            ActionButton.SetActive(false);
        }
    }
}