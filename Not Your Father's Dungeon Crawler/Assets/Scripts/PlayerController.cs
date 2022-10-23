using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
    // Start is called before the first frame update

    public GameObject playerCamera;
    public float speed;
    private Vector3 cameraOffset = new Vector3(0, 0, -10);
    Rigidbody2D rbody;
    Animator anim;

    void Start() {
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        // Make camera follow player
        playerCamera.transform.position = transform.position + cameraOffset;

        Vector2 movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (movementVector != Vector2.zero) {
            anim.SetBool("is_walking", true);
            anim.SetFloat("input_x", movementVector.x);
            anim.SetFloat("input_y", movementVector.y);
        } else {
            anim.SetBool("is_walking", false);
        }

        rbody.MovePosition(rbody.position + movementVector * speed);
    }
}
