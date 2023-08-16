using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackController : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float speed = 1f;
    [SerializeField] private Rigidbody2D rb;


    private void Start()
    {
        rb.velocity = Vector3.right * speed;
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
