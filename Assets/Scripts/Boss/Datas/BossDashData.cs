using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
[CreateAssetMenu(fileName = "BossDashData", menuName = "Boss/Behaviors Data/New BossDashData", order = 0)]
public class BossDashData : ScriptableObject
{
    [Tooltip("in Unity Unit/s")] public float DashSpeed = 5.0f;
    [Tooltip("in seconds")] public float DashLoadDuration = 1.0f;
}
