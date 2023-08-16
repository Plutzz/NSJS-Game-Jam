using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float attackTimer;
    [SerializeField] private AnimationClip attackAnim;
    [SerializeField] private GameObject Projectile;

    public bool StartAttackThisFrame { get; private set; }

    public bool CanAttack { get; private set; }


    private void Start()
    {
        CanAttack = true;
    }

    private void Update()
    {
        if (CanAttack && Input.GetButtonDown("Attack1"))
        {
            Attack();
        }

        else if (CanAttack == false)
        {
            StartAttackThisFrame = false;
        }
    }
    private void Attack()
    {
        Instantiate(Projectile, transform.position, Quaternion.Euler(0, 0, -90));
        StartAttackThisFrame = true;
        StartCoroutine(AttackActive());
    }

    IEnumerator AttackActive()
    {
        CanAttack = false;
        yield return new WaitForSeconds(attackAnim.length);
        CanAttack = true;
        yield return null;


    }


}
