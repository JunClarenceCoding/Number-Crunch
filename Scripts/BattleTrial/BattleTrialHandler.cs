using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Extensions;
using System.Collections.Generic;
using Unity.VisualScripting;
public class BattleTrialHandler : MonoBehaviour
{
    public DamageContentAnimHandler damageContentAnimHandler;
    public Animator clockAnimator;
    public Image playerDisplayImage; 
    public Image npcDisplayImage;  
    public Sprite boyImage; 
    public Sprite girlImage; 
    public Slider phaseSlider; 
    public GameObject itemDetailsPanelPrefab; 
    private GameObject activeItemDetailsPanel; 
    public InventoryUI inventoryUI;
    public GameObject loadingScreen;
    public Slider slider;
    public GameObject HealthPlayerBar;
    public GameObject HealthEnemyBar;
    public GameObject PlayerHealthInfo;
    public GameObject MonsterHealthInfo;
    public EnemyHealthBar enemyHealthBar;
    public PlayerHealthBar playerHealthBar;
    [Header("Pause Animation & Panel")]
    [SerializeField] private GameObject pausePanel; 
    [SerializeField] private Animator pausePanelAnimator;
    private bool isTimerPaused = false;
    [Header("Phase Images")]
    public GameObject attackPhaseImage; 
    public GameObject defensePhaseImage;
    public GameObject DialoguePanel;

    [Header("Start Dialogue")]
    public TMP_Text startDialogue;
    public Button Continue1;
    [Header("Continue1")]
    public TMP_Text Dialogue1;
    public Button Continue2;
    [Header("Continue2")]
    public TMP_Text Dialogue2;
    public Button Continue3;
    [Header("Continue3")]
    public TMP_Text Dialogue3;
    public Button Continue4;
    [Header("Continue4")]
    public TMP_Text Dialogue4;
    public Button Continue5;
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
    [SerializeField] private Button submitButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Button negativeButton;
    [SerializeField] private Button invButton;
    [SerializeField] private Button settingButton;

    [Header("Show Victory Panel")]
    public GameObject VictoryPanel;
    public Button VictoryButton;
    [Header("Show Defeat Panel")]
    public GameObject DefeatPanel;
    public Button DefeatButton;
    [Header("Victory Dialogue")]
    public TMP_Text VictoryMessageText;
    public Button VictoryContinueButton;

