using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : Enemy
{
    [SerializeField] private float attackSpeed;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject bulletSpawnPoint;
    [SerializeField] private GameObject rotationPoint;
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
            Vector2 direction = player.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if(!GetComponent<EnemyPathfind>().facingRight)
            {
                angle -= 180; 
            }

            rotationPoint.transform.rotation = Quaternion.Euler(0, 0, angle);


            Instantiate(bullet, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
            animator.SetBool("Attacking", true);
        }

        Invoke("Attack", 1 / attackSpeed);

    }

    
}
