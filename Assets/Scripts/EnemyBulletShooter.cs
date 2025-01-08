using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletShooter : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _totalTime = 1.5f;
    private float _time;

    [SerializeField] private GameObject _enemyBullet;
    void Start()
    {
        _time = _totalTime;
    }

    // Update is called once per frame
    void Update()
    {
        ShootBullet(_enemyBullet);
    }
    void ShootBullet(GameObject enemyBullet)
    {
        _time -= Time.deltaTime;
        if (_time < 0)
        {
            Instantiate(enemyBullet,gameObject.transform.position, Quaternion.identity);
            //Debug.Log("shoot enemybullet");
            _time = _totalTime;
        }
    }
}
