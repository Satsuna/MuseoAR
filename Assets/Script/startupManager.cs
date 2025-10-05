using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class startupManager : MonoBehaviour
{
    public Toggle toggle;
    public Button button;
    public Button tosButton;
    public ScrollRect scrollRect;

    public bool debug = false;

    void Start()
    {
        if (debug == false)
        {
            if (PlayerPrefs.HasKey("HasLaunchedBefore"))
            {
                SceneManager.LoadScene("Authentication");
            }

            else
            {
                PlayerPrefs.SetInt("HasLaunchedBefore", 1);
                PlayerPrefs.Save();
            }
        }
        toggle.interactable = false;
    }

    void Update()
    {
        if (scrollRect.verticalNormalizedPosition <= 0.001f)
        {
            tosButton.interactable = true;
        }
        else
        {
            tosButton.interactable = false;
        }
    }
    
    public void TCToggle()
    {
        if (toggle.isOn == true && PlayerPrefs.HasKey("HasReadTC"))
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }

    public void TOSButton()
    {
        toggle.interactable = true;
        PlayerPrefs.SetInt("HasReadTC", 1);
    }

    public void ToAuthentication()
    {
        SceneManager.LoadScene("Authentication");
    }
}
