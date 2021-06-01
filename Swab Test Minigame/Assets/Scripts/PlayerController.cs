using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public GameObject itemBubble;
    public GameObject swabStick;

    private Animator anim;
    private bool picked = false;
    private bool onGround = true;
    private bool enteredZone = false;
    private int direction;

    [SerializeField]
    private float moveSpeed = 5.0f;

    private Vector2 movement;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        onGround = true;
    }

    private void Update()
    {
        transform.Translate(new Vector2(movement.x, movement.y) * moveSpeed * Time.deltaTime, Space.World);
        if (movement.x > 0)
        {
            direction = 1;
            transform.localScale = new Vector3(10, 10, 10);
        }
        if (movement.x < 0)
        {
            direction = -1;
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
        if (context.performed && onGround && anim)
        {
            anim.SetTrigger("Jump_trig");
            onGround = false;
            StartCoroutine(SetOnGround());
        }
    }

    IEnumerator SetOnGround()
    {
        yield return new WaitForSeconds(1);
        onGround = true;
    }

    public void OnUse(CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Use performed!");
            if (!picked && enteredZone)
            {
                // Pick Up;
                itemBubble.SetActive(true);
                picked = true;
            }
            else if (picked)
            {
                // Use;
                GameObject stick = Instantiate(swabStick, transform.position, Quaternion.Euler(0, 0, -90 * direction));
                stick.GetComponent<StickMovement>().Shoot(direction);
                itemBubble.SetActive(false);
                picked = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collection_Area"))
        {
            Debug.Log("Enter zone!");
            enteredZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Collection_Area"))
        {
            Debug.Log("Exit zone!");
            enteredZone = false;
        }
    }
}
