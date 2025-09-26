using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation; // ✅ AR Foundation
using System.Collections.Generic;

public class Quiz : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string questionText;
        public string choiceA;
        public string choiceB;
        public int correctAnswer; // 0 = A, 1 = B
        public string questionForPainting; // Painting this question is for
    }

    [Header("Dependencies")]
    public FeedbackSpawner feedbackSpawner;
    public FeedbackPanel feedbackPanel;
    public ARTrackedImageManager trackedImageManager;

    [Header("Quiz Data")]
    public Question[] questions;
    private List<Question> filteredQuestions = new List<Question>();
    private Question currentQuestion;
    private int lastQuestionIndex = -1;
    private string activePaintingName;

    [Header("UI References")]
    public TMP_Text questionTextUI;
    public Button choiceAButton;
    public Button choiceBButton;
    public GameObject popUpIcon;
    public GameObject quizPanel;

    private bool wasQuizPanelActiveLastFrame = false;

    private void OnEnable()
    {
        trackedImageManager.trackablesChanged.AddListener(OnTrackedImagesChanged);
    }

    private void OnDisable()
    {
        trackedImageManager.trackablesChanged.RemoveListener(OnTrackedImagesChanged);
    }

    private void Update()
    {
        if (quizPanel.activeSelf && !wasQuizPanelActiveLastFrame)
        {
            ShowRandomQuestion();
        }

        wasQuizPanelActiveLastFrame = quizPanel.activeSelf;
    }

    private void OnTrackedImagesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            SetPainting(trackedImage.referenceImage.name);
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            if (trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
            {
                SetPainting(trackedImage.referenceImage.name);
            }
        }
    }

    private void SetPainting(string paintingName)
    {
        if (activePaintingName == paintingName)
        {
            return;
        }

        activePaintingName = paintingName;
        FilterQuestionsForPainting();
        ShowRandomQuestion();
    }

    private void FilterQuestionsForPainting()
    {
        filteredQuestions.Clear();

        foreach (var q in questions)
        {
            if (q.questionForPainting == activePaintingName)
            {
                filteredQuestions.Add(q);
            }
        }
    }

    void ShowRandomQuestion()
    {
        if (filteredQuestions.Count == 0) return;

        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, filteredQuestions.Count);
        }
        while (randomIndex == lastQuestionIndex && filteredQuestions.Count > 1);

        lastQuestionIndex = randomIndex;
        currentQuestion = filteredQuestions[randomIndex];

        questionTextUI.text = currentQuestion.questionText;
        choiceAButton.GetComponentInChildren<TMP_Text>().text = currentQuestion.choiceA;
        choiceBButton.GetComponentInChildren<TMP_Text>().text = currentQuestion.choiceB;

        choiceAButton.onClick.RemoveAllListeners();
        choiceBButton.onClick.RemoveAllListeners();

        choiceAButton.onClick.AddListener(() => OnAnswerSelected(0));
        choiceBButton.onClick.AddListener(() => OnAnswerSelected(1));
    }

    void OnAnswerSelected(int choiceIndex)
    {
        bool isCorrect = (choiceIndex == currentQuestion.correctAnswer);

        feedbackSpawner.SpawnFeedback(isCorrect);
        feedbackPanel.ShowFeedback(isCorrect);

        Invoke(nameof(DisableQuiz), 1.5f);
    }

    void DisableQuiz()
    {
        quizPanel.SetActive(false);
        popUpIcon.SetActive(true);
    }
}
