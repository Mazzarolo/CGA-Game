using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private GameObject player;

    private Rigidbody rb;

    public float seekMaxRange = 10.0f, attackRange = 2.0f;

    public float speed, maxSpeed;

    private Animator animator;

    private bool isAttacking = false;

    private Transform FindHead(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.name == "Head")
            {
                return child;
            }
            else
            {
                Transform result = FindHead(child);
                if (result != null)
                {
                    return result;
                }
            }
        }

        return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    Vector3 Seek(Vector3 target)
    {
        Vector3 desired = target - transform.position;

        desired.Normalize();

        desired *= maxSpeed;

        Vector3 steer = desired - rb.velocity;

        steer = Vector3.ClampMagnitude(steer, maxSpeed);

        return steer;
    }

    Vector3 Pursuit(Vector3 target)
    {
        Vector3 distance = target - transform.position;

        float updatesNeeded = distance.magnitude / maxSpeed;

        Vector3 futurePosition = target + player.GetComponent<Rigidbody>().velocity * updatesNeeded;

        return Seek(futurePosition);
    }

    bool isGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 distance = player.transform.position - transform.position;

        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));

        FindHead(transform).LookAt(player.transform);

        if (distance.magnitude < seekMaxRange && distance.magnitude > attackRange && isGrounded() && !isAttacking)
        {
            Vector3 dir = Pursuit(player.transform.position);

            dir = new Vector3(dir.x, 0, dir.z);

            rb.AddForce(dir);
        }

        if (distance.magnitude < attackRange && isGrounded())
        {
            rb.velocity = Vector3.zero;
            isAttacking = true;
            animator.SetBool("isAttacking", true);
        }
    }

    void StateControler()
    {
        //Debug.Log("animation attack: " + animator.GetBool("isAttacking"));
        //Debug.Log("isAttacking: " + isAttacking);

        if (isAttacking && animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
            {
                isAttacking = false;
                animator.SetBool("isAttacking", false);
            }
        }
        
        if (rb.velocity.magnitude > 0.2f )
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    void Update()
    {
        StateControler();
    }
}
