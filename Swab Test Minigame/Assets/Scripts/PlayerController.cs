using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private GameObject item; // item to carry
    private Animator anim;
    private bool onGround = false;

    [SerializeField]
    private float moveSpeed = 5.0f;

    private Vector2 movement;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        StartCoroutine(SetOnGround());
    }

    private void Update()
    {
        transform.Translate(new Vector2(movement.x, movement.y) * moveSpeed * Time.deltaTime, Space.World);
        if (movement.x > 0)
        {
            transform.localScale = new Vector3(10, 10, 10);
        }
        if (movement.x < 0)
        {
            transform.localScale = new Vector3(-10, 10, 10);
        }

        anim.SetFloat("Speed_f", Mathf.Max(Mathf.Abs(movement.x), Mathf.Abs(movement.y)));
    }

    public void OnMove(CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    public void OnJump(CallbackContext context)
    {
        if (onGround && anim)
        {
            anim.SetTrigger("Jump_trig");
            onGround = false;
            StartCoroutine(SetOnGround());
        }
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

    IEnumerator SetOnGround()
    {
        yield return new WaitForSeconds(1);
        onGround = true;
    }
}
