using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase.Database;
using System.Collections.Generic;
using Firebase.Extensions;
public class EndlessModeBattleHandler : MonoBehaviour
{
    public DamageContentAnimHandler damageContentAnimHandler;
    public GameObject itemDetailsPanelPrefab; 
    public Animator clockAnimator;
    public Slider phaseSlider; 
    [SerializeField] private TMP_Text timerText;
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
    [Header("Show Defeat Panel")]
    public GameObject DefeatPanel;
    public Button DefeatButton1, DefeatButton2;
    [Header("Show Concede Defeat Panel")]
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
    private int maxDamage = 600;   // Maximum damage for the current monster
    private int currentDamage;    // Current damage for the monster during the phase
    private PlayerSoloHeallth playerComponent;
    private GameObject instantiatedPlayer; 
    private Animator playerAnimator;
    public BattleTrialCharacterLoader characterLoader;
    private GameObject instantiatedMonster; 
    private MonsterHealthEndless monsterComponent;
    private Animator monsterAnimator;
    private EndlessModeSpawner monsterSpawner;
    [SerializeField] private TMP_Text waveText;  
    private int currentWave = 1;
    private float previousMonsterHealth = -1f;
    private AudioManager audioManager;
    private Coroutine feedbackCoroutine;   
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
        PlayerHealthInfo.SetActive(true);
        MonsterHealthInfo.SetActive(false);
        ReadyPanel.SetActive(true);
        DefeatPanel.SetActive(false);
        monsterDamageText.gameObject.SetActive(false);
        feedbackText.gameObject.SetActive(false);
        monsterHealthText.gameObject.SetActive(false);
        playerHealthText.gameObject.SetActive(false);
        attackPhaseImage.SetActive(false);
        defensePhaseImage.SetActive(false);
        InitializeUIElements();
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
        monsterSpawner = FindObjectOfType<EndlessModeSpawner>();
        if (monsterSpawner != null)
        {
            instantiatedMonster = monsterSpawner.GetInstantiatedMonster();
            if (instantiatedMonster != null)
            {
                monsterComponent = instantiatedMonster.GetComponent<MonsterHealthEndless>();
                monsterAnimator = instantiatedMonster.GetComponent<Animator>();

                if (monsterComponent != null)
                {
                    if (enemyHealthBar != null)
                    {
                        Debug.Log("EnemyHealthBar is assigned.");
                        enemyHealthBar.SetMaxHealth(monsterComponent.maxHealth);
                        UpdateMonsterHealthText();
                    }
                }
            }
        }
        playerComponent = instantiatedPlayer.GetComponent<PlayerSoloHeallth>();
        if (playerComponent == null || healthbar == null)
        {
            Debug.LogError("Player Health Bar or Player Component is null");
            return;
        }
        playerComponent.OnHealthLoaded += InitializePlayerHealthUI;
        FetchMathSettings();
        DisableDigitButtons();
        UpdateTotalDamageText();
        currentDamage = maxDamage;  
        UpdatePhaseResultText(currentDamage);
        UpdateWaveText();
    }
    private void FetchMathSettings()
    {
        int defaultRangeMin = 2;
        int defaultRangeMax = 13;
        string defaultOperators = "+-*/";
        int defaultNumOperandsMin = 2;
        int defaultNumOperandsMax = 3;
        FirebaseDatabase.DefaultInstance.GetReference("MathSettings/Endless").GetValueAsync().ContinueWith(task =>
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
    private void Update()
    {
        if (attackPhaseActive || defensePhaseActive)
        {
            UpdateTimer();
        }
        if (monsterComponent != null)
        {
            if (monsterComponent.currentHealth != previousMonsterHealth)
            {
                UpdateMonsterHealthText();  
                previousMonsterHealth = monsterComponent.currentHealth;  
            }
        }
    }   
    private void UpdateWaveText()
    {
        if (waveText != null)
        {
            waveText.text = "Wave: " + currentWave;
        }
    }
    private void InitializeUIElements()
    {
        if (timerText != null) timerText.text = "" + timeRemaining;
        if (feedbackText != null) feedbackText.text = "";
        if (currentAnswerText != null) currentAnswerText.text = "0";
        if (totalDamageText != null) totalDamageText.text = "";
        if (monsterDamageText != null) monsterDamageText.text = "" + currentDamage;
        if (monsterHealthText != null && monsterComponent != null) monsterHealthText.text = "" + monsterComponent.maxHealth;
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
        if (playerComponent != null)
        {
            playerComponent.currentHealth = playerComponent.MaxHealth;
            healthbar.SetMaxHealth(playerComponent.MaxHealth);
            healthbar.SetHealth(playerComponent.currentHealth);
            UpdatePlayerHealthText();
        }
        if (monsterComponent != null)
        {
            monsterComponent.currentHealth = monsterComponent.maxHealth;
            UpdateMonsterHealthUI(); 
        }
        SetToIdleFight();
        if (!attackPhaseActive && !defensePhaseActive)
        {
            StartCoroutine(StartPhaseWithCountdown("Attack Phase"));
        }
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
        if (monsterComponent == null)
        {
            Debug.LogError("Cannot start attack phase. monsterComponent is null.");
            return;
        }
        attackPhaseActive = true;
        defensePhaseActive = false;
        damagePotionUsed = false;
        damageMultiplier = 1.0f;
        isDamageBoostActive = false;
        defensePhaseActive = false;
        timeRemaining = 60f; 
        correctAnswers = 0; 
        totalDamage = 0; 
        currentDamage = maxDamage; 
        attackPhaseImage.SetActive(true);
        defensePhaseImage.SetActive(false);
        UpdatePhaseResultText(currentDamage);
        UpdateMonsterHealthText();
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
        attackPhaseActive = false;
        timeRemaining = 60f;
        accumulatedDefenseSum = 0; 
        SetDigitButtonsInteractable(true);
        currentDamage = maxDamage;
        defensePhaseImage.SetActive(true);
        attackPhaseImage.SetActive(false);
        UpdatePhaseResultText(currentDamage);
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
            int numOperands = UnityEngine.Random.Range(numOperandsMin, numOperandsMax);
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
            if (correctAnswer != 0)
            {
                isValid = true;
                if (questionText != null)
                {
                    questionText.text = operands[0].ToString();
                    for (int i = 1; i < numOperands; i++)
                    {
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
    private void UpdatePhaseResultText(int damageValue)
    {
        if (phaseResultText != null)
        {
            phaseResultText.text = "" + damageValue;
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
                totalDamage += positiveCorrectAnswer;
                UpdateTotalDamageText(); 
            }
            else if (defensePhaseActive)
            {
                accumulatedDefenseSum += positiveCorrectAnswer; 
                int displayDamage = Mathf.Max(currentDamage - accumulatedDefenseSum, 0);
                UpdatePhaseResultText(displayDamage); 
                if (accumulatedDefenseSum > currentDamage)
                {
                    int excessDamage = accumulatedDefenseSum - currentDamage;
                    totalDamage += excessDamage; 
                    UpdateTotalDamageText(); 
                }
                else
                {
                    currentDamage -= positiveCorrectAnswer;
                    UpdatePhaseResultText(currentDamage);
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
        int monsterDamage = currentDamage;
        int damageToPlayer = Mathf.Max(monsterDamage - accumulatedDefenseSum, 0);
        int excessDamage = Mathf.Max(accumulatedDefenseSum - monsterDamage, 0);
        Debug.Log($"Monster Damage: {monsterDamage}, Accumulated Defense: {accumulatedDefenseSum}, Damage to Player: {damageToPlayer}, Excess Damage: {excessDamage}");
        if (damageToPlayer > 0)
        {
            monsterComponent.MonsterAttack();
            yield return new WaitForSeconds(0.4f);
            Debug.Log($"Applying {damageToPlayer} damage to player. Player Health Before: {playerComponent.currentHealth}");
            TriggerDefPlayer();
            audioManager.getHit();
            playerComponent.TakeDamage(damageToPlayer);
            Debug.Log($"Player Health After Damage: {playerComponent.currentHealth}");
            healthbar.SetHealth(playerComponent.currentHealth);
            UpdatePlayerHealthText();
            CheckPlayerHealth(); 
        }
        else
        {
            if (excessDamage == 0)
            {
                Debug.Log("No damage taken by the player. Defense fully absorbed the attack.");
                TriggerDefend1();
                audioManager.getDefend();
                yield return new WaitForSeconds(1f);
                monsterComponent.MonsterAttack();
            }
        }
        if (excessDamage > 0)
        {
            yield return StartCoroutine(DisplayExcessDamageToMonster(excessDamage));
        }
        else
        {
            Debug.Log("No excess damage dealt to the monster.");
        }
    }
    private IEnumerator DisplayExcessDamageToMonster(int excessDamage)
    {
        TriggerAttackAnim();
        audioManager.triggerAttack();
        yield return new WaitForSeconds(1f);
        monsterComponent.TakeDamage(excessDamage);
        Debug.Log($"Monster takes {excessDamage} excess damage. Current Health: {monsterComponent.currentHealth}");
        UpdateMonsterHealthUI();
        if (monsterComponent.currentHealth <= 0)
        {
            Debug.Log("Monster Defeated! Victory.");
            currentWave += 1;
            maxDamage += 50;
            totalDamage = 0;
            currentDamage = maxDamage;
            UpdateMaxWaveInDatabase();
            yield return StartCoroutine(DelayedMonsterRespawn());
        }
        else
        {
            Debug.Log("Monster survived the excess damage.");
        }
    }
    private IEnumerator DelayedMonsterRespawn()
    {
        DisableDigitButtons();
        yield return new WaitForSeconds(2f);
        UpdateWaveText();
        UpdatePhaseResultText(currentDamage);
        UpdateTotalDamageText();
        EndlessModeSpawner spawner = FindObjectOfType<EndlessModeSpawner>();
        if (spawner != null)
        {
            instantiatedMonster = spawner.SpawnMonster();
            monsterComponent = instantiatedMonster.GetComponent<MonsterHealthEndless>();
            if (monsterComponent != null)
            {
                UpdateMonsterHealthUI(); 
                StartCoroutine(StartPhaseWithCountdown("Attack Phase"));
            }
            else
            {
                Debug.LogError("New monster's MonsterHealthEndless component is null.");
            }
            MonsterHealthInfo.SetActive(true);
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
        if (monsterComponent != null)
        {
            monsterHealthText.text = "" + monsterComponent.currentHealth.ToString();
            enemyHealthBar.SetHealth(monsterComponent.currentHealth); 
            Debug.Log("Monster health text updated: " + monsterComponent.currentHealth); 
        }
    }
    private IEnumerator HandleDefeatSequence()
    {
        DefeatPanel.SetActive(true);
        DisableDigitButtons();
        audioManager.triggerGameoverSFX();
        SetDefeatButtonInteractable(false);
        yield return new WaitForSeconds(audioManager.GameoverSFX.length);
        SetDefeatButtonInteractable(true);
        if (currentWave > 1)
        {
            List<RewardItem> rewards = CalculateRewards(currentWave);
            FindObjectOfType<RewardManager>().ShowRewards(rewards);
            audioManager.PlayRewardSoundWithUnmute();
        }
    }
    private void CheckPlayerHealth()
    {
        if (playerComponent.currentHealth <= 0)
        {
            Debug.Log("Player is dead. Game Over.");
            TriggerDeadAnim();
            UpdateMaxWaveInDatabase();
            StartCoroutine(HandleDefeatSequence());
        }
    }
    private List<RewardItem> CalculateRewards(int wave)
    {
        List<RewardItem> rewards = new List<RewardItem>();
        int xpReward = wave * 75;
        rewards.Add(new RewardItem("XP", xpReward));
        int coinReward = wave * 25;
        rewards.Add(new RewardItem("Coins", coinReward));
        if (wave % 5 == 0)
        {
            int gemsReward = wave / 5;
            rewards.Add(new RewardItem("Gems", gemsReward));
        }
        return rewards;
    }
    private IEnumerator DelayedApplyDamage()
    {
        yield return new WaitForSeconds(1.0f); 
        RandomApplyDamageAnim();
    }
    void RandomApplyDamageAnim()
    {
        int randomValue = Random.Range(0, 2);

        if (randomValue == 0)
        {
            StartCoroutine(ApplyDamage());
        }
        else
        {
            StartCoroutine(ApplyDamage1());
        }
    }
    private IEnumerator ApplyDamage()
    {
        if (monsterComponent != null)
        {
            int finalDamage = Mathf.RoundToInt(totalDamage * damageMultiplier);
            Debug.Log($"ApplyDamage - Final Damage: {finalDamage}");
            if (finalDamage > 0)
            {
                TriggerAttackAnim();
                audioManager.triggerAttack();
                yield return new WaitForSeconds(1f);
                monsterComponent.TakeDamage(finalDamage);
                UpdateMonsterHealthUI(); 
                if (monsterComponent.currentHealth <= 0)
                {
                    Debug.Log("Monster Defeated! Progressing to next wave.");
                    HandleMonsterDefeat();
                    yield break; 
                }
            }
            else
            {
                TriggerNoDamageAnim();
                audioManager.triggerNoDamage();
                Debug.Log("No damage dealt to monster.");
            }
            yield return new WaitForSeconds(3f);
            StartCoroutine(StartPhaseWithCountdown("Defense Phase"));
        }
        else
        {
            Debug.LogWarning("Monster component is null in ApplyDamage!");
        }
    }
    private void HandleMonsterDefeat()
    {
        currentWave += 1;
        maxDamage += 50;   
        currentDamage = maxDamage;
        totalDamage = 0;
        UpdateMaxWaveInDatabase();
        StartCoroutine(HandleMonsterDefeatedWithDelay());
    }
    private IEnumerator ApplyDamage1()
    {
        if (monsterComponent != null)
        {
            int finalDamage = Mathf.RoundToInt(totalDamage * damageMultiplier);
            Debug.Log($"ApplyDamage1 - Final Damage: {finalDamage}");
            if (finalDamage > 0)
            {
                TriggerAttackAnim1();
                audioManager.triggerAttack1();
                yield return new WaitForSeconds(1f);
                monsterComponent.TakeDamage(finalDamage);
                UpdateMonsterHealthUI(); 
                if (monsterComponent.currentHealth <= 0)
                {
                    Debug.Log("Monster Defeated! Progressing to next wave.");
                    HandleMonsterDefeat(); 
                    yield break; 
                }
            }
            else
            {
                TriggerNoDamageAnim();
                audioManager.triggerNoDamage();
                Debug.Log("No damage dealt to monster.");
            }
            yield return new WaitForSeconds(3f);
            StartCoroutine(StartPhaseWithCountdown("Defense Phase"));
        }
        else
        {
            Debug.LogWarning("Monster component is null in ApplyDamage!");
        }
    }
    private IEnumerator HandleMonsterDefeatedWithDelay()
    {
        yield return new WaitForSeconds(2f);
        UpdateWaveText();
        UpdatePhaseResultText(currentDamage);
        UpdateTotalDamageText();
        EndlessModeSpawner spawner = FindObjectOfType<EndlessModeSpawner>();
        if (spawner != null)
        {
            GameObject instantiatedMonster = spawner.SpawnMonster(); 
            monsterComponent = instantiatedMonster.GetComponent<MonsterHealthEndless>();
            if (monsterComponent != null)
            {
                UpdateMonsterHealthUI();
                yield return new WaitForSeconds(1f);
                StartCoroutine(StartPhaseWithCountdown("Attack Phase")); 
            }
            else
            {
                Debug.LogError("New monster's MonsterHealthEndless component is null.");
            }
        }
        
    }
    private void UpdateMonsterHealthUI()
    {
        if (monsterComponent != null)
        {
            enemyHealthBar.SetMaxHealth(monsterComponent.maxHealth);
            enemyHealthBar.SetHealth(monsterComponent.currentHealth);
            monsterHealthText.text = "" + monsterComponent.currentHealth.ToString();
            Canvas.ForceUpdateCanvases();
            Debug.Log("UI Updated - Monster Health: " + monsterComponent.currentHealth);
        }
    }
    public void OnClickReady(){
        onPressedStart();
        ReadyPanel.SetActive(false);
        damageContentAnimHandler.OpenContent();
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
    private void UpdateMaxWaveInDatabase()
    {
        string userId = FirebaseManager.Instance.Auth.CurrentUser.UserId; 
        DatabaseReference userRef = FirebaseManager.Instance.Database.GetReference("users").Child(userId).Child("maxWave");
        userRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error fetching maxWave from database.");
                return;
            }
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                int maxWaveInDatabase = 0;
                if (snapshot.Exists)
                {
                    maxWaveInDatabase = int.Parse(snapshot.Value.ToString());
                }
                if (currentWave > maxWaveInDatabase)
                {
                    userRef.SetValueAsync(currentWave).ContinueWithOnMainThread(setTask =>
                    {
                        if (setTask.IsCompleted)
                        {
                            Debug.Log($"New maxWave {currentWave} recorded in database.");
                        }
                        else if (setTask.IsFaulted)
                        {
                            Debug.LogError("Error updating maxWave in database.");
                        }
                    });
                }
                else
                {
                    Debug.Log($"Current wave {currentWave} did not exceed maxWave {maxWaveInDatabase}.");
                }
            }
        });
    }
    private void SetDefeatButtonInteractable(bool state)
    {
        DefeatButton1.interactable = state;
        DefeatButton2.interactable = state;
    }
    public void OpenDefeatButtons()
    {
        SetDefeatButtonInteractable(true);
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
    public void AddDamage()
    {
        if (attackPhaseActive)
        {
            totalDamage += 1000;
            UpdateTotalDamageText(); 
        }
        else if (defensePhaseActive)
        {
            accumulatedDefenseSum += 1000;
            int displayDamage = Mathf.Max(currentDamage - accumulatedDefenseSum, 0);
            UpdatePhaseResultText(displayDamage); 
            if (accumulatedDefenseSum > currentDamage)
            {
                int excessDamage = accumulatedDefenseSum - currentDamage;
                totalDamage += excessDamage;
                UpdateTotalDamageText(); 
            }
        }
    }
    public void skipAttackPhase()
    {
        EndAttackPhase();
    }
    public void skipDefensePhase()
    {
        EndDefensePhase();
    }
}