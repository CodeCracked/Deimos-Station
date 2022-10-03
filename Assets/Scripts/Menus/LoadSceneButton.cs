using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    public string SceneName = "Station";

    public void OnClick()
    {
        SceneManager.LoadScene(SceneName);
    }
}
