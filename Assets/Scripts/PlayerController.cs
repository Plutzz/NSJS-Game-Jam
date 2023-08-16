using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float attackTimer;
    [SerializeField] private AnimationClip attackAnim;

    public bool AttackingThisFrame { get; private set; }

    public bool CanAttack { get; private set; }


    private void Update()
    {
        if (CanAttack && Input.GetButtonDown("Attack1"))
        {
            CanAttack = false;
            AttackingThisFrame = true;
            attackTimer = attackAnim.length;
        }

        else if (CanAttack == false)
        {
            attackTimer -= Time.deltaTime;
            AttackingThisFrame = false;

            if(attackTimer < 0)
            {
                CanAttack = true;
            }
        }
    }


}
