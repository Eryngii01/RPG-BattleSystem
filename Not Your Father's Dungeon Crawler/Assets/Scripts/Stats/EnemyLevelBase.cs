using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyLevelStatus", menuName = "EnemyLevelStats")]
public class EnemyLevelBase : ScriptableObject 
{

    [System.Serializable]
    public struct Level
    {
        public int level;
        public int maxHealthPoints;
        public int maxSpecialPoints;
        public int strength;
        public int defense;
        public int specialAttack;
        public int specialDefense;
        public int dexterity;
        public float criticalRate;
        public float evasionRate;
    }

    [Header("Insert stats per level below")]
    public Level[] levelInfo;
}
