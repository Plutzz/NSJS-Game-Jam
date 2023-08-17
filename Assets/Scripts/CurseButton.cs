using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurseButton : MonoBehaviour
{
    public Curse currentCurse;
    private Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    private void OnEnable()
    {
        image.sprite = currentCurse.Sprite;
    }

    public void TakeCurse()
    {
        Debug.Log(currentCurse + "Taken");
        CurseManager.instance.TakeCurse(currentCurse.ID);
    }


}
