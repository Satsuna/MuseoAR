using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FeedbackPanel : MonoBehaviour
{
    public Image backgroundImage;
    public TMP_Text feedbackText;

    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;

    public void ShowFeedback(bool isCorrect)
    {
        if (isCorrect)
        {
            backgroundImage.color = correctColor;
            feedbackText.text = "Correct!";
        }
        else
        {
            backgroundImage.color = wrongColor;
            feedbackText.text = "Wrong!";
        }
    }
}
