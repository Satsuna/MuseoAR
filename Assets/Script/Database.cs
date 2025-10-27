using Firebase.Database;
using TMPro;
using UnityEngine;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine.SceneManagement;
using Firebase;

public class Database : MonoBehaviour
{
    public TMP_InputField Age;
    public TMP_InputField Nationality;
    public GameObject loadingScreen;
    public GameObject parent;

    void Awake() {
        parent.SetActive(false);
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available) {
                CheckProfile();
            } else {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }

    public void NextButton() {
        if (Nationality.text.Length != 0 && Age.text.Length != 0) {
            loadingScreen.SetActive(true);

            FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;
            if (user != null) {
                Data Data = new Data();
                Data.Age = int.Parse(Age.text);
                Data.Nationality = Nationality.text;

                WriteNewUser(user.UserId, Data.Nationality, Data.Age);
                SceneManager.LoadScene("Camera");
            }
            else {
                Debug.LogError("Error");
            }

        }
        loadingScreen.SetActive(false);
    }

    private void WriteNewUser(string userId, string nationality, int age) {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        string nat = Nationality.text;
        int ag = int.Parse(Age.text);
        Data Data = new Data(nat, ag);
        string json = JsonUtility.ToJson(Data);

        reference.Child("users").Child(userId).Child("data").SetRawJsonValueAsync(json);
    }

    void CheckProfile() {
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;
        string uid = user.UserId;
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("users").Child(user.UserId).Child("data");
        
        reference.GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted) {
                Debug.LogError("Error reading user data: " + task.Exception);
                loadingScreen.SetActive(false);
                return;
            }

            DataSnapshot snapshot = task.Result;
            if (snapshot.HasChild("Age"))
            {
                SceneManager.LoadScene("Camera");
            }
            else
            {
                loadingScreen.SetActive(false);
                parent.SetActive(true);
            }            
        });
    }
}

[System.Serializable]
public class Data 
{
    public string Nationality;
    public int Age;

    public Data() {
    }

    public Data(string Nationality, int Age) {
        this.Nationality = Nationality;
        this.Age = Age;
    }
}