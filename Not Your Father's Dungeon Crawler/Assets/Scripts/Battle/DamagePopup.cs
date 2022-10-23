using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]
public class DamagePopup : MonoBehaviour {
    private TextMeshPro text;

    //private float floatDuration;
    private Color textColor;
    private static int sortingOrder;

    public float speed;
    public float floatDuration;
    public float disappearSpeed;

    private void Awake() {
        text = gameObject.GetComponent<TextMeshPro>();
    }

    public void SetDamage(int damage) {
        textColor = text.color;
        //floatDuration = 10f;

        text.sortingOrder = sortingOrder++;

        text.SetText(damage.ToString());
    }

    // Update is called once per frame
    private void Update() {
        //float speed = 3f;
        transform.position += new Vector3(0, speed) * Time.deltaTime;
        transform.localScale += new Vector3(0.001f, 0.001f);

        floatDuration -= Time.deltaTime;

        if (floatDuration < 0) {
            //float disappearSpeed = 3f;
            textColor.a -= (disappearSpeed * Time.deltaTime);
            text.color = textColor;

            if (textColor.a < 0) {
                Destroy(gameObject);
            }
        }
    }
}
