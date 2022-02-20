using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private float m_FadeTime = 0.5f;
    [SerializeField] private float m_FadeWaitTime = 0.5f;
    private Fader m_Fader = null; 
    private Coroutine m_LoadSceneCoroutine = null;

    private WaitForSeconds m_WaitFadeWaitTime = null;
    
    private void Start()
    {
        m_Fader = FindObjectOfType<Fader>();
        m_WaitFadeWaitTime = new WaitForSeconds(m_FadeWaitTime);
    }
    
    public void LoadSceneAsync(int idx)
    {
        if (m_LoadSceneCoroutine != null)
            StopCoroutine(m_LoadSceneCoroutine);

        m_LoadSceneCoroutine = StartCoroutine(LoadSceneAsyncCoroutine(idx));
    }

    private IEnumerator LoadSceneAsyncCoroutine(int idx)
    {
        yield return m_Fader.FadeOut(m_FadeTime);
        
        AsyncOperation op = SceneManager.LoadSceneAsync(idx);
        op.allowSceneActivation = false;

        while (op.progress < 0.85f)
        {
            yield return null;
        }

        op.allowSceneActivation = true;

        yield return m_WaitFadeWaitTime;
        yield return m_Fader.FadeIn(m_FadeTime);
        
        yield break;
    }
}
