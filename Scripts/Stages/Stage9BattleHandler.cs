using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase.Database;
using System.Collections.Generic;
using System;

public class Stage9BattleHandler : MonoBehaviour
{
    public DamageContentAnimHandler damageContentAnimHandler;
    public Animator clockAnimator;
    public Slider phaseSlider; 
    public GameObject itemDetailsPanelPrefab; 
    private GameObject activeItemDetailsPanel; 
    public InventoryUI inventoryUI;
    public GameObject loadingScreen;
    public Slider slider;
    public GameObject SettingPanel;
    public GameObject HealthPlayerBar;
    public GameObject HealthEnemyBar;
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
    [SerializeField] private GameObject monster;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private TMP_Text feedbackText;
    [SerializeField] private TMP_Text currentAnswerText;
    [SerializeField] private TMP_Text totalDamageText;
    [SerializeField] private TMP_Text monsterDamageText;
    [SerializeField] private TMP_Text playerHealthText;
    [SerializeField] private TMP_Text monsterHealthText;
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
    [SerializeField] private Button negativeButton;
    [SerializeField] private Button submitButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Button invButton;
    [SerializeField] private Button settingButton;

    [Header("Show Victory Panel")]
    public GameObject VictoryPanel;
    public Button VictoryButton1,VictoryButton2,VictoryButton3;

    [Header("Show Defeat Panel")]
    public GameObject DefeatPanel;
    public Button DefeatBtn1, DefeatBtn2;

     [Header("Show Defeat Panel")]
    public GameObject ConfirmConcedePanel;

    private int correctAnswers = 0;
    private int currentAnswer = 0;
    private int correctAnswer = 0;
    private int totalDamage = 0;
    private bool damagePotionUsed = false; 
    private bool isDamageBoostActive = false;
    private float damageMultiplier = 1.0f;
    private int accumulatedDefenseSum = 0;
    private float timeRemaining = 60f;
    private bool attackPhaseActive = false;
    private bool defensePhaseActive = false;
    private int monsterDamage = 400;
    private Monster monsterComponent;
    private PlayerSoloHeallth playerComponent;
    private GameObject instantiatedPlayer; 
    private Animator playerAnimator;
    private AudioManager audioManager;
    private Coroutine feedbackCoroutine;

    //For monitoring students
    private int totalAmountAnswer = 0;
    private int currentTime = 0;
    private bool isTimerRunning = false;

    // generate questions
    private int rangeMin;
    private int rangeMax;
    private string operators;
    private int numOperandsMin;
    private int numOperandsMax;

    void Start()
    {
        PlayerHealthInfo.SetActive(true);
        MonsterHealthInfo.SetActive(false);
        ReadyPanel.SetActive(true);
        VictoryPanel.SetActive(false);
        DefeatPanel.SetActive(false);
        monsterDamageText.gameObject.SetActive(false);
        feedbackText.gameObject.SetActive(false);
        monsterHealthText.gameObject.SetActive(false);
        playerHealthText.gameObject.SetActive(false);
        attackPhaseImage.SetActive(false);
        defensePhaseImage.SetActive(false);
        InitializeUIElements();

        audioManager = FindObjectOfType<AudioManager>();

        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene.");
            return;
        }

        BattleTrialCharacterLoader characterLoader = FindObjectOfType<BattleTrialCharacterLoader>();
        if (characterLoader != null)
        {
            instantiatedPlayer = characterLoader.GetInstantiatedPlayer();
            if (instantiatedPlayer != null)
            {
                playerAnimator = instantiatedPlayer.GetComponent<Animator>();
                if (playerAnimator != null)
                {
                    Debug.Log("Animator component found on instantiated player.");
                }
            }
        }
        if (instantiatedPlayer == null)
        {
            Debug.LogError("Instantiated Player is null");
            return; 
        }
        monsterComponent = monster.GetComponent<Monster>();
        playerComponent = instantiatedPlayer.GetComponent<PlayerSoloHeallth>();

