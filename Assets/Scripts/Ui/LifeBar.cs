using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    [SerializeField] private Slider m_LifeSlider;
    [SerializeField] private Slider m_LifeSliderPreview;
    [SerializeField] private AnimationCurve easeLerp;
    [SerializeField] float waitTime = 2f;
    private bool m_IsInitialized = false;
    
    private Coroutine m_Co;
    
    public void UpdateSlider(Health hp)
    {
        if (!m_IsInitialized) Initialize(hp);

        m_LifeSlider.value = Mathf.Clamp(hp.CurrentHealth, 0f, hp.StartingHealth);
        if (m_Co != null)
            StopCoroutine(m_Co);
        
        m_Co = StartCoroutine(LerpPreviewHp());
    }

    IEnumerator LerpPreviewHp()
    {
        float elapsedTime = 0;
        float waitTime = 1;
        
        while (elapsedTime < waitTime)
        {
            if (m_LifeSliderPreview != null)
                m_LifeSliderPreview.value = Mathf.Lerp(m_LifeSliderPreview.value, m_LifeSlider.value, easeLerp.Evaluate(elapsedTime / waitTime));

            elapsedTime += Time.deltaTime;

            yield return null;
        }
        yield return null;
    }

    private void Initialize(Health hp)
    {
        m_IsInitialized = true;
        m_LifeSlider.maxValue = hp.StartingHealth;
        m_LifeSliderPreview.maxValue = hp.StartingHealth;
    }
}
