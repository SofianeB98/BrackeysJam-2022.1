using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "DashToTargetSubPatternAction",
    menuName = "FSM/FSM Pattern Actions/New DashToTargetSubPatternAction", order = 0)]
public class DashToTargetSubPatternAction : SubPatternAction
{
    private const float RANGE_RANDOM = 2.0f;

    private Vector3 m_PointToReach = Vector3.zero;
    private float m_DashDuration = 1.0f;
    private Vector3 m_StartPos = Vector3.zero;
    private float m_CurrentDuration = 0f;
    private Vector3 m_Direction = Vector3.forward;
    private float m_CurrentLoadDuration = 0f;
    private float m_Distance = 0f;
    private Vector3 m_NewPos = Vector3.zero;

    public override void OnEnter(FSMController fsmController)
    {
        var dashData = fsmController.Boss.DashData;
        m_CurrentLoadDuration = Time.time + dashData.DashLoadDuration;
        m_StartPos = fsmController.Boss.transform.position;
        RandomPoint(fsmController.Boss.Target.position, RANGE_RANDOM, fsmController, out m_PointToReach);
        m_Direction = m_PointToReach - m_StartPos;
        m_Direction.y = 0f;
        m_Distance = m_Direction.magnitude;
        m_Direction.Normalize();
        m_DashDuration = m_Distance / dashData.DashSpeed;
        m_CurrentDuration = 0f;
        m_NewPos = m_StartPos;
        fsmController.Boss.transform.rotation = Quaternion.LookRotation(m_Direction, Vector3.up);
        fsmController.Boss.DashVFX.SetActive(true);
    }

    public override SubPatternActionState Execute(FSMController fsmController)
    {
        if (m_CurrentDuration >= m_DashDuration || m_Distance <= 0.1f)
        {
            var dir = (fsmController.Boss.Target.position - fsmController.Boss.transform.position);
            dir.y = 0;
            dir.Normalize();
            fsmController.Boss.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
            fsmController.Boss.DashVFX.SetActive(false);
            return SubPatternActionState.ENDED;
        }

        if (m_CurrentLoadDuration > Time.time)
            return SubPatternActionState.PERFORMED;

        m_CurrentDuration += Time.deltaTime;
        m_NewPos += m_Direction * (fsmController.Boss.DashData.DashSpeed * Time.deltaTime);
        if (Vector3.Distance(m_NewPos, m_StartPos) > m_Distance)
        {
            m_NewPos = m_PointToReach;
            m_CurrentDuration = m_DashDuration + 0.1f;
        }

        fsmController.transform.position = m_NewPos;

        return SubPatternActionState.PERFORMED;
    }

    public override void OnEnd(FSMController fsmController)
    {
        m_CurrentDuration = 0f;
        fsmController.Boss.DashVFX.SetActive(false);
    }

    private void RandomPoint(Vector3 center, float range, FSMController fsmController, out Vector3 result)
    {
        for (int i = 0; i < 100; i++)
        {
            var rdm = Random.insideUnitCircle;
            var rdm3D = new Vector3(rdm.x + 0.2f * Mathf.Sign(rdm.x), 0f, rdm.y + 0.2f * Mathf.Sign(rdm.y));
            Vector3 randomPoint = center + rdm3D * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 0.5f, NavMesh.AllAreas))
            {
                result = hit.position;
                var nvp = new NavMeshPath();
                fsmController.Boss.Agent.CalculatePath(result, nvp);
                result = nvp.corners[nvp.corners.Length - 1];
                return;
            }
        }

        result = center;
    }
}