using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperLaneCollider : MonoBehaviour
{
    private void Start() {
        BattlePlayerController2D.Instance.CanMoveUp = true;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "GameManager") {
            BattlePlayerController2D.Instance.CanMoveUp = true;
        } else {
            BattlePlayerController2D.Instance.CanMoveUp = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        BattlePlayerController2D.Instance.CanMoveUp = true;
    }
}
