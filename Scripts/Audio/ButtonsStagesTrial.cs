using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonsStagesTrial : MonoBehaviour
{
    [SerializeField] private List<Button> mainbuttonStage4; 
    [SerializeField] private List<Button> buttonNumbersStage4;

    [SerializeField] private List<Button> mainbuttonStage8; 
    [SerializeField] private List<Button> buttonNumbersStage8;

    [SerializeField] private List<Button> mainbuttonStage12; 
    [SerializeField] private List<Button> buttonNumbersStage12;

    [SerializeField] private List<Button> mainbuttonStage16; 
    [SerializeField] private List<Button> buttonNumbersStage16;
    
    public AudioManager audioManager; 
    
    void Start()
    {
        Debug.Log("Setting the sounds for buttons in stages trial");
        SetButtonsSoundStage4Trial();
        SetButtonsSoundStage8Trial();
        SetButtonsSoundStage12Trial();
        SetButtonsSoundStage16Trial();
    }

    public void SetButtonsSoundStage4Trial()
    {
        foreach (Button button in mainbuttonStage4)
        {
            if (button != null)
            {
                button.onClick.AddListener(audioManager.clickButton);
            }
            else
            {
                Debug.LogWarning("A button in buttonSet1 is null. Please check the ButtonManager script.");
            }
        }

        foreach (Button button in buttonNumbersStage4)
        {
            if (button != null)
            {
                button.onClick.AddListener(audioManager.clickButtonNumbers);
            }
            else
            {
                Debug.LogWarning("A button in buttonSet2 is null. Please check the ButtonManager script.");
            }
        }
    }

    public void SetButtonsSoundStage8Trial()
    {
        foreach (Button button in mainbuttonStage8)
        {
            if (button != null)
            {
                button.onClick.AddListener(audioManager.clickButton);
            }
            else
            {
                Debug.LogWarning("A button in buttonSet1 is null. Please check the ButtonManager script.");
            }
        }

        foreach (Button button in buttonNumbersStage8)
        {
            if (button != null)
            {
                button.onClick.AddListener(audioManager.clickButtonNumbers);
            }
            else
            {
                Debug.LogWarning("A button in buttonSet2 is null. Please check the ButtonManager script.");
            }
        }
    }

    public void SetButtonsSoundStage12Trial()
    {
        foreach (Button button in mainbuttonStage12)
        {
            if (button != null)
            {
                button.onClick.AddListener(audioManager.clickButton);
            }
            else
            {
                Debug.LogWarning("A button in buttonSet1 is null. Please check the ButtonManager script.");
            }
        }

        foreach (Button button in buttonNumbersStage12)
        {
            if (button != null)
            {
                button.onClick.AddListener(audioManager.clickButtonNumbers);
            }
            else
            {
                Debug.LogWarning("A button in buttonSet2 is null. Please check the ButtonManager script.");
            }
        }
    }

    public void SetButtonsSoundStage16Trial()
    {
        foreach (Button button in mainbuttonStage16)
        {
            if (button != null)
            {
                button.onClick.AddListener(audioManager.clickButton);
            }
            else
            {
                Debug.LogWarning("A button in buttonSet1 is null. Please check the ButtonManager script.");
            }
        }

        foreach (Button button in buttonNumbersStage16)
        {
            if (button != null)
            {
                button.onClick.AddListener(audioManager.clickButtonNumbers);
            }
            else
            {
                Debug.LogWarning("A button in buttonSet2 is null. Please check the ButtonManager script.");
            }
        }
    }
}
