using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackController : MonoBehaviour
{
    [SerializeField] private string ignoreTag;



    private void OnTriggerEnter2D(Collider2D _other)
    {

        if (_other.TryGetComponent(out DamageableEntity enemyHit))
        {
            if (ignoreTag != null)
            {
                if (_other.gameObject.CompareTag(ignoreTag))
                {
                    Debug.Log("Ignored Damage");
                    return;
                }
            }



            enemyHit.TakeDamage(PlayerController.playerController.meleeDamage);
        }
    }
}
