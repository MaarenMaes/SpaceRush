using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DisableMouse();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            EnableMouse();
        }
    }

    public void DisableMouse()
    {
        Debug.Log("DisableMouse");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void EnableMouse()
    {
        Debug.Log("EnableMouse");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
