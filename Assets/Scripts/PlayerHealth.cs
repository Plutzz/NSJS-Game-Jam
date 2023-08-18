using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : DamageableEntity
{
    public override void Die()
    {
        Debug.Log("YOU DIED");
        // Game Over Screen
    }
}
