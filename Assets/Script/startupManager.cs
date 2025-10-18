using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class startupManager : MonoBehaviour
{
    public Toggle toggle;
    public Button button;
    public Button tosButton;
    public ScrollRect scrollRect;
    public GameObject warning;

    private void Awake() {
        Application.targetFrameRate = 60;    
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("HasReadTC"))
        {
            toggle.interactable = true;
            toggle.isOn = true;
            warning.SetActive(false);
        }
        else
        {
            toggle.interactable = false;
        }
    }

    void Update()
    {
        if (scrollRect.verticalNormalizedPosition <= 0.001f)
        {
            tosButton.interactable = true;
            warning.SetActive(false);
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
