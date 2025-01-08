using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _creditsTMP;
    [SerializeField] private TextMeshProUGUI _totalScoreTMP;

    [SerializeField] private TextMeshProUGUI _turretUpgradeTMP;
    [SerializeField] private TextMeshProUGUI _bulletUpgradeTMP;
    [SerializeField] private TextMeshProUGUI _HealthUpgradeTMP;

    [SerializeField] private Image _turretUpgradeIMG;
    [SerializeField] private Image _bulletUpgradeIMG;
    [SerializeField] private Image _healthUpgradeIMG;

    [SerializeField] private GameObject _bulletPrefab;

    private float _initialScore;
    public float _credits;
    public float _score;
    public int totalEnemiesKilled = 0;

    private int _turretUpgradeCost = 500;
    private int _bulletUpgradeCost = 100;
    private int _planetHealCost = 300;

    private PlanetTurret _planetTurretScript;
    private Bullet _bulletScript;
    private PlanetHealth _planetHealthScript;

    AudioManager _audioManager;
    private Vector3 originalScale; // Vector3 to store the original scale



    public int HighScore { get; private set; } // Public property for the high score

    private void Awake()
    {
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        _turretUpgradeTMP.SetText($"{_turretUpgradeCost.ToString()} $");
        _bulletUpgradeTMP.SetText($"{_bulletUpgradeCost.ToString()} $");
        _HealthUpgradeTMP.SetText($"{_planetHealCost.ToString()} $");
    }

    void Start()
    {
        _planetTurretScript = GameObject.FindObjectOfType<PlanetTurret>();
        _bulletScript = _bulletPrefab.GetComponent<Bullet>();
        _planetHealthScript = GameObject.FindObjectOfType<PlanetHealth>();

        _bulletScript._speed = 10f;
        _initialScore = 0;

        totalEnemiesKilled = 0;

        // Load the high score at the start
        HighScore = PlayerPrefs.GetInt("HighScore", 0);

        originalScale = _turretUpgradeIMG.transform.localScale;
    }

    void Update()
    {
        KeyActivation();
        if (_planetHealthScript._health < 100)
        {
            _HealthUpgradeTMP.SetText($"{_planetHealCost.ToString()} $");
        }
        else
        {
            _HealthUpgradeTMP.SetText("Full!");
        }
        if (_credits != _initialScore) //don't update score every frame if not needed
        {
            ScoreCalculator();
        }
    }

    void UpgradeTurret(float turretUpgradeCost)
    {
        if (_credits >= turretUpgradeCost)
        {
            if (_planetTurretScript._totalTime >= 0.3) //minimum time is 0.2
            {
                float deductionTime = 0.1f;

                _credits -= turretUpgradeCost;
                _planetTurretScript._totalTime -= deductionTime;
                _turretUpgradeCost += 40;
                _turretUpgradeTMP.SetText($"{_turretUpgradeCost.ToString()} $");
                _audioManager.PlaySFX(_audioManager._upgrade);
                StartCoroutine(ScaleImage(_turretUpgradeIMG)); // Call coroutine for scaling effect
            }
            else
            {
                Debug.Log("Turret is maxed out");
                _audioManager.PlaySFX(_audioManager._denied);
                _turretUpgradeTMP.SetText("Max!");
            }
        }
        else
        {
            Debug.Log($"Not enough credits to upgrade turret => credits: {_credits}");
            _audioManager.PlaySFX(_audioManager._denied);
        }
    }

    void UpgradeBullets(float bulletUpgradeCost)
    {
        if (_credits >= bulletUpgradeCost)
        {
            if (_bulletScript._speed <= 25)
            {
                float deductionTime = 0.5f;

                _credits -= bulletUpgradeCost;
                _bulletScript._speed += deductionTime;
                _bulletUpgradeCost += 20;
                _bulletUpgradeTMP.SetText($"{_bulletUpgradeCost.ToString()} $");
                _audioManager.PlaySFX(_audioManager._upgrade);
                StartCoroutine(ScaleImage(_bulletUpgradeIMG)); // Call coroutine for scaling effect
            }
            else
            {
                Debug.Log("Bullet is maxed out");
                _audioManager.PlaySFX(_audioManager._denied);
                _bulletUpgradeTMP.SetText("Max!");
            }
        }
        else
        {
            Debug.Log($"Not enough credits to upgrade bullet => credits: {_credits}");
            _audioManager.PlaySFX(_audioManager._denied);
        }
    }

    void HealPlanet(float planetHealCost)
    {
        if (_credits >= planetHealCost)
        {
            if (_planetHealthScript._health < 100)
            {
                float healAmount = 8f;

                _credits -= planetHealCost;
                _planetHealthScript.Heal(healAmount);
                _planetHealCost += 10;
                _HealthUpgradeTMP.SetText($"{_planetHealCost.ToString()} $");
                _audioManager.PlaySFX(_audioManager._upgrade);
                StartCoroutine(ScaleImage(_healthUpgradeIMG)); // Call coroutine for scaling effect
            }
            else
            {
                Debug.Log("Planet is already Max Health");
                _audioManager.PlaySFX(_audioManager._denied);
                _HealthUpgradeTMP.SetText("Full!");
            }
        }
        else
        {
            Debug.Log($"Not enough credits to heal planet => credits: {_credits}");
            _audioManager.PlaySFX(_audioManager._denied);
        }
    }

    public void ScoreCalculator()
    {
        _creditsTMP.SetText($"credits: {_credits} $");
        _totalScoreTMP.SetText($"Score: {_score}");
        _initialScore = _credits;

        // Check and update the high score
        UpdateHighScore((int)_score);
    }

    void KeyActivation()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            UpgradeTurret(_turretUpgradeCost);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            UpgradeBullets(_bulletUpgradeCost);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            HealPlanet(_planetHealCost);
        }
    }

    public void OnUpgradeTurret(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            UpgradeTurret(_turretUpgradeCost);
            Debug.Log("UpgradeTurret");
        }
    }

    public void OnUpgradeBullet(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            UpgradeBullets(_bulletUpgradeCost);
            Debug.Log("UpgradeBullet");
        }
    }

    public void OnUpgradeHealth(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            HealPlanet(_planetHealCost);
            Debug.Log("UpgradeHealth");
        }
    }

    void UpdateHighScore(int currentScore)
    {
        if (currentScore > HighScore)
        {
            HighScore = currentScore;
            PlayerPrefs.SetInt("HighScore", HighScore); // Save the new high score
            PlayerPrefs.Save(); // Ensure the new high score is saved
            Debug.Log("New high score: " + HighScore);
        }
    }

    IEnumerator ScaleImage(Image image)
    {
        Vector3 targetScale = originalScale * 1.2f;
        float duration = 0.2f;

        // Scale up
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            image.transform.localScale = Vector3.Lerp(originalScale, targetScale, t / duration);
            yield return null;
        }
        image.transform.localScale = targetScale;

        // Scale down
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            image.transform.localScale = Vector3.Lerp(targetScale, originalScale, t / duration);
            yield return null;
        }
        image.transform.localScale = originalScale;
    }
}
