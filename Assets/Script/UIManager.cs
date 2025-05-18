using Firebase.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject toggleUI;

    public void Toggle()
    {
        if (toggleUI.activeSelf == false)
        {
            toggleUI.SetActive(true);

        }
        else
        {
            toggleUI.SetActive(false);
        }
    }
}
