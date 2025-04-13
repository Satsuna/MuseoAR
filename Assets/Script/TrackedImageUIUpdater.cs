using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TrackedImageUIUpdater : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager trackedImageManager;
    [SerializeField] private List<ImageTextMapping> imageTextMappings;
    [SerializeField] private TextMeshProUGUI uiText;

    private Dictionary<string, string> textDictionary = new Dictionary<string, string>();

    [System.Serializable]
    public class ImageTextMapping
    {
        public string imageName;
        [TextArea] public string displayText;
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
}
