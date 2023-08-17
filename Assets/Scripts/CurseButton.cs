using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurseButton : MonoBehaviour
{
    [SerializeField] private Curse currentCurse;
    private Image image;
    private void Start()
    {
        image = GetComponent<Image>();
        image.sprite = currentCurse.Sprite;
    }

    private void Update()
    {
        
    }


}
