using UnityEngine;

public class FeedbackSpawner : MonoBehaviour
{
    public GameObject feedbackPrefab;
    private GameObject currentFeedback;

    public GameObject SpawnFeedback(bool isCorrect)
    {
        if (currentFeedback != null)
        {
            Destroy(currentFeedback);
        }

        currentFeedback = Instantiate(feedbackPrefab, transform);

        FeedbackPanel feedback = currentFeedback.GetComponent<FeedbackPanel>();
        feedback.ShowFeedback(isCorrect);

        return currentFeedback;
    }
}
