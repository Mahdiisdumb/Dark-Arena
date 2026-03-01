using UnityEngine;

public class Goblin : MonsterBase
{
    protected override void Start()
    {
        base.Start();

        health = 50f;
        moveSpeed = 5f;
        damage = 8f;
        attackRange = 1.5f;
        attackCooldown = 0.8f;
    }

    protected override void Attack()
    {
        base.Attack();
        Debug.Log("Goblin stabs quickly!");
    }
}