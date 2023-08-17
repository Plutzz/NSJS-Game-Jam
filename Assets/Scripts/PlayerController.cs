using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float attackTimer;
    [SerializeField] private AnimationClip projectileAttackAnim;
    [SerializeField] private AnimationClip meleeAttackAnim;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject melee;

    public int attackType;
    public bool StartAttackThisFrame { get; private set; }
    public bool CanAttack { get; private set; }

    public static PlayerController playerController;

    private void Awake()
    {
        playerController = this;
    }
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
            Debug.Log("Melee attack");
            melee.SetActive(true);
            StartCoroutine(MeleeAttackActive(meleeAttackAnim.length));
        }
        else if (attackType == 1)
        {
            Instantiate(projectile, spawnPoint.transform.position, spawnPoint.transform.rotation); //Ranged attack
            StartCoroutine(RangedAttackActive(projectileAttackAnim.length));
        }
        
        StartAttackThisFrame = true;
    }

    IEnumerator RangedAttackActive(float waitTime)
    {
        CanAttack = false;
        yield return new WaitForSeconds(waitTime);
        CanAttack = true;
        yield return null;
    }
    IEnumerator MeleeAttackActive(float waitTime)
    {
        CanAttack = false;
        yield return new WaitForSeconds(waitTime);
        CanAttack = true;
        melee.SetActive(false);
        yield return null;
    }


}
