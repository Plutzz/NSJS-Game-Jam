using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : DamageableEntity
{
    public float numOfHearts;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public bool WhackTheRipper;
    public float secondsPerLifeDecay = 10f;
    public float decayDamage = 1f;
    public GameObject deathScreen;

    private void Start()
    {
        CurseManager.instance.onWhackTheRipper += LifeDecay;
        deathScreen = LevelControl.instance.deathScreen;
    }
    void Update()
    {
        if (hp > numOfHearts)
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
}
