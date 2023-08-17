using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Curse System/Curses")]

public class Curse : ScriptableObject
{
    public int ID;
    public string DisplayName;
    public bool CurseTaken;
}
