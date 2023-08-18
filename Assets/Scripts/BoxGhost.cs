using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoxGhost : Enemy
{
    [SerializeField] private float movementSpeed;
   private void Update()
    {
        GhostChase();
    }


    public void GhostChase()
    {
        var distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
    }
}
