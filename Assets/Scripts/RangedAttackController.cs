using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class RangedAttackController : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float speed = 1f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private string ignoreTag;


    private void Start()
    {
        rb.velocity = transform.rotation * new Vector3(0, speed, 0);
    }



    private void OnTriggerEnter2D(Collider2D _other)
    {
        
        if (_other.TryGetComponent(out DamageableEntity enemyHit))
        {
             if(ignoreTag != null)
             {
                if (_other.gameObject.CompareTag(ignoreTag))
                {
                    Debug.Log("Ignored Damage");
                    return;
                }
             }



            enemyHit.TakeDamage(damage);
        }
    }
}
