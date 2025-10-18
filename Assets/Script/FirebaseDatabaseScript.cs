using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class FirebaseDatabaseScript : MonoBehaviour
{
    private DatabaseReference dbRef;
    private FirebaseAuth auth;

    public bool isAdmin = false;

    [Header("Reference Image Library (for upload)")]
    [SerializeField] private XRReferenceImageLibrary imageLibrary;

    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                dbRef = FirebaseDatabase.DefaultInstance.RootReference;

                if (auth.CurrentUser == null)
                {
                    auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(authTask =>
                    {
                        if (!authTask.IsFaulted && !authTask.IsCanceled)
                        {
                            Debug.Log("Signed in anonymously as " + auth.CurrentUser.UserId);
                            CheckIfAdmin(auth.CurrentUser.UserId);

                            UploadImageList(GetImageNames());
                        }
                    });
                }
                else
                {
                    Debug.Log("Already signed in as " + auth.CurrentUser.UserId);
                    CheckIfAdmin(auth.CurrentUser.UserId);

                    UploadImageList(GetImageNames());
                }
            }
            else
            {
                Debug.LogError("Firebase dependencies not available: " + task.Result);
            }
        });
    }

    private void CheckIfAdmin(string uid)
    {
        dbRef.Child("admin").Child(uid).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && task.Exception == null)
            {
                if (task.Result.Exists)
                {
                    isAdmin = true;
                    Debug.Log("User is admin");
                }
                else
                {
                    isAdmin = false;
                    Debug.Log("User is NOT admin");
                }
            }
        });
    }

    private List<string> GetImageNames()
    {
        List<string> imageNames = new List<string>();
        if (imageLibrary != null)
        {
            foreach (var img in imageLibrary)
            {
                string entry = img.name;
                imageNames.Add(entry);
            }
        }
        return imageNames;
    }

    private void UploadImageList(List<string> images)
    {
        if (dbRef == null) return;

        for (int i = 0; i < images.Count; i++)
        {
            dbRef.Child("images").Child(i.ToString()).SetValueAsync(images[i])
                .ContinueWithOnMainThread(task =>
                {
                    if (task.IsFaulted)
                        Debug.LogError("Upload failed: " + task.Exception);
                    else
                        Debug.Log("Uploaded image: " + images[i]);
                });
        }
    }
}
