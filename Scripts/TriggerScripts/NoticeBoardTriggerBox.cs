using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeBoardTriggerBox : MonoBehaviour
{
    public GameObject ActionButton;
    public GameObject NoticeBoardPanel;

    void Start() {
        ActionButton.SetActive(false);
        gameObject.SetActive(false);
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
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
            ActionButton.SetActive(false);
        }
    }
    public void EnableTriggerBox()
    {
        gameObject.SetActive(true);
    }
}