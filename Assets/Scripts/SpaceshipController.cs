using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceshipController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform barrel;
    public LineRenderer _lineRenderer; // LineRenderer for the laser beam

    public bool _singleBulletGuns;
    public bool _tripleBulletGuns;
    public bool _doubleBulletGuns;
    public bool _lazerGun;

    public float _shootCooldown;
    private float _time;

    public bool _isPickupActive;

    [Header("Pickup Settings")]
    [SerializeField] private float _totalActiveTime;
    [SerializeField] private float _activeTimer;

    [SerializeField] private bool _canShoot;

    [Header("Laser Settings")]
    public float laserMaxDistance = 50f; // Maximum distance the laser can travel

    AudioManager _audioManager;
    private bool _playLazerAudioClip;

    private MouseLock _mouseLockScript;
    private bool _isMouseEnabled;
    private bool _controllerShoot;
    private Vector2 _controllerMoveInput;

    [SerializeField] private float rotationSmoothness = 5f;

    private void Awake()
    {
        _mouseLockScript = GameObject.FindAnyObjectByType<MouseLock>();
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void Start()
    {
        _time = _shootCooldown;
        _activeTimer = _totalActiveTime;

        _mouseLockScript.EnableMouse();
        _isMouseEnabled = true;

        _isPickupActive = false;

        _tripleBulletGuns = false;
        _singleBulletGuns = true;
        _lazerGun = false;
        _doubleBulletGuns = false;

        if (_lineRenderer == null)
        {
            _lineRenderer = barrel.GetComponent<LineRenderer>();
        }
        _lineRenderer.enabled = false;
    }

    void Update()
    {
        CheckIfMouseIsUsed();

        if (_isMouseEnabled)
        {
            AimSpaceShipMouse();
        }
        else if (_isMouseEnabled == false)
        {
            AimSpaceShipController();
        }

        TimerLogic();

        if (_tripleBulletGuns)
        {
            PickupActiveTimer();
        }
        if (_doubleBulletGuns)
        {
            PickupActiveTimer();
        }
        if (_lazerGun)
        {
            PickupActiveTimer();
            VisualizeLazer();
        }

        if (_playLazerAudioClip && _lineRenderer.enabled == true)
        {
            _audioManager.PlaySFX(_audioManager._lazers);
            _playLazerAudioClip = false;
        }
        if (_lineRenderer.enabled == false)
        {
            _playLazerAudioClip = true;
        }
    }

    private bool TimerLogic()
    {
        _time -= Time.deltaTime;
        if (_time < 0)
        {
            _time = _shootCooldown;
            _canShoot = true;
            //Debug.Log("Shoot");
            return _canShoot;
        }
        if (Input.GetMouseButtonUp(0))
        {
            _canShoot = true;
        }
        return _canShoot = false;
    }

    void AimSpaceShipMouse()
    {
        if (Input.GetMouseButton(1) == false && _controllerShoot == false)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.y));
            mousePos.y = transform.position.y;
            transform.LookAt(mousePos);

            if (Input.GetMouseButton(0) && _canShoot)
            {
                if (_singleBulletGuns)
                {
                    _audioManager.PlaySFX(_audioManager._shoot);
                    CinemachineShake.Instance.ShakeCamera(1f, 0.1f);
                    ShootSingleBullet(mousePos);
                }
                else if (_tripleBulletGuns)
                {
                    _audioManager.PlaySFX(_audioManager._shoot);
                    CinemachineShake.Instance.ShakeCamera(1f, 0.1f);
                    ShootTripleBullet(mousePos);
                }
                else if (_doubleBulletGuns)
                {
                    _audioManager.PlaySFX(_audioManager._shoot);
                    CinemachineShake.Instance.ShakeCamera(1f, 0.1f);
                    ShootDoubleBullet(mousePos);
                }
            }
        }
    }
    void AimSpaceShipController()
    {
        // Check if the controller move input magnitude is greater than zero
        if (_controllerMoveInput.sqrMagnitude > 0.01f)
        {
            // Calculate the look direction based on the controller input
            Vector3 lookDirection = new Vector3(_controllerMoveInput.x, 0, _controllerMoveInput.y);
            // Smoothly rotate the spaceship towards the controller input direction
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSmoothness * Time.deltaTime);
        }

        // Check if the shoot button is pressed and shooting is allowed
        if (_controllerShoot && _canShoot)
        {
            // Call the appropriate shoot method based on the weapon type
            if (_singleBulletGuns)
            {
                // Shooting logic for single bullet guns
                _audioManager.PlaySFX(_audioManager._shoot); // Play shooting sound effect
                CinemachineShake.Instance.ShakeCamera(1f, 0.1f);
                ShootSingleBullet(transform.forward);
            }
            else if (_tripleBulletGuns)
            {
                // Shooting logic for triple bullet guns
                _audioManager.PlaySFX(_audioManager._shoot); // Play shooting sound effect
                CinemachineShake.Instance.ShakeCamera(1f, 0.1f);
                ShootTripleBullet(transform.forward);
            }
            else if (_doubleBulletGuns)
            {
                // Shooting logic for double bullet guns
                _audioManager.PlaySFX(_audioManager._shoot); // Play shooting sound effect
                CinemachineShake.Instance.ShakeCamera(1f, 0.1f);
                ShootDoubleBullet(transform.forward);
            }
        }
    }



    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0f)
        {
            _controllerShoot = true;
        }
        if (context.ReadValue<float>() == 0f)
        {
            _controllerShoot = false;
        }
    }

    public void OnMoveShip(InputAction.CallbackContext context)
    {
        _controllerMoveInput = context.ReadValue<Vector2>();
    }

    void ShootSingleBullet(Vector3 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, barrel.position, barrel.rotation);
        bullet.transform.forward = direction;

        _lazerGun = false;
        _tripleBulletGuns = false;
        _doubleBulletGuns = false;
        _lineRenderer.enabled = false;
    }

    void ShootTripleBullet(Vector3 direction)
    {
        Vector3 centerDirection = direction.normalized;
        Vector3 leftDirection = Quaternion.Euler(0, -20, 0) * centerDirection;
        Vector3 rightDirection = Quaternion.Euler(0, 20, 0) * centerDirection;

        ShootBulletWithDirection(centerDirection);
        ShootBulletWithDirection(leftDirection);
        ShootBulletWithDirection(rightDirection);
    }

    void ShootDoubleBullet(Vector3 direction)
    {
        Vector3 centerDirection = direction.normalized;
        Vector3 leftDirection = Quaternion.Euler(0, -20, 0) * centerDirection;
        Vector3 rightDirection = Quaternion.Euler(0, 20, 0) * centerDirection;

        ShootBulletWithDirection(leftDirection);
        ShootBulletWithDirection(rightDirection);
    }

    void VisualizeLazer()
    {
        if (_lazerGun)
        {
            if (_isMouseEnabled == false)
            {
                // Controller shooting logic for the laser...
                if (_controllerShoot)
                {
                    ShootLaserController();
                }
                else if (!_controllerShoot && _lineRenderer.enabled)
                {
                    _lineRenderer.enabled = false;
                }
            }
            else if (Input.GetMouseButton(0))
            {
                // Mouse shooting logic for the laser...
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.y));
                mousePos.y = transform.position.y;
                ShootLazerMouse(mousePos);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _lineRenderer.enabled = false;
            }
        }
    }

    void ShootLazerMouse(Vector3 mousePos)
    {
        if (Input.GetMouseButton(1) == false)
        {
            Vector3 laserDirection = (mousePos - barrel.position).normalized;
            _lineRenderer.SetPosition(0, barrel.position);

            RaycastHit[] hits = Physics.RaycastAll(barrel.position, laserDirection, laserMaxDistance);
            Vector3 endPosition = barrel.position + laserDirection * laserMaxDistance;

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.tag == "Enemy")
                {
                    BasicEnemyMovement enemy = hit.collider.gameObject.GetComponentInParent<BasicEnemyMovement>();
                    if (enemy != null)
                    {
                        Debug.Log("LazerHit");
                        enemy.TakeDamage(10); // Adjust damage value as needed
                    }
                }
                if (hit.collider.gameObject.tag == "EnemyBullet")
                {
                    Destroy(hit.collider.gameObject);
                }

                // Set the laser end position to the last hit point if it is closer than the current end position
                if (hit.distance < Vector3.Distance(barrel.position, endPosition))
                {
                    endPosition = hit.point;
                }
            }

            _lineRenderer.SetPosition(1, endPosition);
            _lineRenderer.enabled = true;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            _lineRenderer.enabled = false;
        }
    }
    void ShootLaserController()
    {
        Vector3 shootDirection = transform.forward;
        _lineRenderer.SetPosition(0, barrel.position);

        RaycastHit[] hits = Physics.RaycastAll(barrel.position, shootDirection, laserMaxDistance);
        Vector3 endPosition = barrel.position + shootDirection * laserMaxDistance;

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.tag == "Enemy")
            {
                BasicEnemyMovement enemy = hit.collider.gameObject.GetComponentInParent<BasicEnemyMovement>();
                if (enemy != null)
                {
                    Debug.Log("LazerHit");
                    enemy.TakeDamage(10); // Adjust damage value as needed
                }
            }
            if (hit.collider.gameObject.tag == "EnemyBullet")
            {
                Destroy(hit.collider.gameObject);
            }

            // Set the laser end position to the last hit point if it is closer than the current end position
            if (hit.distance < Vector3.Distance(barrel.position, endPosition))
            {
                endPosition = hit.point;
            }
        }

        _lineRenderer.SetPosition(1, endPosition);
        _lineRenderer.enabled = true;
    }

    void ShootBulletWithDirection(Vector3 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, barrel.position, barrel.rotation);
        bullet.transform.forward = direction;
    }

    void PickupActiveTimer()
    {
        _activeTimer -= Time.deltaTime;
        if (_activeTimer < 0)
        {
            _activeTimer = _totalActiveTime;
            _singleBulletGuns = true;
            _isPickupActive = false;
        }
    }
    void CheckIfMouseIsUsed()
    {
        // Check if the mouse input is greater than 0
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            // Mouse is being used
            MouseBeingUsed();
        }
        else if (_controllerMoveInput.x > 0 || _controllerMoveInput.y > 0)
        {
            // Mouse is not being used
            MouseNotUsed();
        }
    }
    void MouseBeingUsed()
    {
        _mouseLockScript.EnableMouse();
        _isMouseEnabled = true;
    }

    void MouseNotUsed()
    {
        _mouseLockScript.DisableMouse();
        _isMouseEnabled = false;
    }
}
