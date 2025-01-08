using UnityEngine;
using System.Collections.Generic;

public class PlanetTurret : MonoBehaviour
{
    [SerializeField]
    public float _totalTime = 1.5f;
    private float _time;

    [SerializeField]
    private float _rotationSpeed = 10f;

    public bool _inRange = false;

    public GameObject _barrel;
    public GameObject _bulletPrefab;
    public GameObject _rotatingPart;

    private GameObject _targetEnemy;

    private EnemyObjectSpawner _enemySpawner;

    private void Start()
    {
        _time = _totalTime;
        _enemySpawner = FindObjectOfType<EnemyObjectSpawner>();
    }

    private void Update()
    {
        //Debug.Log(_inRange);
        if (_inRange)
        {
            _time -= Time.deltaTime;
            if (_time < 0)
            {
                if (_targetEnemy != null)
                {
                    Instantiate(_bulletPrefab, _barrel.transform.position, _barrel.transform.rotation);
                    _time = _totalTime;
                }
                else
                {
                    // No valid target, reset variables or take other action
                    _inRange = false;
                    _time = _totalTime;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            assignEnemy(other.gameObject);
            _inRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other != null && other.tag == "Enemy")
        {
            if (other.gameObject == _targetEnemy.gameObject)
            {
                _enemySpawner.totalEnemiesKilled++;
                Destroy(other.gameObject);
            }
        }
        
    }

    private void assignEnemy(GameObject enemy)
    {
        if (_targetEnemy == null || Vector3.Distance(transform.position, enemy.transform.position) < Vector3.Distance(transform.position, _targetEnemy.transform.position))
        {
            _targetEnemy = enemy;
        }

        var lookPos = _targetEnemy.transform.position - _rotatingPart.transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        _rotatingPart.transform.rotation = Quaternion.Slerp(_rotatingPart.transform.rotation, rotation, Time.deltaTime * _rotationSpeed);
    }
}
