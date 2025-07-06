using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Firebase.Database;
using System.Collections.Generic;

public class Boss2BattleHandler : MonoBehaviourPunCallbacks
{
    public DamageContentAnimHandler damageContentAnimHandler;
    public static Boss2BattleHandler Instance { get; private set; }
    private PlayerHealthManager playerHealthManager;
    public Animator clockAnimator;
    public Slider phaseSlider; 
    public GameObject itemDetailsPanelPrefab; 
    private GameObject activeItemDetailsPanel;
    public InventoryUI inventoryUI;
    public GameObject loadingScreen;
    public Slider slider;
    public GameObject SettingPanel;
    public GameObject PlayerHealthInfo;
    public GameObject MonsterHealthInfo;
    public EnemyHealthBar enemyHealthBar;
    public PlayerHealthBar healthbar;
    public GameObject ReadyPanel;

    [Header("Pause Animation & Panel")]
    [SerializeField] private GameObject pausePanel; 
    [SerializeField] private Animator pausePanelAnimator;
    private bool isTimerPaused = false;

    [Header("Phase Images")]
    public GameObject attackPhaseImage; 
    public GameObject defensePhaseImage; 

    [Header("Battle inputs")]
    [SerializeField] private GameObject boss;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private TMP_Text feedbackText;
    [SerializeField] private TMP_Text currentAnswerText;
    [SerializeField] private TMP_Text totalDamageText;
    [SerializeField] private TMP_Text bossDamageText;
    [SerializeField] private TMP_Text playerHealthText;
    [SerializeField] private TMP_Text bossHealthText;
    [SerializeField] private TMP_Text phaseCountdownText;
    public GameObject countdownPanel;
    [SerializeField] private TMP_Text phaseResultText;

    [SerializeField] private Button digitButton0;
    [SerializeField] private Button digitButton1;
    [SerializeField] private Button digitButton2;
    [SerializeField] private Button digitButton3;
    [SerializeField] private Button digitButton4;
    [SerializeField] private Button digitButton5;
    [SerializeField] private Button digitButton6;
    [SerializeField] private Button digitButton7;
    [SerializeField] private Button digitButton8;
    [SerializeField] private Button digitButton9;
    [SerializeField] private Button clearButton;
    [SerializeField] private Button submitButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Button negativeButton;
    [SerializeField] private Button invButton;
    [SerializeField] private Button settingButton;

    [Header("Show Victory Panel")]
    public GameObject VictoryPanel;
    public Button VictoryButton1;

    [Header("Show Defeat Panel")]
    public GameObject DefeatPanel;
    public Button DefeatBtn1;

    [Header("Show Defeat Panel")]
    public GameObject ConfirmConcedePanel;

    private int correctAnswers = 0;
    private int currentAnswer = 0;
    private int correctAnswer = 0;
    private int totalDamage = 0;
    private int totalAccumulatedDamageToMonster = 0;
    private bool isDamageBoostActive = false;
    private float damageMultiplier = 1.0f;
    private bool damagePotionUsed = false;
    private int accumulatedDefenseSum = 0;
    private float timeRemaining =60f;
    private bool attackPhaseActive = false;
    private bool defensePhaseActive = false;
    private int bossDamage = 600;

    private Boss2 bossComponent;
    private PlayerSoloHeallth playerComponent;
    public PlayerSoloHeallth PlayerComponent => playerComponent;
    private GameObject instantiatedPlayer; 
    private Animator playerAnimator;
    private AudioManager audioManager;
    private Coroutine feedbackCoroutine;


