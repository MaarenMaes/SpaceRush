using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShieldController : MonoBehaviour
{
    // Smoothness of rotation
    [SerializeField] private float rotationSmoothness = 10f;

    // Update is called once per frame
    void Update()
    {
        // Rotate the shield based on mouse input
        if (Input.GetMouseButton(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.y));
            mousePos.y = transform.position.y;
            transform.LookAt(mousePos);
        }
    }

    // Called when the shield movement input is triggered
    public void OnMoveShield(InputAction.CallbackContext context)
    {
        // Check if the input is performed
        if (context.performed)
        {
            // Get the movement vector from the input
            Vector2 moveInput = context.ReadValue<Vector2>();

            // Calculate the angle based on the input
            float angle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg;

            // Convert the angle to a positive value
            /*if (angle < 0)
            {
                angle += 360f;
            }*/

            // Calculate the target rotation
            Quaternion targetRotation = Quaternion.Euler(0f, angle, 0f);

            // Smoothly interpolate towards the target rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSmoothness * Time.fixedDeltaTime);
        }
    }
}