        if (playerComponent == null || healthbar == null)
        {
            Debug.LogError("Player Health Bar or Player Component is null");
            return; 
        }
        enemyHealthBar.SetMaxHealth(monsterComponent.MaxHealth); 
        FetchMathSettings();
        playerComponent.OnHealthLoaded += InitializePlayerHealthUI;
        UpdateMonsterHealthText();
        DisableDigitButtons();
        UpdateTotalDamageText();
        UpdatePhaseResultText(monsterDamage);
    }
    private void FetchMathSettings()
    {
        // Default fallback values
        int defaultRangeMin = 1;
        int defaultRangeMax = 11;
        string defaultOperators = "*";
        int defaultNumOperandsMin = 2;
        int defaultNumOperandsMax = 2;

        // Fetching data from Firebase
        FirebaseDatabase.DefaultInstance.GetReference("MathSettings/Stage 9").GetValueAsync().ContinueWith(task =>
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
            healthbar.SetMaxHealth(playerComponent.MaxHealth); 
            healthbar.SetHealth(playerComponent.currentHealth);

            if (!playerHealthText.gameObject.activeSelf)
            {
                playerHealthText.gameObject.SetActive(true);
            }

            if (playerHealthText != null)
            {
                playerHealthText.text = playerComponent.currentHealth.ToString();
            }
        }
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
        if (monsterDamageText != null) monsterDamageText.text = "" + monsterDamage;
        if (monsterHealthText != null && monsterComponent != null) monsterHealthText.text = "" + monsterComponent.MaxHealth;
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
        else
        {
            Debug.LogError("Player animator is null. Cannot trigger attack animation.");
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
        PlayerHealthInfo.SetActive(true);
        MonsterHealthInfo.SetActive(true);
        feedbackText.gameObject.SetActive(true);
        monsterDamageText.gameObject.SetActive(true);
        monsterHealthText.gameObject.SetActive(true);
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
        monsterDamage = 400; 
        attackPhaseImage.SetActive(true);
        defensePhaseImage.SetActive(false);
        UpdatePhaseResultText(monsterDamage);
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
        monsterDamage = 400;
        defensePhaseImage.SetActive(true);
        attackPhaseImage.SetActive(false);
        UpdatePhaseResultText(monsterDamage); 
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
            int numOperands = UnityEngine.Random.Range(numOperandsMin, numOperandsMax);
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
            if (correctAnswer != 0)
            {
                isValid = true;

                // Update the question text
                if (questionText != null)
                {
                    questionText.text = operands[0].ToString();
                    for (int i = 1; i < numOperands; i++)
                    {
                        // Replace '*' with 'x' for display
                        char displayOperator = chosenOperators[i - 1] == '*' ? 'x' : chosenOperators[i - 1];
                        questionText.text += " " + displayOperator + " " + operands[i].ToString();
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
                currentAnswer = currentAnswer * 10 - digit; // Subtract digit to maintain the negative number
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
            totalAmountAnswer += 1;
            
            if (attackPhaseActive)
            {
                totalDamage += positiveCorrectAnswer;
                UpdateTotalDamageText(); // Update UI for total damage
            }
            else if (defensePhaseActive)
            {
                // Add the correct answer to accumulatedDefenseSum
                accumulatedDefenseSum += positiveCorrectAnswer;

                // In defense phase, reduce monster damage
                if (positiveCorrectAnswer <= monsterDamage)
                {
                    monsterDamage -= positiveCorrectAnswer;
                    UpdatePhaseResultText(monsterDamage); // Update UI for monster damage
                }
                else
                {
                    // If damage exceeds monster damage
                    int excessDamage = positiveCorrectAnswer - monsterDamage; // Calculate excess damage
                    totalDamage += excessDamage; // Add excess damage to totalDamage
                    monsterDamage = 0; // Monster damage reduced to 0

                    UpdatePhaseResultText(monsterDamage); // Update UI for monster damage
                    UpdateTotalDamageText(); // Update UI for total damage
                }
            }
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
        if (playerComponent.currentHealth > 0 && monsterComponent.currentHealth> 0)
        {
            StartCoroutine(StartPhaseWithCountdown("Attack Phase"));
        }
    }
    private void SetDigitButtonsInteractable(bool state)
    {
        digitButton0.interactable = state;
        digitButton1.interactable = state;
        digitButton2.interactable = state;
        digitButton3.interactable = state;
        digitButton4.interactable = state;
        digitButton5.interactable = state;
        digitButton6.interactable = state;
        digitButton7.interactable = state;
        digitButton8.interactable = state;
        digitButton9.interactable = state;
        submitButton.interactable = state;
        negativeButton.interactable = state;
        clearButton.interactable = state;
        deleteButton.interactable = state;
        invButton.interactable = state;
        settingButton.interactable = state;
    }
    private void DisableDigitButtons()
    {
        SetDigitButtonsInteractable(false);
    }
    private IEnumerator CalculateDefenseImpact()
    {
        yield return new WaitForSeconds(audioManager.TimesUp.length);
        // Set the monster damage to a fixed value
        int monsterDamage = 400;
        
        // Initialize damage to the full monster damage
        int damageToPlayer = monsterDamage;
        int excessDamage = 0; // Initialize excessDamage variable

        // If accumulated defense is greater than or equal to monster damage, 
        // set damage to player to 0 (defense absorbs all damage)
        if (accumulatedDefenseSum >= monsterDamage)
        {
            damageToPlayer = 0;  // No damage to the player, defense absorbed it

            // Calculate excess damage (if defense exceeds monster damage)
            excessDamage = accumulatedDefenseSum - monsterDamage;
            if (excessDamage > 0)
            {
                StartCoroutine(DisplayExcessDamageToMonster(excessDamage));  // Apply excess damage to the monster
            }
        }
        else
        {
            // If accumulated defense is less than monster damage, 
            // player takes the remaining damage (monster damage - defense)
            damageToPlayer = monsterDamage - accumulatedDefenseSum;
        }

        // Log for debugging
        Debug.Log($"Monster Damage: {monsterDamage}, Accumulated Defense: {accumulatedDefenseSum}, Damage to Player: {damageToPlayer}");

        // Apply the damage to the player
        if (damageToPlayer > 0)
        {
            monsterComponent.GetComponent<Animator>().SetTrigger("isAttack");
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
                monsterComponent.GetComponent<Animator>().SetTrigger("isAttack"); 
            }
        }
    }

    private IEnumerator DisplayExcessDamageToMonster(int excessDamage)
    {
        // Apply excess damage to the monster if player defense exceeds monster damage
        TriggerAttackAnim();
        audioManager.triggerAttack();
        yield return new WaitForSeconds(1f);
        monsterComponent.TakeDamage(excessDamage);
        Debug.Log($"Monster takes {excessDamage} excess damage. Current Health: {monsterComponent.currentHealth}");
        UpdateMonsterHealthText();

        // Check if the monster is defeated
        if (monsterComponent.currentHealth <= 0)
        {
            Debug.Log("Monster Defeated! Victory.");
            StartCoroutine(CheckStageClearedAndHandleVictory());
            DisableDigitButtons();
            UpdateStageProgress();
        }
    }
    private void UpdatePlayerHealthText()
    {
        if (playerHealthText != null && playerComponent != null)
        {
            playerHealthText.text = "" + playerComponent.currentHealth;
        }
    }
    private void UpdateMonsterHealthText()
    {
        if (monsterHealthText != null && monsterComponent != null)
        {
            monsterHealthText.text = "" + monsterComponent.currentHealth;
            enemyHealthBar.SetHealth(monsterComponent.currentHealth); 
        }
    }
    private void CheckPlayerHealth()
    {
        if (playerComponent.currentHealth <= 0)
        {
            Debug.Log("Player is dead. Game Over.");
            TriggerDeadAnim();
            StartCoroutine(HandleDefeatSequence());
            RecordTotalAttempt();
        }
    }
    private IEnumerator DelayedApplyDamage()
    {
        yield return new WaitForSeconds(1.0f); // Wait for 1 second (adjust this duration as needed)
        RandomApplyDamageAnim();
    }
    void RandomApplyDamageAnim()
    {
        int randomValue = UnityEngine.Random.Range(0, 2); // Generates 0 or 1

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
        if (monster != null)
        {
            Monster monsterComponent = monster.GetComponent<Monster>();
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
                    monsterComponent.TakeDamage(finalDamage); // Apply total damage accumulated
                    UpdateMonsterHealthText();

                    // Check if the monster is defeated
                    if (monsterComponent.currentHealth <= 0)
                    {
                        Debug.Log("Monster Defeated! Victory.");
                        StartCoroutine(CheckStageClearedAndHandleVictory());
                        DisableDigitButtons();
                        UpdateStageProgress();
                        yield break; // Exit the coroutine as the monster is defeated
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
        if (monster != null)
        {
            Monster monsterComponent = monster.GetComponent<Monster>();
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
                    monsterComponent.TakeDamage(finalDamage); // Apply total damage accumulated
                    UpdateMonsterHealthText();

                    // Check if the monster is defeated
                    if (monsterComponent.currentHealth <= 0)
                    {
                        Debug.Log("Monster Defeated! Victory.");
                        StartCoroutine(CheckStageClearedAndHandleVictory());
                        DisableDigitButtons();
                        UpdateStageProgress();
                        yield break; // Exit the coroutine as the monster is defeated
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
    public void OnClickReady()
    {
        onPressedStart();
        ReadyPanel.SetActive(false);
        damageContentAnimHandler.OpenContent();

        // Start the timer
        StartTimer();
    }

    private void StartTimer()
    {
        // Start counting time when ready button is pressed
        isTimerRunning = true;
        currentTime = 0;

        // Optionally, use a coroutine to track time
        StartCoroutine(TrackTime());
    }

    private IEnumerator TrackTime()
    {
        while (isTimerRunning)
        {
            yield return new WaitForSeconds(1);  // Count every second
            currentTime++;
        }
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
    private void SetVictoryButtonInteractable(bool state)
    {
        VictoryButton1.interactable = state;
        VictoryButton2.interactable = state;
        VictoryButton3.interactable = state;
    }
    private void UpdateStageProgress()
    {
        int currentStage = 9; // Set this to your actual current stage
        FirebaseDatabase database = FirebaseManager.Instance.Database;
        string userId = FirebaseManager.Instance.Auth.CurrentUser.UserId;
        string currentStageKey = currentStage.ToString("D2");
        string nextStageKey = (currentStage + 1).ToString("D2");
        DatabaseReference databaseReference = database.RootReference;

        // Check if the current stage is already cleared
        databaseReference.Child("users").Child(userId).Child("stages").Child("stage" + currentStageKey).Child("cleared").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted && task.Result.Exists && task.Result.Value != null && (bool)task.Result.Value)
            {
                Debug.Log("Stage " + currentStage + " has already been cleared. No update needed.");
            }
            else
            {
                // Mark current stage as cleared
                databaseReference.Child("users").Child(userId).Child("stages").Child("stage" + currentStageKey).Child("cleared").SetValueAsync(true);

                // Unlock next stage, if it exists
                if (currentStage + 1 <= 20) // Assuming stage 20 is the last stage
                {
                    databaseReference.Child("users").Child(userId).Child("stages").Child("stage" + nextStageKey).Child("unlocked").SetValueAsync(true);
                }

                // Increment stagesReached if first time clearing this stage
                databaseReference.Child("users").Child(userId).Child("stagesReached").GetValueAsync().ContinueWith(stagesTask =>
                {
                    if (stagesTask.IsCompleted)
                    {
                        int stagesReached = 0;
                        if (stagesTask.Result.Exists && stagesTask.Result.Value != null)
                        {
                            stagesReached = Convert.ToInt32(stagesTask.Result.Value);
                        }

                        // Increment stagesReached only if it's the first time clearing the stage
                        if (currentStage > stagesReached)
                        {
                            databaseReference.Child("users").Child(userId).Child("stagesReached").SetValueAsync(currentStage);
                            Debug.Log("First time clearing stage " + currentStage + ". stagesReached updated to " + currentStage);
                        }
                    }
                });
                Debug.Log("Stage " + currentStage + " cleared and stage " + (currentStage + 1) + " unlocked.");
            }
        });
    }

    private IEnumerator CheckStageClearedAndHandleVictory()
    {
        bool isStageCleared = false;
        yield return CheckStageCleared((result) => isStageCleared = result);

        // Pass the stage cleared status to HandleVictorySequence
        StartCoroutine(HandleVictorySequence(isStageCleared));
    }

    private IEnumerator HandleVictorySequence(bool isStageCleared)
    {
        VictoryPanel.SetActive(true);
        SetBoolTrueWin();
        audioManager.TriggerWinSound();
        SetVictoryButtonInteractable(false);
        RecordTotalAttempt();
        UpdateHighestAnswers(totalAmountAnswer);
        OnComplete();

        yield return new WaitForSeconds(audioManager.WinSound.length);

        int xpReward = isStageCleared ? 65 : 420; 
        int coinReward = isStageCleared ? 30 : 260; 

        // Add both XP and Coin rewards
        List<RewardItem> rewards = new List<RewardItem> 
        {
            new RewardItem("XP", xpReward),
            new RewardItem("Coins", coinReward)
        };
        if (!isStageCleared)
        {
            int gemsReward = 2;
            rewards.Add(new RewardItem("Gems", gemsReward));
        }
        FindObjectOfType<RewardManager>().ShowRewards(rewards);
        audioManager.PlayRewardSound();
    }

    private IEnumerator CheckStageCleared(System.Action<bool> callback)
    {
        int currentStage = 9; 
        FirebaseDatabase database = FirebaseManager.Instance.Database;
        string userId = FirebaseManager.Instance.Auth.CurrentUser.UserId;
        string currentStageKey = currentStage.ToString("D2");
        DatabaseReference databaseReference = database.RootReference.Child("users").Child(userId).Child("stages").Child("stage" + currentStageKey).Child("cleared");

        // Check if the current stage is cleared
        bool isCleared = false;
        var task = databaseReference.GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Result.Exists && task.Result.Value != null)
        {
            isCleared = (bool)task.Result.Value;
        }

        callback(isCleared);
    }
    public void OpenVictoryButtons()
    {
        SetVictoryButtonInteractable(true);
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
            
        StartCoroutine(HandleDefeatSequence());
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
    public void RepeatBattle(string SceneName)
    {
         StartCoroutine(RepeatBattleWithDelay(SceneName));
    }

    private IEnumerator RepeatBattleWithDelay(string SceneName)
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneName);
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
    public void skipAttackPhase()
    {
        EndAttackPhase();
    }

    public void skipDefensePhase()
    {
        EndDefensePhase();
    }

    public void forceVictory()
    {
        // Stop any active phases
        attackPhaseActive = false;
        defensePhaseActive = false;
        
        // Stop the timer
        if (timerText != null){
            timerText.text = "0";
            phaseSlider.value = 0;
        }
            StartCoroutine(CheckStageClearedAndHandleVictory());
            DisableDigitButtons();
            UpdateStageProgress();
    }
    private void SetDefeatButtonsInteractable(bool state)
    {
        DefeatBtn1.interactable = state;
        DefeatBtn2.interactable = state;
    }
    private IEnumerator HandleDefeatSequence()
    {
        DefeatPanel.SetActive(true);
        DisableDigitButtons();
        SetDefeatButtonsInteractable(false);
        audioManager.triggerGameoverSFX();
        yield return new WaitForSeconds(audioManager.GameoverSFX.length);
        SetDefeatButtonsInteractable(true);
        yield return new WaitForSeconds(0.2f);
        StopAllCoroutines();
    }

    public void AddDamage()
    {
        if (attackPhaseActive)
        {
            // In attack phase, accumulate total damage
            totalDamage += 400;
            UpdateTotalDamageText(); // Update UI for total damage
        }
        else if (defensePhaseActive)
        {
            // Add the correct answer to accumulatedDefenseSum
            accumulatedDefenseSum += 400;

            // In defense phase, reduce monster damage
            if (400 <= monsterDamage)
            {
                monsterDamage -= 400;
                UpdatePhaseResultText(monsterDamage); // Update UI for monster damage
            }
            else
            {
                // If damage exceeds monster damage
                int excessDamage = 400 - monsterDamage; // Calculate excess damage
                totalDamage += excessDamage; // Add excess damage to totalDamage
                monsterDamage = 0; // Monster damage reduced to 0
                UpdatePhaseResultText(monsterDamage); // Update UI for monster damage
                UpdateTotalDamageText(); // Update UI for total damage
            }
        }
    } 

    public void RecordTotalAttempt()
    {
        // Get the Firebase database and current user ID
        FirebaseDatabase database = FirebaseManager.Instance.Database;
        string userId = FirebaseManager.Instance.Auth.CurrentUser.UserId;

        // Directly specify the stage key ("stage09")
        DatabaseReference totalAttemptRef = database
            .RootReference
            .Child("users")
            .Child(userId)
            .Child("stages")
            .Child("stage09")
            .Child("totalAttempt");

        // Retrieve the current value of totalAttempt
        totalAttemptRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                // Handle any errors during the task
                Debug.LogError("Error retrieving totalAttempt: " + task.Exception);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                // Parse the current value or set it to 0 if it doesn't exist
                int currentAttempt = snapshot.Exists ? Convert.ToInt32(snapshot.Value) : 0;

                // Increment the value by 1
                int updatedAttempt = currentAttempt + 1;

                // Update the value in the database
                totalAttemptRef.SetValueAsync(updatedAttempt).ContinueWith(updateTask =>
                {
                    if (updateTask.IsFaulted)
                    {
                        // Handle any errors during the update
                        Debug.LogError("Error updating totalAttempt: " + updateTask.Exception);
                    }
                    else if (updateTask.IsCompleted)
                    {
                        Debug.Log("TotalAttempt updated successfully to: " + updatedAttempt);
                    }
                });
            }
        });
    }

    public void UpdateHighestAnswers(int totalAmountAnswer)
    {
        // Get the Firebase database and current user ID
        FirebaseDatabase database = FirebaseManager.Instance.Database;
        string userId = FirebaseManager.Instance.Auth.CurrentUser.UserId;

        // References to the necessary fields
        DatabaseReference stageRef = database
            .RootReference
            .Child("users")
            .Child(userId)
            .Child("stages")
            .Child("stage09");

        DatabaseReference highestAnswerRef = stageRef.Child("highestTotalAnswer");
        DatabaseReference firstPreviousRef = stageRef.Child("firstPreviousHighestTotalAnswer");
        DatabaseReference secondPreviousRef = stageRef.Child("secondPreviousHighestTotalAnswer");
        DatabaseReference thirdPreviousRef = stageRef.Child("thirdPreviousHighestTotalAnswer");
        DatabaseReference fourthPreviousRef = stageRef.Child("fourthPreviousHighestTotalAnswer");

        // Retrieve the current highestTotalAnswer
        highestAnswerRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                // Handle errors
                Debug.LogError("Error retrieving highestTotalAnswer: " + task.Exception);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                // Parse the current highest value or set it to 0 if it doesn't exist
                int highestTotalAnswer = snapshot.Exists ? Convert.ToInt32(snapshot.Value) : 0;

                // Check if the totalAmountAnswer is greater
                if (totalAmountAnswer > highestTotalAnswer)
                {
                    // Begin updating the hierarchy
                    fourthPreviousRef.SetValueAsync(thirdPreviousRef.GetValueAsync().Result?.Value ?? 0).ContinueWith(t4 =>
                    {
                        thirdPreviousRef.SetValueAsync(secondPreviousRef.GetValueAsync().Result?.Value ?? 0).ContinueWith(t3 =>
                        {
                            secondPreviousRef.SetValueAsync(firstPreviousRef.GetValueAsync().Result?.Value ?? 0).ContinueWith(t2 =>
                            {
                                firstPreviousRef.SetValueAsync(highestTotalAnswer).ContinueWith(t1 =>
                                {
                                    // Update the highestTotalAnswer
                                    highestAnswerRef.SetValueAsync(totalAmountAnswer).ContinueWith(updateTask =>
                                    {
                                        if (updateTask.IsFaulted)
                                        {
                                            Debug.LogError("Error updating highestTotalAnswer: " + updateTask.Exception);
                                        }
                                        else if (updateTask.IsCompleted)
                                        {
                                            Debug.Log("highestTotalAnswer updated successfully!");
                                        }
                                    });
                                });
                            });
                        });
                    });
                }
                else
                {
                    Debug.Log("totalAmountAnswer is not greater than the current highestTotalAnswer.");
                }
            }
        });
    }

    public void OnComplete()
{
    // Stop the timer
    isTimerRunning = false;

    // Call the function to update the shortest time
    UpdateShortestTime(currentTime);
}

