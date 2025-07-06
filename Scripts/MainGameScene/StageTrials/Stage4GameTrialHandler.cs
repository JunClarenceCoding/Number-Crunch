using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
public class Stage4GameTrialHandler : MonoBehaviour
{
    public AudioManager audioManager;
    public Animator clockAnimator;
    public GameObject MainStage4TrialPanel,SettingPanel;
    public Slider phaseSlider; 
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private TMP_Text feedbackText;
    [SerializeField] private TMP_Text currentAnswerText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject penaltyGameObject;
    public GameObject ReadyPanel;
    public TMP_Text countdownText;
    public GameObject countdownPanel;
    public Button readyButton;
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
    public GameObject VictoryPanel;
    public Button victoryButton;
    public GameObject DefeatPanel;
    public Button DefeatBtn1, DefeatBtn2;
    public GameObject ConfirmConcedePanel;
    [SerializeField] private Button concedeConfirmButton;
    [SerializeField] private Button restartButton;
    private int currentAnswer = 0;
    private int correctAnswer = 0;
    private int score = 0;
    private float timeRemaining = 60f;
    private bool isGameActive = false;
    private Coroutine timerCoroutine;
    private FirebaseManager firebaseManager;
    private Coroutine feedbackCoroutine;
    private void Start()
    {
        firebaseManager = FirebaseManager.Instance;
    }
    public void OpenStage4Trial()
    {
        StartCoroutine(OpenStage4TrialPanel());
    }
    private IEnumerator OpenStage4TrialPanel()
    {
        MainStage4TrialPanel.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        audioManager.PlayBattleBackground();
        InitializeUIElements();
        DisableDigitButtons();
    }
    private void InitializeUIElements()
    {
        timerText.text = "60";
        phaseSlider.value = timeRemaining;
        feedbackText.text = "";
        currentAnswerText.text = "0";
        scoreText.text = "Score: 0";
        if (penaltyGameObject != null)
        {
            penaltyGameObject.SetActive(false); 
        }
        submitButton.onClick.RemoveAllListeners();
        digitButton0.onClick.RemoveAllListeners();
        digitButton1.onClick.RemoveAllListeners();
        digitButton2.onClick.RemoveAllListeners();
        digitButton3.onClick.RemoveAllListeners();
        digitButton4.onClick.RemoveAllListeners();
        digitButton5.onClick.RemoveAllListeners();
        digitButton6.onClick.RemoveAllListeners();
        digitButton7.onClick.RemoveAllListeners();
        digitButton8.onClick.RemoveAllListeners();
        digitButton9.onClick.RemoveAllListeners();
        clearButton.onClick.RemoveAllListeners();
        deleteButton.onClick.RemoveAllListeners();
        negativeButton.onClick.RemoveAllListeners();
        readyButton.onClick.RemoveAllListeners();
        concedeConfirmButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
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
        readyButton.onClick.AddListener(OnReadyButtonClick);
        ReadyPanel.SetActive(true);
        countdownText.gameObject.SetActive(false);
        concedeConfirmButton.onClick.AddListener(ConcedeBattle);
        restartButton.onClick.AddListener(RestartGame);
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
    }
     private void DisableDigitButtons()
    {
        SetDigitButtonsInteractable(false);
    }
    private void OnReadyButtonClick()
    {
        ReadyPanel.SetActive(false);
        countdownText.gameObject.SetActive(true);
        countdownPanel.SetActive(true); 
        StartCoroutine(ShowCountdown());
        DisableDigitButtons();
    }
    private IEnumerator ShowCountdown()
    {
        yield return new WaitForSeconds(0.1f); 
        countdownText.gameObject.SetActive(true);
        countdownText.text = "Trial starts in";
        yield return new WaitForSeconds(1f);
        audioManager.playCountdown();
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        countdownText.text = "Go!";
        yield return new WaitForSeconds(0.5f);
        countdownText.gameObject.SetActive(false);
        countdownPanel.SetActive(false);
        StartGame();
    }
    private void StartGame()
    {
        isGameActive = true;
        SetDigitButtonsInteractable(true);
        VictoryPanel.SetActive(false);
        DefeatPanel.SetActive(false);
        ConfirmConcedePanel.SetActive(false);
        score = 0;
        timeRemaining = 60f;
        currentAnswer = 0;
        UpdateCurrentAnswerText();
        scoreText.text = "Score: 0";
        GenerateQuestion();
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        timerCoroutine = StartCoroutine(UpdateTimer());
    }
    private IEnumerator UpdateTimer()
    {
        while (timeRemaining > 0 && isGameActive)
        {
            timeRemaining -= Time.deltaTime;
            timerText.text = Mathf.Ceil(timeRemaining).ToString();
            phaseSlider.value = timeRemaining;
            if (timeRemaining <= 0)
            {
                SetDigitButtonsInteractable(false);
                StartCoroutine(ClockTimesUp());
            }
            yield return null;
        }
    }
    private void GenerateQuestion()
    {
        bool isValid = false;
        while (!isValid)
        {
            int numOperands = Random.Range(2, 3);
            int[] operands = new int[numOperands];
            for (int i = 0; i < numOperands; i++)
            {
                operands[i] = Random.Range(1, 11); 
            }
            correctAnswer = operands[0];
            if (questionText != null)
            {
                questionText.text = operands[0].ToString();
                for (int i = 1; i < numOperands; i++)
                {
                    correctAnswer -= operands[i];
                    questionText.text += " - " + operands[i].ToString();
                }
                questionText.text += " =";
            }
            if (correctAnswer != 0 && correctAnswer != 1 && correctAnswer != -1)
            {
                isValid = true;
            }
        }
        Debug.Log("Generated Question: " + questionText.text + " = " + correctAnswer);
    }
    private void OnSubmitButtonClick()
    {
        if (!isGameActive) return;
        if (currentAnswer == correctAnswer)
        {
            SetFeedback("Correct", new Color(0f, 0.36f, 0f));
            audioManager.triggerCorrectAnswer();
            score += Mathf.Abs(correctAnswer);
            scoreText.text = "Score: " + score;
            if (score >= 50)
            {
                EndGame(true);
                scoreText.text = "Score: " + 50;
            }
            else
            {
                currentAnswer = 0;
                UpdateCurrentAnswerText();
                GenerateQuestion();
            }
        }
        else
        {
            SetFeedback("Incorrect", Color.red);
            currentAnswer = 0;
            if(currentAnswerText != null) currentAnswerText.text = "0";
            timeRemaining -= 2;
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                timerText.text = "0";
                phaseSlider.value = 0;
                StartCoroutine(ClockTimesUp());
            }
            else
            {
               ShowPenalty();
            }
        }
    }
    private IEnumerator ClockTimesUp()
    {
        clockAnimator.SetBool("isTimesUp", true);
        audioManager.triggerTimesUp();
        yield return new WaitForSeconds(audioManager.TimesUp.length);
        clockAnimator.SetBool("isTimesUp", false);
        EndGame(false);
    }
    private void ShowPenalty()
    {
        if (penaltyGameObject != null)
        {
            penaltyGameObject.SetActive(true); 
            clockAnimator.SetTrigger("isPenalty");
            audioManager.triggerWrongClock();
            StartCoroutine(HidePenaltyGameObjectAfterDelay(1f));
        }
    }
    private IEnumerator HidePenaltyGameObjectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (penaltyGameObject != null)
        {
            penaltyGameObject.SetActive(false); 
        }
    }
    private void EndGame(bool isVictory)
    {
        isGameActive = false;
        if (isVictory)
        {
            StartCoroutine(HandleVictorySequence());
            firebaseManager.UpdateStage4Task(true);
        }
        else
        {
            StartCoroutine(HandleDefeatSequence());
        }
    }
    private IEnumerator HandleVictorySequence()
    {
        VictoryPanel.SetActive(true);
        audioManager.TriggerWinSound();
        victoryButton.interactable = false;
        DisableDigitButtons();
        yield return new WaitForSeconds(audioManager.WinSound.length);
        List<RewardItem> rewards = new List<RewardItem> { new RewardItem("Coins", 200),  new RewardItem("Gems", 3) };
        FindObjectOfType<RewardManager>().ShowRewards(rewards);
        audioManager.PlayRewardSound();
        victoryButton.interactable = true;
    }
    private IEnumerator HandleDefeatSequence()
    {
        DefeatPanel.SetActive(true);
        DisableDigitButtons();
        SetDefeatButtonsInteractable(false);
        audioManager.triggerGameoverSFX();
        yield return new WaitForSeconds(audioManager.GameoverSFX.length);
        SetDefeatButtonsInteractable(true);
    }
    private void ConcedeBattle()
    {
        ConfirmConcedePanel.SetActive(false);
        EndGame(false);
    }
    private void RestartGame()
    {
        timerText.text = "60";
        feedbackText.text = "";
        currentAnswerText.text = "0";
        scoreText.text = "Score: 0";
        DefeatPanel.SetActive(false);
        ReadyPanel.SetActive(true);
        DisableDigitButtons();
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
    private void UpdateCurrentAnswerText()
    {
        if (currentAnswerText != null)
        {
            currentAnswerText.text = currentAnswer.ToString();
        }
    }
    private void ClearInput()
    {
        currentAnswer = 0;
        UpdateCurrentAnswerText();
        feedbackText.text = "";
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
    public void OpenConfirmConcedePanel()
    {
        ConfirmConcedePanel.SetActive(true);
        SettingPanel.SetActive(false);
    }
    public void CloseConfirmConcedePanel()
    {
        ConfirmConcedePanel.SetActive(false);
    }
    public void OpenSettingPanel()
    {
        SettingPanel.SetActive(true);
    }
    public void CloseSettingPanel()
    {
        SettingPanel.SetActive(false);
    }
    public void CloseMainStage4GameTrialPanel()
    {
        MainStage4TrialPanel.SetActive(false);
        DefeatPanel.SetActive(false);
        VictoryPanel.SetActive(false);
        audioManager.PlayBackgroundMusic();
    }
    private void SetDefeatButtonsInteractable(bool state)
    {
        DefeatBtn1.interactable = state;
        DefeatBtn2.interactable = state;
    }
}