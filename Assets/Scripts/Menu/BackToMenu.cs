using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    [Header("Level")]
    [SerializeField] private int _menuScene = 2;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Debug.Log("Open Menu");
            LoadMenuScene();
        }
    }

    private void LoadMenuScene()
    {
        SceneManager.LoadScene(_menuScene);
    }

    public void OnStart(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LoadMenuScene();
        }
    }
}
