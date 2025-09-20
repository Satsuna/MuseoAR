using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string questionText;
        public string choiceA;
        public string choiceB;
        public int correctAnswer; // 0 = A, 1 = B
    }
    public FeedbackSpawner feedbackSpawner;
    public FeedbackPanel feedbackPanel;
    public Question[] questions;
    private Question currentQuestion;
    private int lastQuestionIndex = -1;
    public GameObject popUpIcon;

    [Header("UI References")]
    public TMP_Text questionTextUI;
    public Button choiceAButton;
    public Button choiceBButton;

    void OnEnable()
    {
        ShowRandomQuestion();
    }

    void ShowRandomQuestion()
    {
        if (questions.Length == 0) return;

        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, questions.Length);
        }
        while (randomIndex == lastQuestionIndex && questions.Length > 1);

        lastQuestionIndex = randomIndex;
        currentQuestion = questions[randomIndex];

        // Update UI
        questionTextUI.text = currentQuestion.questionText;
        choiceAButton.GetComponentInChildren<TMP_Text>().text = currentQuestion.choiceA;
        choiceBButton.GetComponentInChildren<TMP_Text>().text = currentQuestion.choiceB;

        // Update button listeners
        choiceAButton.onClick.RemoveAllListeners();
        choiceBButton.onClick.RemoveAllListeners();

        choiceAButton.onClick.AddListener(() => OnAnswerSelected(0));
        choiceBButton.onClick.AddListener(() => OnAnswerSelected(1));
    }

    void OnAnswerSelected(int choiceIndex)
    {
        bool isCorrect = (choiceIndex == currentQuestion.correctAnswer);

        feedbackSpawner.SpawnFeedback(isCorrect);

        if (isCorrect)
        {
            Debug.Log("Correct!");
        }
        else
        {
            Debug.Log("Wrong!");
        }

        feedbackPanel.ShowFeedback(isCorrect);

        Invoke(nameof(DisableQuiz), 1.5f);
    }

    void DisableQuiz()
    {
        gameObject.SetActive(false);
        popUpIcon.SetActive(true);
    }
}
