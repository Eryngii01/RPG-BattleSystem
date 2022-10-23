using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelStats")]
public class LevelBase : ScriptableObject 
{
    [System.Serializable]
    public struct Level {};

    [Header("Insert stats or points per level below")]
    public Level[] levelInfo;
}
