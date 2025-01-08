using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ShieldBehavior : MonoBehaviour
{
    private BasicEnemyMovement _enemyMovementScript;
    private Bullet _bulletScript;
    private BounceKill _bounceKillScript;

    AudioManager _audioManager;

    private void Awake()
    {
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        DeflectEnemy(other);
        DestoryEnemyBullet(other);
    }
    void DeflectEnemy(Collider enemyCollider)
    {
        if (enemyCollider.gameObject.CompareTag("Enemy"))
        {
            //Debug.Log("ShieldHit");
            _enemyMovementScript = enemyCollider.GetComponent<BasicEnemyMovement>();

            if (_enemyMovementScript == null)
            {
                Debug.Log("Could not find BasicEnemyMovement script on collider. Checking parent...");
                //BasicEnemyMovement enemy = other.GetComponentInParent<BasicEnemyMovement>();
                _enemyMovementScript = enemyCollider.GetComponentInParent<BasicEnemyMovement>();

                if (_enemyMovementScript == null)
                {
                    Debug.Log("Could not find BasicEnemyMovement script on collider's children.");
                }
                else //if (CheckForBounceKillScript(other))
                {
                    _audioManager.PlaySFX(_audioManager._shieldHit);
                    _enemyMovementScript.speed = -(_enemyMovementScript.speed);
                    //_bounceKillScript._hasBounced = true;
                }
            }
            else //if (_bounceKillScript._hasBounced == false)
            {
                _audioManager.PlaySFX(_audioManager._shieldHit);
                _enemyMovementScript.speed = -(_enemyMovementScript.speed);
                //_bounceKillScript._hasBounced = true;
            }
        }
    }
    void DestoryEnemyBullet(Collider enemyBulletCollider)
    {
        if(enemyBulletCollider.tag == "EnemyBullet")
        {
            Destroy(enemyBulletCollider.gameObject);
            _audioManager.PlaySFX(_audioManager._shieldHit);
            Debug.Log("destroyed bullet");
        }
    }
    /*private bool CheckForBounceKillScript(Collider other)
    {
        _bounceKillScript = other.GetComponent<BounceKill>();
        if(_bounceKillScript == null)
        {
            _bounceKillScript = other.GetComponentInParent<BounceKill>();
            if(_bounceKillScript == null)
            {
                Debug.Log("Could not find bouncekillScript");
                return false;
                
            }
            return false;
        }
        else
        {
            return _bounceKillScript._hasBounced;
        }
        
    }*/
}
