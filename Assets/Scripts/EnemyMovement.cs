using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private GameObject player;

    private Rigidbody rb;

    public float seekMaxRange = 10.0f, attackRange = 2.0f;

    public float speed, maxSpeed;

    public Animator animator;

    public bool isAttacking = false;

    public bool isSpawning = false;
    private bool isDead = false;

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
        if(!isDead && !isSpawning)
            Move();
    }

    void Move()
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
            if (!GetComponentInParent<EnemyStatus>().isInvincible())
                rb.velocity = Vector3.zero;
            isAttacking = true;
            animator.SetBool("isAttacking", true);
        }
    }

    void StateControler()
    {
        if (GetComponent<EnemyStatus>().health <= 0 && !animator.GetBool("isDead"))
        {
            isDead = true;
            animator.SetBool("isDead", true);
            return;
        }

        if (animator.GetBool("isDead") && animator.GetCurrentAnimatorStateInfo(0).IsTag("Dead") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
            GetComponent<EnemyStatus>().Die();

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
