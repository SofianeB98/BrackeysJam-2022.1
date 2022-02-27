using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossLifeBar : MonoBehaviour
{
    [SerializeField] private Slider[] m_LifeSliders;
    [SerializeField] private Slider[] m_LifeSliderPreview;
    [SerializeField] private AnimationCurve easeLerp;
    [SerializeField] float waitTime = 2f;
    private bool m_IsInitialized = false;

    private Coroutine m_Co;
    
    public void UpdateSlider(Health hp)
    {
        if (!m_IsInitialized) Initialize(hp);

        foreach (var s in m_LifeSliders)
        {
            s.value = Mathf.Clamp(hp.CurrentHealth, s.minValue, s.maxValue);
        }

        if (m_Co != null)
            StopCoroutine(m_Co);
        
        m_Co = StartCoroutine(LerpPreviewHp());
    }

    IEnumerator LerpPreviewHp()
    {
        float elapsedTime = 0;
        
        while (elapsedTime < waitTime)
        {
            for (int i = 0; i < m_LifeSliderPreview.Length; i++) 
            {
                var p = m_LifeSliderPreview[i];
                if (p == null)
                    continue;
                    
                p.value = Mathf.Lerp(p.value, m_LifeSliders[i].value, easeLerp.Evaluate(elapsedTime / waitTime));
            }

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        yield return null;
    }

    private void Initialize(Health hp)
    {
        m_IsInitialized = true;


        int seq = m_LifeSliders.Length;
        float delta = hp.StartingHealth / seq;

        for (int i = 0; i < seq; i++)
        {
            m_LifeSliders[i].maxValue = hp.StartingHealth - delta * i;
            m_LifeSliders[i].minValue = hp.StartingHealth - delta * (i + 1);

            if (i + 1 >= seq)
                m_LifeSliders[i].minValue = 0;

            m_LifeSliderPreview[i].maxValue = m_LifeSliders[i].maxValue;
            m_LifeSliderPreview[i].minValue = m_LifeSliders[i].minValue;
        }
    }
}