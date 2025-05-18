using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class startupManager : MonoBehaviour
{
    public Toggle toggle;
    public Button button;
    public GameObject getStarted;
    public GameObject selectLanguage;

    void Start()
    {
        if (PlayerPrefs.HasKey("ChangeLanguage"))
        {
            getStarted.SetActive(false);
            selectLanguage.SetActive(true);
        }

        if (PlayerPrefs.HasKey("HasLaunchedBefore"))
        {
            SceneManager.LoadScene("Sign In");
        }

        else
        {
            PlayerPrefs.SetInt("HasLaunchedBefore", 1);
            PlayerPrefs.Save();
        }
    }
    public void TCToggle()
    {
        if(toggle.isOn == true) {
            button.interactable = true;
        }
        else {
            button.interactable = false;
        }
    }

    public void changeScene() {
        if (PlayerPrefs.HasKey("ChangeLanguage"))
        {
            SceneManager.LoadScene("Camera");
            PlayerPrefs.DeleteKey("ChangeLanguage");
        }
        else
        {
            SceneManager.LoadScene("Sign In");
        }
    }
}
