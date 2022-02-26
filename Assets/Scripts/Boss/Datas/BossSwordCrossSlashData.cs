using UnityEngine;

[CreateAssetMenu(fileName = "BossSwordCrossSlashData", menuName = "Boss/Behaviors Data/New BossSwordCrossSlashData",
    order = 0)]
public class BossSwordCrossSlashData : ScriptableObject
{
    [Header("Attack")] 
    public float DamageFirstSlash = 10.0f;
    public float DamageSecondSlash = 10.0f;

    [Header("Detection")] 
    public SlashDetectionParameter[] SlashDetectionParameters;
}

[System.Serializable]
public struct SlashDetectionParameter
{
    public Vector3 m_OffsetPosition;
    public Vector3 m_SlashBoxDetection;
}