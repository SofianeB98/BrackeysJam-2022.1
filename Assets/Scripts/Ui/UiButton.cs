using UnityEngine.SceneManagement;
using UnityEngine;

public class UiButton : MonoBehaviour
{
    public bool useGamepad = false;
    [SerializeField] private GameObject Bg_Off;
    [SerializeField] private GameObject Bg_On;
    [SerializeField] private GameObject Text_Off;
    [SerializeField] private GameObject Text_On;
    [SerializeField] private GameObject Handle_Off;
    [SerializeField] private GameObject Handle_On;

    public void Quit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    public void UseGamepad()
    {
        if (useGamepad == false)
        {
            useGamepad = true;
            Bg_On.SetActive(true);
            Text_On.SetActive(true);
            Handle_On.SetActive(true);
            Handle_Off.SetActive(false);
            Text_Off.SetActive(false);
            Bg_Off.SetActive(false);
            var go = GameObject.FindObjectOfType<SceneLoader>();
            go.useGamepad = useGamepad;
        }
        else
        {
            useGamepad = false;
            Bg_On.SetActive(false);
            Text_On.SetActive(false);
            Handle_On.SetActive(false);
            Handle_Off.SetActive(true);
            Text_Off.SetActive(true);
            Bg_Off.SetActive(true);
            var go = GameObject.FindObjectOfType<SceneLoader>();
            go.useGamepad = useGamepad;
        }
    }

    public void Play(int s)
    {
        FindObjectOfType<SceneLoader>().LoadSceneAsync(s);
    }
}