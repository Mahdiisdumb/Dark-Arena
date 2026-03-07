using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    public float damage = 25f;
    public Collider damageCollider;
    public bool requireSwing = true;
    public float minSwingSpeed = 1f;

    private Rigidbody rb;

    void Start()
    {
        if (damageCollider == null)
            Debug.LogWarning("Assign a damageCollider!");

        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        MonsterBase monster = other.GetComponentInParent<MonsterBase>();
        if (monster == null) return;

        if (requireSwing && rb != null && rb.linearVelocity.magnitude < minSwingSpeed)
            return;

        monster.TakeDamage(damage);
    }
}