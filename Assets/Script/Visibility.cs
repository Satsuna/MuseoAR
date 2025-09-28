using TMPro;
using UnityEngine;

public class Visibility : MonoBehaviour
{
    public TMP_InputField password;
    public void OnButtonClick()
    {
        if (password.contentType == TMP_InputField.ContentType.Password)
        {
            password.contentType = TMP_InputField.ContentType.Standard;
        }
        else
        {
            password.contentType = TMP_InputField.ContentType.Password;
        }

        password.ForceLabelUpdate();
    }
}
