using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class RangedAttackController : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float speed = 1f;
    [SerializeField] private Rigidbody2D rb;


    private void Start()
    {
        rb.velocity = transform.rotation * new Vector3(0, speed, 0);
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
