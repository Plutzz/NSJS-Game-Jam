using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Can be inherited by the player, enemies, or any object that will take damage and then be destroyed
public class DamageableEntity : MonoBehaviour
{
    [SerializeField] public float hp;
    [SerializeField] private AudioSource hitSFX;

    public virtual void TakeDamage(float damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            Instantiate(hitSFX);
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public virtual void TakeDamageAndKnockback(float damage, float knockbackStrength, float knockbackDelay, Transform other)
    {
        TakeDamage(damage);
        takeKnockback(knockbackStrength, knockbackDelay, other);
    }

    private void takeKnockback(float knockbackStrength, float knockbackDelay, Transform source)
    {

        Rigidbody2D enemy = GetComponent<Rigidbody2D>();  // Gets enemy game object
                                                          //enemy.isKinematic = false;
        Vector2 difference = transform.position - source.position;
        difference = difference.normalized * knockbackStrength;
        enemy.AddForce(difference, ForceMode2D.Impulse);
        //enemy.isKinematic = true;
        StartCoroutine(KnockbackCo(enemy, knockbackDelay));
    }

    // Adds a delay before the enemy can start walking towards the player again after knockback
    //-------------------------------------------------------------------------------------------
    private IEnumerator KnockbackCo(Rigidbody2D enemy, float knockbackDelay)
    {
        yield return new WaitForSeconds(knockbackDelay);
        enemy.velocity = Vector2.zero;
        //enemy.isKinematic = true;
    }

}