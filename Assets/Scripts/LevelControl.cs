using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelControl : MonoBehaviour
{
    public void ReturnToTitle()
    {
        Debug.Log("Returning to the menu");
        SceneManager.LoadScene("Main Menu");
    }
}
