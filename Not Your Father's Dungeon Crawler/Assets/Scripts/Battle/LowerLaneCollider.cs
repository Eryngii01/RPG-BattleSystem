using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerLaneCollider : MonoBehaviour
{
    private void Start() {
        BattlePlayerController2D.Instance.CanMoveDown = true;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "GameManager") {
            BattlePlayerController2D.Instance.CanMoveDown = true;
        } else {
            BattlePlayerController2D.Instance.CanMoveDown = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        BattlePlayerController2D.Instance.CanMoveDown = true;
    }
}
