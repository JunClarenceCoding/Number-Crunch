using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIManager : MonoBehaviour
{
    public GameObject SettingPanel, InventoryPanel, LedearboardsPanel, CreditsPanel, MapPanel, EndlessModePanel, SelectionBossPanel, FeedbackPanel, ConfirmLogoutPanel, ProfilePanel, ConfirmExitPanel, ConfirmDeleteAccount;
    public void OpenSettingPanel()
    {
        SettingPanel.SetActive(true);
    }
    public void CloseSettingPanel()
    {
        SettingPanel.SetActive(false);
    }
    public void OpenInventoryPanel()
    {
        InventoryPanel.SetActive(true);
        ProfilePanel.SetActive(false);
    }
    public void CloseInventoryPanel()
    {
        InventoryPanel.SetActive(false);
        ProfilePanel.SetActive(true);
    }
    public void OpenLeaderboardsPanel()
    {
        LedearboardsPanel.SetActive(true);
        ProfilePanel.SetActive(false);
    }
    public void CloseLeaderboardsPanel()
    {
        LedearboardsPanel.SetActive(false);
        ProfilePanel.SetActive(true);
    }
    public void OpenCreditsPanel()
    {
        CreditsPanel.SetActive(true);
    }
    public void CloseCreditsPanel()
    {
        CreditsPanel.SetActive(false);
    }
    public void OpenMapPanel()
    {
        MapPanel.SetActive(true);
    }
    public void CloseMapPanel()
    {
        MapPanel.SetActive(false);
    }
    public void OpenEndlessModePanel()
    {
        EndlessModePanel.SetActive(true);
    }
    public void CloseEndlessModePanel()
    {
        EndlessModePanel.SetActive(false);
    }
    public void OpenSelectiobBossPanel()
    {
        SelectionBossPanel.SetActive(true);
    }
    public void OpenFeedbackPanel()
    {
        FeedbackPanel.SetActive(true);
        SettingPanel.SetActive(false);
    }
    public void CloseFeedbaackPanel()
    {
        FeedbackPanel.SetActive(false);
        SettingPanel.SetActive(true);
    }
    public void OpenConfirmLogoutPanel()
    {
        ConfirmLogoutPanel.SetActive(true);
    }
    public void CloseConfirmLogoutPanel()
    {
        ConfirmLogoutPanel.SetActive(false);
    }
    public void CloseConfirmExitPanel()
    {
        ConfirmExitPanel.SetActive(false);
    }
    public void ShowConfirmExitPanel()
    {
        ConfirmExitPanel.SetActive(true);
    }
    public void OpenConfirmDeleteAccount()
    {
        ConfirmDeleteAccount.SetActive(true);
    }
    public void CloseConfirmDeleteAccount()
    {
        ConfirmDeleteAccount.SetActive(false);
    }
}