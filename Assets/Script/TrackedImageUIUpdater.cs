using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation;
using Firebase.Auth;
using UnityEngine.SceneManagement;
using Firebase.Database;
using UnityEngine.UI;

public class TrackedImageUIUpdater : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager trackedImageManager;
    [SerializeField] private List<ImageTextMapping> imageTextMappings;
    [SerializeField] private TextMeshProUGUI uiText;

    public TMP_InputField feedbackText;
    public GameObject indicator;
    public Button m_button;

    private Dictionary<string, string> textDictionary = new Dictionary<string, string>();

    [System.Serializable]
    public class ImageTextMapping
    {
        public string imageName;
        [TextArea] public string displayText;
    }

    private void Start() {
        m_button.onClick.AddListener(RemoveIndicator);
    }

    public void RemoveIndicator()
    {
        indicator.SetActive(false);
    }

    private void Awake()
    {
        // Populate dictionary for quick lookup
        foreach (var mapping in imageTextMappings)
        {
            if (!textDictionary.ContainsKey(mapping.imageName))
            {
                textDictionary[mapping.imageName] = mapping.displayText;
            }
        }
    }

    private void OnEnable()
    {
        trackedImageManager.trackablesChanged.AddListener(OnTrackedImagesChanged);
    }

    private void OnDisable()
    {
        trackedImageManager.trackablesChanged.RemoveListener(OnTrackedImagesChanged);
    }

    private void OnTrackedImagesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            UpdateUIText(trackedImage);
            indicator.SetActive(true);
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            UpdateUIText(trackedImage);
        }

        foreach (var trackedImage in eventArgs.removed)
        {
            ResetUIText();
        }
    }

    private void UpdateUIText(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        if (textDictionary.TryGetValue(imageName, out string displayText))
        {
            uiText.text = displayText;
        }
    }

    private void ResetUIText()
    {
        uiText.text = "Scan an image...";
    }

    public void SignOut()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        auth.SignOut();
        PlayerPrefs.DeleteKey("LoggedIn");
        SceneManager.LoadScene("Sign In");
    }
    public void ChangeLanguage()
    {
        PlayerPrefs.SetInt("ChangeLanguage", 1);
        SceneManager.LoadScene("Startup");
    }

    private void SubmitFeedback(string userId, string feedback)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        string Content = feedback;

        Feedback feedback1 = new Feedback(Content);
        string json = JsonUtility.ToJson(feedback1);

        reference.Child("users").Child(userId).Child("data").Child("feedback").SetRawJsonValueAsync(json);
    }

    public void Submit()
    {
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;
        SubmitFeedback(user.UserId, feedbackText.text);
    }
}

[System.Serializable]
public class Feedback
{
    public string feedback;

    public Feedback() {

    }

    public Feedback(string content) {
        feedback = content;
    }
}
