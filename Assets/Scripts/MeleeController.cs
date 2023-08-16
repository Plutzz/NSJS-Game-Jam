using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    private void OnCollisionEnter2D(Collision2D _other)
    {
        Debug.Log("enemy hit");
        if (_other.gameObject.TryGetComponent(out DamageableEntity enemyHit))
        {
            enemyHit.TakeDamage(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        Debug.Log("enemy hit");
        if (_other.TryGetComponent(out DamageableEntity enemyHit))
        {
            enemyHit.TakeDamage(damage);
        }
    }
}
