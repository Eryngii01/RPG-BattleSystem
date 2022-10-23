using System.Collections;
using System.Collections.Generic;
using Stat.CharacterStats;
using UnityEngine;
using TMPro;

public class EnemyStatus : MonoBehaviour {

    private Status _characterStatus;

    public EnemyLevelBase levelsData;
    private EnemyLevelBase.Level _levelData;

    public string enemyName;
    public TextMeshProUGUI _levelText;
    public TextMeshProUGUI _nameText;
    public TextMeshProUGUI _currentHPText;
    public TextMeshProUGUI _maximumHPText;

    private void Awake() {
        _levelData = levelsData.levelInfo[Random.Range(0, levelsData.levelInfo.Length - 1)];

        _characterStatus = gameObject.GetComponent<Status>();

        _characterStatus.MaxHealthPoints = new CharacterStats(_levelData.maxHealthPoints);
        _characterStatus.MaxSpecialPoints = new CharacterStats(_levelData.maxSpecialPoints);
        _characterStatus.Strength = new CharacterStats(_levelData.strength);
        _characterStatus.Defense = new CharacterStats(_levelData.defense);
        _characterStatus.SpecialAttack = new CharacterStats(_levelData.specialAttack);
        _characterStatus.SpecialDefense = new CharacterStats(_levelData.specialDefense);
        _characterStatus.Dexterity = new CharacterStats(_levelData.dexterity);

        _levelText.SetText(_levelData.level.ToString());
        _nameText.SetText(enemyName);
        _maximumHPText.SetText(_characterStatus.MaxHealthPoints.Value.ToString());
    }

    // Update is called once per frame
    void Update() {
        _currentHPText.SetText(_characterStatus.CurrentHealth.ToString());
    }
}
