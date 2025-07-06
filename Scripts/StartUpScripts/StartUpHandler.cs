using System;
using System.Collections;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class StartUpHandler : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public static StartUpHandler Instance { get; private set; }
    private FirebaseAuth auth;
    private FirebaseUser user;
    private bool isSignIn = false;
    private bool isSigned = false;
    private bool isRegisterPasswordVisible = false;
    private bool isRegisterConfirmPasswordVisible = false;
    private bool isLoginPasswordVisible = false;
    private bool canResendVerification = true;
    private const int verificationCooldownSeconds = 60;
    private DatabaseReference databaseReference;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private IEnumerator Start()
    {
        if (FirebaseManager.Instance != null)
        {
            yield return FirebaseManager.Instance.WaitForFirebaseInitialization();

            databaseReference = FirebaseManager.Instance.Database.RootReference;
            Debug.Log("Firebase database reference acquired.");
        }
        StartCoroutine(WaitForFirebaseInitialization());
        UIManager.Instance.RegisterPassInputField.contentType = TMP_InputField.ContentType.Password;
        UIManager.Instance.RegisterPassInputField.ForceLabelUpdate();
        UIManager.Instance.ConfirmPassInputField.contentType = TMP_InputField.ContentType.Password;
        UIManager.Instance.ConfirmPassInputField.ForceLabelUpdate();
        UIManager.Instance.LoginPassInputField.contentType = TMP_InputField.ContentType.Password;
        UIManager.Instance.LoginPassInputField.ForceLabelUpdate();
        UIManager.Instance.registerToggleVisibilityButton.onClick.AddListener(RegisterTogglePasswordVisibility);
        UIManager.Instance.registerConfirmToggleVisibilityButton.onClick.AddListener(RegisterToggleConfirmPasswordVisibility);
        UIManager.Instance.loginToggleVIsibilityButton.onClick.AddListener(LoginTogglePasswordVisibility);
    }
    private IEnumerator WaitForFirebaseInitialization()
    {
        yield return new WaitUntil(() => FirebaseManager.Instance.IsFirebaseInitialized());

        InitializeFirebase();
    }
    private void OnEnable()
    {
        if (auth == null)
        {
            InitializeFirebase();
        }
    }
    public void ForgotPass()
    {
        StartCoroutine(forgotPasswordSubmit(UIManager.Instance.RecoverEmailInputField.text));
    }
    private void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }
    
    [System.Serializable]
    public class Stage
    {
        public bool cleared;
        public bool unlocked;
        public Stage() {}
        public Stage(bool cleared, bool unlocked)
        {
            this.cleared = cleared;
            this.unlocked = unlocked;
        }
    }
    public void OpenWarningVerification()
    {
        string email = UIManager.Instance.RegisterEmailInputField.text;
        string password = UIManager.Instance.RegisterPassInputField.text;
        string confirmPassword = UIManager.Instance.ConfirmPassInputField.text;
        if (string.IsNullOrEmpty(email))
        {
            UIManager.Instance.ShowRegisterErrorMessage("MISSING EMAIL", UIManager.RegisterStatus.Warning);
            return;
        }
        if (string.IsNullOrEmpty(password))
        {
            UIManager.Instance.ShowRegisterErrorMessage("MISSING PASSWORD", UIManager.RegisterStatus.Warning);
            return;
        }
        if (string.IsNullOrEmpty(confirmPassword))
        {
            UIManager.Instance.ShowRegisterErrorMessage("MISSING CONFIRM PASSWORD", UIManager.RegisterStatus.Warning);
            return;
        }
        if (password != confirmPassword)
        {
            UIManager.Instance.ShowRegisterErrorMessage("PASSWORD DO NOT MATCH", UIManager.RegisterStatus.Warning);
            return;
        }
        if (!UIManager.Instance.RegisterAgreementToggle.isOn)
        {
            UIManager.Instance.ShowRegisterErrorMessage("YOU MUST AGREE TO THE TERMS AND CONDITIONS", UIManager.RegisterStatus.Failed);
            return;
        }
        StartCoroutine(CheckIfEmailExists(email));
    }
    private IEnumerator CheckIfEmailExists(string email)
    {
        var reference = FirebaseDatabase.DefaultInstance.GetReference("users");
        var task = reference.OrderByChild("email").EqualTo(email).GetValueAsync();

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogError($"Error checking email existence: {task.Exception}");
            yield break;
        }
        if (task.Result.Exists)
        {
            // Email exists in the database
            UIManager.Instance.ShowRegisterErrorMessage("EMAIL ALREADY IN USE", UIManager.RegisterStatus.Warning);
        }
        else
        {
            // Email does not exist, proceed
            UIManager.Instance.WarningVerificationPanel.SetActive(true);
        }
    }
    public void BackRegisterPanel()
    {
        UIManager.Instance.RegisterPagePanel.SetActive(true);
        UIManager.Instance.WarningVerificationPanel.SetActive(false);
    }
    private IEnumerator CloseWarningVerification()
    {
        UIManager.Instance.WarningVerificationPanel.SetActive(false);
        yield return new WaitForSeconds(4);
        UIManager.Instance.LoginPagePanel.SetActive(true);
        UIManager.Instance.RegisterPagePanel.SetActive(false);
    }
    public void ResendEmailVerification()
    {
        string email = UIManager.Instance.LoginEmailInputField.text;
        string password = UIManager.Instance.LoginPassInputField.text;
        if (string.IsNullOrEmpty(email))
        {
            UIManager.Instance.ShowLoginErrorMessage("MISSING EMAIL", UIManager.LoginStatus.Warning);
            return;
        }
        if (string.IsNullOrEmpty(password))
        {
            UIManager.Instance.ShowLoginErrorMessage("MISSING PASSWORD", UIManager.LoginStatus.Warning);
            return;
        }
        StartCoroutine(CheckEmailVerification(email, password));
    }

    private IEnumerator CheckEmailVerification(string email, string password)
    {
        Task<Firebase.Auth.AuthResult> loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => loginTask.IsCompleted);
        if (loginTask.Exception != null)
        {
            FirebaseException firebaseEx = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
            switch (errorCode)
            {
                case AuthError.InvalidEmail:
                    UIManager.Instance.ShowLoginErrorMessage("INVALID EMAIL", UIManager.LoginStatus.Failed);
                    break;
                case AuthError.WrongPassword:
                    UIManager.Instance.ShowLoginErrorMessage("WRONG PASSWORD", UIManager.LoginStatus.Failed);
                    break;
                case AuthError.UserNotFound:
                    UIManager.Instance.ShowLoginErrorMessage("USER NOT FOUND", UIManager.LoginStatus.Failed);
                    break;
                default:
                    UIManager.Instance.ShowLoginErrorMessage("LOGIN FAILED", UIManager.LoginStatus.Failed);
                    break;
            }
            yield break;
        }
        // Get the FirebaseUser from AuthResult
        FirebaseUser user = loginTask.Result.User;
        yield return user.ReloadAsync(); 
        if (!user.IsEmailVerified)
        {
            UIManager.Instance.ShowLoginErrorMessage("EMAIL NOT VERIFIED. PLEASE VERIFY YOUR EMAIL.", UIManager.LoginStatus.Warning);
            // Allow the user to resend the verification email
            StartCoroutine(ResendVerificationWithCooldown(user));
            yield break; 
        }
        UIManager.Instance.ShowLoginErrorMessage("EMAIL VERIFIED", UIManager.LoginStatus.Success);
    }
    public void SendEmailVerification()
    {
        string email = UIManager.Instance.RegisterEmailInputField.text;
        string password = UIManager.Instance.RegisterPassInputField.text;
        string confirmPassword = UIManager.Instance.ConfirmPassInputField.text;
        if (string.IsNullOrEmpty(email))
        {
            UIManager.Instance.ShowRegisterErrorMessage("MISSING EMAIL", UIManager.RegisterStatus.Warning);
            return;
        }
        if (string.IsNullOrEmpty(password))
        {
            UIManager.Instance.ShowRegisterErrorMessage("MISSING PASSWORD", UIManager.RegisterStatus.Warning);
            return;
        }
        if (string.IsNullOrEmpty(confirmPassword))
        {
            UIManager.Instance.ShowRegisterErrorMessage("MISSING CONFIRM PASSWORD", UIManager.RegisterStatus.Warning);
            return;
        }
        if (password != confirmPassword)
        {
            UIManager.Instance.ShowRegisterErrorMessage("PASSWORD DO NOT MATCH", UIManager.RegisterStatus.Warning);
            return;
        }
        StartCoroutine(RegisterAndSendVerification(email, password));
        StartCoroutine(CloseWarningVerification());
    }
    private IEnumerator RegisterAndSendVerification(string email, string password)
    {
        Task<AuthResult> RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);
        if (RegisterTask.Exception != null)
        {
            FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
            if (errorCode == AuthError.EmailAlreadyInUse)
            {
                user = auth.CurrentUser;
                if (user != null)
                {
                    yield return user.ReloadAsync(); 
                    if (!user.IsEmailVerified)
                    {
                        StartCoroutine(ResendVerificationWithCooldown(user));
                    }
                    else
                    {
                        UIManager.Instance.ShowRegisterErrorMessage("EMAIL ALREADY VERIFIED", UIManager.RegisterStatus.Warning);
                        UIManager.Instance.ShowLoginErrorMessage("EMAIL ALREADY VERIFIED", UIManager.LoginStatus.Warning);
                    }
                }
            }
            yield break;
        }
        user = RegisterTask.Result.User;
        yield return new WaitForSeconds(2);
        yield return user.ReloadAsync();
        if (user.IsEmailVerified)
        {
            UIManager.Instance.ShowRegisterErrorMessage("EMAIL ALREADY VERIFIED", UIManager.RegisterStatus.Warning);
            UIManager.Instance.ShowLoginErrorMessage("EMAIL ALREADY VERIFIED", UIManager.LoginStatus.Warning);
            yield break;
        }
        DateTime philippineTime = DateTime.UtcNow.AddHours(8);
        string formattedTime = philippineTime.ToString("yyyy-MM-dd HH:mm:ss");
        var userData = new Dictionary<string, object>
        {
            { "email", email },
            { "passwordHash", HashPassword(password) },
            { "tutorialCompleted", false },
            { "selectedCharacter", 0 },
            { "currency", 0 },  
            { "gems", 0 },      
            { "inventory", new Dictionary<string, int> { { "Blue Potion", 1 }, {"Red Potion", 1}, {"Damage Potion", 1} } }, 
            { "maxHealth", 600 },
            { "stages", new Dictionary<string, object>
                {
                    { "stage01", new Dictionary<string, object> {
                        { "cleared", false },
                        { "unlocked", true },
                        { "totalAttempt", 0},
                        { "highestTotalAnswer", 0},
                        { "firstPreviousHighestTotalAnswer", 0},
                        { "secondPreviousHighestTotalAnswer", 0},
                        { "thirdPreviousHighestTotalAnswer", 0},
                        { "fourthPreviousHighestTotalAnswer", 0},
                        { "shortestTime", "none"},
                        { "firstPreviousShortestTime", "none"},
                        { "secondPreviousShortestTime", "none"},
                        { "thirdPreviousShortestTime", "none"},
                        { "fourthPreviousShortestTime", "none"}
                    }},
                    { "stage02", new Dictionary<string, object> {
                        { "cleared", false },
                        { "unlocked", false },
                        { "totalAttempt", 0},
                        { "highestTotalAnswer", 0},
                        { "firstPreviousHighestTotalAnswer", 0},
                        { "secondPreviousHighestTotalAnswer", 0},
                        { "thirdPreviousHighestTotalAnswer", 0},
                        { "fourthPreviousHighestTotalAnswer", 0},
                        { "shortestTime", "none"},
                        { "firstPreviousShortestTime", "none"},
                        { "secondPreviousShortestTime", "none"},
                        { "thirdPreviousShortestTime", "none"},
                        { "fourthPreviousShortestTime", "none"}
                    }},
                    { "stage03", new Dictionary<string, object> {
                        { "cleared", false },
                        { "unlocked", false },
                        { "totalAttempt", 0},
                        { "highestTotalAnswer", 0},
                        { "firstPreviousHighestTotalAnswer", 0},
                        { "secondPreviousHighestTotalAnswer", 0},
                        { "thirdPreviousHighestTotalAnswer", 0},
                        { "fourthPreviousHighestTotalAnswer", 0},
                        { "shortestTime", "none"},
                        { "firstPreviousShortestTime", "none"},
                        { "secondPreviousShortestTime", "none"},
                        { "thirdPreviousShortestTime", "none"},
                        { "fourthPreviousShortestTime", "none"}
                    }},
                    { "stage04", new Dictionary<string, object> {
                        { "cleared", false },
                        { "unlocked", false },
                        { "stage4Task",false},
                        { "totalAttempt", 0},
                        { "highestTotalAnswer", 0},
                        { "firstPreviousHighestTotalAnswer", 0},
                        { "secondPreviousHighestTotalAnswer", 0},
                        { "thirdPreviousHighestTotalAnswer", 0},
                        { "fourthPreviousHighestTotalAnswer", 0},
                        { "shortestTime", "none"},
                        { "firstPreviousShortestTime", "none"},
                        { "secondPreviousShortestTime", "none"},
                        { "thirdPreviousShortestTime", "none"},
                        { "fourthPreviousShortestTime", "none"}
                    }},
                    { "stage05", new Dictionary<string, object> {
                        { "cleared", false },
                        { "unlocked", false },
                        { "totalAttempt", 0},
                        { "highestTotalAnswer", 0},
                        { "firstPreviousHighestTotalAnswer", 0},
                        { "secondPreviousHighestTotalAnswer", 0},
                        { "thirdPreviousHighestTotalAnswer", 0},
                        { "fourthPreviousHighestTotalAnswer", 0},
                        { "shortestTime", "none"},
                        { "firstPreviousShortestTime", "none"},
                        { "secondPreviousShortestTime", "none"},
                        { "thirdPreviousShortestTime", "none"},
                        { "fourthPreviousShortestTime", "none"}
                    }},
                    { "stage06", new Dictionary<string, object> {
                        { "cleared", false },
                        { "unlocked", false },
                        { "totalAttempt", 0},
                        { "highestTotalAnswer", 0},
                        { "firstPreviousHighestTotalAnswer", 0},
                        { "secondPreviousHighestTotalAnswer", 0},
                        { "thirdPreviousHighestTotalAnswer", 0},
                        { "fourthPreviousHighestTotalAnswer", 0},
                        { "shortestTime", "none"},
                        { "firstPreviousShortestTime", "none"},
                        { "secondPreviousShortestTime", "none"},
                        { "thirdPreviousShortestTime", "none"},
                        { "fourthPreviousShortestTime", "none"}
                    }},
                    { "stage07", new Dictionary<string, object> {
                        { "cleared", false },
                        { "unlocked", false },
                        { "totalAttempt", 0},
                        { "highestTotalAnswer", 0},
                        { "firstPreviousHighestTotalAnswer", 0},
                        { "secondPreviousHighestTotalAnswer", 0},
                        { "thirdPreviousHighestTotalAnswer", 0},
                        { "fourthPreviousHighestTotalAnswer", 0},
                        { "shortestTime", "none"},
                        { "firstPreviousShortestTime", "none"},
                        { "secondPreviousShortestTime", "none"},
                        { "thirdPreviousShortestTime", "none"},
                        { "fourthPreviousShortestTime", "none"}
                    }},
                    { "stage08", new Dictionary<string, object> {
                        { "cleared", false },
                        { "unlocked", false },
                        { "stage8Task",false},
                        { "totalAttempt", 0},
                        { "highestTotalAnswer", 0},
                        { "firstPreviousHighestTotalAnswer", 0},
                        { "secondPreviousHighestTotalAnswer", 0},
                        { "thirdPreviousHighestTotalAnswer", 0},
                        { "fourthPreviousHighestTotalAnswer", 0},
                        { "shortestTime", "none"},
                        { "firstPreviousShortestTime", "none"},
                        { "secondPreviousShortestTime", "none"},
                        { "thirdPreviousShortestTime", "none"},
                        { "fourthPreviousShortestTime", "none"}
                    }},
                    { "stage09", new Dictionary<string, object> {
                        { "cleared", false },
                        { "unlocked", false },
                        { "totalAttempt", 0},
                        { "highestTotalAnswer", 0},
                        { "firstPreviousHighestTotalAnswer", 0},
                        { "secondPreviousHighestTotalAnswer", 0},
                        { "thirdPreviousHighestTotalAnswer", 0},
                        { "fourthPreviousHighestTotalAnswer", 0},
                        { "shortestTime", "none"},
                        { "firstPreviousShortestTime", "none"},
                        { "secondPreviousShortestTime", "none"},
                        { "thirdPreviousShortestTime", "none"},
                        { "fourthPreviousShortestTime", "none"}
                    }},
                    { "stage10", new Dictionary<string, object> {
                        { "cleared", false },
                        { "unlocked", false },
                        { "totalAttempt", 0},
                        { "highestTotalAnswer", 0},
                        { "firstPreviousHighestTotalAnswer", 0},
                        { "secondPreviousHighestTotalAnswer", 0},
                        { "thirdPreviousHighestTotalAnswer", 0},
                        { "fourthPreviousHighestTotalAnswer", 0},
                        { "shortestTime", "none"},
                        { "firstPreviousShortestTime", "none"},
                        { "secondPreviousShortestTime", "none"},
                        { "thirdPreviousShortestTime", "none"},
                        { "fourthPreviousShortestTime", "none"}
                    }},
                    { "stage11", new Dictionary<string, object> {
                        { "cleared", false },
                        { "unlocked", false },
                        { "totalAttempt", 0},
                        { "highestTotalAnswer", 0},
                        { "firstPreviousHighestTotalAnswer", 0},
                        { "secondPreviousHighestTotalAnswer", 0},
                        { "thirdPreviousHighestTotalAnswer", 0},
                        { "fourthPreviousHighestTotalAnswer", 0},
                        { "shortestTime", "none"},
                        { "firstPreviousShortestTime", "none"},
                        { "secondPreviousShortestTime", "none"},
                        { "thirdPreviousShortestTime", "none"},
                        { "fourthPreviousShortestTime", "none"}
                    }},
                    { "stage12", new Dictionary<string, object> {
                        { "cleared", false },
                        { "unlocked", false },
                        { "stage12Task",false},
                        { "totalAttempt", 0},
                        { "highestTotalAnswer", 0},
                        { "firstPreviousHighestTotalAnswer", 0},
                        { "secondPreviousHighestTotalAnswer", 0},
                        { "thirdPreviousHighestTotalAnswer", 0},
                        { "fourthPreviousHighestTotalAnswer", 0},
                        { "shortestTime", "none"},
                        { "firstPreviousShortestTime", "none"},
                        { "secondPreviousShortestTime", "none"},
                        { "thirdPreviousShortestTime", "none"},
                        { "fourthPreviousShortestTime", "none"}
                    }},
                    { "stage13", new Dictionary<string, object> {
                        { "cleared", false },
                        { "unlocked", false },
                        { "totalAttempt", 0},
                        { "highestTotalAnswer", 0},
                        { "firstPreviousHighestTotalAnswer", 0},
                        { "secondPreviousHighestTotalAnswer", 0},
                        { "thirdPreviousHighestTotalAnswer", 0},
                        { "fourthPreviousHighestTotalAnswer", 0},
                        { "shortestTime", "none"},
                        { "firstPreviousShortestTime", "none"},
                        { "secondPreviousShortestTime", "none"},
                        { "thirdPreviousShortestTime", "none"},
                        { "fourthPreviousShortestTime", "none"}
                    }},
                    { "stage14", new Dictionary<string, object> {
                        { "cleared", false },
                        { "unlocked", false },
                        { "totalAttempt", 0},
                        { "highestTotalAnswer", 0},
                        { "firstPreviousHighestTotalAnswer", 0},
                        { "secondPreviousHighestTotalAnswer", 0},
                        { "thirdPreviousHighestTotalAnswer", 0},
                        { "fourthPreviousHighestTotalAnswer", 0},
                        { "shortestTime", "none"},
                        { "firstPreviousShortestTime", "none"},
                        { "secondPreviousShortestTime", "none"},
                        { "thirdPreviousShortestTime", "none"},
                        { "fourthPreviousShortestTime", "none"}
                    }},
                    { "stage15", new Dictionary<string, object> {
                        { "cleared", false },
                        { "unlocked", false },
                        { "totalAttempt", 0},
                        { "highestTotalAnswer", 0},
                        { "firstPreviousHighestTotalAnswer", 0},
                        { "secondPreviousHighestTotalAnswer", 0},
                        { "thirdPreviousHighestTotalAnswer", 0},
                        { "fourthPreviousHighestTotalAnswer", 0},
                        { "shortestTime", "none"},
                        { "firstPreviousShortestTime", "none"},
                        { "secondPreviousShortestTime", "none"},
                        { "thirdPreviousShortestTime", "none"},
                        { "fourthPreviousShortestTime", "none"}
                    }},
                    { "stage16", new Dictionary<string, object> {
                        { "cleared", false },
                        { "unlocked", false },
                        { "stage16Task",false},
                        { "totalAttempt", 0},
                        { "highestTotalAnswer", 0},
                        { "firstPreviousHighestTotalAnswer", 0},
                        { "secondPreviousHighestTotalAnswer", 0},
                        { "thirdPreviousHighestTotalAnswer", 0},
                        { "fourthPreviousHighestTotalAnswer", 0},
                        { "shortestTime", "none"},
                        { "firstPreviousShortestTime", "none"},
                        { "secondPreviousShortestTime", "none"},
                        { "thirdPreviousShortestTime", "none"},
                        { "fourthPreviousShortestTime", "none"}
                    }},
                    { "stage17", new Dictionary<string, object> {
                        { "cleared", false },
                        { "unlocked", false },
                        { "totalAttempt", 0},
                        { "highestTotalAnswer", 0},
                        { "firstPreviousHighestTotalAnswer", 0},
                        { "secondPreviousHighestTotalAnswer", 0},
                        { "thirdPreviousHighestTotalAnswer", 0},
                        { "fourthPreviousHighestTotalAnswer", 0},
                        { "shortestTime", "none"},
                        { "firstPreviousShortestTime", "none"},
                        { "secondPreviousShortestTime", "none"},
                        { "thirdPreviousShortestTime", "none"},
                        { "fourthPreviousShortestTime", "none"}
                    }},
                    { "stage18", new Dictionary<string, object> {
                        { "cleared", false },
                        { "unlocked", false },
                        { "totalAttempt", 0},
                        { "highestTotalAnswer", 0},
                        { "firstPreviousHighestTotalAnswer", 0},
                        { "secondPreviousHighestTotalAnswer", 0},
                        { "thirdPreviousHighestTotalAnswer", 0},
                        { "fourthPreviousHighestTotalAnswer", 0},
                        { "shortestTime", "none"},
                        { "firstPreviousShortestTime", "none"},
                        { "secondPreviousShortestTime", "none"},
                        { "thirdPreviousShortestTime", "none"},
                        { "fourthPreviousShortestTime", "none"}
                    }},
                    { "stage19", new Dictionary<string, object> {
                        { "cleared", false },
                        { "unlocked", false },
                        { "totalAttempt", 0},
                        { "highestTotalAnswer", 0},
                        { "firstPreviousHighestTotalAnswer", 0},
                        { "secondPreviousHighestTotalAnswer", 0},
                        { "thirdPreviousHighestTotalAnswer", 0},
                        { "fourthPreviousHighestTotalAnswer", 0},
                        { "shortestTime", "none"},
                        { "firstPreviousShortestTime", "none"},
                        { "secondPreviousShortestTime", "none"},
                        { "thirdPreviousShortestTime", "none"},
                        { "fourthPreviousShortestTime", "none"}
                    }},
                    { "stage20", new Dictionary<string, object> {
                        { "cleared", false },
                        { "unlocked", false },
                        { "totalAttempt", 0},
                        { "highestTotalAnswer", 0},
                        { "firstPreviousHighestTotalAnswer", 0},
                        { "secondPreviousHighestTotalAnswer", 0},
                        { "thirdPreviousHighestTotalAnswer", 0},
                        { "fourthPreviousHighestTotalAnswer", 0},
                        { "shortestTime", "none"},
                        { "firstPreviousShortestTime", "none"},
                        { "secondPreviousShortestTime", "none"},
                        { "thirdPreviousShortestTime", "none"},
                        { "fourthPreviousShortestTime", "none"}
                    }},
                    { "stage21", new Dictionary<string, object> {
                        { "cleared", false },
                        { "unlocked", false }
                    }},
                }
            },
            {"maxWave", 0}, 
            {"level", 1},   
            {"xp", 0},
            {"stagesReached", 0},
            {"xpToNextLevel", 1000},
            { "AccountCreated", formattedTime },
            { "clothes", new Dictionary<string, object>
                {
                    { "tops", new Dictionary<string, object>
                        {
                            { "BoyShirt1", new Dictionary<string, bool> { { "owned", true }, { "equipped", true } } },
                            { "BoySweater", new Dictionary<string, bool> { { "owned", false }, { "equipped", false } } },
                            { "BoyShirt3", new Dictionary<string, bool> { { "owned", false }, { "equipped", false } } },
                            { "GirlShirt1", new Dictionary<string, bool> { { "owned", true }, { "equipped", true } } },
                            { "GirlShirt2", new Dictionary<string, bool> { { "owned", false }, { "equipped", false } } },
                            { "GirlSweater", new Dictionary<string, bool> { { "owned", false }, { "equipped", false } } },
                        }
                    },
                    { "bottoms", new Dictionary<string, object>
                        {
                            { "BoyPants1", new Dictionary<string, bool> { { "owned", true }, { "equipped", true } } },
                            { "BoyPants2", new Dictionary<string, bool> { { "owned", false }, { "equipped", false } } },
                            { "BoyPants3", new Dictionary<string, bool> { { "owned", false }, { "equipped", false } } },
                            { "GirlPants1", new Dictionary<string, bool> { { "owned", true }, { "equipped", true } } },
                            { "GirlPants2", new Dictionary<string, bool> { { "owned", false }, { "equipped", false } } },
                        }
                    },
                    { "shoes", new Dictionary<string, object>
                        {
                            { "BoyBoots1", new Dictionary<string, bool> { { "owned", true }, { "equipped", true } } },
                            { "BoyBoots2", new Dictionary<string, bool> { { "owned", false }, { "equipped", false } } },
                            { "GirlBoots1", new Dictionary<string, bool> { { "owned", true }, { "equipped", true } } },
                            { "GirlBoots2", new Dictionary<string, bool> { { "owned", false }, { "equipped", false } } },
                        }
                    },
                }
            },
            {"achievements", new Dictionary<string, object>
                {
                    {"currency1000", new Dictionary<string, object>{
                        {"claimed", false},
                        {"isUnlocked", false}
                    }},
                    {"currency5000", new Dictionary<string, object>{
                        {"claimed", false},
                        {"isUnlocked", false}
                    }},
                    {"achieveLevel10", new Dictionary<string, object>{
                        {"claimed", false},
                        {"isUnlocked", false}
                    }},
                    {"achieveLevel20", new Dictionary<string, object>{
                        {"claimed", false},
                        {"isUnlocked", false}
                    }},
                    {"achieveWave10", new Dictionary<string, object>{
                        {"claimed", false},
                        {"isUnlocked", false}
                    }},
                    {"achieveWave20", new Dictionary<string, object>{
                        {"claimed", false},
                        {"isUnlocked", false}
                    }},
                    {"stagesReached10", new Dictionary<string, object>{
                        {"claimed", false},
                        {"isUnlocked", false}
                    }},
                    {"stagesReached20", new Dictionary<string, object>{
                        {"claimed", false},
                        {"isUnlocked", false}
                    }}
                }
            },
            { "isAdmin", false },
            { "DemoPowers", false }
        };
        var dbRef = FirebaseDatabase.DefaultInstance.GetReference("users").Child(user.UserId);
        Task setUserDataTask = dbRef.SetValueAsync(userData);
        yield return new WaitUntil(() => setUserDataTask.IsCompleted);
        if (setUserDataTask.Exception != null)
        {
            UIManager.Instance.ShowRegisterErrorMessage("Failed to initialize user data in the database.", UIManager.RegisterStatus.Failed);
            UIManager.Instance.ShowLoginErrorMessage("Failed to initialize user data in the database.", UIManager.LoginStatus.Failed);
            yield break;
        }
        Task sendVerificationTask = user.SendEmailVerificationAsync();
        yield return new WaitUntil(() => sendVerificationTask.IsCompleted);

        if (sendVerificationTask.Exception != null)
        {
            UIManager.Instance.ShowRegisterErrorMessage("FAILED TO SEND VERIFICATION EMAIL", UIManager.RegisterStatus.Failed);
            UIManager.Instance.ShowLoginErrorMessage("FAILED TO SEND VERIFICATION EMAIL", UIManager.LoginStatus.Failed);
            yield break;
        }
        UIManager.Instance.ShowRegisterErrorMessage("VERIFICATION EMAIL SENT AND SUCCESSFULY CREATED ACCOUNT", UIManager.RegisterStatus.Success);
        UIManager.Instance.ShowLoginErrorMessage("VERIFICATION EMAIL SENT AND SUCCESSFULY CREATED ACCOUNT", UIManager.LoginStatus.Success);
    }
    private IEnumerator ResendVerificationWithCooldown(FirebaseUser user)
    {
        if (!canResendVerification)
        {
            UIManager.Instance.ShowRegisterErrorMessage("Please wait before resending verification.", UIManager.RegisterStatus.Warning);
            UIManager.Instance.ShowLoginErrorMessage("Please wait before resending verification.", UIManager.LoginStatus.Warning);
            yield break;
        }
        yield return new WaitForSeconds(2); 
        yield return user.ReloadAsync();
        if (user.IsEmailVerified)
        {
            UIManager.Instance.ShowRegisterErrorMessage("EMAIL ALREADY VERIFIED", UIManager.RegisterStatus.Warning);
            UIManager.Instance.ShowLoginErrorMessage("EMAIL ALREADY VERIFIED", UIManager.LoginStatus.Warning);
            yield break;
        }
        canResendVerification = false;
        Task sendVerificationTask = user.SendEmailVerificationAsync();
        yield return new WaitUntil(() => sendVerificationTask.IsCompleted);
        if (sendVerificationTask.Exception != null)
        {
            FirebaseException firebaseEx = sendVerificationTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
            switch (errorCode)
            {
                case AuthError.NetworkRequestFailed:
                    UIManager.Instance.ShowRegisterErrorMessage("Network error, please try again.", UIManager.RegisterStatus.Failed);
                    UIManager.Instance.ShowLoginErrorMessage("Network error, please try again.", UIManager.LoginStatus.Failed);
                    break;
                case AuthError.TooManyRequests:
                    UIManager.Instance.ShowRegisterErrorMessage("Please wait 1 minute before resending verification", UIManager.RegisterStatus.Failed);
                    UIManager.Instance.ShowLoginErrorMessage("Please wait 1 minute before resending verification", UIManager.LoginStatus.Failed);
                    break;
                default:
                    UIManager.Instance.ShowRegisterErrorMessage("Failed to send verification email.", UIManager.RegisterStatus.Failed);
                    UIManager.Instance.ShowLoginErrorMessage("Failed to send verification email.", UIManager.LoginStatus.Failed);
                    Debug.LogError($"Error sending verification email: {firebaseEx.Message}");
                    break;
            }
            canResendVerification = true; 
            yield break;
        }
        UIManager.Instance.ShowRegisterErrorMessage("Verification email sent and registered successfully!", UIManager.RegisterStatus.Success);
        UIManager.Instance.ShowLoginErrorMessage("Verification email sent!", UIManager.LoginStatus.Success);
        StartCoroutine(VerificationCooldownTimer());
    }
    private IEnumerator VerificationCooldownTimer()
    {
        yield return new WaitForSeconds(verificationCooldownSeconds);
        canResendVerification = true;
    }
    public void LoginIfVerified()
    {
        string email = UIManager.Instance.LoginEmailInputField.text;
        string password = UIManager.Instance.LoginPassInputField.text;
        if (string.IsNullOrEmpty(email))
        {
            UIManager.Instance.ShowLoginErrorMessage("MISSING EMAIL", UIManager.LoginStatus.Warning);
            return;
        }
        if (string.IsNullOrEmpty(password))
        {
            UIManager.Instance.ShowLoginErrorMessage("MISSING PASSWORD", UIManager.LoginStatus.Warning);
            return;
        }
        StartCoroutine(LoginOnlyIfVerified(email, password));
    }

    private IEnumerator LoginOnlyIfVerified(string email, string password)
    {
        string trimmedEmail = email.Trim();
        Debug.Log("Attempting to login for email: " + trimmedEmail);
        var userQuery = FirebaseDatabase.DefaultInstance.GetReference("users")
                    .OrderByChild("email")
                    .EqualTo(trimmedEmail)
                    .GetValueAsync();

        yield return new WaitUntil(() => userQuery.IsCompleted);
        if (userQuery.Exception != null)
        {
            UIManager.Instance.ShowLoginErrorMessage("NETWORK ISSUE OR CREDENTIALS DON'T EXIST", UIManager.LoginStatus.Warning);
            yield break;
        }
        if (userQuery.Result.ChildrenCount == 0)
        {
            UIManager.Instance.ShowLoginErrorMessage("CREDENTIALS DON'T EXIST", UIManager.LoginStatus.Warning);
            yield break;
        }
        var userDataSnapshot = userQuery.Result.Children.GetEnumerator();
        userDataSnapshot.MoveNext();
        var userData = userDataSnapshot.Current;
        string storedHash = userData.Child("passwordHash").Value.ToString();
        string enteredHash = HashPassword(password);
        if (storedHash != enteredHash)
        {
            UIManager.Instance.ShowLoginErrorMessage("PASSWORD INCORRECT!", UIManager.LoginStatus.Warning);
            yield break;
        }
        Task<AuthResult> loginTask = auth.SignInWithEmailAndPasswordAsync(trimmedEmail, password);
        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            FirebaseException firebaseEx = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "LOGIN FAILED!";
            switch (errorCode)
            {
                case AuthError.UserNotFound:
                    message = "EMAIL DOESN'T EXIST";
                    break;
                case AuthError.InvalidEmail:
                    message = "INVALID EMAIL FORMAT";
                    break;
                case AuthError.WrongPassword:
                    message = "WRONG PASSWORD";
                    break;
                default:
                    message = "LOGIN FAILED!";
                    break;
            }
            UIManager.Instance.ShowLoginErrorMessage(message, UIManager.LoginStatus.Failed);
            yield break;
        }
        user = loginTask.Result.User;
        yield return user.ReloadAsync();
        if (!user.IsEmailVerified)
        {
            UIManager.Instance.ShowLoginErrorMessage("EMAIL NOT VERIFIED", UIManager.LoginStatus.Warning);
            auth.SignOut();
            yield break;
        }
        PhotonNetwork.NickName = user.DisplayName;
        UIManager.Instance.ShowLoginErrorMessage("LOGIN SUCCESSFUL!", UIManager.LoginStatus.Success, HandleSignIn);
    }
    private string HashPassword(string password)
    {
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            var builder = new System.Text.StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
    private IEnumerator forgotPasswordSubmit(string forgotPasswordEmail)
    {
        Task sendPasswordResetTask = auth.SendPasswordResetEmailAsync(forgotPasswordEmail);
        yield return new WaitUntil(() => sendPasswordResetTask.IsCompleted);

        if (sendPasswordResetTask.IsCanceled)
        {
            UIManager.Instance.ShowForgotPassErrorMessage("SENDING PASSWORD RESET EMAIL WAS CANCELED", UIManager.ResetStatus.Failed);
            yield break;
        }
        if (sendPasswordResetTask.IsFaulted)
        {
            foreach (Exception exception in sendPasswordResetTask.Exception.Flatten().InnerExceptions)
            {
                FirebaseException firebaseEx = exception as FirebaseException;
                if (firebaseEx != null)
                {
                    var errorCode = (AuthError)firebaseEx.ErrorCode;
                    string errorMessage = "AN ERROR OCCURRED, PLEASE TRY AGAIN";
                    switch (errorCode)
                    {
                        case AuthError.MissingEmail:
                            errorMessage = "PLEASE ENTER YOUR EMAIL";
                            break;
                        case AuthError.InvalidEmail:
                            errorMessage = "PLEASE ENTER A VALID EMAIL ADDRESS";
                            break;
                        case AuthError.UserNotFound:
                            errorMessage = "NO USER FOUND WITH THIS EMAIL";
                            break;
                        case AuthError.NetworkRequestFailed:
                            errorMessage = "NETWORK ERROR";
                            break;
                        default:
                            errorMessage = "AN ERROR OCCURRED, PLEASE TRY AGAIN";
                            break;
                    }
                    UIManager.Instance.ShowForgotPassErrorMessage(errorMessage, UIManager.ResetStatus.Failed);
                }
            }
        }
        else
        {
            UIManager.Instance.ShowForgotPassErrorMessage("SUCCESSFULLY SENT EMAIL FOR PASSWORD RESET", UIManager.ResetStatus.Success);
            UIManager.Instance.RecoverEmailInputField.text = "";
        }
    }
    void RegisterTogglePasswordVisibility()
    {
        isRegisterPasswordVisible = !isRegisterPasswordVisible;

        if(isRegisterPasswordVisible)
        {
            UIManager.Instance.RegisterPassInputField.contentType = TMP_InputField.ContentType.Standard;
        }else
        {
            UIManager.Instance.RegisterPassInputField.contentType = TMP_InputField.ContentType.Password;
        }
        UIManager.Instance.RegisterPassInputField.ForceLabelUpdate();
    }
    void RegisterToggleConfirmPasswordVisibility()
    {
        isRegisterConfirmPasswordVisible = !isRegisterConfirmPasswordVisible;

        if(isRegisterConfirmPasswordVisible)
        {
            UIManager.Instance.ConfirmPassInputField.contentType = TMP_InputField.ContentType.Standard;
        }else
        {
            UIManager.Instance.ConfirmPassInputField.contentType = TMP_InputField.ContentType.Password;
        }
        UIManager.Instance.ConfirmPassInputField.ForceLabelUpdate();
    }
    void LoginTogglePasswordVisibility()
    {
        isLoginPasswordVisible = !isLoginPasswordVisible;

        if(isLoginPasswordVisible)
        {
            UIManager.Instance.LoginPassInputField.contentType = TMP_InputField.ContentType.Standard;
        }else
        {
            UIManager.Instance.LoginPassInputField.contentType = TMP_InputField.ContentType.Password;
        }
        UIManager.Instance.LoginPassInputField.ForceLabelUpdate();
    }
    private void AuthStateChanged(object sender, EventArgs eventArgs)
    {
        StartCoroutine(CheckAuthStateWithDelay());
    }
    private IEnumerator CheckAuthStateWithDelay()
    {
        yield return new WaitForSeconds(3f);
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null && auth.CurrentUser.IsValid();
            if (!signedIn && user != null)
            {
                isSignIn = false;
                yield break;
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                yield return user.ReloadAsync();
                if (user.IsEmailVerified)
                {
                    isSignIn = true; 
                    StartCoroutine(LoadUserPreferencesAndProceed());
                }
                else
                {
                    auth.SignOut();
                    isSignIn = false; 
                }
            }
        }
    }
    private void OnDestroy()
    {
        if (auth != null)
        {
            auth.StateChanged -= AuthStateChanged;
            auth = null;
        }
    }
    private void Update()
    {
        if (isSignIn)
        {
            if (!isSigned)
            {
                isSigned = true;
                HandleSignIn();
            }
        }
    }
    private void HandleSignIn()
    {
        if (auth.CurrentUser != null)
        {
            StartCoroutine(UpdateLastSignedInDateAndProceed());
        }
    }
    private IEnumerator UpdateLastSignedInDateAndProceed()
    {
        var userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        var dbRef = FirebaseDatabase.DefaultInstance.GetReference("users").Child(userId);
        DateTime utcNow = DateTime.UtcNow;
        DateTime philippineTime = utcNow.AddHours(8); 
        string currentDateTime = philippineTime.ToString("yyyy-MM-dd HH:mm:ss");
        var updateTask = dbRef.Child("SignedIn").SetValueAsync(currentDateTime);
        yield return new WaitUntil(() => updateTask.IsCompleted);
        if (updateTask.IsFaulted)
        {
            Debug.LogError("Failed to update SignedIn field: " + updateTask.Exception);
            yield break; 
        }
        else if (updateTask.IsCompleted)
        {
            Debug.Log("SignedIn field successfully updated with: " + currentDateTime);
        }
        PhotonNetwork.NickName = user.DisplayName;
        StartCoroutine(LoadUserPreferencesAndProceed());
    }
    private IEnumerator LoadUserPreferencesAndProceed()
    {
        var userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        var dbRef = FirebaseDatabase.DefaultInstance.GetReference("users").Child(userId);
        var tutorialTask = dbRef.Child("tutorialCompleted").GetValueAsync();
        var characterTask = dbRef.Child("selectedCharacter").GetValueAsync();

        yield return new WaitUntil(() => tutorialTask.IsCompleted && characterTask.IsCompleted);

        if (tutorialTask.IsCompleted && characterTask.IsCompleted)
        {
            bool tutorialCompleted = Convert.ToBoolean(tutorialTask.Result.Value);
            int selectedCharacter = Convert.ToInt32(characterTask.Result.Value);
            PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
            PlayerPrefs.Save();
            if (!tutorialCompleted)
            {
                LoadCharacterSelectionScene();
            }
            else
            {
                LoadMainGameScene();
            }
        }
    }
    private void LoadCharacterSelectionScene()
    {
        GoToScene("CharacterSelectionScene");
    }
    private void LoadMainGameScene()
    {
        GoToScene("MainGameScene");
    }
    public void GoToScene(string sceneName)
    {
        StartCoroutine(DelayedSceneTransition(sceneName));
    }
    IEnumerator DelayedSceneTransition(string sceneName)
    {
        yield return new WaitForSeconds(1f);
        loadingScreen.SetActive(true);
        slider.value = 0;
        StartCoroutine(LoadAsynchronously(sceneName));
    }
    IEnumerator LoadAsynchronously(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false; 
        float targetProgress = 0f;
        while (operation.progress < 0.9f)
        {
            targetProgress = Mathf.Clamp01(operation.progress / 0.9f);
            while (slider.value < targetProgress)
            {
                slider.value = Mathf.MoveTowards(slider.value, targetProgress, Time.deltaTime * 0.5f); 
                yield return null; 
            }
            yield return null;
        }
        targetProgress = 1f;
        while (slider.value < targetProgress)
        {
            slider.value = Mathf.MoveTowards(slider.value, targetProgress, Time.deltaTime * 0.5f); 
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        operation.allowSceneActivation = true;
        yield return null;
        loadingScreen.SetActive(false);
    }
}