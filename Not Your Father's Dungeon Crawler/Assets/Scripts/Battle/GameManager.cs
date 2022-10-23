using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(BoxCollider2D))]
public class GameManager : MonoBehaviour
{
    private BoxCollider2D _arenaBounds;
    public BoxCollider2D ArenaBounds {
        get { return _arenaBounds; }
    }

    private static GameManager _instance;
    public static GameManager Instance {
        get {
            if (_instance == null)
                Debug.LogError("Game Manager is null!");

            return _instance;
        }
    }

    private GameObject[] _enemyList;
    public GameObject[] EnemyList {
        get { return _enemyList; }
    }

    private int _currentEnemy = 0;
    public int CurrentEnemy {
        get { return _currentEnemy; }
    }

    public TextMeshProUGUI previewText;
    
    private static GameObject _comboUI;
    private static Slider _comboSlider;
    private static TextMeshProUGUI _comboCountText;
    private static TextMeshProUGUI _comboHitText;
    private static Vector3 _countUpdatePos;
    private Vector3 _countDefaultPos;
    private static bool _isUpdatingCombo;

    private static float _comboDuration;
    private static float _comboSpeed;
    private static int _comboCount;
    public static int ComboCount {
        get { return _comboCount; }
        set { 
            _comboCount = value; 
            UpdateComboUI();
            SetComboMeter();
        }
    }

    private float _arenaPreviewTimer;

    private void Awake() {
        _instance = this;

        // Change this later to 3f!!!
        _arenaPreviewTimer = 0.3f;
        DisableControls();

        _comboUI = GameObject.Find("Combo UI");

        _comboCountText = _comboUI.gameObject.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        _countDefaultPos = _comboCountText.rectTransform.position;
        _countUpdatePos = _comboCountText.gameObject.transform.GetChild(0).position;

        _comboHitText = _comboUI.gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _comboSlider = _comboUI.gameObject.transform.GetChild(2).GetComponent<Slider>();
        
        _comboUI.gameObject.SetActive(false);
        _isUpdatingCombo = false;
        SetArena();
    }

    // Start is called before the first frame update
    void Start() {
        _enemyList = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update() {
        if (_arenaPreviewTimer > 0) {
            PreviewArena();
        }

        if (_comboCount > 0) {
            UpdateComboMeter();
        }

        if (_isUpdatingCombo) {
            MoveComboCount();
        }
    }

    // Set battle arena boundaries
    private void SetArena () {
        _arenaBounds = GetComponent<BoxCollider2D>();
    }

    private static void SetComboMeter() {
        _comboDuration = 1f;

        if (_comboCount >= 50) {
            _comboSpeed = 1.0f;
        } else {
            _comboSpeed = _comboCount * .01f + .5f;
        }
    
    }

    private void UpdateComboMeter() {
        _comboDuration -= _comboSpeed * Time.deltaTime;
        _comboSlider.value = _comboDuration;

        if (_comboDuration < 0) {
            _comboCount = 0;

            // Set combo UI inactive
            _comboUI.SetActive(false);
        }
    }

    private static void UpdateComboUI() {
        _comboCountText.SetText(_comboCount.ToString());
        _comboCountText.rectTransform.position = _countUpdatePos;

        _isUpdatingCombo = true;
        _comboCountText.rectTransform.localScale = new Vector3(1.5f, 1.5f, 1);

        if (_comboCount != 1) {
            _comboHitText.SetText("Hits");
        } else if (_comboCount == 1) {
            _comboHitText.SetText("Hit");
            _comboUI.SetActive(true);
        }
    }

    private void MoveComboCount() {
        Transform curPos = _comboCountText.rectTransform;
        float step = 300f * Time.deltaTime;

        curPos.position = Vector3.MoveTowards(curPos.position, _countDefaultPos, step);

        curPos.localScale = Vector3.MoveTowards(curPos.localScale, Vector3.one, 4.5f * Time.deltaTime);

        if (curPos.position == _countDefaultPos) {
            _isUpdatingCombo = false;
        }
    }

    private void DisableControls() {
        BattlePlayerController2D.Instance.enabled = false;
    }

    private void EnableControls() {
        BattlePlayerController2D.Instance.enabled = true;
    }

    private void PreviewArena() {
        _arenaPreviewTimer -= Time.deltaTime;
        previewText.SetText(_arenaPreviewTimer.ToString());

        if (_arenaPreviewTimer < 0) {
            CameraController.Instance.QuitPreview();
            EnableControls();
        }
    }

    public void ActivateTargeting() {
        GameObject enemy = _enemyList[_currentEnemy];

        enemy.gameObject.transform.GetChild(1).gameObject.SetActive(true);
        CameraController.Instance.SetTarget(enemy);

        // Get current enemy lane
        // Get current enemy position
        // Move camera towards position and zoom in/out depending on current character lane
    }

    private void ResetTargeting(GameObject currentTarget) {
        currentTarget.gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void SwitchTarget(float direction) {
        GameObject enemy = _enemyList[_currentEnemy];
        ResetTargeting(enemy);

        if (direction > 0.5) {
            _currentEnemy = (_currentEnemy + 1) % _enemyList.Length;
        } else if (direction < -0.5 && _currentEnemy != 0) {
            _currentEnemy = _currentEnemy - 1;
        } else if (direction < -0.5) {
            _currentEnemy = _enemyList.Length - 1;
        }

        ActivateTargeting();
    }

    public void QuitTargeting() {
        GameObject enemy = _enemyList[_currentEnemy];
        ResetTargeting(enemy);

        CameraController.Instance.SnapToPlayer();
    }
}
