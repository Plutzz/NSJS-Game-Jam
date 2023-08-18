using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : Enemy
{
    [SerializeField] private float attackSpeed;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject bulletSpawnPoint;
    public override void Start()
    {
        player = PlayerController.playerController.gameObject;
        Invoke("Attack", 0f);
    }
    private void Attack()
    {
        animator.SetBool("Attacking", false);

        if (CheckPlayerIsInAttackRange())
        {
            Instantiate(bullet, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
            animator.SetBool("Attacking", true);
        }

        Invoke("Attack", 1 / attackSpeed);

    }

    
}
