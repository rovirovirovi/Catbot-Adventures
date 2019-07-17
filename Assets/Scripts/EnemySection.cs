using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "EnemySection")]
public class EnemySection : ScriptableObject {
    public float duration;
    public bool sectionEnd;
    public List<Vector3> simpleEnemyPos;
    public List<Vector3> mediumEnemyPos;
    public List<Vector3> hardEnemyPos;
    public List<Vector3> bossEnemyPos;
}