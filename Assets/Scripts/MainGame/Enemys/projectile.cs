using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    public float damage = 15f;
    public float lifetime = 5f;

    void Start() => Destroy(gameObject, lifetime);

    void OnCollisionEnter(Collision collision)
    {
        PlayerHealth ph = collision.gameObject.GetComponent<PlayerHealth>();
        if (ph != null)
            ph.TakeDamage(damage);

        Destroy(gameObject);
    }
}