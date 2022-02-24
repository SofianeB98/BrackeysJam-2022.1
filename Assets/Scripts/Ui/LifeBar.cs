using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    [SerializeField] private Slider m_LifeSlider;
    private bool m_IsInitialized = false;
    
    public void UpdateSlider(Health hp)
    {
        if (!m_IsInitialized) Initialize(hp);

        m_LifeSlider.value = Mathf.Clamp(hp.CurrentHealth, 0f, hp.StartingHealth);
    }

    private void Initialize(Health hp)
    {
        m_IsInitialized = true;
        m_LifeSlider.maxValue = hp.StartingHealth;
    }
}
