using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : DamageableEntity
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float damage;
    [SerializeField] private GameObject player;

    private float distance;






    public virtual void GhostFollow()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
    }
}
