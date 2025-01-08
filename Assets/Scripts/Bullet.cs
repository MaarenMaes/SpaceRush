using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _maximumTimeAlive = 10;
    private float _currentTimeAlive = 0;

    public float _speed = 10;
    [SerializeField] private float _bulletDamage; // Damage inflicted by the bullet
    [SerializeField] private float _EnemyBulletDamage;

    private GameObject _planet;
    //private EnemyObjectSpawner _enemySpawner; // Reference to EnemyObjectSpawner script

    void Start()
    {        
        _planet = GameObject.FindWithTag("Planet");
    }

    void Update()
    {
        ShootSingleBullet();
        Timer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.gameObject.tag != "EnemyBullet")
        {
            if (other.gameObject.tag == "Enemy")
            {
                BasicEnemyMovement enemy = other.GetComponent<BasicEnemyMovement>(); // Get the BasicEnemyMovement script of the enemy

                if (enemy != null)
                {
                    enemy.TakeDamage((int)_bulletDamage); // Call TakeDamage function of the enemy, passing bullet damage
                }
                else if (enemy == null)
                {
                    enemy = other.GetComponentInParent<BasicEnemyMovement>();
                    enemy.TakeDamage((int)_bulletDamage);
                }
                else
                {
                    Debug.LogWarning("BasicEnemyMovement script not found on collided enemy!");
                }

                Destroy(gameObject); // Destroy the bullet object
            }
        }
    }
    void ShootSingleBullet()
    {
        if (this.gameObject.tag != "EnemyBullet")
        {
            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        }
        else
        {
            Vector3 direction = (_planet.transform.position - transform.position).normalized;
            transform.Translate(direction * _speed * Time.deltaTime);
        }
    }    
    void Timer()
    {
        _currentTimeAlive += Time.deltaTime;
        if(_currentTimeAlive >= _maximumTimeAlive)
        {
            Destroy(gameObject);
        }
    }
}
