using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceKill : MonoBehaviour
{
    [SerializeField] public bool _hasBounced = false; // Flag to indicate if the object has bounced
    [SerializeField] private int _bounceDamage = 3;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Shield"))
        {
            // If the object has bounced off the shield
            
            _hasBounced = true;
        }
        if (other.gameObject.CompareTag("Enemy") && _hasBounced)
        {
            BasicEnemyMovement enemy = other.GetComponent<BasicEnemyMovement>();
            if (enemy = null)
            {
                enemy = other.GetComponentInParent<BasicEnemyMovement>();
            }
            else if (enemy != null)
            {
                enemy.TakeDamage(_bounceDamage);
            }
            Destroy(this.gameObject);
        }
    }
}