    // generate questions
    private int rangeMin;
    private int rangeMax;
    private string operators;
    private int numOperandsMin;
    private int numOperandsMax;
 
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene.");
            return;
        }
        playerHealthManager = FindObjectOfType<PlayerHealthManager>();
        // Set initial panel states
        VictoryPanel.SetActive(false);
        DefeatPanel.SetActive(false);
        attackPhaseImage.SetActive(false);
        defensePhaseImage.SetActive(false);
        InitializeUIElements();

        // Enable HUD elements
        PlayerHealthInfo.SetActive(true);
        MonsterHealthInfo.SetActive(true);
        feedbackText.gameObject.SetActive(true);
        bossDamageText.gameObject.SetActive(true);
        bossHealthText.gameObject.SetActive(true);
        playerHealthText.gameObject.SetActive(true);

        // Find the instantiated player through Boss2CharacterLoader
        Boss2CharacterLoader characterLoader = FindObjectOfType<Boss2CharacterLoader>();
        if (characterLoader != null)
        {
            instantiatedPlayer = characterLoader.GetInstantiatedPlayer();
            if (instantiatedPlayer != null)
            {
                playerAnimator = instantiatedPlayer.GetComponent<Animator>();
                if (playerAnimator == null)
                {
                    Debug.LogError("Animator component not found on instantiated player.");
                }
            }
            if (inventoryUI == null)
            {
                inventoryUI = FindObjectOfType<InventoryUI>();
            }
        }

        if (instantiatedPlayer == null)
        {
            Debug.LogError("Instantiated Player is null");
            return;
        }
        
        playerComponent = instantiatedPlayer.GetComponent<PlayerSoloHeallth>();
        
        if (playerComponent == null || healthbar == null)
        {
            Debug.LogError("Player Health Bar or Player Component is null");
            return; 
        }

        // Set max health for the enemy health bar
        if (bossComponent == null && boss != null)
        {
            bossComponent = boss.GetComponent<Boss2>();
        }

        // Check if we have the current health set from the network, then update UI
        if (bossComponent != null)
        {
            enemyHealthBar.SetMaxHealth(bossComponent.MaxHealth);
            UpdateBossHealthUI(bossComponent.CurrentHealth, bossComponent.MaxHealth);
        }

        playerComponent.OnHealthLoaded += InitializePlayerHealthUI;
        UpdateTotalDamageText();
        UpdatePhaseResultText(bossDamage);
        DisableDigitButtons();
        FetchMathSettings();
        StartCoroutine(OnPlayStart());
    }
    private void FetchMathSettings()
    {
        // Default fallback values
        int defaultRangeMin = 1;
        int defaultRangeMax = 51;
        string defaultOperators = "+";
        int defaultNumOperandsMin = 2;
        int defaultNumOperandsMax = 3;

        // Fetching data from Firebase
        FirebaseDatabase.DefaultInstance.GetReference("MathSettings/Boss 1").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted && task.Result.Exists)
            {
                DataSnapshot snapshot = task.Result;

                // Use fetched values if available
                rangeMin = snapshot.Child("rangeMin").Exists ? int.Parse(snapshot.Child("rangeMin").Value.ToString()) : defaultRangeMin;
                rangeMax = snapshot.Child("rangeMax").Exists ? int.Parse(snapshot.Child("rangeMax").Value.ToString()) : defaultRangeMax;
                operators = snapshot.Child("operators").Exists ? snapshot.Child("operators").Value.ToString() : defaultOperators;
                numOperandsMin = snapshot.Child("numOperandsMin").Exists ? int.Parse(snapshot.Child("numOperandsMin").Value.ToString()) : defaultNumOperandsMin;
                numOperandsMax = snapshot.Child("numOperandsMax").Exists ? int.Parse(snapshot.Child("numOperandsMax").Value.ToString()) : defaultNumOperandsMax;

                // Debug log for successful data fetching
                Debug.Log("Firebase data fetched successfully:");
                Debug.Log($"rangeMin: {rangeMin}, rangeMax: {rangeMax}");
                Debug.Log($"operators: {operators}");
                Debug.Log($"numOperandsMin: {numOperandsMin}, numOperandsMax: {numOperandsMax}");
            }
            else
            {
                Debug.LogWarning("Failed to fetch data from Firebase. Using default values.");
                // If data fetch fails, set defaults
                rangeMin = defaultRangeMin;
                rangeMax = defaultRangeMax;
                operators = defaultOperators;
                numOperandsMin = defaultNumOperandsMin;
                numOperandsMax = defaultNumOperandsMax;
            }
        });
    }
    private void InitializePlayerHealthUI()
    {
        if (playerComponent != null)
        {
            // Ensure health bar is correctly updated
            healthbar.SetMaxHealth(playerComponent.MaxHealth); 
            healthbar.SetHealth(playerComponent.currentHealth);

            // Activate player health text GameObject if not already active
            if (!playerHealthText.gameObject.activeSelf)
            {
                playerHealthText.gameObject.SetActive(true);
            }

            if (playerHealthText != null)
            {
                Debug.Log("Updating player health text with value: " + playerComponent.currentHealth);

                playerHealthText.text = playerComponent.currentHealth.ToString();
                Debug.Log("Player Health Text Updated: " + playerHealthText.text);
            }
            else
            {
                Debug.LogError("playerHealthText is not assigned in the inspector.");
            }
        }
    }
