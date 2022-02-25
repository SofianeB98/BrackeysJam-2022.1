using UnityEngine;

[CreateAssetMenu(fileName = "DistanceFromPlayerDecision", menuName = "FSM/FSM Decisions/New DistanceFromPlayerDecision", order = 0)]
public class DistanceFromPlayerDecision : FSMDecision
{
    [SerializeField, Range(0f, 100f)] private float m_DistanceFromPlayer = 10.0f;
    [SerializeField] private bool m_CurrentDistanceSmallerOrEqual = true;
    
    public override bool Decide(FSMController fsmController)
    {
        var currentDistance = Vector3.Distance(fsmController.Boss.transform.position, fsmController.Boss.Target.position);
        if (m_CurrentDistanceSmallerOrEqual)
            return currentDistance <=
                   m_DistanceFromPlayer;

        return currentDistance > m_DistanceFromPlayer;
    }
}