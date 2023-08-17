using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : DamageableEntity
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float damage;
    private GameObject player;

    private float distance;

    public virtual void Awake()
    {
        player = PlayerController.playerController.gameObject;
    }




    public virtual void GhostChase()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
    }

    public override void Die()
    {
        EnemySpawner.numEnemies--;
        Destroy(gameObject);
    }
}
