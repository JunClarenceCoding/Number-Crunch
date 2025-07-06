using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Firebase.Database;
using System.Collections.Generic;
public class Boss1BattleHandler : MonoBehaviourPunCallbacks
{
    public DamageContentAnimHandler damageContentAnimHandler;
   public static Boss1BattleHandler Instance { get; private set; }
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
    private int bossDamage = 700;
    private Boss1 bossComponent;
    private PlayerSoloHeallth playerComponent;
    public PlayerSoloHeallth PlayerComponent => playerComponent;
    private GameObject instantiatedPlayer; 
    private Animator playerAnimator;
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
        VictoryPanel.SetActive(false);
        DefeatPanel.SetActive(false);
        attackPhaseImage.SetActive(false);
        defensePhaseImage.SetActive(false);
        InitializeUIElements();
        PlayerHealthInfo.SetActive(true);
        MonsterHealthInfo.SetActive(true);
        feedbackText.gameObject.SetActive(true);
        bossDamageText.gameObject.SetActive(true);
        bossHealthText.gameObject.SetActive(true);
        playerHealthText.gameObject.SetActive(true);
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
        if (bossComponent == null && boss != null)
        {
            bossComponent = boss.GetComponent<Boss1>();
        }
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
        int defaultRangeMin = 2;
        int defaultRangeMax = 26;
        string defaultOperators = "+-*/";
        int defaultNumOperandsMin = 2;
        int defaultNumOperandsMax = 3;
        FirebaseDatabase.DefaultInstance.GetReference("MathSettings/Boss 2").GetValueAsync().ContinueWith(task =>
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
            Destroy(gameObject);  
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
        yield return new WaitForSeconds(2f);
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
        damageMultiplier = 1.0f;
        isDamageBoostActive = false;
        timeRemaining = 60f;
        correctAnswers = 0;
        totalDamage = 0;
        bossDamage = 700; 
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
        bossDamage = 700; 
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
            if (correctAnswer >= 20 && correctAnswer <= 135)
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
            int positiveCorrectAnswer = Mathf.Abs(correctAnswer);
            correctAnswers++;
            if (attackPhaseActive)
            {
                totalDamage += positiveCorrectAnswer;
                totalAccumulatedDamageToMonster += positiveCorrectAnswer;
                Debug.Log("Current Accumulated Damage: " + totalAccumulatedDamageToMonster);
                UpdateTotalDamageText();
            }
            else if (defensePhaseActive)
            {
                accumulatedDefenseSum += positiveCorrectAnswer;
                if (positiveCorrectAnswer <= bossDamage)
                {
                    bossDamage -= positiveCorrectAnswer;
                    UpdatePhaseResultText(bossDamage); 
                }
                else
                {
                    int excessDamage = positiveCorrectAnswer - bossDamage; 
                    totalDamage += excessDamage; 
                    totalAccumulatedDamageToMonster += excessDamage;
                    bossDamage = 0; 
                    Debug.Log("Current Accumulated Damage: " + totalAccumulatedDamageToMonster);
                    UpdatePhaseResultText(bossDamage); 
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
        if (playerComponent.currentHealth > 0 && bossComponent.CurrentHealth> 0)
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
        int bossDamage = 700;
        int damageToPlayer = bossDamage;
        int excessDamage = 0; 
        if (accumulatedDefenseSum >= bossDamage)
        {
            damageToPlayer = 0;  
            excessDamage = accumulatedDefenseSum - bossDamage;
            if (excessDamage > 0)
            {
                StartCoroutine(DisplayExcessDamageToMonster(excessDamage)); 
            }
        }
        else
        {
            damageToPlayer = bossDamage - accumulatedDefenseSum;
        }
        Debug.Log($"Monster Damage: {bossDamage}, Accumulated Defense: {accumulatedDefenseSum}, Damage to Player: {damageToPlayer}");
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
            if (excessDamage == 0)
            {
                TriggerDefend1(); 
                audioManager.getDefend();
                yield return new WaitForSeconds(1f);
                bossComponent.GetComponent<Animator>().SetTrigger("isAttack"); 
            }
        }
    }
    private IEnumerator DisplayExcessDamageToMonster(int excessDamage)
    {
        TriggerAttackAnim();
        audioManager.triggerAttack();
        yield return new WaitForSeconds(1f);
        bossComponent.photonView.RPC("TakeDamage", RpcTarget.AllBuffered, excessDamage);
        bossComponent.GetComponent<Animator>().SetTrigger("isHit");
        Debug.Log($"Monster takes {excessDamage} excess damage. Current Health: {bossComponent.CurrentHealth}");
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
            Debug.Log("Updated Player Health Text: " + playerHealthText.text); 
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
    private IEnumerator ApplyDamage1()
    {
        if (boss != null)
        {
            Boss1 monsterComponent = boss.GetComponent<Boss1>();
            if (monsterComponent != null)
            {
                int finalDamage = Mathf.RoundToInt(totalDamage * damageMultiplier);
                Debug.Log("ApplyDamage1 - Final Damage: " + finalDamage); 
                if (finalDamage > 0)
                {
                    TriggerAttackAnim1();
                    audioManager.triggerAttack1();
                    yield return new WaitForSeconds(1f);
                    monsterComponent.photonView.RPC("TakeDamage", RpcTarget.AllBuffered, finalDamage); 
                    bossComponent.GetComponent<Animator>().SetTrigger("isHit");
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
                yield return new WaitForSeconds(3f);
                StartCoroutine(StartPhaseWithCountdown("Defense Phase"));
            }
        }
    }
    private IEnumerator ApplyDamage()
    {
        if (boss != null)
        {
            Boss1 monsterComponent = boss.GetComponent<Boss1>();
            if (monsterComponent != null)
            {
                int finalDamage = Mathf.RoundToInt(totalDamage * damageMultiplier);
                Debug.Log("ApplyDamage - Final Damage: " + finalDamage); 
                if (finalDamage > 0)
                {
                    TriggerAttackAnim();
                    audioManager.triggerAttack();
                    yield return new WaitForSeconds(1f);
                    monsterComponent.photonView.RPC("TakeDamage", RpcTarget.AllBuffered, finalDamage); 
                    bossComponent.GetComponent<Animator>().SetTrigger("isHit");
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
    public void ConcedeBattle()
    {
        attackPhaseActive = false;
        defensePhaseActive = false;
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
        if (totalAccumulatedDamageToMonster >= 0 && totalAccumulatedDamageToMonster < 50)
        {
            rewards.Add(new RewardItem("Coins", 50));
            rewards.Add(new RewardItem("XP", 100));
        }
        else if (totalAccumulatedDamageToMonster >= 51 && totalAccumulatedDamageToMonster < 100)
        {
            rewards.Add(new RewardItem("Coins", 100));
            rewards.Add(new RewardItem("XP", 250));
        }
        else if (totalAccumulatedDamageToMonster >= 101 && totalAccumulatedDamageToMonster < 200)
        {
            rewards.Add(new RewardItem("Coins", 150));
            rewards.Add(new RewardItem("XP", 300));
        }
        else if (totalAccumulatedDamageToMonster >= 201 && totalAccumulatedDamageToMonster < 300)
        {
            rewards.Add(new RewardItem("Coins", 250));
            rewards.Add(new RewardItem("XP", 350));
        } 
        else if (totalAccumulatedDamageToMonster >= 301 && totalAccumulatedDamageToMonster < 400)
        {
            rewards.Add(new RewardItem("Coins", 300));
            rewards.Add(new RewardItem("XP", 400));
        } 
        else if (totalAccumulatedDamageToMonster >= 401 && totalAccumulatedDamageToMonster < 599)
        {
            rewards.Add(new RewardItem("Coins", 350));
            rewards.Add(new RewardItem("XP", 450));
        }
        else if (totalAccumulatedDamageToMonster >= 600)
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
        int positiveCorrectAnswer = Mathf.Abs(100);
        if (attackPhaseActive)
        {
            totalDamage += positiveCorrectAnswer;
            totalAccumulatedDamageToMonster += positiveCorrectAnswer;
            UpdateTotalDamageText(); 
        }
        else if (defensePhaseActive)
        {
            accumulatedDefenseSum += positiveCorrectAnswer;

            if (positiveCorrectAnswer <= bossDamage)
            {
                bossDamage -= positiveCorrectAnswer;
                UpdatePhaseResultText(bossDamage); 
            }
            else
            {
                int excessDamage = positiveCorrectAnswer - bossDamage; 
                totalDamage += excessDamage; 
                totalAccumulatedDamageToMonster += excessDamage;
                bossDamage = 0; 
                UpdatePhaseResultText(bossDamage); 
                UpdateTotalDamageText(); 
            }
        }
    }
}