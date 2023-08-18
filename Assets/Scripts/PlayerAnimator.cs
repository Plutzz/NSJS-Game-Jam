
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private ParticleSystem jumpParticles, launchParticles;
    [SerializeField] private ParticleSystem moveParticles, landParticles;
    [SerializeField] private AudioSource emoteSFX;

    private Rigidbody2D rb;
    private float xVelocity;
    private float yVelocity;
    private ParticleSystem.MinMaxGradient currentGradient;
    private PlayerController playerController;
    private PlayerMovement playerMovement;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
        playerMovement = GetComponent<PlayerMovement>();
        currentGradient = new ParticleSystem.MinMaxGradient(Color.cyan);
    }

    private void Update()
    {
        xVelocity = rb.velocity.x;
        yVelocity = rb.velocity.y;

        if (rb == null) return;

        // Flip sprite

        if (xVelocity != 0 && playerController.CanAttack)
        {
            if (xVelocity > 0) transform.localScale = new Vector3(1, 1, 1);
            else transform.localScale = new Vector3(-1, 1, 1);
        }
        

        if(Mathf.Abs(xVelocity) > 0)
        {
            anim.SetBool("Moving", true);
        }
        else
        {
            anim.SetBool("Moving", false);
        }

        if (yVelocity == 0 && playerMovement.IsGrounded)
        {
            anim.SetBool("Jumping", false);
        }
        else
        {
            anim.SetBool("Jumping", true);
        }

        anim.SetFloat("VerticalVelocity", yVelocity);


        if(true)
        {
            //Debug.Log("Jump Particles");
            //Instantiate(jumpParticles);
        }

        if(playerController.StartAttackThisFrame)
        {
            anim.SetBool("Attacking", true);
        }
        else
        {
            anim.SetBool("Attacking", false);
        }
        if(playerController.playerHoldingUp)
        {
            anim.SetBool("HoldingUp", true);
        }
        else
        {
            anim.SetBool("HoldingUp", false);
        }
        if(playerController.playerHoldingDown)
        {
            anim.SetBool("HoldingDown", true);
        }
        else
        {
            anim.SetBool("HoldingDown", false);
        }

        if(playerController.CanAttack)
        {
            anim.SetBool("CurrentlyAttacking", false);
        }
        else
        {
            anim.SetBool("CurrentlyAttacking", true);
        }

        if(Input.GetButtonDown("Emote"))
        {
            emoteSFX.Play();
            anim.SetBool("Emoting", true);
        }
        else
        {
            anim.SetBool("Emoting", false);
        }


    }

    private void SetColor(ParticleSystem ps)
    {
        var main = ps.main;
        main.startColor = currentGradient;
    }
}
