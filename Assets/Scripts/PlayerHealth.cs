using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : DamageableEntity
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            return;
        }
        Debug.Log("Took " + enemy.damage + "damage");

        TakeDamage(enemy.damage);
    }


    public override void Die()
    {
        // Game Over Screen
    }
}
