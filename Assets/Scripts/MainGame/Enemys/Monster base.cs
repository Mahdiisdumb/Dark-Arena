using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MonsterBase : MonoBehaviour
{
    public float health = 100f;
    public float moveSpeed = 3f;
    public float attackRange = 2f;
    public float damage = 10f;
    public float attackCooldown = 1.5f;

    [HideInInspector]
    public Transform player;

    Rigidbody rb;
    float attackTimer = 0f;
    bool isDead = false;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        GameObject p = GameObject.FindGameObjectWithTag("Player");

        if (p != null)
            player = p.transform;
        else
            Debug.LogWarning("No object tagged Player found.");
    }

    protected virtual void FixedUpdate()
    {
        if (isDead || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            attackTimer -= Time.fixedDeltaTime;

            if (attackTimer <= 0f)
            {
                Attack();
                attackTimer = attackCooldown;
            }
        }
    }

    protected virtual void MoveTowardsPlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;

        rb.MovePosition(
            transform.position + dir * moveSpeed * Time.fixedDeltaTime
        );

        FacePlayer();
    }

    protected virtual void FacePlayer()
    {
        Vector3 lookPos = new Vector3(
            player.position.x,
            transform.position.y,
            player.position.z
        );

        transform.LookAt(lookPos);
    }

    protected virtual void Attack()
    {
        if (player == null) return;

        // safer for VR rigs where the health script may be on parent
        PlayerHealth ph = player.GetComponentInParent<PlayerHealth>();

        if (ph != null)
        {
            ph.TakeDamage(damage);
            Debug.Log(name + " attacked player for " + damage);
        }
    }

    public virtual void TakeDamage(float amount)
    {
        if (isDead) return;

        health -= amount;

        Debug.Log(name + " took " + amount + " damage. HP: " + health);

        if (health <= 0f)
            Die();
    }

    protected virtual void Die()
    {
        if (isDead) return;

        isDead = true;

        Debug.Log(name + " died.");

        Destroy(gameObject);
    }
}