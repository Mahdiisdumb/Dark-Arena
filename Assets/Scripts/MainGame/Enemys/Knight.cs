using UnityEngine;

public class CorruptedKnight : MonsterBase
{
    protected override void Start()
    {
        base.Start();

        health = 200f;
        moveSpeed = 2f;
        damage = 25f;
        attackRange = 2.5f;
        attackCooldown = 2f;
    }

    protected override void Attack()
    {
        base.Attack();
        Debug.Log("Corrupted Knight swings!");
    }
}