public void UpdateShortestTime(int time)
{
    // Get the Firebase database and current user ID
    FirebaseDatabase database = FirebaseManager.Instance.Database;
    string userId = FirebaseManager.Instance.Auth.CurrentUser.UserId;

    // References to the necessary fields
    DatabaseReference stageRef = database
        .RootReference
        .Child("users")
        .Child(userId)
        .Child("stages")
        .Child("stage09");

    DatabaseReference shortestTimeRef = stageRef.Child("shortestTime");
    DatabaseReference firstPreviousRef = stageRef.Child("firstPreviousShortestTime");
    DatabaseReference secondPreviousRef = stageRef.Child("secondPreviousShortestTime");
    DatabaseReference thirdPreviousRef = stageRef.Child("thirdPreviousShortestTime");
    DatabaseReference fourthPreviousRef = stageRef.Child("fourthPreviousShortestTime");

    // Retrieve the current shortestTime
    shortestTimeRef.GetValueAsync().ContinueWith(task =>
    {
        if (task.IsFaulted)
        {
            // Handle errors
            Debug.LogError("Error retrieving shortestTime: " + task.Exception);
            return;
        }

        if (task.IsCompleted)
        {
            DataSnapshot snapshot = task.Result;

            // Parse the current shortest value or treat it as "none" if it doesn't exist
            string currentShortestTime = snapshot.Exists ? snapshot.Value.ToString() : "none";

            if (currentShortestTime == "none" || time < Convert.ToInt32(currentShortestTime))
            {
                // Begin updating the hierarchy for previous times
                fourthPreviousRef.SetValueAsync(thirdPreviousRef.GetValueAsync().Result?.Value ?? int.MaxValue).ContinueWith(t4 =>
                {
                    thirdPreviousRef.SetValueAsync(secondPreviousRef.GetValueAsync().Result?.Value ?? int.MaxValue).ContinueWith(t3 =>
                    {
                        secondPreviousRef.SetValueAsync(firstPreviousRef.GetValueAsync().Result?.Value ?? int.MaxValue).ContinueWith(t2 =>
                        {
                            firstPreviousRef.SetValueAsync(currentShortestTime).ContinueWith(t1 =>
                            {
                                // Update the shortestTime
                                shortestTimeRef.SetValueAsync(time).ContinueWith(updateTask =>
                                {
                                    if (updateTask.IsFaulted)
                                    {
                                        Debug.LogError("Error updating shortestTime: " + updateTask.Exception);
                                    }
                                    else if (updateTask.IsCompleted)
                                    {
                                        Debug.Log("shortestTime updated successfully!");
                                    }
                                });
                            });
                        });
                    });
                });
            }
            else
            {
                Debug.Log("New time is not shorter than the current shortest time.");
            }
        }
    });
}
}
