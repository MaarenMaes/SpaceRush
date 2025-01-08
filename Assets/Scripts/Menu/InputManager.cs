using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private bool isUsingMouse = false;
    private EventSystem eventSystem;
    public GameObject firstButton;

    void Start()
    {
        eventSystem = EventSystem.current;
        SelectFirstButton();
    }

    void Update()
    {
        CheckInputMethod();
    }

    private void CheckInputMethod()
    {
        if (Mouse.current != null && Mouse.current.delta.ReadValue() != Vector2.zero)
        {
            if (!isUsingMouse)
            {
                isUsingMouse = true;
                eventSystem.SetSelectedGameObject(null);
            }
        }
        else if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame || Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            if (isUsingMouse)
            {
                isUsingMouse = false;
                SelectFirstButton();
            }
        }
    }

    private void SelectFirstButton()
    {
        if (firstButton != null)
        {
            eventSystem.SetSelectedGameObject(firstButton);
        }
    }
}
