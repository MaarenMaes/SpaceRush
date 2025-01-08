using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlanetHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] Image _healthBar;
    [SerializeField] public float _health = 100;

    [Header("PostProcessing Settings")]
    [SerializeField] private GameObject _explosionParticleSystem;

    private ScoreManager _scoreManagerScript;

    AudioManager _audioManager;
    private void Awake()
    {
        _scoreManagerScript = FindObjectOfType<ScoreManager>();
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Update()
    {
        RestartLevel();
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        _healthBar.fillAmount = _health / 100f;
    }

    public void Heal(float healingAmount)
    {
        _health += healingAmount;
        _health = Mathf.Clamp(_health, 0, 100);
        _healthBar.fillAmount = _health / 100f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            GameObject parentGameObject = null;
            CinemachineShake.Instance.ShakeCamera(3f, 0.1f);
            Instantiate(_explosionParticleSystem, other.transform.position, Quaternion.identity);
            _audioManager.PlaySFX(_audioManager._explosion);

            if (other.gameObject.transform.parent != null)
            {
                parentGameObject = other.gameObject.transform.parent.gameObject;
            }

            Destroy(other.gameObject);

            if (parentGameObject != null)
            {
                Destroy(parentGameObject);
            }

            TakeDamage(7);
        }

        if (other.gameObject.tag == "EnemyBullet")
        {
            TakeDamage(0.5f);
            Destroy(other.gameObject);
        }
        Debug.Log(other.gameObject.name);
    }

    private void RestartLevel()
    {
        if (_health <= 0)
        {

            // Retrieve current score
            float currentScore = _scoreManagerScript._score;

            // Save current score in PlayerPrefs
            PlayerPrefs.SetFloat("CurrentScore", currentScore);
            PlayerPrefs.Save();

            // Load the scene with scene index 3
            SceneManager.LoadScene(3);
        }
    }
}
