using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthDecision", menuName = "FSM/FSM Decisions/New Health Decision", order = 0)]
public class HealthDecision : FSMDecision
{
    [SerializeField, Range(0f, 100f)] private float m_HealthThresholdPercent = 50.0f;
    [SerializeField] private bool m_CurrentPercentSmallerOrEqualThreshold = true;
    
    public override bool Decide(FSMController fsmController)
    {
        var currentPercent = (fsmController.Boss.Health.CurrentHealth / fsmController.Boss.Health.StartingHealth) * 100.0f;
        if (m_CurrentPercentSmallerOrEqualThreshold)
            return currentPercent <=
                   m_HealthThresholdPercent;

        return currentPercent > m_HealthThresholdPercent;
    }
}
