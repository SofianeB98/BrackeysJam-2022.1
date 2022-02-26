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
    
    public void UpdateSlider(Health hp)
    {
        if (!m_IsInitialized) Initialize(hp);

        foreach (var s in m_LifeSliders)
        {
            s.value = Mathf.Clamp(hp.CurrentHealth, s.minValue, s.maxValue);
        }
        
        //StartCoroutine("LerpPreviewHp");
    }

    /*IEnumerator LerpPreviewHp()
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
    }*/

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
        }
        
        //m_LifeSliderPreview.maxValue = hp.StartingHealth;
    }

}

