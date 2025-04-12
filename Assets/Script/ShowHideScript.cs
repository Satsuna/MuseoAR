using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShowHideScript : MonoBehaviour
{
    public GameObject uiActive;
    public GameObject uiInactive;
    public GameObject toggleUI;

    public void onClick() {
        uiActive.SetActive(true);
        uiInactive.SetActive(false);
    }

    public void changeScene() {
        SceneManager.LoadScene(1);
    }

    public void Toggle() {
        if (toggleUI.activeSelf == false) {
            toggleUI.SetActive(true);
        }
        else {
            toggleUI.SetActive(false);
        }
    }
}
