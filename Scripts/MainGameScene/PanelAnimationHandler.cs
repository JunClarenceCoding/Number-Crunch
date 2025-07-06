using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PanelAnimationHandler : MonoBehaviour
{
    public Animator profilePanelAnimator; 
    public void OpenProfile()
    {
        profilePanelAnimator.gameObject.SetActive(true); 
        profilePanelAnimator.Play("OpenProfile");        
    }
    public void CloseProfile()
    {
        profilePanelAnimator.Play("CloseProfile");       
    }
    public void DeactivatePanelAfterClose()
    {
        profilePanelAnimator.gameObject.SetActive(false); 
    }
    public void OpenPanelJun()
    {
        profilePanelAnimator.gameObject.SetActive(true); 
        profilePanelAnimator.Play("JunOpenPanel");        
    }
    public void CloseJunPanel()
    {
        profilePanelAnimator.Play("JunClosePanel");     
    }
    public void OpenPanelKhia()
    {
        profilePanelAnimator.gameObject.SetActive(true); 
        profilePanelAnimator.Play("KhiaOpenPanel");        
    }
    public void CloseKhiaPanel()
    {
        profilePanelAnimator.Play("KhiaClosePanel");    
    }
    public void OpenPanelMich()
    {
        profilePanelAnimator.gameObject.SetActive(true); 
        profilePanelAnimator.Play("MichOpenPanel");     
    }
    public void CloseMichPanel()
    {
        profilePanelAnimator.Play("MichClosePanel");
    }
    public void OpenPanelPam()
    {
        profilePanelAnimator.gameObject.SetActive(true); 
        profilePanelAnimator.Play("PamOpenPanel");     
    }
    public void ClosePamPanel()
    {
        profilePanelAnimator.Play("PamClosePanel");      
    }
    public void OpenTermsPanel()
    {
        profilePanelAnimator.gameObject.SetActive(true);
        profilePanelAnimator.Play("TermsAnim"); 
    }
}