using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;

public class CurseManager : MonoBehaviour
{
    [SerializeField] private GameObject curseMenu;
    public static CurseManager instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Timer.timer.onCurseTime += OpenMenu;
    }
    public void OpenMenu()
    {
        curseMenu.SetActive(true);
        PauseMenu.Instance.Pause();
        
    }

    public void CloseMenu()
    {
        curseMenu.SetActive(false);
        PauseMenu.Instance.Resume();
    }



    [Header("Cardboard Box")]
    [SerializeField] private float enemyAttackRangeDown;
    [SerializeField] private float playerMovementSpeedDown;

    [Header("Salt Shaker")]
    [SerializeField] private float playerDamageDown;

    [Header("Whack the ripper")]
    [SerializeField] private float lifeSteal;
    [SerializeField] private float damageOverTime;

    [Header("Path of the wind god")]
    [SerializeField] private float playerMovementSpeedUp1;
    [SerializeField] private float playerMovementSpeedDebuff;

    [Header("Muffin Attack")]

    [Header("Roller Skates")]
    [SerializeField] private float slideAmount;
    [SerializeField] private float playerMovementSpeedUp2;


    public void TakeCurse(int _id)
    {
        switch(_id)
        {
            case 1:
                // Cardboard box: -enemy attack range, -player movement speed
                break;
            case 2:
                // Salt Shaker: gain ranged attack, lose melee attack, -dmg
                break;
            case 3:
                // Whack the ripper: +lifesteal, -health over time
                break;
            case 4:
                // Path of the wind god: +Movement Speed, +Attack Speed, -health, -attack damage
                break;
            case 5:
                // Muffin Man: +Muffin Attack
                break;
            case 6:
                // Roller Skates: +Movement Speed, slide instead of stopping
                break;

        }
    }




















}
