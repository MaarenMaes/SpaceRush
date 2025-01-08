using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    // Serialized fields for level and canvases
    [Header("Level")]
    private int _mainMenu = 0;

    [Header("Controls Canvas")]
    [SerializeField] private Canvas controlsCanvas;
    [SerializeField] private Canvas mainCanvas;

    [Header("Volume Canvas")]
    [SerializeField] private Canvas volumeCanvas1;
    [SerializeField] private Canvas volumeCanvas2;

    // Method to open the level 0 scene
    public void OpenMainMenu()
    {
        SceneManager.LoadScene(_mainMenu);
    }

    // Method to show the controls canvas and hide the main canvas
    public void ShowControls()
    {
        if (controlsCanvas != null && mainCanvas != null)
        {
            controlsCanvas.gameObject.SetActive(true);
            mainCanvas.gameObject.SetActive(false);
        }
    }

    // Method to toggle between volumeCanvas1 and volumeCanvas2
    public void ShowVolume()
    {
        if (volumeCanvas1 != null && volumeCanvas2 != null)
        {
            bool isVolumeCanvas1Active = volumeCanvas1.gameObject.activeSelf;
            volumeCanvas1.gameObject.SetActive(!isVolumeCanvas1Active);
            volumeCanvas2.gameObject.SetActive(isVolumeCanvas1Active);
        }
    }
}
