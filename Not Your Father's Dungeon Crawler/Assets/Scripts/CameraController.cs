using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(BoxCollider2D))]
public class CameraController : MonoBehaviour
{
    private static CameraController _instance;
    public static CameraController Instance {
        get { return _instance; }
    }

    public GameObject player;
    public float movementSpeed;

    private Vector3 playerPos;
    private static bool cameraExists;
    private Animator _anim;

    private Vector3 _minBounds, _maxBounds;

    private Camera mainCamera;
    private float _halfHeight, _halfWidth;

    private int _currentLane;
    private float _zoomDestination;
    private bool _isZooming;

    private Vector3 _targetPos;
    private int _targetLane;
    private bool _isSeeking;

    private void Awake() {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start() {
        _anim = GetComponent<Animator>();

        mainCamera = GetComponent<Camera>();
        _halfHeight = mainCamera.orthographicSize;
        _halfWidth = _halfHeight * Screen.width / Screen.height;

        _minBounds = GameManager.Instance.ArenaBounds.bounds.min;
        _maxBounds = GameManager.Instance.ArenaBounds.bounds.max;

        if (!cameraExists) {
            cameraExists = true;
            DontDestroyOnLoad(transform.root.gameObject);
        } else {
            Destroy(gameObject);
        }

        _currentLane = BattlePlayerController2D.Instance.CurrentLane;
    }

    // Update is called once per frame
    void FixedUpdate() {
        MovePlayerCamera();
        MoveToTarget();
        Zoom();
    }

    private void MovePlayerCamera() {
        if (!_isSeeking) {
            playerPos = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
            transform.position = playerPos * movementSpeed;

            float clampedX = Mathf.Clamp(transform.position.x, _minBounds.x + _halfWidth, _maxBounds.x - _halfWidth);
            // float clampedY = Mathf.Clamp(transform.position.y, _minBounds.y + _halfHeight, _maxBounds.y - _halfHeight);
            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);

            if (BattlePlayerController2D.Instance.CurrentLane > _currentLane) {
                // Increase scale and adjust camera bounds
                _isZooming = true;
                _zoomDestination = mainCamera.orthographicSize + 0.15f;

                _currentLane++;
            } else if (BattlePlayerController2D.Instance.CurrentLane < _currentLane) {
                // Reduce scale and adjust camera bounds
                _isZooming = true;
                _zoomDestination = mainCamera.orthographicSize - 0.15f;

                _currentLane--;
            }
        }
    }

    private void Zoom() {
        float distance = Mathf.Abs(_targetPos.x) + Mathf.Abs(transform.position.x);

        if (_isZooming && _zoomDestination > mainCamera.orthographicSize) {
            mainCamera.orthographicSize += (2.5f / distance) * Time.deltaTime;
            _halfHeight = mainCamera.orthographicSize;
            _halfWidth = _halfHeight * Screen.width / Screen.height;

            if (mainCamera.orthographicSize > _zoomDestination) {
                _isZooming = false;
            }
        } else if (_isZooming && _zoomDestination < mainCamera.orthographicSize) {
            mainCamera.orthographicSize -= (2.5f / distance) * Time.deltaTime;
            _halfHeight = mainCamera.orthographicSize;
            _halfWidth = _halfHeight * Screen.width / Screen.height;

            if (mainCamera.orthographicSize < _zoomDestination) {
                _isZooming = false;
            }
        }
    }

    public void SetTarget(GameObject target) {
        _targetPos = target.gameObject.transform.position;
        _targetLane = target.gameObject.GetComponent<Status>().currentLane;

        _isSeeking = true;

        if (_currentLane < _targetLane) {
            int step = _targetLane - _currentLane;

            // Increase scale and adjust camera bounds
            _isZooming = true;
            _zoomDestination = mainCamera.orthographicSize + (0.15f * step);

            _currentLane = _currentLane + step;
        } else if (_currentLane > _targetLane) {
            int step = _currentLane - _targetLane;

            // Reduce scale and adjust camera bounds
            _isZooming = true;
            _zoomDestination = mainCamera.orthographicSize - (0.15f * step);

            _currentLane = _currentLane - step;
        }
    }

    private void MoveToTarget() {
        if (_isSeeking) {
            float step = 7f * Time.deltaTime;

            // Clamp this for any target position since the camera may go out of bounds
            float clampedX = Mathf.Clamp(_targetPos.x, _minBounds.x + _halfWidth, _maxBounds.x - _halfWidth);
            Vector3 destinationVector = new Vector3(_targetPos.x, transform.position.y, transform.position.z);

            transform.position = Vector3.MoveTowards(transform.position, destinationVector, step);

            if (transform.position.x == BattlePlayerController2D.Instance.transform.position.x) {
                _isSeeking = false;
                
                // Restore player controls
                BattlePlayerController2D.Instance.IsTargeting = false;
                BattlePlayerController2D.Instance.CanTarget = true;
            } else if (transform.position.x == _targetPos.x && !BattlePlayerController2D.Instance.CanTarget) {
                BattlePlayerController2D.Instance.CanTarget = true;
                BattlePlayerController2D.Instance.TargetCounter = 0.5f;
            }
        }
    }

    public void SnapToPlayer() {
        _targetPos = BattlePlayerController2D.Instance.transform.position;
        int playerLane = BattlePlayerController2D.Instance.CurrentLane;

        _isSeeking = true;

        if (playerLane < _currentLane) {
            int step = _currentLane - playerLane;

            // Reduce scale and adjust camera bounds
            _isZooming = true;
            _zoomDestination = mainCamera.orthographicSize - (0.15f * step);

            _currentLane = _currentLane - step;
        } else if (playerLane > _currentLane) {
            int step = playerLane - _currentLane;

            // Increase scale and adjust camera bounds
            _isZooming = true;
            _zoomDestination = mainCamera.orthographicSize + (0.15f * step);

            _currentLane = _currentLane + step;
        }
    }

    public void QuitPreview() {
        _anim.enabled = false;
    }
}
