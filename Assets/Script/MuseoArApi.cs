using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Painting
{
    public int id;
    public string title;
    public string artist;
    public string image_url;
    public string model_url;
    public string texture_url;
    public string marker_url;
    public float ar_scale;
    public float ar_rotation_x;
    public float ar_rotation_y;
    public float ar_rotation_z;
}

[System.Serializable]
public class PaintingList
{
    public Painting[] paintings;
}

[System.Serializable]
public class ScanData
{
    public int painting_id;
    public string user_id;
    public string device_info;
    public string location_info;
    public int scan_duration;
}

public class MuseoARAPI : MonoBehaviour
{
    private string baseUrl = "http://localhost:3000/api/unity";
    
    void Start()
    {
        // Load paintings on app start
        StartCoroutine(GetPaintings());
    }
    
    public IEnumerator GetPaintings()
    {
        UnityWebRequest www = UnityWebRequest.Get(baseUrl + "/paintings");
        yield return www.SendWebRequest();
        
        if (www.result == UnityWebRequest.Result.Success)
        {
            string json = "{\"paintings\":" + www.downloadHandler.text + "}";
            PaintingList paintingList = JsonUtility.FromJson<PaintingList>(json);
            
            // Process paintings
            foreach (Painting painting in paintingList.paintings)
            {
                Debug.Log($"Loaded: {painting.title} by {painting.artist}");
                // Download and set up AR content
                StartCoroutine(LoadPaintingAssets(painting));
            }
        }
        else
        {
            Debug.LogError("Failed to load paintings: " + www.error);
        }
    }
    
    public IEnumerator LoadPaintingAssets(Painting painting)
    {
        // Load 3D model
        if (!string.IsNullOrEmpty(painting.model_url))
        {
            yield return StartCoroutine(DownloadModel(painting.model_url));
        }
        
        // Load texture
        if (!string.IsNullOrEmpty(painting.texture_url))
        {
            yield return StartCoroutine(DownloadTexture(painting.texture_url));
        }
        
        // Load AR marker
        if (!string.IsNullOrEmpty(painting.marker_url))
        {
            yield return StartCoroutine(DownloadMarker(painting.marker_url));
        }
    }
    
    private IEnumerator DownloadModel(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        
        if (www.result == UnityWebRequest.Result.Success)
        {
            byte[] modelData = www.downloadHandler.data;
            // Process 3D model data
            Debug.Log("Model downloaded successfully");
        }
    }
    
    private IEnumerator DownloadTexture(string url)
{
    UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
    yield return www.SendWebRequest();
    
    if (www.result == UnityWebRequest.Result.Success)
    {
        Texture2D texture = DownloadHandlerTexture.GetContent(www);
        Debug.Log("Texture downloaded successfully");
    }
    else
    {
        Debug.LogError("Failed to download texture: " + www.error);
    }
}

private IEnumerator DownloadMarker(string url)
{
    UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
    yield return www.SendWebRequest();
    
    if (www.result == UnityWebRequest.Result.Success)
    {
        Texture2D marker = DownloadHandlerTexture.GetContent(www);
        Debug.Log("AR marker downloaded successfully");
    }
    else
    {
        Debug.LogError("Failed to download marker: " + www.error);
    }
}
    
    public void RecordScan(int paintingId, int duration)
    {
        ScanData scanData = new ScanData
        {
            painting_id = paintingId,
            user_id = SystemInfo.deviceUniqueIdentifier,
            device_info = SystemInfo.deviceModel,
            location_info = "Museum", // You can implement GPS location here
            scan_duration = duration
        };
        
        StartCoroutine(SendScanData(scanData));
    }
    
    private IEnumerator SendScanData(ScanData scanData)
    {
        string json = JsonUtility.ToJson(scanData);
        
        UnityWebRequest www = new UnityWebRequest(baseUrl + "/scan", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        
        yield return www.SendWebRequest();
        
        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Scan data recorded successfully");
        }
        else
        {
            Debug.LogError("Failed to record scan: " + www.error);
        }
    }
}