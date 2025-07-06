using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase.Database;
using System.Collections.Generic;
using System;
public class Stage1BattleHandler : MonoBehaviour
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
    [Header("Freeze Time Animation & Panel")]
    [SerializeField] private GameObject pausePanel; 
    [SerializeField] private Animator pausePanelAnimator;
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
    [Header("Buttons")]
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
    private bool isDamageBoostActive = false;
    private float damageMultiplier = 1.0f;
    private int accumulatedDefenseSum = 0;
    private float timeRemaining = 60f;
    private bool attackPhaseActive = false;
    private bool defensePhaseActive = false;
    private bool damagePotionUsed = false;
    private bool isTimerPaused = false;
    private int monsterDamage = 150;
    private Coroutine feedbackCoroutine;
    private Monster monsterComponent;
    private PlayerSoloHeallth playerComponent;
    private GameObject instantiatedPlayer; 
    private Animator playerAnimator;
    private AudioManager audioManager;
    private int totalAmountAnswer = 0;
    private int currentTime = 0;
    private bool isTimerRunning = false;
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
        playerHealthText.gameObject.SetActive(true);
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
        int defaultRangeMin = 1;
        int defaultRangeMax = 12;
        string defaultOperators = "+";
        int defaultNumOperandsMin = 2;
        int defaultNumOperandsMax = 2;
        FirebaseDatabase.DefaultInstance.GetReference("MathSettings/Stage 1").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted && task.Result.Exists)
            {
                DataSnapshot snapshot = task.Result;
                rangeMin = snapshot.Child("rangeMin").Exists ? int.Parse(snapshot.Child("rangeMin").Value.ToString()) : defaultRangeMin;
                rangeMax = snapshot.Child("rangeMax").Exists ? int.Parse(snapshot.Child("rangeMax").Value.ToString()) : defaultRangeMax;
                operators = snapshot.Child("operators").Exists ? snapshot.Child("operators").Value.ToString() : defaultOperators;
                numOperandsMin = snapshot.Child("numOperandsMin").Exists ? int.Parse(snapshot.Child("numOperandsMin").Value.ToString()) : defaultNumOperandsMin;
                numOperandsMax = snapshot.Child("numOperandsMax").Exists ? int.Parse(snapshot.Child("numOperandsMax").Value.ToString()) : defaultNumOperandsMax;
            }
            else
            {
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
        SetDigitButtonsInteractable(true);
    }
    private IEnumerator StartPhaseWithCountdown(string phase)
    {
        if (countdownPanel != null)
        {
            countdownPanel.SetActive(true);
        }
        if (phaseCountdownText != null)
        {
            phaseCountdownText.text = phase;
        }
        yield return new WaitForSeconds(1.5f); 
        if (phaseCountdownText != null)
        {
            phaseCountdownText.text = "Starting in";
        }
        yield return new WaitForSeconds(1f);
        audioManager.playCountdown();
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
        if (phaseCountdownText != null)
        {
            phaseCountdownText.text = "Go!";
        }
        yield return new WaitForSeconds(0.5f);
        if (countdownPanel != null)
        {
            countdownPanel.SetActive(false);
        }
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
        timeRemaining = 60f;
        correctAnswers = 0;
        totalDamage = 0; 
        monsterDamage = 150; 
        damageMultiplier = 1.0f;
        isDamageBoostActive = false;
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
        timeRemaining = 60f;
        accumulatedDefenseSum = 0; 
        SetDigitButtonsInteractable(true);
        monsterDamage = 150;
        damageMultiplier = 1.0f;
        isDamageBoostActive = false;
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
            int numOperands = UnityEngine.Random.Range(numOperandsMin, numOperandsMax + 1);
            int[] operands = new int[numOperands];
            char[] chosenOperators = new char[numOperands - 1];
            for (int i = 0; i < numOperands; i++)
            {
                operands[i] = UnityEngine.Random.Range(rangeMin, rangeMax);
            }
            for (int i = 0; i < chosenOperators.Length; i++)
            {
                chosenOperators[i] = operators[UnityEngine.Random.Range(0, operators.Length)];
            }
            for (int i = 1; i < numOperands; i++)
            {
                if (chosenOperators[i - 1] == '/')
                {
                    operands[i] = UnityEngine.Random.Range(1, 10);
                    operands[i - 1] = operands[i] * UnityEngine.Random.Range(1, 10);
                }
            }
            List<int> nums = new List<int>(operands);
            List<char> ops = new List<char>(chosenOperators);
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
            correctAnswer = nums[0];
            for (int i = 0; i < ops.Count; i++)
            {
                correctAnswer = ops[i] == '+' ? correctAnswer + nums[i + 1] : correctAnswer - nums[i + 1];
            }
            if (correctAnswer > 0)
            {
                isValid = true;
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
        int boostedDamage = Mathf.RoundToInt(totalDamage * damageMultiplier);  
        if (totalDamageText != null)
        {
            totalDamageText.text = "" + boostedDamage;  
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
            correctAnswers++;
            totalAmountAnswer += 1;
            if (attackPhaseActive)
            {
                totalDamage += correctAnswer;
                UpdateTotalDamageText();
            }
            else if (defensePhaseActive)
            {
                accumulatedDefenseSum += correctAnswer;
                if (correctAnswer <= monsterDamage)
                {
                    monsterDamage -= correctAnswer;
                    UpdatePhaseResultText(monsterDamage); 
                }
                else
                {
                    int excessDamage = correctAnswer - monsterDamage; 
                    totalDamage += excessDamage; 
                    monsterDamage = 0; 
                    UpdatePhaseResultText(monsterDamage); 
                    UpdateTotalDamageText(); 
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
            feedbackText.text = message;
            feedbackText.color = color;
            if (feedbackCoroutine != null)
            {
                StopCoroutine(feedbackCoroutine);
            }
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
        feedbackCoroutine = null; 
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
        int monsterDamage = 150;
        int damageToPlayer = monsterDamage;
        int excessDamage = 0; 
        if (accumulatedDefenseSum >= monsterDamage)
        {
            damageToPlayer = 0;  
            excessDamage = accumulatedDefenseSum - monsterDamage;
            if (excessDamage > 0)
            {
                StartCoroutine(DisplayExcessDamageToMonster(excessDamage)); 
            }
        }
        else
        {
            damageToPlayer = monsterDamage - accumulatedDefenseSum;
        }
        if (damageToPlayer > 0)
        {
            monsterComponent.GetComponent<Animator>().SetTrigger("isAttack");
            yield return new WaitForSeconds(0.4f);
            TriggerDefPlayer();
            audioManager.getHit();
            playerComponent.TakeDamage(damageToPlayer);
            healthbar.SetHealth(playerComponent.currentHealth);
            UpdatePlayerHealthText();
            CheckPlayerHealth();
        }
        else
        {
            if (excessDamage == 0)
            {
                TriggerDefend1(); 
                audioManager.getDefend();
                yield return new WaitForSeconds(1f);
                monsterComponent.GetComponent<Animator>().SetTrigger("isAttack"); 
            }
        }
    }
    private IEnumerator DisplayExcessDamageToMonster(int excessDamage)
    {
        TriggerAttackAnim();
        audioManager.triggerAttack();
        yield return new WaitForSeconds(1f);
        monsterComponent.TakeDamage(excessDamage);
        UpdateMonsterHealthText();
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
        yield return new WaitForSeconds(1.0f); 
        RandomApplyDamageAnim();
    }
    void RandomApplyDamageAnim()
    {
        int randomValue = UnityEngine.Random.Range(0, 2);
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
                int finalDamage = Mathf.RoundToInt(totalDamage * damageMultiplier);
                if (finalDamage > 0)
                {
                    TriggerAttackAnim1();
                    audioManager.triggerAttack1();
                    yield return new WaitForSeconds(1f);
                    monsterComponent.TakeDamage(finalDamage); 
                    UpdateMonsterHealthText();
                    if (monsterComponent.currentHealth <= 0)
                    {
                        Debug.Log("Monster Defeated! Victory.");
                        StartCoroutine(CheckStageClearedAndHandleVictory());
                        DisableDigitButtons();
                        UpdateStageProgress();
                        yield break;
                    }
                }
                else
                {
                    TriggerNoDamageAnim();
                    audioManager.triggerNoDamage();
                }
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
                int finalDamage = Mathf.RoundToInt(totalDamage * damageMultiplier);
                if (finalDamage > 0)
                {
                    TriggerAttackAnim();
                    audioManager.triggerAttack();
                    yield return new WaitForSeconds(1f);
                    monsterComponent.TakeDamage(finalDamage); 
                    UpdateMonsterHealthText();
                    if (monsterComponent.currentHealth <= 0)
                    {
                        Debug.Log("Monster Defeated! Victory.");
                        StartCoroutine(CheckStageClearedAndHandleVictory());
                        DisableDigitButtons();
                        UpdateStageProgress();
                        yield break; 
                    }
                }
                else
                {
                    TriggerNoDamageAnim();
                    audioManager.triggerNoDamage();
                }
                yield return new WaitForSeconds(3f);
                StartCoroutine(StartPhaseWithCountdown("Defense Phase"));
            }
        }
    }
    private void UpdateStageProgress()
    {
        int currentStage = 1; 
        FirebaseDatabase database = FirebaseManager.Instance.Database;
        string userId = FirebaseManager.Instance.Auth.CurrentUser.UserId;
        string currentStageKey = currentStage.ToString("D2");
        string nextStageKey = (currentStage + 1).ToString("D2");
        DatabaseReference databaseReference = database.RootReference;
        databaseReference.Child("users").Child(userId).Child("stages").Child("stage" + currentStageKey).Child("cleared").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted && task.Result.Exists && task.Result.Value != null && (bool)task.Result.Value)
            {
                Debug.Log("Stage " + currentStage + " has already been cleared. No update needed.");
            }
            else
            {
                databaseReference.Child("users").Child(userId).Child("stages").Child("stage" + currentStageKey).Child("cleared").SetValueAsync(true);
                if (currentStage + 1 <= 21) 
                {
                    databaseReference.Child("users").Child(userId).Child("stages").Child("stage" + nextStageKey).Child("unlocked").SetValueAsync(true);
                }
                databaseReference.Child("users").Child(userId).Child("stagesReached").GetValueAsync().ContinueWith(stagesTask =>
                {
                    if (stagesTask.IsCompleted)
                    {
                        int stagesReached = 0;
                        if (stagesTask.Result.Exists && stagesTask.Result.Value != null)
                        {
                            stagesReached = Convert.ToInt32(stagesTask.Result.Value);
                        }
                        if (currentStage > stagesReached)
                        {
                            databaseReference.Child("users").Child(userId).Child("stagesReached").SetValueAsync(currentStage);
                        }
                    }
                });
            }
        });
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
    public void OnClickReady()
    {
        onPressedStart();
        ReadyPanel.SetActive(false);
        damageContentAnimHandler.OpenContent();
        StartTimer();
    }
    private void StartTimer()
    {
        isTimerRunning = true;
        currentTime = 0;
        StartCoroutine(TrackTime());
    }
    private IEnumerator TrackTime()
    {
        while (isTimerRunning)
        {
            yield return new WaitForSeconds(1); 
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
        if (activeItemDetailsPanel != null)
        {
            Destroy(activeItemDetailsPanel);
        }
        activeItemDetailsPanel = Instantiate(itemDetailsPanelPrefab, transform);
        Button useButton = activeItemDetailsPanel.transform.Find("UseButton")?.GetComponent<Button>();
        if (useButton != null)
        {
            useButton.onClick.AddListener(() => UsePotionFromInventory("Red Potion"));
        }
    }
    private void SetVictoryButtonInteractable(bool state)
    {
        VictoryButton1.interactable = state;
        VictoryButton2.interactable = state;
        VictoryButton3.interactable = state;
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
        int xpReward = isStageCleared ? 40 : 300; 
        int coinReward = isStageCleared ? 25 : 200; 
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
        int currentStage = 1; 
        FirebaseDatabase database = FirebaseManager.Instance.Database;
        string userId = FirebaseManager.Instance.Auth.CurrentUser.UserId;
        string currentStageKey = currentStage.ToString("D2");
        DatabaseReference databaseReference = database.RootReference.Child("users").Child(userId).Child("stages").Child("stage" + currentStageKey).Child("cleared");
        bool isCleared = false;
        var task = databaseReference.GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);
        if (task.Result.Exists && task.Result.Value != null)
        {
            isCleared = (bool)task.Result.Value;
        }
        callback(isCleared);
    }
    private IEnumerator CheckStageClearedAndHandleVictory()
    {
        bool isStageCleared = false;
        yield return CheckStageCleared((result) => isStageCleared = result);
        StartCoroutine(HandleVictorySequence(isStageCleared));
    }
    public void OpenVictoryButtons()
    {
        SetVictoryButtonInteractable(true);
    }
    public void ConcedeBattle()
    {
        attackPhaseActive = false;
        defensePhaseActive = false;
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
    public void ApplyDamageBoost()
    {
        if (attackPhaseActive && !isDamageBoostActive)
        {
            damageMultiplier = 1.2f;  
            isDamageBoostActive = true;
            UpdateTotalDamageText(); 
        }
    }
    public bool IsAttackPhaseActive()
    {
        return attackPhaseActive;
    }
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
        attackPhaseActive = false;
        defensePhaseActive = false;
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
            totalDamage += 150;
            UpdateTotalDamageText(); 
        }
        else if (defensePhaseActive)
        {
            accumulatedDefenseSum += 150;
            if (150 <= monsterDamage)
            {
                monsterDamage -= 150;
                UpdatePhaseResultText(monsterDamage); 
            }
            else
            {
                int excessDamage = 150 - monsterDamage;
                totalDamage += excessDamage; 
                monsterDamage = 0; 
                UpdatePhaseResultText(monsterDamage); 
                UpdateTotalDamageText(); 
            }
        }
    } 
    public void RecordTotalAttempt()
    {
        FirebaseDatabase database = FirebaseManager.Instance.Database;
        string userId = FirebaseManager.Instance.Auth.CurrentUser.UserId;
        DatabaseReference totalAttemptRef = database
            .RootReference
            .Child("users")
            .Child(userId)
            .Child("stages")
            .Child("stage01")
            .Child("totalAttempt");
        totalAttemptRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error retrieving totalAttempt: " + task.Exception);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                int currentAttempt = snapshot.Exists ? Convert.ToInt32(snapshot.Value) : 0;
                int updatedAttempt = currentAttempt + 1;
                totalAttemptRef.SetValueAsync(updatedAttempt).ContinueWith(updateTask =>
                {
                    if (updateTask.IsFaulted)
                    {
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
        FirebaseDatabase database = FirebaseManager.Instance.Database;
        string userId = FirebaseManager.Instance.Auth.CurrentUser.UserId;
        DatabaseReference stageRef = database
            .RootReference
            .Child("users")
            .Child(userId)
            .Child("stages")
            .Child("stage01");
        DatabaseReference highestAnswerRef = stageRef.Child("highestTotalAnswer");
        DatabaseReference firstPreviousRef = stageRef.Child("firstPreviousHighestTotalAnswer");
        DatabaseReference secondPreviousRef = stageRef.Child("secondPreviousHighestTotalAnswer");
        DatabaseReference thirdPreviousRef = stageRef.Child("thirdPreviousHighestTotalAnswer");
        DatabaseReference fourthPreviousRef = stageRef.Child("fourthPreviousHighestTotalAnswer");
        highestAnswerRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error retrieving highestTotalAnswer: " + task.Exception);
                return;
            }
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                int highestTotalAnswer = snapshot.Exists ? Convert.ToInt32(snapshot.Value) : 0;
                if (totalAmountAnswer > highestTotalAnswer)
                {
                    fourthPreviousRef.SetValueAsync(thirdPreviousRef.GetValueAsync().Result?.Value ?? 0).ContinueWith(t4 =>
                    {
                        thirdPreviousRef.SetValueAsync(secondPreviousRef.GetValueAsync().Result?.Value ?? 0).ContinueWith(t3 =>
                        {
                            secondPreviousRef.SetValueAsync(firstPreviousRef.GetValueAsync().Result?.Value ?? 0).ContinueWith(t2 =>
                            {
                                firstPreviousRef.SetValueAsync(highestTotalAnswer).ContinueWith(t1 =>
                                {
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
        isTimerRunning = false;
        UpdateShortestTime(currentTime);
    }

    public void UpdateShortestTime(int time)
    {
        FirebaseDatabase database = FirebaseManager.Instance.Database;
        string userId = FirebaseManager.Instance.Auth.CurrentUser.UserId;
        DatabaseReference stageRef = database
            .RootReference
            .Child("users")
            .Child(userId)
            .Child("stages")
            .Child("stage01");
        DatabaseReference shortestTimeRef = stageRef.Child("shortestTime");
        DatabaseReference firstPreviousRef = stageRef.Child("firstPreviousShortestTime");
        DatabaseReference secondPreviousRef = stageRef.Child("secondPreviousShortestTime");
        DatabaseReference thirdPreviousRef = stageRef.Child("thirdPreviousShortestTime");
        DatabaseReference fourthPreviousRef = stageRef.Child("fourthPreviousShortestTime");
        shortestTimeRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error retrieving shortestTime: " + task.Exception);
                return;
            }
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string currentShortestTime = snapshot.Exists ? snapshot.Value.ToString() : "none";
                if (currentShortestTime == "none" || time < Convert.ToInt32(currentShortestTime))
                {
                    fourthPreviousRef.SetValueAsync(thirdPreviousRef.GetValueAsync().Result?.Value ?? int.MaxValue).ContinueWith(t4 =>
                    {
                        thirdPreviousRef.SetValueAsync(secondPreviousRef.GetValueAsync().Result?.Value ?? int.MaxValue).ContinueWith(t3 =>
                        {
                            secondPreviousRef.SetValueAsync(firstPreviousRef.GetValueAsync().Result?.Value ?? int.MaxValue).ContinueWith(t2 =>
                            {
                                firstPreviousRef.SetValueAsync(currentShortestTime).ContinueWith(t1 =>
                                {
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