using UnityEngine.SceneManagement;
using UnityEngine;

public class UiButton : MonoBehaviour
{
    public void Quit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    public void Options()
    {

    }

    public void Play(int s)
    {
        FindObjectOfType<SceneLoader>().LoadSceneAsync(s);
    }
}