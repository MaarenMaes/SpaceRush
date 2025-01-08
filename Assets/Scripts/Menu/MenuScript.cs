using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Level")]
    [SerializeField] private int _settingMenu = 2;
    public void LoadSettingsMenu()
    {
        SceneManager.LoadScene(_settingMenu);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadSettingsMenu();
        }
    }
    public void OnExitMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LoadSettingsMenu();
        }
    }
}
