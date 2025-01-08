using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyObjectSpawner : MonoBehaviour
{
    public GameObject[] _enemyType;
    public GameObject[] _pickUps;

    [Header("Wave Settings")]
    public float spawnDelay = 2.5f;
    private float elapsedTime;
    private int currentWave = 2;
    public int enemiesKilled = 0;
    public int totalEnemiesKilled = 0;
    private int waveGoal = 10;
    private int enemiesIncreasePerWave = 2;
    private bool isWaveCompleted = false; // Flag to check if wave is completed
    private bool isResting = false; // Flag to check if rest period is active

    [Header("Pickup Settings")]
    private float _pickupSpawnDelay = 15;
    private float _pickUpTimer;

    [Header("Speed Settings")]
    public float minEnemySpeed = 3f; // Minimum speed of enemies
    public float maxEnemySpeed = 7f; // Maximum speed of enemies

    [Header("SpawnChance Settings")]
    public float _commonEnemySpawnChance = 1f; // Default spawn chance
    public float _mediumRareSpawnChance = 1f; // Medium spawn chance
    public float _rareSpawnChance = 1f; // Quick spawn chance

    public Transform spaceshipLocation;

    [Header("SpawnLocation Settings")]
    [SerializeField] private Transform _leftTopCorner;
    [SerializeField] private Transform _rightTopCorner;
    [SerializeField] private Transform _leftBottomCorner;
    [SerializeField] private Transform _rightBottomCorner;

    private SpaceshipController _spaceshipController;
    [SerializeField] private TextMeshProUGUI _waveText;

    private void Start()
    {
        spawnDelay = 3f;
        _waveText.enabled = false;
        _spaceshipController = GameObject.FindObjectOfType<SpaceshipController>();
        _pickUpTimer = _pickupSpawnDelay;
        totalEnemiesKilled = 0;
    }

    void Update()
    {
        if (isResting) return; // Do not update if resting

        elapsedTime += Time.deltaTime;

        if (elapsedTime > spawnDelay)
        {
            SpawnRandomEnemy();
            elapsedTime = 0;
        }

        if (enemiesKilled >= waveGoal && !isWaveCompleted)
        {
            isWaveCompleted = true;
            StartCoroutine(StartNextWave());
        }

        SpawnPickups();
    }

    Vector3 CalculateSpawnLocation()
    {
        Vector3 spawnPosition;
        float side = Random.Range(0f, 4f);
        if (side < 1f)
        {
            spawnPosition = SpawnPositionLogic(_leftTopCorner.position, _leftBottomCorner.position);
        }
        else if (side < 2f)
        {
            spawnPosition = SpawnPositionLogic(_rightTopCorner.position, _rightBottomCorner.position);
        }
        else if (side < 3f)
        {
            spawnPosition = SpawnPositionLogic(_leftTopCorner.position, _rightTopCorner.position);
        }
        else
        {
            spawnPosition = SpawnPositionLogic(_leftBottomCorner.position, _rightBottomCorner.position);
        }

        return spawnPosition;
    }

    Vector3 SpawnPositionLogic(Vector3 corner1, Vector3 corner2)
    {
        float randomX = Random.Range(corner1.x, corner2.x);
        float randomZ = Random.Range(corner1.z, corner2.z);
        return new Vector3(randomX, 0, randomZ);
    }

    void SpawnRandomEnemy()
    {
        if (isResting) return; // Do not spawn enemies if resting

        int randomIndex = Random.Range(0, _enemyType.Length);
        GameObject enemyToSpawn = _enemyType[randomIndex];

        // Determine spawn chance based on index
        float spawnChance = _commonEnemySpawnChance;
        if (randomIndex == 1)
        {
            spawnChance = _mediumRareSpawnChance;
        }
        else if (randomIndex == 2)
        {
            // Ensure the third enemy can only spawn from tier 3
            if (currentWave >= 4)
            {
                spawnChance = _rareSpawnChance;
            }
            else
            {
                return;
            }
        }

        // Check if enemy should spawn based on spawn chance
        if (Random.value <= spawnChance)
        {
            // Randomize enemy speed within the new range
            float randomSpeed = Random.Range(minEnemySpeed, maxEnemySpeed);
            enemyToSpawn.GetComponent<BasicEnemyMovement>().speed = randomSpeed;

            Instantiate(enemyToSpawn, CalculateSpawnLocation(), enemyToSpawn.transform.rotation);
            Debug.Log(enemyToSpawn.transform.rotation);
        }
    }

    IEnumerator StartNextWave()
    {
        isResting = true; // Set resting to true to prevent enemy spawning
        _waveText.enabled = true;
        _waveText.SetText("Wave " + (currentWave) + " started! Difficulty increased.");
        Debug.Log("Wave " + currentWave + " completed! 5 seconds rest time.");

        // Start the scaling animation coroutine and wait for it to complete
        yield return StartCoroutine(AnimateWaveText());

        yield return new WaitForSeconds(5); // 5 seconds rest time

        currentWave++;
        enemiesKilled = 0;
        waveGoal += enemiesIncreasePerWave; // Increase the wave goal
        if(spawnDelay > 1)
        {
            spawnDelay -= 0.3f; // Increase spawn rate
        }
        else if(spawnDelay <= 1)
        {
            spawnDelay -= 0.05f; // Increase spawn rate
        }
        minEnemySpeed += 0.3f; // Increase minimum enemy speed
        maxEnemySpeed += 0.3f; // Increase maximum enemy speed

        Debug.Log("Wave " + currentWave + " started! Difficulty increased.");

        isWaveCompleted = false; // Reset the wave completed flag
        isResting = false; // Set resting to false to resume enemy spawning
    }

    IEnumerator AnimateWaveText()
    {
        float duration = 2f; // Duration of the animation in seconds
        float halfDuration = duration / 2f;
        Vector3 originalScale = _waveText.transform.localScale;
        Vector3 targetScale = originalScale * 1.5f; // Scale up to twice the original size

        // Scale up
        for (float t = 0; t < halfDuration; t += Time.deltaTime)
        {
            _waveText.transform.localScale = Vector3.Lerp(originalScale, targetScale, t / halfDuration);
            yield return null;
        }
        _waveText.transform.localScale = targetScale;

        // Scale down
        for (float t = 0; t < halfDuration; t += Time.deltaTime)
        {
            _waveText.transform.localScale = Vector3.Lerp(targetScale, originalScale, t / halfDuration);
            yield return null;
        }
        _waveText.transform.localScale = originalScale;

        // Disable the text after the animation
        _waveText.enabled = false;
    }

    void SpawnPickups()
    {
        _pickUpTimer -= Time.deltaTime;
        if (_pickUpTimer < 0)
        {
            Pickup pickup = GameObject.FindObjectOfType<Pickup>();
            if (_spaceshipController._isPickupActive == false && pickup == null)
            {
                int randomIndex = Random.Range(0, _pickUps.Length);
                GameObject pickupGameobject = _pickUps[randomIndex];

                Instantiate(pickupGameobject, new Vector3(SpawnPositionLogic(_leftTopCorner.position, _rightTopCorner.position).x,
                    0f, SpawnPositionLogic(_leftTopCorner.position, _leftBottomCorner.position).z), Quaternion.identity);
                _pickUpTimer = _pickupSpawnDelay;
            }
            else if (pickup == null && _spaceshipController._isPickupActive == true)
            {
                _pickUpTimer = _pickupSpawnDelay;
            }
        }
    }
}
