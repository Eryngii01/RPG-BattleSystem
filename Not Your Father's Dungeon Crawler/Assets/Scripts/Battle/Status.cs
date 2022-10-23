using System.Collections;
using System.Collections.Generic;
using Stat.CharacterStats;
using UnityEngine;

public class Status : MonoBehaviour
{
    public GameObject DamageText;
    public int currentLane;

    protected SpriteRenderer _sprite;

    private int _currentHealth;
    public int CurrentHealth {
        get { return _currentHealth; }
        set { _currentHealth = value; }
    }

    private CharacterStats _maxHealthPoints;
    public CharacterStats MaxHealthPoints {
        get { return _maxHealthPoints; }
        set { _maxHealthPoints = value; }
    }

    private CharacterStats _maxSpecialPoints;
    public CharacterStats MaxSpecialPoints {
        get { return _maxSpecialPoints; }
        set { _maxSpecialPoints = value; }
    }

    private CharacterStats _strength;
    public CharacterStats Strength {
        get { return _strength; }
        set { _strength = value; }
    }

    private CharacterStats _defense;
    public CharacterStats Defense {
        get { return _defense; }
        set { _defense = value; }
    }
    
    private CharacterStats _specialAttack;
    public CharacterStats SpecialAttack {
        get { return _specialAttack; }
        set { _specialAttack = value; }
    }

    private CharacterStats _specialDefense;
    public CharacterStats SpecialDefense {
        get { return _specialDefense; }
        set { _specialDefense = value; }
    }
    
    private CharacterStats _dexterity;
    public CharacterStats Dexterity {
        get { return _dexterity; }
        set { _dexterity = value; }
    }

    // Agility will just be treated as a skill, there may be a skill in the future that speeds up movement in battle

    // Start is called before the first frame update
    void Start() {
        _sprite = GetComponent<SpriteRenderer>();

        _currentHealth = (int)_maxHealthPoints.Value;
    }

    // Update is called once per frame
    void Update()
    {
        // For single player mode
        if (BattlePlayerController2D.Instance.CurrentLane != currentLane) {
            _sprite.color = new Color(0.7f, 0.7f, 0.7f, 1.0f);
        } else {
            _sprite.color = Color.white;
        }
    }

    public void TakeDamage(int damage)
    {
        // Deal damage
        _currentHealth -= damage;

        // Create damage popup
        GameObject damageText = Instantiate(DamageText, transform.Find("DamagePosition").transform.position, DamageText.transform.rotation);
        damageText.GetComponent<DamagePopup>().SetDamage(damage);

        // Notify animation to play a hurt animation

        // Check if _currentHealth is below threshold
        if (_currentHealth <= 0)
        {
            Defeat();
        }

        // Trigger combo increase
        int combo = GameManager.ComboCount;
        combo++;
        GameManager.ComboCount = combo;
    }

    void Defeat() {
        // Play defeat animation

        // Disable enemy
        gameObject.SetActive(false);
        //GetComponent<Collider2D>().enabled = false;
        //this.enabled = false;
    }

    /**public static void CreateDamageTag(Vector3 position, int damage) {
        Instantiate(DamageText, position, DamageText.transform.rotation);
        //Transform damagePopupTransform = Instantiate(DamageText, position, DamageText.transform.rotation);

        //DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        //damagePopup.SetDamage(damage);

        //return damagePopup;
    }**/
}
