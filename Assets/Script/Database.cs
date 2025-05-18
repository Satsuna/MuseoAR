using Firebase.Database;
using TMPro;
using UnityEngine;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine.SceneManagement;
using Firebase;

public class Database : MonoBehaviour
{
    public TextMeshProUGUI Gender;
    public TMP_InputField Nationality;
    public GameObject loadingScreen;
    public GameObject parent;

    void Awake() {
        parent.SetActive(false);
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available) {
                FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri("https://museoar-ace55-default-rtdb.asia-southeast1.firebasedatabase.app/");
                Debug.Log("Firebase ready!");
                CheckProfile();
            } else {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }

    public void NextButton() {
        if (Nationality.text.Length != 0) {
            loadingScreen.SetActive(true);

            FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;
            if (user != null) {
                Data Data= new Data();
                Data.Gender = Gender.text;
                Data.Nationality = Nationality.text;

                WriteNewUser(user.UserId, Data.Gender, Data.Nationality);
                SceneManager.LoadScene("Camera");
            }
            else {
                Debug.LogError("Error");
            }

        }
        loadingScreen.SetActive(false);
    }

    private void WriteNewUser(string userId, string nationality, string gender) {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        string nat = Nationality.text;
        string gen = Gender.text;
        Data Data= new Data(nat, gen);
        string json = JsonUtility.ToJson(Data);

        reference.Child("users").Child(userId).Child("data").SetRawJsonValueAsync(json);
    }

        void CheckProfile() {
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("users").Child(user.UserId).Child("data");
        
        reference.GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted) {
                Debug.LogError("Error reading user data: " + task.Exception);
                loadingScreen.SetActive(false);
                return;
            }

            DataSnapshot snapshot = task.Result;
            if (snapshot.Exists)
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
