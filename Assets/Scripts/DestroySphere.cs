using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySphere : MonoBehaviour
{
    // Trigger method to detect collision
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider has the tag "Enemy" or "EnemyBullet"
        if (other.tag == "Enemy" || other.tag == "EnemyBullet")
        {
            // Check if the other object has a parent to avoid NullReferenceException
            if (other.transform.parent != null)
            {
                // Destroy the parent of the other object
                Destroy(other.transform.parent.gameObject);
            }
            else
            {
                // Destroy the other object if it doesn't have a parent
                Destroy(other.gameObject);
            }
        }
        Debug.Log(other.gameObject.name);
    }
}
