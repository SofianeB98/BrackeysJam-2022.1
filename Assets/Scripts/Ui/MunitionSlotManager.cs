using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MunitionSlotManager : MonoBehaviour
{
    [SerializeField] private Image[] m_Slots;
    [SerializeField] private int m_ThresholdEmpty = 5;
    [SerializeField] private Color m_CloseToEmptyColor = Color.red;
    [SerializeField] private Color m_DefaultColor = Color.white;
    [SerializeField] private Color m_EmptyColor = Color.black;

    private bool m_DefautlSelected = true;
    
    private void OnEnable()
    {
        UpdateColor(15);
        
        CharacterEvents.ProjectileAdded += AddSlot;
        CharacterEvents.ProjectileRemoved += RemoveSlot;
    }

    private void OnDisable()
    {
        CharacterEvents.ProjectileAdded -= AddSlot;
        CharacterEvents.ProjectileRemoved -= RemoveSlot;
    }

    private void RemoveSlot(int idx)
    {
        m_Slots[idx].color = m_EmptyColor;
        
        if (idx <= m_ThresholdEmpty && m_DefautlSelected)
        {
            m_DefautlSelected = false;
            UpdateColor(idx);
        }
        else if (idx > m_ThresholdEmpty && !m_DefautlSelected)
        {
            m_DefautlSelected = true;
            UpdateColor(idx);
        }
    }
    
    private void AddSlot(int idx)
    {
        m_Slots[idx - 1].color = m_DefautlSelected ? m_DefaultColor : m_CloseToEmptyColor;
        
        if (idx <= m_ThresholdEmpty && m_DefautlSelected)
        {
            m_DefautlSelected = false;
            UpdateColor(idx);
        }
        else if (idx > m_ThresholdEmpty && !m_DefautlSelected)
        {
            m_DefautlSelected = true;
            UpdateColor(idx);
        }
    }

    private void UpdateColor(int idx)
    {
        for (int i = 0; i < idx; ++i)
        {
            m_Slots[i].color = m_DefautlSelected ? m_DefaultColor : m_CloseToEmptyColor;
        }
    }
}
