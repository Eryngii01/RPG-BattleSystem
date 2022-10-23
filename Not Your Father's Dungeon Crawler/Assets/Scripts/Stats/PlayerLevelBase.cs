using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerLevelStatus")]
public class PlayerLevelBase : ScriptableObject 
{
    [System.Serializable]
    public struct Level
    {
        public int level;
        public int obtainablePoints;
    }

    [Header("Insert number stat points per level below")]
    public Level[] levelInfo;
}
