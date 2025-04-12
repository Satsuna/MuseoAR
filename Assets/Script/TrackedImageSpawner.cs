using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TrackedImageSpawner : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager trackedImageManager;
    [SerializeField] private List<PrefabMapping> prefabMappings; //kesa naman sa ifelse HAHAHA

    private Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> spawnedObjects = new Dictionary<string, GameObject>();

    [System.Serializable]
    public class PrefabMapping
    {
        public string imageName; // Reference Image Library
        public GameObject prefab; // Prefab to spawn
    }

    private void Awake()
    {
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
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
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
            RemovePrefab(trackedImage);
        }
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
            }
        }
    }

    private void UpdatePrefab(ARTrackedImage trackedImage)
    {
        if (spawnedObjects.TryGetValue(trackedImage.referenceImage.name, out GameObject obj))
        {
            obj.transform.position = trackedImage.transform.position;
            obj.transform.rotation = trackedImage.transform.rotation;
        }
    }

    private void RemovePrefab(ARTrackedImage trackedImage)
    {
        if (spawnedObjects.TryGetValue(trackedImage.referenceImage.name, out GameObject obj))
        {
            Destroy(obj);
            spawnedObjects.Remove(trackedImage.referenceImage.name);
        }
    }
}
