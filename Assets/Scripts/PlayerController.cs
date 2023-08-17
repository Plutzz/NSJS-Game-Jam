using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float attackTimer;
    [SerializeField] private AnimationClip attackAnim;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject spawnPoint;

    public int attackType;
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
        if(attackType == 0)
        {
            //Melee attack
        }
        else if (attackType == 1)
        {
            Instantiate(projectile, spawnPoint.transform.position, spawnPoint.transform.rotation); //Ranged attack
        }
        
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