private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);  // Prevent duplicate instances
        }
    }
    public void UpdateBossHealthUI(int currentHealth, int maxHealth)
    {
        if (enemyHealthBar != null)
        {
            enemyHealthBar.SetHealth(currentHealth);
        }

        if (bossHealthText != null)
        {
            bossHealthText.text = currentHealth.ToString();
        }
    }

    private IEnumerator OnPlayStart()
    {
        yield return new WaitForSeconds(1f);
        if(playerComponent != null)
        {
            playerComponent.currentHealth = playerComponent.MaxHealth;
            healthbar.SetMaxHealth(playerComponent.MaxHealth);
            healthbar.SetHealth(playerComponent.currentHealth);
            UpdatePlayerHealthText();
        }
        UpdateHealthUI();
        yield return new WaitForSeconds(2f);

        onPressedStart();
    }

    void Update()
    {
        if (attackPhaseActive || defensePhaseActive)
        {
            UpdateTimer();
        }
    }

    private void InitializeUIElements()
    {
        if (timerText != null) timerText.text = "" + timeRemaining;
        if (feedbackText != null) feedbackText.text = "";
        if (currentAnswerText != null) currentAnswerText.text = "0";
        if (totalDamageText != null) totalDamageText.text = "";
        if (bossDamageText != null) bossDamageText.text = "" + bossDamage;
        if (bossHealthText != null && bossComponent != null) bossHealthText.text = "" + bossComponent.MaxHealth;
        if (phaseCountdownText != null) phaseCountdownText.text = "";
        if (phaseResultText != null) phaseResultText.text = "";

        submitButton.onClick.AddListener(OnSubmitButtonClick);
        digitButton0.onClick.AddListener(() => OnDigitButtonClick(0));
        digitButton1.onClick.AddListener(() => OnDigitButtonClick(1));
        digitButton2.onClick.AddListener(() => OnDigitButtonClick(2));
        digitButton3.onClick.AddListener(() => OnDigitButtonClick(3));
        digitButton4.onClick.AddListener(() => OnDigitButtonClick(4));
        digitButton5.onClick.AddListener(() => OnDigitButtonClick(5));
        digitButton6.onClick.AddListener(() => OnDigitButtonClick(6));
        digitButton7.onClick.AddListener(() => OnDigitButtonClick(7));
        digitButton8.onClick.AddListener(() => OnDigitButtonClick(8));
        digitButton9.onClick.AddListener(() => OnDigitButtonClick(9));
        clearButton.onClick.AddListener(ClearInput);
        deleteButton.onClick.AddListener(DeleteLastDigit);
        negativeButton.onClick.AddListener(OnNegativeButtonClick);
    }
    private void OnNegativeButtonClick()
    {
        if (currentAnswerText.text.Length == 0 || currentAnswerText.text == "0")
        {
            currentAnswerText.text = "-";
        }
        else if (currentAnswerText.text == "-")
        {
            currentAnswerText.text = "0";
        }
    }

    private void UpdateTimer()
    {
        if (!isTimerPaused && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timerText.text = Mathf.Ceil(timeRemaining).ToString();
            phaseSlider.value = timeRemaining; 
        }
        else if (timeRemaining <= 0)
        {
            if (attackPhaseActive)
            {
                EndAttackPhase();
            }
            else if (defensePhaseActive)
            {
                EndDefensePhase();
            }
        }
    }

    public void StopTimerForSeconds(float duration)
    {
        StartCoroutine(PauseTimerCoroutine(duration));
    }

    private IEnumerator PauseTimerCoroutine(float duration)
    {
        isTimerPaused = true;
        ShowPausePanel(duration);
        yield return new WaitForSeconds(duration);
        HidePausePanel();
        isTimerPaused = false;
    }
    private void ShowPausePanel(float duration)
    {
        if (pausePanel != null && pausePanelAnimator != null)
        {
            pausePanel.SetActive(true); 
            pausePanelAnimator.Play("PauseTimerAnim"); 
        }
    }

    private void HidePausePanel()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }
    private void SetToIdleFight()
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("isIdleFight",true);
        }
    }

    private void TriggerAttackAnim(){
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("isAttack"); 
        }
    }

    private void TriggerAttackAnim1(){
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("isAttack1");
        }
    }

    private void TriggerDefPlayer()
    {
       if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("isDefend"); 
        }
    }

    private void TriggerDeadAnim()
    {
       if (playerAnimator != null)
        {
            playerAnimator.SetBool("isDead", true); 
        }
    }

    private void SetBoolTrueWin()
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("isWin", true);
        } 
    }

    private void TriggerNoDamageAnim()
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("isNoDamage");
        }
    }
    private void TriggerDefend1()
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("isDefend1");
        }
    }

    public void onPressedStart()
    {
        // ReadyPanel.SetActive(false);
        PlayerHealthInfo.SetActive(true);
        MonsterHealthInfo.SetActive(true);
        feedbackText.gameObject.SetActive(true);
        bossDamageText.gameObject.SetActive(true);
        bossHealthText.gameObject.SetActive(true);
        playerHealthText.gameObject.SetActive(true);

        if(playerComponent != null)
        {
            playerComponent.currentHealth = playerComponent.MaxHealth;
            healthbar.SetMaxHealth(playerComponent.MaxHealth);
            healthbar.SetHealth(playerComponent.currentHealth);
            UpdatePlayerHealthText();
        }

        SetToIdleFight();

        if (!attackPhaseActive && !defensePhaseActive)
        {
            StartCoroutine(StartPhaseWithCountdown("Attack Phase"));
        }

        SetDigitButtonsInteractable(true);
        damageContentAnimHandler.OpenContent();
    }

    private IEnumerator StartPhaseWithCountdown(string phase)
    {
        // Ensure the countdown panel is visible
        if (countdownPanel != null)
        {
            countdownPanel.SetActive(true);
        }

        // Display the phase name first
        if (phaseCountdownText != null)
        {
            phaseCountdownText.text = phase;
        }
        yield return new WaitForSeconds(1.5f); // Hold the phase name for a second

        // Display "Starting in"
        if (phaseCountdownText != null)
        {
            phaseCountdownText.text = "Starting in";
        }
        yield return new WaitForSeconds(1f);

        audioManager.playCountdown();
        // Countdown from 3
        int countdown = 3;
        while (countdown > 0)
        {
            if (phaseCountdownText != null)
            {
                phaseCountdownText.text = countdown.ToString();
            }
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        // Show "Go!" before starting the phase
        if (phaseCountdownText != null)
        {
            phaseCountdownText.text = "Go!";
        }
        yield return new WaitForSeconds(0.5f);

        // Hide the countdown panel
        if (countdownPanel != null)
        {
            countdownPanel.SetActive(false);
        }

        // Start the appropriate phase
        if (phase == "Attack Phase")
        {
            StartAttackPhase();
        }
        else if (phase == "Defense Phase")
        {
            StartDefensePhase();
        }
    }

    private void StartAttackPhase()
    {
        attackPhaseActive = true;
        defensePhaseActive = false;
        damagePotionUsed = false;
        damageMultiplier = 1.0f;
        isDamageBoostActive = false;
        timeRemaining = 60f;
        correctAnswers = 0;
        totalDamage = 0;
        bossDamage = 600; 
        attackPhaseImage.SetActive(true);
        defensePhaseImage.SetActive(false);
        UpdatePhaseResultText(bossDamage);
        GenerateQuestion();
        UpdateTotalDamageText(); 
        SetDigitButtonsInteractable(true);
        currentAnswer = 0;
        if(currentAnswerText != null) currentAnswerText.text = "0";
    }

    private void StartDefensePhase()
    {
        attackPhaseActive = false;
        defensePhaseActive = true;
        damagePotionUsed = false;
        damageMultiplier = 1.0f;
        isDamageBoostActive = false;
        timeRemaining = 60f;
        accumulatedDefenseSum = 0;
        SetDigitButtonsInteractable(true);
        bossDamage = 600; 
        defensePhaseImage.SetActive(true);
        attackPhaseImage.SetActive(false);
        UpdatePhaseResultText(bossDamage);
        GenerateQuestion();
        totalDamage = 0;
        UpdateTotalDamageText();
        currentAnswer = 0;
        if(currentAnswerText != null) currentAnswerText.text = "0";
    }

    private void GenerateQuestion()
    {
        bool isValid = false;

        while (!isValid)
        {
            // Generate random number of operands
            int numOperands = UnityEngine.Random.Range(numOperandsMin, numOperandsMax + 1);
            int[] operands = new int[numOperands];
            char[] chosenOperators = new char[numOperands - 1];

            // Generate operands
            for (int i = 0; i < numOperands; i++)
            {
                operands[i] = UnityEngine.Random.Range(rangeMin, rangeMax);
            }

            // Generate operators
            for (int i = 0; i < chosenOperators.Length; i++)
            {
                chosenOperators[i] = operators[UnityEngine.Random.Range(0, operators.Length)];
            }

            // Adjust operands for division to ensure integer results
            for (int i = 1; i < numOperands; i++)
            {
                if (chosenOperators[i - 1] == '/')
                {
                    operands[i] = UnityEngine.Random.Range(1, 10);
                    operands[i - 1] = operands[i] * UnityEngine.Random.Range(1, 10);
                }
            }

            // Apply PEMDAS
            List<int> nums = new List<int>(operands);
            List<char> ops = new List<char>(chosenOperators);

            // Handle multiplication and division first
            for (int i = 0; i < ops.Count; i++)
            {
                if (ops[i] == '*' || ops[i] == '/')
                {
                    int result = ops[i] == '*' ? nums[i] * nums[i + 1] : nums[i] / nums[i + 1];
                    nums[i] = result;
                    nums.RemoveAt(i + 1);
                    ops.RemoveAt(i);
                    i--;
                }
            }

            // Handle addition and subtraction
            correctAnswer = nums[0];
            for (int i = 0; i < ops.Count; i++)
            {
                correctAnswer = ops[i] == '+' ? correctAnswer + nums[i + 1] : correctAnswer - nums[i + 1];
            }

            // Validate the answer
            if (correctAnswer >= 20 && correctAnswer <= 135)
            {
                isValid = true;

                // Update the question text
                if (questionText != null)
                {
                    questionText.text = operands[0].ToString();
                    for (int i = 1; i < numOperands; i++)
                    {
                        questionText.text += " " + chosenOperators[i - 1] + " " + operands[i].ToString();
                    }
                    questionText.text += " =";
                }
            }
            else
            {
                Debug.Log("Invalid answer generated: " + correctAnswer + ". Regenerating question...");
            }
        }
        Debug.Log("Generated Question: " + questionText.text + " " + correctAnswer);
    }


    private void OnDigitButtonClick(int digit)
    {
        if (currentAnswerText.text == "-")
        {
            currentAnswer = -digit;
        }
        else
        {
            if (currentAnswerText.text.StartsWith("-"))
            {
                currentAnswer = currentAnswer * 10 - digit; 
            }
            else
            {
                currentAnswer = currentAnswer * 10 + digit;
            }
        }
        UpdateCurrentAnswerText();
    }
    private void UpdateTotalDamageText()
    {
        int boostedDamage = Mathf.RoundToInt(totalDamage * damageMultiplier);  // Apply the multiplier
        if (totalDamageText != null)
        {
            totalDamageText.text = "" + boostedDamage;  // Update the damage text in the UI
        }
    }

    private void UpdatePhaseResultText(int monsterDamageValue)
    {
        if (phaseResultText != null)
        {
            phaseResultText.text = "" + monsterDamageValue;
        }
    }
    private void OnSubmitButtonClick()
    {
        if (currentAnswer == correctAnswer)
        {
            SetFeedback("Correct", new Color(0.388f, 0.525f, 0.314f));
            audioManager.triggerCorrectAnswer();
            int positiveCorrectAnswer = Mathf.Abs(correctAnswer);
            correctAnswers++;

            if (attackPhaseActive)
            {
                // In attack phase, accumulate total damage
                totalDamage += positiveCorrectAnswer;
                totalAccumulatedDamageToMonster += positiveCorrectAnswer;
                Debug.Log("Current Accumulated Damage: " + totalAccumulatedDamageToMonster);
                UpdateTotalDamageText(); // Update UI for total damage
            }
            else if (defensePhaseActive)
        {
            // Add the correct answer to accumulatedDefenseSum
            accumulatedDefenseSum += positiveCorrectAnswer;

            // In defense phase, reduce monster damage
            if (positiveCorrectAnswer <= bossDamage)
            {
                bossDamage -= positiveCorrectAnswer;
                UpdatePhaseResultText(bossDamage); // Update UI for monster damage
            }
            else
            {
                // If damage exceeds monster damage
                int excessDamage = positiveCorrectAnswer - bossDamage; // Calculate excess damage
                totalDamage += excessDamage; // Add excess damage to totalDamage
                totalAccumulatedDamageToMonster += excessDamage;
                bossDamage = 0; // Monster damage reduced to 0
                Debug.Log("Current Accumulated Damage: " + totalAccumulatedDamageToMonster);
                UpdatePhaseResultText(bossDamage); // Update UI for monster damage
                UpdateTotalDamageText(); // Update UI for total damage
            }
        }

            // After the player submits, reset the current answer and generate a new question
            currentAnswer = 0;
            UpdateCurrentAnswerText();
            GenerateQuestion();
        }
        else
        {
            SetFeedback("Incorrect", Color.red);
            audioManager.triggerWrongAnswer();
            currentAnswer = 0;
            if(currentAnswerText != null) currentAnswerText.text = "0";
        }
    }

    private void DeleteLastDigit()
    {
        if (currentAnswer != 0)
        {
            currentAnswer /= 10; 
            UpdateCurrentAnswerText(); 
            SetFeedback("", Color.clear);
        }
    }

    private void UpdateCurrentAnswerText()
    {
        if (currentAnswerText != null)
        {
            currentAnswerText.text = currentAnswer.ToString();
        }
    }

    private void SetFeedback(string message, Color color)
    {
        if (feedbackText != null)
        {
            // Update the feedback message and color
            feedbackText.text = message;
            feedbackText.color = color;

            // Stop any existing coroutine before starting a new one
            if (feedbackCoroutine != null)
            {
                StopCoroutine(feedbackCoroutine);
            }

            // Start the coroutine to clear feedback after a delay
            feedbackCoroutine = StartCoroutine(ClearFeedbackAfterDelay());
        }
    }

    private IEnumerator ClearFeedbackAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        if (feedbackText != null)
        {
            feedbackText.text = "";
        }
        feedbackCoroutine = null; // Clear reference after coroutine finishes
    }

    private void ClearInput()
    {
        currentAnswer = 0;
        UpdateCurrentAnswerText();
        if (feedbackText != null) feedbackText.text = "";
    }

    private void EndAttackPhase()
    {
        StartCoroutine(EndAttackPhasewithClock());
    }
    private IEnumerator EndAttackPhasewithClock()
    {
        attackPhaseActive = false;
        if (timerText != null) timerText.text = "0";
        DisableDigitButtons();
        clockAnimator.SetBool("isTimesUp", true);
        audioManager.triggerTimesUp();
        UpdateTotalDamageText();
        StartCoroutine(DelayedApplyDamage());
        yield return new WaitForSeconds(audioManager.TimesUp.length);
        clockAnimator.SetBool("isTimesUp", false);
    }
    private void EndDefensePhase()
    {
        StartCoroutine(EndDefensePhaseWithClock());
    }
    private IEnumerator EndDefensePhaseWithClock()
    {
        defensePhaseActive = false;
        if (timerText != null) timerText.text = "0";
        DisableDigitButtons();
        clockAnimator.SetBool("isTimesUp", true);
        audioManager.triggerTimesUp();
        StartCoroutine(CalculateDefenseImpact());
        CheckPlayerHealth();
        yield return new WaitForSeconds(audioManager.TimesUp.length);
        clockAnimator.SetBool("isTimesUp", false);
        StartCoroutine(switchAttackPhase());
    }
    private IEnumerator switchAttackPhase(){
        yield return new WaitForSeconds(2f);
        if (playerComponent.currentHealth > 0 && bossComponent.CurrentHealth> 0)
        {
            StartCoroutine(StartPhaseWithCountdown("Attack Phase"));
        }
    }

    private IEnumerator CalculateDefenseImpact()
    {
        yield return new WaitForSeconds(audioManager.TimesUp.length);
        // Set the monster damage to a fixed value
        int bossDamage = 600;
        
        // Initialize damage to the full monster damage
        int damageToPlayer = bossDamage;
        int excessDamage = 0; // Initialize excessDamage variable

        // If accumulated defense is greater than or equal to monster damage, 
        // set damage to player to 0 (defense absorbs all damage)
        if (accumulatedDefenseSum >= bossDamage)
        {
            damageToPlayer = 0;  // No damage to the player, defense absorbed it

            // Calculate excess damage (if defense exceeds monster damage)
            excessDamage = accumulatedDefenseSum - bossDamage;
            if (excessDamage > 0)
            {
                StartCoroutine(DisplayExcessDamageToMonster(excessDamage));  // Apply excess damage to the monster
            }
        }
        else
        {
            // If accumulated defense is less than monster damage, 
            // player takes the remaining damage (monster damage - defense)
            damageToPlayer = bossDamage - accumulatedDefenseSum;
        }

        // Log for debugging
        Debug.Log($"Monster Damage: {bossDamage}, Accumulated Defense: {accumulatedDefenseSum}, Damage to Player: {damageToPlayer}");

        // Apply the damage to the player
        if (damageToPlayer > 0)
        {
            bossComponent.GetComponent<Animator>().SetTrigger("isAttack");
            yield return new WaitForSeconds(0.4f);
            Debug.Log($"Damage to Player: {damageToPlayer}, Player Health Before: {playerComponent.currentHealth}");
            TriggerDefPlayer();
            audioManager.getHit();
            playerComponent.TakeDamage(damageToPlayer);
            Debug.Log($"Damage to Player: {damageToPlayer}, New Player Health: {playerComponent.currentHealth}");
            healthbar.SetHealth(playerComponent.currentHealth);
            UpdatePlayerHealthText();
            CheckPlayerHealth();
        }
        else
        {
            Debug.Log("No damage taken by the player. Defense fully absorbed the attack.");

            // If monster damage is 0 and there's no excess damage, trigger TriggerDefend1()
            if (excessDamage == 0)
            {
                TriggerDefend1(); // Trigger the defense animation or effect for no damage scenario
                audioManager.getDefend();
                yield return new WaitForSeconds(1f);
                bossComponent.GetComponent<Animator>().SetTrigger("isAttack"); 
            }
        }
    }

    private IEnumerator DisplayExcessDamageToMonster(int excessDamage)
    {
        // Apply excess damage to the monster if player defense exceeds monster damage
        TriggerAttackAnim();
        audioManager.triggerAttack();
        yield return new WaitForSeconds(1f);
        bossComponent.photonView.RPC("TakeDamage", RpcTarget.AllBuffered, excessDamage);
        bossComponent.GetComponent<Animator>().SetTrigger("isHit");
        Debug.Log($"Monster takes {excessDamage} excess damage. Current Health: {bossComponent.CurrentHealth}");
        // Check if the monster is defeated
        if (bossComponent.CurrentHealth <= 0)
        {
            Debug.Log("Monster Defeated! Victory.");
            DisableDigitButtons();
        }
    }

    public void UpdatePlayerHealthText()
    {
        if (playerHealthText != null && playerComponent != null)
        {
            playerHealthText.text = playerComponent.currentHealth.ToString();
            Debug.Log("Updated Player Health Text: " + playerHealthText.text); // Confirm update
        }
    }
    private void CheckPlayerHealth()
    {
        if (playerComponent.currentHealth <= 0)
        {
            Debug.Log("Player is dead. Game Over.");
            TriggerDeadAnim();
            StartCoroutine(ShowDefeatSequence());
        }
    }

    private IEnumerator ShowDefeatSequence()
    {
        yield return new WaitForSeconds(0.3f);
        DefeatPanel.SetActive(true);
        DisableDigitButtons();
        DefeatBtn1.interactable = false;
        audioManager.triggerGameoverSFX();
        yield return new WaitForSeconds(audioManager.GameoverSFX.length);
        DefeatBtn1.interactable = true;
        yield return new WaitForSeconds(9f);
        DefeatPanel.SetActive(false);
        leaveScene();
    }

    private IEnumerator DelayedApplyDamage()
    {
        yield return new WaitForSeconds(1.0f); // Wait for 1 second (adjust this duration as needed)
        RandomApplyDamageAnim();
    }
    void RandomApplyDamageAnim()
    {
        int randomValue = Random.Range(0, 2); // Generates 0 or 1

        if (randomValue == 0)
        {
            StartCoroutine(ApplyDamage());
        }
        else
        {
            StartCoroutine(ApplyDamage1());
        }
    }

    private IEnumerator ApplyDamage1()
    {
        if (boss != null)
        {
            Boss2 monsterComponent = boss.GetComponent<Boss2>();
            if (monsterComponent != null)
            {
                // Calculate final damage
                int finalDamage = Mathf.RoundToInt(totalDamage * damageMultiplier);
                Debug.Log("ApplyDamage1 - Final Damage: " + finalDamage); // Debug finalDamage value

                // Trigger appropriate animation and apply damage
                if (finalDamage > 0)
                {
                    TriggerAttackAnim1();
                    audioManager.triggerAttack1();
                    yield return new WaitForSeconds(1f);
                    monsterComponent.photonView.RPC("TakeDamage", RpcTarget.AllBuffered, finalDamage); // Apply total damage accumulated
                    bossComponent.GetComponent<Animator>().SetTrigger("isHit");

                    // Check if the monster is defeated
                    if (monsterComponent.CurrentHealth <= 0)
                    {
                        Debug.Log("Monster Defeated! Victory.");
                        DisableDigitButtons();
                        yield break;
                    }
                }
                else
                {
                    TriggerNoDamageAnim();
                    audioManager.triggerNoDamage();
                    Debug.Log("Trigger No Damage");
                }

                // Proceed to the next phase only if the monster is not defeated
                yield return new WaitForSeconds(3f);
                StartCoroutine(StartPhaseWithCountdown("Defense Phase"));
            }
        }
    }

    private IEnumerator ApplyDamage()
    {
        if (boss != null)
        {
            Boss2 monsterComponent = boss.GetComponent<Boss2>();
            if (monsterComponent != null)
            {
                // Calculate final damage
                int finalDamage = Mathf.RoundToInt(totalDamage * damageMultiplier);
                Debug.Log("ApplyDamage - Final Damage: " + finalDamage); // Debug finalDamage value

                // Trigger appropriate animation and apply damage
                if (finalDamage > 0)
                {
                    TriggerAttackAnim();
                    audioManager.triggerAttack();
                    yield return new WaitForSeconds(1f);
                    monsterComponent.photonView.RPC("TakeDamage", RpcTarget.AllBuffered, finalDamage); // Apply total damage accumulated
                    bossComponent.GetComponent<Animator>().SetTrigger("isHit");

                    // Check if the monster is defeated
                    if (monsterComponent.CurrentHealth <= 0)
                    {
                        Debug.Log("Monster Defeated! Victory.");
                        DisableDigitButtons();
                        yield break;
                    }
                }
                else
                {
                    TriggerNoDamageAnim();
                    audioManager.triggerNoDamage();
                    Debug.Log("Trigger No Damage");
                }

                // Proceed to the next phase only if the monster is not defeated
                yield return new WaitForSeconds(3f);
                StartCoroutine(StartPhaseWithCountdown("Defense Phase"));
            }
        }
    }

    public void OnClickReady(){
        onPressedStart();
        ReadyPanel.SetActive(false);
    }

    public void UsePotionFromInventory(string potionName)
    {
        if (inventoryUI != null)
        {
            inventoryUI.UseItem(potionName);
            UpdateHealthUI();
        }
    }

    public void UpdateHealthUI()
    {
        if(playerComponent != null)
        {
            healthbar.SetHealth(playerComponent.currentHealth);
            UpdatePlayerHealthText();
        }
    }
    public void OpenSettingPanel()
    {
        SettingPanel.SetActive(true);
    }

    public void CloseSettingPanel()
    {
        SettingPanel.SetActive(false);
    }
    public void leaveScene()
    {
        PhotonNetwork.LeaveRoom();
        GoToScene("MainGameScene");
    }

    public void GoToScene(string sceneName)
    {
        loadingScreen.SetActive(true);
        slider.value = 0;
        audioManager.StopMusic();
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
    public void ShowItemDetailsPanel()
    {
        // If there's already an active panel, destroy it
        if (activeItemDetailsPanel != null)
        {
            Destroy(activeItemDetailsPanel);
        }

        // Instantiate the ItemDetailsPanel prefab
        activeItemDetailsPanel = Instantiate(itemDetailsPanelPrefab, transform);

        // Find the UseButton inside the instantiated panel
        Button useButton = activeItemDetailsPanel.transform.Find("UseButton")?.GetComponent<Button>();

        // Check if the button was found
        if (useButton != null)
        {
            useButton.onClick.AddListener(() => UsePotionFromInventory("Red Potion"));
        }
        else
        {
            Debug.LogError("UseButton not found in ItemDetailsPanel!");
        }
    }
    public void ConcedeBattle()
    {
        // Stop any active phases
        attackPhaseActive = false;
        defensePhaseActive = false;
        
        // Stop the timer
        if (timerText != null)
            timerText.text = "0";
            phaseSlider.value = 0;

        StartCoroutine(ShowDefeatSequence());
        ConfirmConcedePanel.SetActive(false);
    }
    public void OpenConfirmConcedePanel()
    {
        ConfirmConcedePanel.SetActive(true);
        SettingPanel.SetActive(false);
    }

    public void CloseConfirmConcedePanel()
    {
        ConfirmConcedePanel.SetActive(false);
        SettingPanel.SetActive(true);
    }


    private void DisableDigitButtons()
    {
        SetDigitButtonsInteractable(false);
    }

    private void SetDigitButtonsInteractable(bool interactable)
    {
        if (digitButton0 != null) digitButton0.interactable = interactable;
        if (digitButton1 != null) digitButton1.interactable = interactable;
        if (digitButton2 != null) digitButton2.interactable = interactable;
        if (digitButton3 != null) digitButton3.interactable = interactable;
        if (digitButton4 != null) digitButton4.interactable = interactable;
        if (digitButton5 != null) digitButton5.interactable = interactable;
        if (digitButton6 != null) digitButton6.interactable = interactable;
        if (digitButton7 != null) digitButton7.interactable = interactable;
        if (digitButton8 != null) digitButton8.interactable = interactable;
        if (digitButton9 != null) digitButton9.interactable = interactable;
        if (clearButton != null) clearButton.interactable = interactable;
        submitButton.interactable = interactable;
        negativeButton.interactable = interactable;
        deleteButton.interactable = interactable;
        invButton.interactable = interactable;
        settingButton.interactable = interactable;
    }

    public void ApplyDamageBoost()
    {
        if (attackPhaseActive && !isDamageBoostActive)
        {
            Debug.Log("Applying 1.2x damage boost.");
            damageMultiplier = 1.2f;  // Apply the boost
            isDamageBoostActive = true;
            UpdateTotalDamageText();  // Update UI to show boosted damage
        }
    }

    public bool IsAttackPhaseActive()
    {
        return attackPhaseActive;
    }

    // Method to check if the defense phase is active
    public bool IsDefensePhaseActive()
    {
        return defensePhaseActive;
    }

    public void SetDamagePotionUsed(bool value)
    {
        damagePotionUsed = value;
    }

    public bool IsDamagePotionUsed()
    {
        return damagePotionUsed;
    }

    private IEnumerator ShowVictoryAndRewards()
    {
        VictoryPanel.SetActive(true);
        SetBoolTrueWin();
        audioManager.TriggerWinSound();
        VictoryButton1.interactable = false;
        DisableDigitButtons();
        yield return new WaitForSeconds(audioManager.WinSound.length);

        List<RewardItem> rewards = CalculateRewards();
        FindObjectOfType<RewardManager>().ShowRewards(rewards);
        audioManager.PlayRewardSound();

        yield return new WaitForSeconds(6f);

        FindObjectOfType<RewardManager>().ClaimRewards();
        VictoryButton1.interactable = true;
        yield return new WaitForSeconds(3f);
        VictoryPanel.SetActive(false);
        leaveScene();
    }
    private List<RewardItem> CalculateRewards()
    {
        List<RewardItem> rewards = new List<RewardItem>();

        Debug.Log("Total Accumulated Damage for Reward Calculation: " + totalAccumulatedDamageToMonster);

        if (totalAccumulatedDamageToMonster >= 0 && totalAccumulatedDamageToMonster < 200)
        {
            rewards.Add(new RewardItem("Coins", 100));
            rewards.Add(new RewardItem("XP", 250));
        }
        else if (totalAccumulatedDamageToMonster >= 201 && totalAccumulatedDamageToMonster < 300)
        {
            rewards.Add(new RewardItem("Coins", 150));
            rewards.Add(new RewardItem("XP", 300));
        }
        else if (totalAccumulatedDamageToMonster >= 301 && totalAccumulatedDamageToMonster < 400)
        {
            rewards.Add(new RewardItem("Coins", 250));
            rewards.Add(new RewardItem("XP", 350));
        }
        else if (totalAccumulatedDamageToMonster >= 401 && totalAccumulatedDamageToMonster < 500)
        {
            rewards.Add(new RewardItem("Coins", 300));
            rewards.Add(new RewardItem("XP", 400));
        } else if (totalAccumulatedDamageToMonster >= 501 && totalAccumulatedDamageToMonster < 600)
        {
            rewards.Add(new RewardItem("Coins", 350));
            rewards.Add(new RewardItem("XP", 450));
        } else if (totalAccumulatedDamageToMonster >= 601)
        {
            rewards.Add(new RewardItem("Coins", 500));
            rewards.Add(new RewardItem("XP", 800));
        }
        return rewards;
    }

    public void CallVictory()
    {
        StartCoroutine(ShowVictoryAndRewards());
    }

    public void SetVictoryButtonTrue()
    {
        VictoryButton1.interactable = true;
    }
    public void skipAttackPhase()
    {
        EndAttackPhase();
    }

    public void skipDefensePhase()
    {
        EndDefensePhase();
    }
    public void AddDamage()
    {
        if (attackPhaseActive)
        {
            totalDamage += 100;
            totalAccumulatedDamageToMonster += 100;
            UpdateTotalDamageText(); 
        }
        else if (defensePhaseActive)
        {
            accumulatedDefenseSum += 100;

            if (100 <= bossDamage)
            {
                bossDamage -= 100;
                UpdatePhaseResultText(bossDamage); // Update UI for monster damage
            }
            else
            {
                int excessDamage = 100 - bossDamage; // Calculate excess damage
                totalDamage += excessDamage; // Add excess damage to totalDamage
                totalAccumulatedDamageToMonster += excessDamage;
                bossDamage = 0; // Monster damage reduced to 0

                UpdatePhaseResultText(bossDamage); // Update UI for monster damage
                UpdateTotalDamageText(); // Update UI for total damage
            }
        }
    }
}