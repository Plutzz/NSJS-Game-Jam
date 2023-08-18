using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;
using Random = UnityEngine.Random;
using static UnityEditor.Progress;

public class CurseManager : MonoBehaviour
{
    [SerializeField] private GameObject curseMenu;
    [SerializeField] private List<CurseButton> curseButtons;
    public static CurseManager instance;
    public Dictionary<int, Curse> CurseDictionary;
    private List<Curse> possibleCurses = new List<Curse>();
    private List<Curse> tempPossibleCurses = new List<Curse>();
    private GameObject[] childs;
    private int numButtons;
    private GameObject player;

    private void Awake()
    {
        instance = this;
        CurseDictionary = new Dictionary<int, Curse>();

        var items = Resources.LoadAll<Curse>("Curses");
        possibleCurses = new List<Curse>();
        foreach (var item in items)
        {
            CurseDictionary.Add(item.ID, item);

            if (!item.CurseTaken)
            {
                possibleCurses.Add(item);
                tempPossibleCurses.Add(item);
            }
        }



    }

    private void Start()
    {
        Timer.timer.onCurseTime += OpenMenu;
        player = PlayerController.playerController.gameObject;
    }
    public void OpenMenu()
    {
        if(possibleCurses.Count == 2)
        {
            curseButtons.RemoveAt(2);
        }
        else if(possibleCurses.Count == 1)
        {
            curseButtons.RemoveAt(0);
        }
        else
        {
            return; //Skips curse menu if all curses are taken
        }


        RollCurses();

        foreach (var curseButton in curseButtons) //Loop trough childs unity array
        {
            curseButton.gameObject.SetActive(true);
        }
            PauseMenu.Instance.Pause();
        
    }

    public void CloseMenu()
    {
        foreach (var curseButton in curseButtons) //Loop trough childs unity array
        {
            curseButton.gameObject.SetActive(false);
        }
        PauseMenu.Instance.Resume();
    }

    public void RollCurses()
    {

        foreach(var curseButton in curseButtons)
        {
            int _index = Random.Range(0, tempPossibleCurses.Count);
            Debug.Log(_index);
            curseButton.currentCurse = tempPossibleCurses[_index];
            tempPossibleCurses.RemoveAt(_index);
        }
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
        CurseDictionary.TryGetValue(_id, out Curse _curseTaken);
        possibleCurses.Remove(_curseTaken);
        tempPossibleCurses.Clear();

        foreach (var curse in possibleCurses)
        {
            tempPossibleCurses.Add(curse);
        }

        switch (_id)
        {
            case 1:
                // Cardboard box: -enemy attack range, -player movement speed

                break;
            case 2:
                // Salt Shaker: gain ranged attack, lose melee attack, -dmg
                player.GetComponent<PlayerController>().attackType = 1;
                break;
            case 3:
                // Whack the ripper: +lifesteal, -health over time
                break;
            case 4:
                // Path of the wind god: +Movement Speed, +Attack Speed, -health, -attack damage
                player.GetComponent<PlayerMovement>().maxSpeed += playerMovementSpeedUp1;
                break;
            case 5:
                // Muffin Man: +Muffin Attack
                break;
            case 6:
                // Roller Skates: +Movement Speed, slide instead of stopping
                player.GetComponent<PlayerMovement>().maxSpeed += playerMovementSpeedUp2;
                player.GetComponent<PlayerMovement>().deceleration -= slideAmount;
                break;   
        }
        CloseMenu();
    }




















}
