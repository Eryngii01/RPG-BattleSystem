using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class BattlePlayerController2D : MonoBehaviour
{
    public float speed, jumpForce, gravityForce;
    private float _clampDisplacement = 0.5f;

    private static BattlePlayerController2D _instance;
    public static BattlePlayerController2D Instance {
        get { return _instance; }
    }

    private float _actionCooldown = 0f;
    public TextMeshProUGUI cooldownText;

    public TextMeshProUGUI xInputText;

    private bool _isAttacking = false;
    public bool IsAttacking {
        get { return _isAttacking; }
        set { _isAttacking = value; }
    }
    public TextMeshProUGUI attackText;

    private bool _canMoveUp;
    public bool CanMoveUp {
        get { return _canMoveUp; }
        set { _canMoveUp = value; }
    }
    public TextMeshProUGUI moveUpText;

    private bool _canMoveDown;
    public bool CanMoveDown {
        get { return _canMoveDown; }
        set { _canMoveDown = value; }
    }
    public TextMeshProUGUI moveDownText;

    private bool _isSwitching;
    private float _destinationY;
    private float _scaleDestination;
    private SpriteRenderer _sprite;

    private bool _isTargeting = false;
    public bool IsTargeting {
        get { return _isTargeting; }
        set { _isTargeting = value; }
    }

    private bool _canTarget = true;
    public bool CanTarget {
        get { return _canTarget; }
        set { _canTarget = value; }
    }

    private float _targetCounter = 0f;
    public float TargetCounter {
        get { return _targetCounter; }
        set { _targetCounter = value; }
    }

    // Bookkeeping for background, midground, and foreground enemy layers
    public LayerMask[] enemyLayers;

    // Player is currently in the frontline
    private int _currentLane = 2;
    public int CurrentLane {
        get { return _currentLane; }
    }
    public TextMeshProUGUI laneText;

    private Animator _anim;

    /** The battle collider stuff **/
    public Transform attackPoint;
    public float attackRange = 0.5f;

    public int attackDamage = 40;
    //public float attackRate = 2f;
    //private float timeToAttack = 0f;
    //private AttackController2D attack;

    private Rigidbody2D _rBody;

    private void Awake() {
        _instance = this;
        _isSwitching = false;

        _anim = GetComponent<Animator>();
        _rBody = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update()
    {
        cooldownText.SetText(_actionCooldown.ToString());
        attackText.SetText(_isAttacking.ToString());
        moveUpText.SetText(_canMoveUp.ToString());
        moveDownText.SetText(_canMoveDown.ToString());

        if (!_isTargeting) {
            BeginAttackChain();

            /* if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
            {
                _rBody.AddForce(Vector3.up * jumpForce, ForceMode.Acceleration);
                isOnGround = false;
            } */

            // Commence attack
            /**if (Input.GetKeyDown(KeyCode.Space) && Time.time >= timeToAttack) {
                StartAttackChain();

                // Next time to be able to attack
                //timeToAttack = Time.time + 1f / attackRate;

                return;
            }**/

            SwitchLanes();
            MovePlayer();
        }

        Target();
    }

    // Document later -> tries to check if the user input is attacking then begins animation chain for attack sequence
    private void BeginAttackChain() {
        if (Input.GetButtonDown("Xbox_A") && !_isAttacking && _actionCooldown < 0) {
            _isAttacking = true;
        } else {
            _actionCooldown -= Time.deltaTime;
        }
    }

    // Function to calculate and deal damage to opponents
    public void Attack() {
        // Play attack animation
        //anim.SetBool("is_attacking", true);

        // Detect enemies in range
        //Creates a circle that detects all the colliders within it
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers[_currentLane]);

        // Damage the enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Status>().TakeDamage(attackDamage);
        }
    }

    private void MovePlayer() {
        Vector2 movementVector = new Vector2(Input.GetAxisRaw("Xbox_LJH"), 0);
        _rBody.velocity = Vector2.zero;

        xInputText.SetText(Input.GetAxisRaw("Xbox_LJH").ToString());

        if (movementVector.x != Vector2.zero.x && !_anim.GetBool("is_attacking") && _actionCooldown < 0) {
            _anim.SetBool("is_running", true);
            _anim.SetFloat("input_x", movementVector.x);

            _rBody.velocity = movementVector;
        }
        else
        {
            _anim.SetBool("is_running", false);
        }

        float clampedX = Mathf.Clamp(transform.position.x, GameManager.Instance.ArenaBounds.bounds.min.x + _clampDisplacement, GameManager.Instance.ArenaBounds.bounds.max.x - _clampDisplacement);
        transform.position = new Vector2(clampedX, gameObject.transform.position.y);
    }

    private void SwitchLanes() {
        laneText.SetText(_currentLane.ToString());
        float switchDirection = Input.GetAxisRaw("Xbox_T");

        if (_isSwitching) {
            MoveLanes();
        } else if (switchDirection > 0 && _canMoveUp && _currentLane != 0 && _actionCooldown < 0) {
            // Move up
            _destinationY = _rBody.position.y + 0.3f;

            _scaleDestination = transform.localScale.x - 0.15f;
            _clampDisplacement += 0.05f;
            _isSwitching = true;

            _sprite.sortingOrder = _sprite.sortingOrder - 2;

            _currentLane = _currentLane - 1;
        } else if (switchDirection < 0 && _canMoveDown && _currentLane != 2 && _actionCooldown < 0) {
            // Move down
            //Vector2 directionVector = new Vector2(0f, 0.4f);
            //_rBody.MovePosition((Vector2)transform.position - (directionVector));
            _destinationY = _rBody.position.y - 0.3f;
            
            _scaleDestination = transform.localScale.x + 0.15f;
            _clampDisplacement -= 0.05f;
            _isSwitching = true;

            _sprite.sortingOrder = _sprite.sortingOrder + 2;

            _currentLane = _currentLane + 1;
        }
    }

    private void MoveLanes() {
        float step = 2.5f * Time.deltaTime;
        Vector2 destinationVector = new Vector2(_rBody.position.x, _destinationY);
        Vector3 scaleVector = new Vector3(_scaleDestination, _scaleDestination, 1f);

        _rBody.position = Vector2.MoveTowards(_rBody.position, destinationVector, step);

        transform.localScale = Vector3.MoveTowards(transform.localScale, scaleVector, step);

        if (_rBody.position.y == _destinationY) {
            _isSwitching = false;
            ResetCooldown();
        }
    }

    public void ResetCooldown() {
        _actionCooldown = 0.2f;
    }

    private void Target() {
        _targetCounter -= Time.deltaTime;
        Debug.Log(_targetCounter);

        if (Input.GetButtonDown("Xbox_RB")) {
            // Stop player movement for now, but must find a way later to pause all animations
            _rBody.velocity = Vector2.zero;
            _anim.SetBool("is_running", false);
            //////////////////////////////////////

            _isTargeting = true;
            _canTarget = false;
            GameManager.Instance.ActivateTargeting();
        } else if (Input.GetButtonUp("Xbox_RB")) {
            GameManager.Instance.QuitTargeting();
        } else if (Input.GetButton("Xbox_RB") && _isTargeting && _canTarget && _targetCounter < 0) {
            float selection = Input.GetAxisRaw("Xbox_LJH");

            Debug.Log("Checking to switch target");
            
            if (selection != 0) {
                GameManager.Instance.SwitchTarget(selection);
                _canTarget = false;
            }
        }
    }
}