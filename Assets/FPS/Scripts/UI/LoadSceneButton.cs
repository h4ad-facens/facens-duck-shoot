using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    public string sceneName = "";

    public void LoadTargetScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
