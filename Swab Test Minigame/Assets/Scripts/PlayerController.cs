using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private GameObject item; // item to carry

    [SerializeField]
    private float moveSpeed = 5.0f;

    private Vector2 movement;

    private void Update()
    {
        transform.Translate(new Vector2(movement.x, movement.y) * moveSpeed * Time.deltaTime, Space.World);
    }

    public void OnMove(CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    public void OnUse(CallbackContext context)
    {
        if (item == null)
        {
            Debug.Log("No item to use!");
        } else
        {
            Debug.Log("Use item!");
        }
    }
}
