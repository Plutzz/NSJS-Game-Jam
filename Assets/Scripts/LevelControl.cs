using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelControl : MonoBehaviour
{
    public static LevelControl instance;
    public GameObject deathScreen;
   
    private void Awake()
    {
        instance = this;
    }

    public void ReturnToTitle()
    {
       
        SceneManager.LoadScene("Main Menu");
    }
}
