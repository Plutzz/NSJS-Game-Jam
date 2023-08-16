using PlayerMovement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private ParticleSystem jumpParticles, launchParticles;
    [SerializeField] private ParticleSystem moveParticles, landParticles;

    private float xVelocity;
    private float yVelocity;
    private ParticleSystem.MinMaxGradient currentGradient;
    private IPlayerController player;
    private PlayerController playerController;


    private void Awake()
    {
        player = GetComponentInParent<IPlayerController>();
        playerController = GetComponentInParent<PlayerController>();
        currentGradient = new ParticleSystem.MinMaxGradient(Color.cyan);
    }

    private void Update()
    {
        xVelocity = player.Input.X;
        yVelocity = player.Velocity.y;

        if (player == null) return;

        // Flip sprite

        if (xVelocity != 0)
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

        if (yVelocity == 0)
        {
            anim.SetBool("Jumping", false);
        }
        else
        {
            anim.SetBool("Jumping", true);
        }

        anim.SetFloat("VerticalVelocity", yVelocity);


        if(player.JumpingThisFrame)
        {
            //Debug.Log("Jump Particles");
            //Instantiate(jumpParticles);
        }

        if(playerController.AttackingThisFrame)
        {
            anim.SetBool("Attacking", true);
        }
        else
        {
            anim.SetBool("Attacking", false);
        }

        if(playerController.CanAttack)
        {
            anim.SetBool("CurrentlyAttacking", false);
        }
        else
        {
            anim.SetBool("CurrentlyAttacking", true);
        }
    
    }

    private void SetColor(ParticleSystem ps)
    {
        var main = ps.main;
        main.startColor = currentGradient;
    }
}
