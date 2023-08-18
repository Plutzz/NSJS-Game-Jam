using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : DamageableEntity
{
    [SerializeField] public float movementSpeed;
    [SerializeField] public float damage;
    [SerializeField] public float attackRange;
    [SerializeField] public string ignoreTag;
    public GameObject player;

    private float distance;

    public virtual void Start()
    {
        player = PlayerController.playerController.gameObject;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out DamageableEntity enemyHit))
        {
            if (ignoreTag != null)
            {
                if (collision.gameObject.CompareTag(ignoreTag))
                {
                    Debug.Log("Ignored Damage");
                    return;
                }
            }

            Debug.Log("Damage Player");

            enemyHit.TakeDamage(damage);
        }
    }


    public virtual void GhostChase()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
    }

    public virtual bool CheckPlayerIsInAttackRange()
    {
        float distance = (player.transform.position - transform.position).magnitude;

        if (distance > attackRange) return false;

        return true;

    }

    

    public override void Die()
    {
        EnemySpawner.numEnemies--;
        Destroy(gameObject);
    }
}
