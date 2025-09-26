using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TrackedImageSpawner : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager trackedImageManager;
    [SerializeField] private List<PrefabMapping> prefabMappings;
    [SerializeField] private ARSession arSession;
    private Dictionary<string, Quiz> activeQuizzes = new Dictionary<string, Quiz>();

    private Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> spawnedObjects = new Dictionary<string, GameObject>();
    public TextMeshProUGUI debug;

    [Serializable]
    public class PrefabMapping
    {
        public string imageName; // Reference Image Library
        public GameObject prefab; // Prefab to spawn
    }

    private void Awake()
    {
        arSession.Reset();
        
        // Convert list to dictionary for quick lookup
        foreach (var mapping in prefabMappings)
        {
            if (!prefabDictionary.ContainsKey(mapping.imageName))
            {
                prefabDictionary[mapping.imageName] = mapping.prefab;
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
            SpawnPrefab(trackedImage);
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            UpdatePrefab(trackedImage);
        }

        foreach (var trackedImage in eventArgs.removed)
        {
            RemovePrefab(trackedImage.Value);
        }
    }

    public void Rescan()
    {
        arSession.Reset();
    }

    private void SpawnPrefab(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        if (prefabDictionary.TryGetValue(imageName, out GameObject prefab))
        {
            if (!spawnedObjects.ContainsKey(imageName))
            {
                GameObject spawnedObject = Instantiate(prefab, trackedImage.transform.position, trackedImage.transform.rotation);
                spawnedObjects[imageName] = spawnedObject;

                Vector2 size = trackedImage.size; // in meters (width, height of the painting)
                float uniformScale = Mathf.Min(size.x, size.y); // pick smaller dimension to preserve aspect ratio
                spawnedObject.transform.localScale = Vector3.one * uniformScale;

                Debug.Log("Spawned a 3d Object! " + spawnedObject);
                debug.text = "Spawned a 3d object " + spawnedObject + " at " + trackedImage.transform.position;
            }
        }
    }

    private void UpdatePrefab(ARTrackedImage trackedImage)
    {
        if (spawnedObjects.TryGetValue(trackedImage.referenceImage.name, out GameObject obj))
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                obj.SetActive(true);
                obj.transform.SetPositionAndRotation(trackedImage.transform.position, trackedImage.transform.rotation);

                Vector2 size = trackedImage.size;
                float uniformScale = Mathf.Min(size.x, size.y);
                obj.transform.localScale = Vector3.one * uniformScale;
            }
        }
    }

    private void RemovePrefab(ARTrackedImage trackedImage)
    {
        if (spawnedObjects.TryGetValue(trackedImage.referenceImage.name, out GameObject obj))
        {
            obj.SetActive(false);
            Destroy(obj);
            spawnedObjects.Remove(trackedImage.referenceImage.name);
        }
    }
}
