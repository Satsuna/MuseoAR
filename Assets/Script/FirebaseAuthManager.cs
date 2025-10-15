using System.Collections;
using UnityEngine;
using TMPro;
using Firebase.Extensions;
using Firebase.Auth;
using Firebase;
using Firebase.Database;
using UnityEngine.SceneManagement;

public class FirebaseAuthManager : MonoBehaviour
{
    #region variables
    [Header("Login")]
    public TMP_InputField LoginEmail;
    public TMP_InputField loginPassword;

    [Header("Sign up")]
    public TMP_InputField SignupEmail;
    public TMP_InputField SignupPassword;
    public TMP_InputField SignupPasswordConfirm;

    [Header("Extra")]
    public GameObject loadingScreen;
    public TextMeshProUGUI logTxt;
    public GameObject parent;
    #endregion

    #region signup 
    public void SignUp()
    {
        logTxt.text = "";
        loadingScreen.SetActive(true);

        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        string email = SignupEmail.text;
        string password = SignupPassword.text;
        string confirmPassword = SignupPasswordConfirm.text;

        if (password != confirmPassword)
        {
            loadingScreen.SetActive(false);
            showLogMsg("Password does not match!");
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                return;
            }
            if (task.IsFaulted)
            {
                loadingScreen.SetActive(false);
                FirebaseException firebaseEx = task.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                showLogMsg("Sign up failed: " + errorCode.ToString());
                return;
            }

            loadingScreen.SetActive(false);
            AuthResult result = task.Result;

            SignupEmail.text = "";
            SignupPassword.text = "";
            SignupPasswordConfirm.text = "";

            FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;
            if (user != null)
            {
                user.SendEmailVerificationAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsFaulted)
                    {
                        FirebaseException firebaseEx = task.Exception.GetBaseException() as FirebaseException;
                        AuthError error = (AuthError)firebaseEx.ErrorCode;
                        showLogMsg("Email send failed: " + error.ToString());
                    }
                    else
                    {
                        showLogMsg("Verification email sent! Please check your inbox.");
                    }
                });
            }
            else
            {
                showLogMsg("Error: No user found to send verification.");
            }


            // Check if user has data in database; if not, go to "User Data"
            FirebaseDatabase.DefaultInstance
                .GetReference("users")
                .Child(result.User.UserId)
                .Child("data")
                .GetValueAsync()
                .ContinueWithOnMainThread(dbTask =>
                {
                    if (dbTask.IsFaulted)
                    {
                        Debug.LogError("Database check failed: " + dbTask.Exception);
                        return;
                    }

                    DataSnapshot snapshot = dbTask.Result;
                    if (snapshot == null || !snapshot.Exists)
                    {
                        SceneManager.LoadScene("User Data");
                    }
                    else
                    {
                        SceneManager.LoadScene("Camera");
                    }
                });
        });
    }

    public void SendEmailVerification() {
        StartCoroutine(SendEmailForVerificationAsync());
    }

    IEnumerator SendEmailForVerificationAsync() {
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;
        if (user!=null)
        {
            var sendEmailTask = user.SendEmailVerificationAsync();
            yield return new WaitUntil(() => sendEmailTask.IsCompleted);

            if (sendEmailTask.Exception != null)
            {
                FirebaseException firebaseException = sendEmailTask.Exception.GetBaseException() as FirebaseException;
                AuthError error = (AuthError)firebaseException.ErrorCode;
                showLogMsg("Email send failed: " + error.ToString());
            }
            else {
                showLogMsg("Verification email sent!");
            }
        }
    }
    #endregion

    #region Login
    public void Login() {
        loadingScreen.SetActive(true);

        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        string email = LoginEmail.text;
        string password = loginPassword.text;

        Credential credential = EmailAuthProvider.GetCredential(email, password);
        auth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                return;
            }
            if (task.IsFaulted)
            {
                loadingScreen.SetActive(false);
                FirebaseException firebaseEx = task.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                showLogMsg("Login failed: " + errorCode.ToString());
                return;
            }

            loadingScreen.SetActive(false);
            AuthResult result = task.Result;

            PlayerPrefs.SetInt("LoggedIn", 1);
            PlayerPrefs.Save();

            // ✅ Check user data before deciding which scene
            FirebaseDatabase.DefaultInstance
                .GetReference("users")
                .Child(result.User.UserId)
                .Child("data")
                .GetValueAsync()
                .ContinueWithOnMainThread(dbTask =>
                {
                    if (dbTask.IsFaulted)
                    {
                        Debug.LogError("Database check failed: " + dbTask.Exception);
                        SceneManager.LoadScene("User Data");
                        return;
                    }

                    DataSnapshot snapshot = dbTask.Result;
                    if (snapshot == null || !snapshot.Exists)
                    {
                        SceneManager.LoadScene("User Data");
                    }
                    else
                    {
                        SceneManager.LoadScene("Camera");
                    }
                });

            if (!result.User.IsEmailVerified)
            {
                showLogMsg("Please verify email!!");
            }
        });       
    }
    #endregion

    #region Start
    void Start()
    {
        AutoLogin();
    }
    #endregion

    public void SignOut() {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        auth.SignOut();
        PlayerPrefs.DeleteKey("LoggedIn");
        SceneManager.LoadScene("Authentication");
    }

    #region extra
    void showLogMsg(string msg)
    {
        logTxt.text = msg;
        logTxt.GetComponent<Animation>().Play("textFadeout");
    }
    #endregion

    void AutoLogin()
    {
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;

        if (user != null)
        {
            SceneManager.LoadScene("Camera");
        }
        else
        {
            parent.SetActive(true);
        }
    }
}
