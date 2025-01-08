using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
public class CinemachineShake : MonoBehaviour
{
    // Start is called before the first frame update
    public static CinemachineShake Instance { get; private set; }

    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private float _shakeTimer;
    private float _shakeTimerTotal;
    private float _startingIntensity;
    private void Awake()
    {
        Instance = this;
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_shakeTimer> 0)
        {
            _shakeTimer -= Time.deltaTime;
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain =
                Mathf.Lerp(_startingIntensity, 0f, (1 - (_shakeTimer / _shakeTimerTotal)));
        }
        
    }
    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        _startingIntensity = intensity;
        _shakeTimerTotal = time;
        _shakeTimer = time;

    }
}
