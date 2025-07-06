using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public GameObject SettingPanel;
    public void OpenSettingPanel(){
        SettingPanel.SetActive(true);
    }
    public void CloseSettingPanel(){
        SettingPanel.SetActive(false);
    }
}