using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayerController : MonoBehaviour
{
    public float circle_bounds;
    public float speed, jumpForce, gravityForce;

    public int damage;

    private List<GameObject> enemToDamage = new List<GameObject>();

    private Vector3 centerPosition;
    // public bool isOnGround = true;
    private Animator anim;
    Rigidbody rBody;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rBody = GetComponent<Rigidbody>();
        centerPosition = transform.localPosition;
        // Physics.gravity *= gravityForce;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement_vector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        rBody.velocity = Vector3.zero;

        /* if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            rBody.AddForce(Vector3.up * jumpForce, ForceMode.Acceleration);
            isOnGround = false;
        } */

        // Commence attack
        if (Input.GetKeyDown(KeyCode.Space) && !anim.GetBool("is_attacking"))
        {
            anim.SetBool("is_attacking", true);
        }
        else
        {
            enemToDamage.Clear();
        }

        if (movement_vector != Vector3.zero)
        {
            anim.SetBool("is_running", true);
            anim.SetFloat("input_x", movement_vector.x);
            anim.SetFloat("input_y", movement_vector.z);
        }
        else
        {
            anim.SetBool("is_running", false);
        }

        // Restrict character movement to circle boundary
        float distance = Vector3.Distance(transform.position, centerPosition);
        if (distance > circle_bounds)
        {
            Vector3 fromOriginToObject = transform.position - centerPosition;
            fromOriginToObject *= circle_bounds / distance;
            fromOriginToObject = Vector3.ClampMagnitude(fromOriginToObject, circle_bounds);
            transform.position = centerPosition + fromOriginToObject;
        } else
        {
            rBody.velocity = movement_vector;
        }
    }

    private void AlertObservers(string message)
    {
        if (message.Equals("AttackAnimationEnded"))
        {
            anim.SetBool("is_attacking", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && !enemToDamage.Contains(other.gameObject))
        {
            other.gameObject.GetComponent<Status>().TakeDamage(damage);
            enemToDamage.Add(other.gameObject);
        }
    }

    /* private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isOnGround = true;
        }
    } */

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(0, 1, 0), circle_bounds);

    }
}