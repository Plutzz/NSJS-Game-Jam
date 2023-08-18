using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : DamageableEntity
{
    [SerializeField] private AudioSource playerHitSFX;
    public float numOfHearts;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public bool WhackTheRipper;
    public float secondsPerLifeDecay = 10f;
    public float decayDamage = 1f;
    public GameObject deathScreen;


    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Start()
    {
        CurseManager.instance.onWhackTheRipper += LifeDecay;
        deathScreen = LevelControl.instance.deathScreen;
    }
    void Update()
    {
        if (hp < numOfHearts || hp > numOfHearts)
        {
            numOfHearts = hp;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < hp)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < numOfHearts)
            {
                hearts[i].gameObject.SetActive(true);
            }
            else
            {
                hearts[i].gameObject.SetActive(false);
            }
        }



    }
    public override void TakeDamage(float damage)
    {
        hp -= damage;
        Instantiate(playerHitSFX);
        if (hp <= 0)
        {
            Die();
        }

        StartCoroutine(Invunerability());
    }
    public override void Die()
    {
        if (deathScreen != null) deathScreen.SetActive(true);
        PauseMenu.Instance.Pause();
        

    }

    public void heal(float amount)
    {
        hp += amount;
    }

    private void LifeDecay()
    {

        this.TakeDamage(decayDamage);


        Invoke("LifeDecay", secondsPerLifeDecay);
    }

    private IEnumerator Invunerability()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for(int i = 0; i < numberOfFlashes; i++)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }
}
