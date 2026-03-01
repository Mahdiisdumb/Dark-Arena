using UnityEngine;

public class Gnome : MonsterBase
{
    protected override void Start()
    {
        base.Start();

        health = 40f;
        moveSpeed = 3;
        damage = 30f;
        attackRange = 1f;
        attackCooldown = 3f;
    }

    protected override void Attack()
    {
        base.Attack();
        Debug.Log("Gnome attacks!");
    }
}