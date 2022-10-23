using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private Vector2 startPos;
    private float repeatWidth;

    private float speed = 0.15f;

    void Awake() {
        startPos = transform.position;
        repeatWidth = GetComponent<BoxCollider2D>().size.x / 2;
    }

    // Update is called once per frame
    void Update() {
        transform.Translate(Vector2.left * Time.deltaTime * speed);

        ResetBackground();
    }

    private void ResetBackground() {
        if (transform.position.x < startPos.x - repeatWidth) {
            transform.position = startPos;
        }
    }
}
