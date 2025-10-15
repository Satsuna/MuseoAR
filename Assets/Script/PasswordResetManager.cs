using Firebase;
using Firebase.Auth;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PasswordResetManager : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TextMeshProUGUI statusText;

    private FirebaseAuth auth;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void SendPasswordResetEmail()
    {
        string email = emailInput.text;

        if (string.IsNullOrEmpty(email))
        {
            statusText.text = "Please enter your email address.";
            return;
        }

        auth.SendPasswordResetEmailAsync(email).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SendPasswordResetEmailAsync was canceled.");
                statusText.text = "Password reset request canceled.";
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SendPasswordResetEmailAsync encountered an error: " + task.Exception);
                if (task.Exception.GetBaseException() is FirebaseException firebaseEx)
                {
                    AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                    switch (errorCode)
                    {
                        case AuthError.UserNotFound:
                            statusText.text = "No account found with that email.";
                            break;
                        case AuthError.InvalidEmail:
                            statusText.text = "Invalid email address format.";
                            break;
                        default:
                            statusText.text = "Error sending reset email.";
                            break;
                    }
                }
                else
                {
                    statusText.text = "Error sending reset email.";
                }
                return;
            }

            statusText.text = "Password reset email sent successfully! Check your inbox.";
        });

        statusText.text = "Password reset email sent successfully! Check your inbox.";
    }
}