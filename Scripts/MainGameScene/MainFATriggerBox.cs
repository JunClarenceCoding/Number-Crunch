using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainFATriggerBox : MonoBehaviour
{
    public GameObject ActionButton;
    public GameObject GameModesPanel;

    void Start()
    {
        ActionButton.SetActive(false);
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
        if (!GameModesPanel.activeInHierarchy)
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
}