using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation;
using Firebase.Auth;
using UnityEngine.SceneManagement;
using Firebase.Database;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class TrackedImageUIUpdater : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager trackedImageManager;
    [SerializeField] private List<ImageTextMapping> imageTextMappings;
    [SerializeField] private TextMeshProUGUI paintingNameText;
    [SerializeField] private TextMeshProUGUI painterNameText;
    [SerializeField] private TextMeshProUGUI uiText;
    public GameObject moreInformationGameObject;
    public GameObject moreInformationText;
    public GameObject moreInformationOpen;

    public TMP_InputField feedbackText;
    public GameObject indicator;
    public Button m_button;

    [SerializeField] private LocalizedString defaultScanMessage;


    private Dictionary<string, LocalizedString> textDictionary = new Dictionary<string, LocalizedString>();

    private Dictionary<string, LocalizedString> paintingNameDictionary = new Dictionary<string, LocalizedString>();
    private Dictionary<string, LocalizedString> painterNameDictionary = new Dictionary<string, LocalizedString>();

    [System.Serializable]
    public class ImageTextMapping
    {
        public string imageName;
        public LocalizedString paintingName;
        public LocalizedString painterName;
        public LocalizedString localizedText;
    }

    private void Start()
    {
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
                textDictionary[mapping.imageName] = mapping.localizedText;
            }
        }

        foreach (var mapping in imageTextMappings)
        {
            if (!paintingNameDictionary.ContainsKey(mapping.imageName))
                paintingNameDictionary[mapping.imageName] = mapping.paintingName;

            if (!painterNameDictionary.ContainsKey(mapping.imageName))
                painterNameDictionary[mapping.imageName] = mapping.painterName;
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
            moreInformationGameObject.SetActive(true);
            moreInformationText.SetActive(false);
            moreInformationOpen.SetActive(false);
            if (!moreInformationGameObject.activeSelf)
            {
                indicator.SetActive(true);
            }
            else
            {
                RemoveIndicator();
            }

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

        if (textDictionary.TryGetValue(imageName, out LocalizedString localizedText))
        {
            localizedText.StringChanged += (value) =>
            {
                uiText.text = value;
            };
        }

        if (paintingNameDictionary.TryGetValue(imageName, out LocalizedString paintingName))
        {
            paintingName.StringChanged += (value) =>
            {
                paintingNameText.text = value;
            };
        }

        if (painterNameDictionary.TryGetValue(imageName, out LocalizedString painterName))
        {
            painterName.StringChanged += (value) =>
            {
                painterNameText.text = value;
            };
        }
    }

    private void ResetUIText()
    {
        defaultScanMessage.StringChanged += (value) =>
        {
            uiText.text = value;
        };
    }

    public void SignOut()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        auth.SignOut();
        PlayerPrefs.DeleteKey("LoggedIn");
        SceneManager.LoadScene("Authentication");
    }

    private void SubmitFeedback(string userId, string feedback)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        Feedback feedback1 = new Feedback(feedback);
        string json = JsonUtility.ToJson(feedback1);

        reference.Child("users").Child(userId).Child("data").Child("feedback").Push().SetRawJsonValueAsync(json);
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

    public Feedback() { }

    public Feedback(string content)
    {
        feedback = content;
    }
}
