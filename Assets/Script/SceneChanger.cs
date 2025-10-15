using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneName;
    public void SceneChange()
    {
        SceneManager.LoadScene(sceneName);
    }
}
