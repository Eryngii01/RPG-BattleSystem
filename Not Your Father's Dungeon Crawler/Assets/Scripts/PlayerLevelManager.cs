using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat.CharacterStats;

// Test file to check level up system of the player stats
public class PlayerLevelManager : MonoBehaviour
{
    public PlayerLevelBase levelData;

    private CharacterStats _healthPoints = new CharacterStats(82);
    private CharacterStats _specialPoints = new CharacterStats(55);
    private CharacterStats _strength = new CharacterStats(43);
    private CharacterStats _defense = new CharacterStats(36);
    private CharacterStats _specialAtk = new CharacterStats(36);
    private CharacterStats _specialDef = new CharacterStats(40);
    private CharacterStats _dexterity = new CharacterStats(29);

    private float _totalHp = 0;
    private float _totalSp = 0;
    private float _totalStr = 0;
    private float _totalDef = 0;
    private float _totalSAtk = 0;
    private float _totalSDef = 0;
    private float _totalDex = 0;

    // Start is called before the first frame update
    void Start() {
        int i;

        for (i = 0; i < 500; i++) {
            LevelUpTests();
        }

        float average; 

        Debug.Log("<------------------- Averages ------------------->");
        Debug.Log("HP: " + (_totalHp / i));
        Debug.Log("SP: " + (_totalSp / i));
        Debug.Log("STR: " + (_totalStr / i));
        Debug.Log("DEF: " + (_totalDef / i));
        Debug.Log("S.ATK: " + (_totalSAtk / i));
        Debug.Log("S.DEF: " + (_totalSDef / i));
        Debug.Log("DEX: " + (_totalDex / i));
    }

    // Update is called once per frame
    void Update() {
        
    }

    void LevelUpTests() {
        _healthPoints.baseValue = 82;
        _specialPoints.baseValue = 55;
        _strength.baseValue = 43;
        _defense.baseValue = 36;
        _specialAtk.baseValue = 36;
        _specialDef.baseValue = 40;
        _dexterity.baseValue = 29;

        foreach (PlayerLevelBase.Level level in levelData.levelInfo) {
            int points = level.obtainablePoints;

            for (int i = 0; i < points; i++) {
                float stat = Random.Range(0f, 100f);

                if (stat < 58.25f) {
                    // Assign HP
                    _healthPoints.baseValue += 1;
                } else if (stat >= 58.25f && stat < 76.07) {
                    // Assign SP
                    _specialPoints.baseValue += 1;
                } else if (stat >= 76.07 && stat < 81.16) {
                    // Assign STR
                    _strength.baseValue += 1;
                } else if (stat >= 81.16 && stat < 86.34) { // Changed from 85.84 to 86.34
                    // Assign DEF
                    _defense.baseValue += 1;
                } else if (stat >= 86.34 && stat < 90.01) {
                    // Assign S.ATK
                    _specialAtk.baseValue += 1;
                } else if (stat >= 90.01 && stat < 94.39) {
                    // Assign S.DEF
                    _specialDef.baseValue += 1;
                } else if (stat >= 94.39) {
                    // Assign DEX
                    _dexterity.baseValue += 1;
                }
            }

            // Debug.Log("Level " + index + " -> HP: " + _healthPoints.Value + " SP: " + _specialPoints.Value + " STR: " + _strength.Value + 
            // " DEF: " + _defense.Value + " S.ATK: " + _specialAtk.Value + " S.DEF: " + _specialDef.Value + " DEX: " + _dexterity.Value);
        }

        _totalHp += (_healthPoints.Value - 82);
        _totalSp += (_specialPoints.Value - 55);
        _totalStr += (_strength.Value - 43);
        _totalDef += (_defense.Value - 36);
        _totalSAtk += (_specialAtk.Value - 36);
        _totalSDef += (_specialDef.Value - 40);
        _totalDex += (_dexterity.Value - 29);

        Debug.Log("Level 100 -> HP: " + (_healthPoints.Value - 82) + " SP: " + (_specialPoints.Value - 55) + " STR: " + (_strength.Value - 43) + 
        " DEF: " + (_defense.Value - 36) + " S.ATK: " + (_specialAtk.Value - 36) + " S.DEF: " + (_specialDef.Value - 40) + " DEX: " + (_dexterity.Value - 29));
    }
}
