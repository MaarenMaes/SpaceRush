using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    // Start is called before the first frame update
    

    [SerializeField] private float _timeTillDespawn;
    private float _despawnTimer;

    [SerializeField] private bool _tripleBulletPickup;
    [SerializeField] private bool _laserBulletsPickup;
    [SerializeField] private bool _doubleBulletsPickup;

    private SpaceshipController _spaceshipControllerScript;

    AudioManager _audioManager;
    private void Awake()
    {
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void Start()
    {        
        _despawnTimer = _timeTillDespawn;
        _spaceshipControllerScript = FindObjectOfType<SpaceshipController>();
    }

    // Update is called once per frame
    void Update()
    {
        AliveTimer();
        Rotate();
    }
    void ActivateTripleBullets()
    {
        _audioManager.PlaySFX(_audioManager._pickup);
        _spaceshipControllerScript._isPickupActive= true;
        _spaceshipControllerScript._singleBulletGuns = false;
        _spaceshipControllerScript._tripleBulletGuns = true;
        _spaceshipControllerScript._doubleBulletGuns = false;
    }
    void ActivateDoubleBullets()
    {
        _audioManager.PlaySFX(_audioManager._pickup);
        _spaceshipControllerScript._isPickupActive = true;
        _spaceshipControllerScript._singleBulletGuns = false;
        _spaceshipControllerScript._tripleBulletGuns = false;
        _spaceshipControllerScript._doubleBulletGuns = true;
    }
    void ActivateLazerBullets()
    {
        _audioManager.PlaySFX(_audioManager._pickup);
        _spaceshipControllerScript._isPickupActive = true;
        _spaceshipControllerScript._singleBulletGuns = false;
        _spaceshipControllerScript._lazerGun = true;
        _spaceshipControllerScript._doubleBulletGuns = false;
    }
    
    void AliveTimer() // the time that the pickup is available to pickup
    {
        _despawnTimer -= Time.deltaTime;
        if(_despawnTimer < 0)
        {
            _despawnTimer = _timeTillDespawn;
            Debug.Log("Pickup No longer available");
            Destroy(this.gameObject);
            
        }
    }
    void Rotate()
    {
        // Rotate around the object's local X axis
        transform.Rotate(Vector3.right * Time.deltaTime * 25f); // Adjust the rotation speed as needed
        transform.Rotate(Vector3.up * Time.deltaTime * 25f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bullet")
        {
            if (_tripleBulletPickup)
            {
                ActivateTripleBullets();
                Destroy(this.gameObject);
            }
            else if (_laserBulletsPickup)
            {
                ActivateLazerBullets();
                Destroy(this.gameObject);
            }
            else if (_doubleBulletsPickup)
            {
                ActivateDoubleBullets();
                Destroy(this.gameObject);
            }
            
        }
    }
}
