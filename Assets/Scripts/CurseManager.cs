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




















}
