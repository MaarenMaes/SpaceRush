using UnityEngine;

public class BasicEnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float minSpeed = 3f; // Minimum speed of the enemy movement
    public float maxSpeed = 7f; // Maximum speed of the enemy movement
    private string targetTag = "Planet"; // Tag of the object the enemy will move towards

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 1; // Maximum health of the enemy
    private int currentHealth; // Current health of the enemy

    private EnemyObjectSpawner _enemySpawner;
    private ScoreManager _scoreManagerScript;
    private GameObject target; // Reference to the target object
    public float speed; // Current speed of the enemy

    [Header("PostProcessing Settings")]
    [SerializeField] private GameObject _explosionParticleSystem;

    AudioManager _audioManager;

    private void Awake()
    {
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void Start()
    {
        target = GameObject.FindWithTag(targetTag);
        speed = Random.Range(minSpeed, maxSpeed); // Generate a random speed within the specified range
        currentHealth = maxHealth; // Set current health to max health at start
        _enemySpawner = FindObjectOfType<EnemyObjectSpawner>();
        _scoreManagerScript = FindObjectOfType<ScoreManager>();
    }

    void Update()
    {
        MoveTowardsPlanet();
    }

    void MoveTowardsPlanet()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            transform.LookAt(target.transform.position);
        }
        else
        {
            Debug.Log($"No target found with tag {targetTag}");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Reduce current health by damage amount

        if (currentHealth <= 0)
        {
            Die(); // If health drops to or below 0, call Die function
        }
    }

    void Die()
    {
        Destroy(gameObject); // Destroy the enemy object when it dies
        CinemachineShake.Instance.ShakeCamera(3f, 0.3f);
        _audioManager.PlaySFX(_audioManager._explosion);

        Instantiate(_explosionParticleSystem, transform.position, Quaternion.identity);

        if (_enemySpawner != null)
        {
            // Increment the enemies killed in the spawner
            _enemySpawner.enemiesKilled++;
            _enemySpawner.totalEnemiesKilled++;

            // Add credits for killing this enemy to the current credits
            int creditsPerKill = 100;
            _scoreManagerScript._credits += creditsPerKill;
            _scoreManagerScript._score = _enemySpawner.totalEnemiesKilled * 100;
        }
    }

}