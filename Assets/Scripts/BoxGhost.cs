using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxGhost : Enemy
{
   private void Update()
    {
        GhostFollow();
    }
}
