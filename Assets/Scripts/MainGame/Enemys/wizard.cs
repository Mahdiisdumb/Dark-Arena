using UnityEngine;

public class DarkWizard : MonsterBase
{
    public GameObject projectilePrefab;
    public float castCooldown = 3f;
    private float castTimer;

    protected override void Start()
    {
        base.Start();
        health = 80f;
        moveSpeed = 2f;
        attackRange = 10f;
        castTimer = castCooldown;
    }

    protected override void FixedUpdate()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            castTimer -= Time.fixedDeltaTime;
            if (castTimer <= 0f)
            {
                CastSpell();
                castTimer = castCooldown;
            }
            FacePlayer();
        }
    }

    void CastSpell()
    {
        if (projectilePrefab == null) return;

        GameObject proj = Instantiate(projectilePrefab,
            transform.position + transform.forward,
            Quaternion.identity);

        Rigidbody prb = proj.GetComponent<Rigidbody>();
        if (prb != null)
            prb.linearVelocity = transform.forward * 12f;
    }
}