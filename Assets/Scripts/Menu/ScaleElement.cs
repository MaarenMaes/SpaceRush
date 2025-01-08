using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScaleElement : MonoBehaviour
{
    public Vector3 minScale = new Vector3(1f, 1f, 1f);
    public Vector3 maxScale = new Vector3(1.5f, 1.5f, 1.5f);
    public float duration = 1f;

    private Vector3 originalScale;
    private float elapsedTime = 0f;
    private TextMeshProUGUI tmpElement;

    private void Start()
    {
        tmpElement = GetComponent<TextMeshProUGUI>();
        if (tmpElement == null)
        {
            Debug.LogError("No TextMeshProUGUI component found on this GameObject.");
            return;
        }

        originalScale = tmpElement.transform.localScale;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        float lerpFactor = (Mathf.Sin(elapsedTime / duration * Mathf.PI * 2) + 1) / 2; // Sinusoidal factor
        tmpElement.transform.localScale = Vector3.Lerp(minScale, maxScale, lerpFactor);
    }
}
