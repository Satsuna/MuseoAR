using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject toggleUI;

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
