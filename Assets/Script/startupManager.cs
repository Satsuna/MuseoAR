using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class startupManager : MonoBehaviour
{
    public Toggle toggle;
    public Button button;
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
        SceneManager.LoadScene("Sign In");
    }
}