    private int correctAnswers = 0;
    private int currentAnswer = 0;
    private int correctAnswer = 0;
    private int totalDamage = 0;
    private bool damagePotionUsed = false; 
    private bool isDamageBoostActive = false;
    private float damageMultiplier = 1.0f;
    private int accumulatedDefenseSum = 0;
    private float timeRemaining = 30f;
    private bool attackPhaseActive = false;
    private bool defensePhaseActive = false;
    private int monsterDamage = 100;
    private Monster monsterComponent;
    private PlayerSoloHeallth playerComponent;
    private GameObject instantiatedPlayer; 
    private Animator playerAnimator;
    private AudioManager audioManager;
    private Coroutine feedbackCoroutine;
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene.");
            return;
        }
        PlayerHealthInfo.SetActive(false);
        MonsterHealthInfo.SetActive(false);
        ConvoStart();
        VictoryPanel.SetActive(false);
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
            UpdatePlayerImage();
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
        if (playerComponent == null || playerHealthBar == null)
        {
            Debug.LogError("Player Health Bar or Player Component is null");
            return; 
        }
        enemyHealthBar.SetMaxHealth(monsterComponent.MaxHealth);
        playerComponent.OnHealthLoaded += InitializePlayerHealthUI;
        UpdateMonsterHealthText();
        DisableDigitButtons();
        UpdateTotalDamageText();
        UpdatePhaseResultText(monsterDamage);
    }
    private void UpdatePlayerImage()
    {
        BattleTrialCharacterLoader characterLoader = FindObjectOfType<BattleTrialCharacterLoader>();
        if ( characterLoader != null)
        {
            GameObject player = characterLoader.GetInstantiatedPlayer();
            int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
            if (selectedCharacter == 0)
            {
                playerDisplayImage.sprite = boyImage; 
            }
            else if (selectedCharacter == 1)
            {
                playerDisplayImage.sprite = girlImage; 
            }
        }
    }
    private void ShowCharacter(bool isPlayerSpeaking)
    {
        playerDisplayImage.gameObject.SetActive(isPlayerSpeaking);
        npcDisplayImage.gameObject.SetActive(!isPlayerSpeaking);
    }
    private void InitializePlayerHealthUI()
    {
        if (playerComponent != null)
        {
            playerHealthBar.SetMaxHealth(playerComponent.MaxHealth); 
            playerHealthBar.SetHealth(playerComponent.currentHealth);
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
    private void SetDialogueState(TMP_Text activeDialogue, Button[] activeButtons, string dialogueText)
    {
        DialoguePanel.SetActive(true);

        TMP_Text[] dialogues = { startDialogue, Dialogue1, Dialogue2, Dialogue3, Dialogue4, VictoryMessageText};
        Button[] allButtons = { Continue1, Continue2, Continue3, Continue4, Continue5, VictoryContinueButton};

        foreach (TMP_Text dialogue in dialogues)
        {
            dialogue.gameObject.SetActive(false);
        }
        foreach (Button button in allButtons)
        {
            button.gameObject.SetActive(false);
        }
        activeDialogue.gameObject.SetActive(true);
        activeDialogue.GetComponent<TypeWriterEffect>().StartTypewriterEffect(dialogueText, () =>
        {
            foreach (Button button in activeButtons)
            {
                button.gameObject.SetActive(true); 
            }
        });
    }
    public void ConvoStart()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[0]);
        SetDialogueState(startDialogue, new Button[] { Continue1 }, "You'll get a chance to attack and defend yourself at every turn.");
        ShowCharacter(false);
    }
    public void StartDialogue1()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[1]);
        SetDialogueState(Dialogue1, new Button[] { Continue2 }, "Normally, you'll have 60 seconds to solve as many problems as you can and gather many points. But for now we will only use 30 seconds for this trial.");
    }
    public void StartDialogue2()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[2]);
        SetDialogueState(Dialogue2, new Button[] { Continue3 }, "If you get high points in the attack you'll get to deal larger damage on your enemy.");
    }
    public void StartDialogue3()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[3]);
        SetDialogueState(Dialogue3, new Button[] { Continue4 }, "And when you get points that are higher than your enemy's attack when defending you can technically deal damage instead of taking one from the enemy.");
    }
    public void StartDialogue4()
    {
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[4]);
        SetDialogueState(Dialogue4, new Button[] { Continue5 }, "That should be the basics of it. Let's start with the practice, do your best. I'm sure you can do it.");
    }
    public void StartVictoryDialogue(){
        audioManager.PlaySFXWithPriority(audioManager.tutorialPart4Clips[5]);
        Debug.Log("StartVictoryDialogue called");
        DialoguePanel.SetActive(true);
        SetDialogueState(VictoryMessageText, new Button[] {VictoryContinueButton}, "Great job! I knew you could do it. You can officially start your journey now. Good luck and I wish you the best");
        ShowCharacter(false);
    }
    public void ConvoEnd()
    {
        audioManager.StopCurrentSFX();
        DialoguePanel.SetActive(false);
        TMP_Text[] dialogues = { startDialogue, Dialogue1, Dialogue2, Dialogue3, Dialogue4, VictoryMessageText};
        Button[] allButtons = { Continue1, Continue2, Continue3, Continue4, Continue5, VictoryContinueButton};

        foreach (TMP_Text dialogue in dialogues)
        {
            dialogue.gameObject.SetActive(false);
        }
        foreach (Button button in allButtons)
        {
            button.gameObject.SetActive(false);
        }   
    }
    void Update()
    {
        if (attackPhaseActive || defensePhaseActive)
        {
            UpdateTimer();
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
    private void UnTriggerDeadAnim()
    {
       if (playerAnimator != null)
        {
            playerAnimator.SetBool("isDead", false); 
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
    private void TriggerAttackAnim1(){
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("isAttack1");
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
            playerHealthBar.SetMaxHealth(playerComponent.MaxHealth);
            playerHealthBar.SetHealth(playerComponent.currentHealth);
            UpdatePlayerHealthText();
        }
        SetToIdleFight();
        if (!attackPhaseActive && !defensePhaseActive)
        {
            StartCoroutine(StartPhaseWithCountdown("Attack Phase"));
        }
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
        timeRemaining = 30f;
        correctAnswers = 0;
        totalDamage = 0;
        monsterDamage = 100;
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
        timeRemaining = 30f;
        accumulatedDefenseSum = 0;
        SetDigitButtonsInteractable(true);
        monsterDamage = 100;
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
        int numOperands = Random.Range(2,3);
        int[] operands = new int[numOperands];
        for (int i = 0; i < numOperands; i++)
        {
            operands[i] = Random.Range(1, 10); 
        }
        correctAnswer = 0;
        if (questionText != null)
        {
            questionText.text = "";
            for (int i = 0; i < numOperands; i++)
            {
                correctAnswer += operands[i];
                questionText.text += operands[i].ToString();
                if (i < numOperands - 1)
                {
                    questionText.text += " + ";

                }
            }
             questionText.text += " =";
        }
        Debug.Log("Generated Question: " + questionText.text + correctAnswer);
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
        int monsterDamage = 100;
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
            playerHealthBar.SetHealth(playerComponent.currentHealth);
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
            VictorySuccess();
            StartCoroutine(HandleVictorySequence1());
            DisableDigitButtons();
            yield break;
        }
    }
    private void UpdatePlayerHealthText()
    {
        if (playerHealthText != null)
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
            return;
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
                        SetBoolTrueWin();
                        VictorySuccess();
                        StartCoroutine(HandleVictorySequence1());
                        DisableDigitButtons();
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
                        SetBoolTrueWin();
                        VictorySuccess();
                        StartCoroutine(HandleVictorySequence1());
                        DisableDigitButtons();
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
    public void VictorySuccess()
    {
        int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
        if (!FirebaseManager.Instance.IsFirebaseInitialized())
        {
            Debug.LogError("Firebase is not initialized.");
            return;
        }
        var userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        var dbRef = FirebaseDatabase.DefaultInstance.GetReference("users").Child(userId);
        dbRef.Child("tutorialCompleted").SetValueAsync(true);
        dbRef.Child("selectedCharacter").SetValueAsync(selectedCharacter).ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                Debug.Log("Character selection and tutorial status saved successfully.");
            }
        });
    }
    public void OnClickVictoryButton()
    {
        VictoryPanel.SetActive(false);
    }

    public void OnClickDefeatButton()
    {
        DefeatPanel.SetActive(false);
        correctAnswers = 0;
        currentAnswer = 0;
        totalDamage = 0;
        accumulatedDefenseSum = 0;
        timeRemaining =10f;
        attackPhaseActive = false;
        defensePhaseActive = false;
        if(playerComponent != null)
        {
            playerComponent.currentHealth = playerComponent.MaxHealth;
            playerHealthBar.SetMaxHealth(playerComponent.MaxHealth);
            playerHealthBar.SetHealth(playerComponent.currentHealth);
            UpdatePlayerHealthText();
        }
        if (monsterComponent != null)
        {
            monsterComponent.currentHealth = monsterComponent.MaxHealth;
            enemyHealthBar.SetMaxHealth(monsterComponent.MaxHealth);
            enemyHealthBar.SetHealth(monsterComponent.currentHealth);
            UpdateMonsterHealthText();
        }
        if (timerText != null) timerText.text = "" + timeRemaining;
        if (feedbackText != null) feedbackText.text = "";
        if (currentAnswerText != null) currentAnswerText.text = "0";
        if (totalDamageText != null) totalDamageText.text = "";
        if (monsterDamageText != null) monsterDamageText.text = "" + monsterDamage;
        if (playerHealthText != null) playerHealthText.text = "" + playerComponent.currentHealth;
        if (phaseCountdownText != null) phaseCountdownText.text = "";
        if (phaseResultText != null) phaseResultText.text = "";
        feedbackText.gameObject.SetActive(false);
        monsterDamageText.gameObject.SetActive(false);
        monsterHealthText.gameObject.SetActive(false);
        playerHealthText.gameObject.SetActive(false);
        UnTriggerDeadAnim();
        onPressedStart();
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
            playerHealthBar.SetHealth(playerComponent.currentHealth);
            UpdatePlayerHealthText();
        }
    }
    public void GoToScene(string sceneName)
    {
       SceneManager.LoadScene(sceneName);
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
    private IEnumerator HandleVictorySequence1()
    {
        yield return new WaitForSeconds(1f);
        StartVictoryDialogue();
    }
    private IEnumerator HandleVictorySequence2()
    {
        VictoryPanel.SetActive(true);
        SetBoolTrueWin();
        audioManager.TriggerWinSound();
        VictoryButton.interactable = false;
        yield return new WaitForSeconds(audioManager.WinSound.length);
        List<RewardItem> rewards = new List<RewardItem> { new RewardItem("Coins", 500), new RewardItem("XP", 300), new RewardItem("Gems", 5) };
        FindObjectOfType<RewardManager>().ShowRewards(rewards);
        audioManager.PlayRewardSound();
    }
    public void OpenVictoryAndRewards()
    {
        StartCoroutine(HandleVictorySequence2());
    }
    public void SetIntoInteractable(){
        VictoryButton.interactable = true;
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
    private void SetDefeatButtonsInteractable(bool state)
    {
        DefeatButton.interactable = state;
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
            totalDamage += 50;
            UpdateTotalDamageText(); 
        }
        else if (defensePhaseActive)
        {
            accumulatedDefenseSum += 50;
            if (50 <= monsterDamage)
            {
                monsterDamage -= 50;
                UpdatePhaseResultText(monsterDamage);
            }
            else
            {
                int excessDamage = 50 - monsterDamage; 
                totalDamage += excessDamage; 
                monsterDamage = 0; 
                UpdatePhaseResultText(monsterDamage); 
                UpdateTotalDamageText(); 
            }
        }
    } 
}