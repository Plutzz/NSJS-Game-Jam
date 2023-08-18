using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float attackTimer;
    [SerializeField] public float rangedDamage;
    [SerializeField] public float meleeDamage;
    [SerializeField] public float muffinAttackRate;
    [SerializeField] public float numberOfMuffins;
    [SerializeField] public float knockbackStrength;
    [SerializeField] public float knockbackDelay;
    [SerializeField] public Light2D playerLight;
    [SerializeField] private AnimationClip projectileAttackAnim;
    [SerializeField] private AnimationClip meleeAttackAnim;
    [SerializeField] private AnimationClip meleeUpAttackAnim;
    [SerializeField] private AnimationClip meleeDownAttackAnim;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject muffin;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject rotationPoint;
    [SerializeField] private GameObject melee;
    [SerializeField] private GameObject meleeUp;
    [SerializeField] private GameObject meleeDown;
    [SerializeField] private GameObject chefHat;
    [SerializeField] private GameObject attackSFX;

    public int attackType;
    public bool StartAttackThisFrame { get; private set; }
    public bool CanAttack { get; private set; }

    public static PlayerController playerController;

    public bool playerHoldingUp;
    public bool playerHoldingDown;

    private float yAxis;

    private void Awake()
    {
        playerController = this;
    }
    private void Start()
    {
        CanAttack = true;
        CurseManager.instance.onMuffinMan += MuffinAttack;
    }

    private void Update()
    {
        if (PauseMenu.getGameIsPaused()) return;

        yAxis = Input.GetAxisRaw("Vertical");

        playerHoldingUp = yAxis > 0.4f;

        playerHoldingDown = yAxis < -0.4f;

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
        Instantiate(attackSFX);
        if(attackType == 0)
        {
            if(!playerHoldingUp && !playerHoldingDown)
            {
                melee.SetActive(true);
                StartCoroutine(MeleeAttackActive(meleeAttackAnim.length));
            }
            else if(playerHoldingDown)
            {
                meleeDown.SetActive(true);
                StartCoroutine(MeleeDownAttackActive(meleeDownAttackAnim.length));
            }
            else
            {
                meleeUp.SetActive(true);
                StartCoroutine(MeleeUpAttackActive(meleeUpAttackAnim.length));
            }
        }
        else if (attackType == 1)
        {
            Instantiate(projectile, spawnPoint.transform.position, spawnPoint.transform.rotation); //Ranged attack
            StartCoroutine(RangedAttackActive(projectileAttackAnim.length));
        }
        
        StartAttackThisFrame = true;
    }

    private void MuffinAttack()
    {
        chefHat.SetActive(true);

        for(int i = 0; i < numberOfMuffins; i++)
        {
            float angle = Random.Range(0f, 360f);

            rotationPoint.transform.rotation = Quaternion.Euler(0, 0, angle);


            Instantiate(muffin, transform.position, rotationPoint.transform.rotation);
        }
        Invoke("MuffinAttack", 1/muffinAttackRate);
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
    IEnumerator MeleeUpAttackActive(float waitTime)
    {
        CanAttack = false;
        yield return new WaitForSeconds(waitTime);
        CanAttack = true;
        meleeUp.SetActive(false);
        yield return null;
    }

    IEnumerator MeleeDownAttackActive(float waitTime)
    {
        CanAttack = false;
        yield return new WaitForSeconds(waitTime);
        CanAttack = true;
        meleeDown.SetActive(false);
        yield return null;
    }




}
