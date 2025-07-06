using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public VolumeSettings volumeSettings;
    public AudioManager audioManager;
    public static UIManager Instance { get; private set; }
    public GameObject WarningVerificationPanel, GuestPagePanel, RegisterPagePanel, LoginPagePanel, ForgotPassPagePanel, LoginErrorPanel, RegisterErrorPanel, ForgotPassErrorPanel, SettingsMenuPanel, FeedbackPagePanel, CanvasGameobj, TermsCondition;
    public TMP_InputField LoginEmailInputField, LoginPassInputField, RegisterEmailInputField, RegisterPassInputField, RecoverEmailInputField, ConfirmPassInputField;
    public TMP_Text LoginErrorMessage, RegisterErrorMessage, ForgotPassErrorMessage;
    public Toggle RegisterAgreementToggle;
    public Button registerToggleVisibilityButton, registerConfirmToggleVisibilityButton, loginToggleVIsibilityButton;
    public Image LoginSuccessCheck, LoginBlueExclamation, LoginRedExclamation, RegisterSuccessNB, RegisterBlueExclamation, RegisterRedExclamation, ResetSuccessCheck, ResetRedExclamation;

    public Color successColor = new Color32(0x47, 0x95, 0x69, 0xFF);  // #479569
    public Color failedColor = new Color32(0x9D, 0x2B, 0x1F, 0xFF);   // #9d2b1f
    public Color warningColor = new Color32(0x00, 0x4A, 0xAD, 0xFF);  // #004aad

     private void Start()
    {
        if (!SceneManager.GetSceneByName("StartupScene").isLoaded)
        {
            SceneManager.LoadScene("StartupScene", LoadSceneMode.Additive);
        }
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }
    private void OnEnable(){
        ResetPanels();
        if (SceneManager.GetActiveScene().name == "StartupScene"){
            CanvasGameobj.SetActive(true);
            Debug.Log("UIManager CanvasGameobj manually reactivated in OnEnable.");
            volumeSettings.setSettingVolume();
            Debug.Log("UIManager set volume.");
            audioManager.PlayMusic();
        }
    }
    private void OnDestroy(){
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }
    private void OnActiveSceneChanged(Scene previousScene, Scene newScene){
        if (newScene.name == "StartupScene"){
            CanvasGameobj.SetActive(true);
            Debug.Log("UIManager reactivated in StartupScene.");
            volumeSettings.setSettingVolume();
            Debug.Log("UIManager set volume.");
            audioManager.PlayMusic();
        }
        else{
            CanvasGameobj.SetActive(false);
            Debug.Log("UIManager deactivated in non-StartupScene.");
            audioManager.StopStartMusic();
        }
    }
    private void ResetPanels(){
        LoginErrorPanel.SetActive(false);
        RegisterErrorPanel.SetActive(false);
        ForgotPassErrorPanel.SetActive(false);
        GuestPagePanel.SetActive(false);
        RegisterPagePanel.SetActive(false);
        ForgotPassPagePanel.SetActive(false);
        LoginPagePanel.SetActive(false);
        CanvasGameobj.SetActive(true);
    }
    public void OpenGuestPagePanel(){
        GuestPagePanel.SetActive(true);
        LoginPagePanel.SetActive(false);
        RegisterPagePanel.SetActive(false);
        ForgotPassPagePanel.SetActive(false);
    }
    public void OpenLoginPagePanel(){
        LoginPagePanel.SetActive(true);
        GuestPagePanel.SetActive(false);
        RegisterPagePanel.SetActive(false);
        ForgotPassPagePanel.SetActive(false);
    }
    public void OpenRegisterPagePanel()
    {
        LoginPagePanel.SetActive(false);
        GuestPagePanel.SetActive(false);
        RegisterPagePanel.SetActive(true);
        ForgotPassPagePanel.SetActive(false);
    }
    public void OpenSettingsPanel()
    {
        SettingsMenuPanel.SetActive(true);
    }
    public void CloseSettingsPanel()
    {
        SettingsMenuPanel.SetActive(false);
    }
    public void OpenForgotPassPanel()
    {
        ForgotPassPagePanel.SetActive(true);
        LoginPagePanel.SetActive(false);
    }
    public void CloseLoginPagePanel()
    {
        LoginPagePanel.SetActive(false);
    }
    public void CloseRegisterPagePanel()
    {
        RegisterPagePanel.SetActive(false);
    }
    public void CloseForgotPassPanel()
    {
        ForgotPassPagePanel.SetActive(false);
        LoginPagePanel.SetActive(true);
    }
    public void CloseGuestPagePanel()
    {
        GuestPagePanel.SetActive(false);
    }
    public enum LoginStatus
    {
        Success,
        Warning,
        Failed,
    }
    public enum RegisterStatus
    {
        Success,
        Warning,
        Failed,
    }
    public enum ResetStatus
    {
        Success,
        Failed,
    }
    public void ShowLoginErrorMessage(string message, LoginStatus status, Action callback = null)
    {
        if (!CanvasGameobj.activeSelf)
        {
            CanvasGameobj.SetActive(true);
        }
        LoginErrorMessage.text = message;

        // Show the appropriate image based on the login status
        LoginSuccessCheck.gameObject.SetActive(status == LoginStatus.Success);
        LoginBlueExclamation.gameObject.SetActive(status == LoginStatus.Warning);
        LoginRedExclamation.gameObject.SetActive(status == LoginStatus.Failed);

        switch (status)
        {
            case LoginStatus.Success:
                LoginErrorMessage.color = successColor;
                break;
            case LoginStatus.Warning:
                LoginErrorMessage.color = warningColor;
                break;
            case LoginStatus.Failed:
                LoginErrorMessage.color = failedColor;
                break;
        }
        LoginErrorPanel.SetActive(true);
        StartCoroutine(HideErrorMessageAfterDelay(LoginErrorPanel, 3f, () =>
        {
            LoginSuccessCheck.gameObject.SetActive(false);
            LoginBlueExclamation.gameObject.SetActive(false);
            LoginRedExclamation.gameObject.SetActive(false);
            callback?.Invoke();
        }));
    }
    public void ShowRegisterErrorMessage(string message, RegisterStatus status,Action callback = null)
    {
        if (!CanvasGameobj.activeSelf)
        {
            CanvasGameobj.SetActive(true);
        }
        RegisterErrorMessage.text = message;

        // Show the appropriate image based on the login status
        RegisterSuccessNB.gameObject.SetActive(status == RegisterStatus.Success);
        RegisterBlueExclamation.gameObject.SetActive(status == RegisterStatus.Warning);
        RegisterRedExclamation.gameObject.SetActive(status == RegisterStatus.Failed);

        switch (status)
        {
            case RegisterStatus.Success:
                RegisterErrorMessage.color = successColor;
                break;
            case RegisterStatus.Warning:
                RegisterErrorMessage.color = warningColor;
                break;
            case RegisterStatus.Failed:
                RegisterErrorMessage.color = failedColor;
                break;
        }
        RegisterErrorPanel.SetActive(true);

        StartCoroutine(HideErrorMessageAfterDelay(RegisterErrorPanel, 3f, () =>
        {
            RegisterSuccessNB.gameObject.SetActive(false);
            RegisterBlueExclamation.gameObject.SetActive(false);
            RegisterRedExclamation.gameObject.SetActive(false);
            callback?.Invoke();
        }));
    }

    public void ShowForgotPassErrorMessage(string message, ResetStatus status, Action callback = null)
    {
        if (!CanvasGameobj.activeSelf)
        {
            CanvasGameobj.SetActive(true);
        }
        ForgotPassErrorMessage.text = message;

        // Show the appropriate image based on the login status
        ResetSuccessCheck.gameObject.SetActive(status == ResetStatus.Success);
        ResetRedExclamation.gameObject.SetActive(status == ResetStatus.Failed);

        switch (status)
        {
            case ResetStatus.Success:
                ForgotPassErrorMessage.color = successColor;
                break;
            case ResetStatus.Failed:
                ForgotPassErrorMessage.color = failedColor;
                break;
        }
        ForgotPassErrorPanel.SetActive(true);
        StartCoroutine(HideErrorMessageAfterDelay(ForgotPassErrorPanel,3f, () =>
        {
            ResetSuccessCheck.gameObject.SetActive(false);
            ResetRedExclamation.gameObject.SetActive(false);
            callback?.Invoke();
        }));
    }
    private IEnumerator HideErrorMessageAfterDelay(GameObject errorPanel, float delay, Action callback = null)
    {
        yield return new WaitForSeconds(delay);
        errorPanel.SetActive(false);
        callback?.Invoke();
    }
    public void ShowFeedbackPagePanel()
    {
        SettingsMenuPanel.SetActive(false);
        FeedbackPagePanel.SetActive(true);
    }
    public void CloseFeedbackPagePanel()
    {
        SettingsMenuPanel.SetActive(true);
        FeedbackPagePanel.SetActive(false);
    }
    public void OpenTermsConditionPanel()
    {
        TermsCondition.SetActive(true);
    }
    public void CloseTermsConditionPanel()
    {
        TermsCondition.SetActive(false);
    }
}