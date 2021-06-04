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
    public GameObject healthbar;

    private HealthControl health;
    private Animator anim;
    private bool picked = false;
    private bool onGround = true;
    private bool isCrouching = false;
    private bool enteredZone = false;
    private int direction;
    private bool buttonPressed = false; // Custom debounce for controllers

    [SerializeField]
    private float moveSpeed = 5.0f;

    private Vector2 movement;

    private void Awake()
    {
        healthbar.SetActive(false);
        health = healthbar.GetComponentInChildren<HealthControl>();
        health.SetMaxHealth(100);
        health.SetHealth(100);
        anim = GetComponentInChildren<Animator>();
        onGround = true;
    }

    private void Update()
    {
        if (isCrouching)
        {
            movement.x = Mathf.Clamp(movement.x, -0.5f, 0.5f);
            movement.y = Mathf.Clamp(movement.y, -0.5f, 0.5f);
        }
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

    public void OnCrouch(CallbackContext context)
    {
        if (context.performed && !isCrouching)
        {
            isCrouching = true;
            anim.SetBool("Crouch_b", true);
        }
        else if (context.canceled && isCrouching)
        {
            isCrouching = false;
            anim.SetBool("Crouch_b", false);
        }
    }

    IEnumerator SetOnGround()
    {
        yield return new WaitForSeconds(1);
        onGround = true;
    }

    public void OnUse(CallbackContext context)
    {
        if (context.performed && !buttonPressed)
        {
            buttonPressed = true;
            StartCoroutine(ResetButton());
            if (!picked && enteredZone)
            {
                // Pick Up;
                itemBubble.SetActive(true);
                picked = true;
            }
            else if (picked)
            {
                // Use;
                GameObject stick = Instantiate(swabStick, transform.position + new Vector3(1 * direction, 0, 0), Quaternion.Euler(0, 0, -90 * direction));
                stick.GetComponent<StickMovement>().Shoot(direction);
                itemBubble.SetActive(false);
                picked = false;
            }
        }
    }

    IEnumerator ResetButton()
    {
        yield return new WaitForSeconds(0.2f);
        buttonPressed = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collection_Area"))
        {
            enteredZone = true;
        }
        if (collision.CompareTag("Stick"))
        {
            // Decrease blood
            int currentHealth = health.GetHealth() - 10;
            health.SetHealth(currentHealth);
            healthbar.SetActive(true);
            Invoke("HideHealthBar", 1);
            anim.SetTrigger("Hit_trig");
            if (currentHealth <= 0)
            {
                anim.SetBool("Dead_b", true);
                GetComponent<Collider2D>().enabled = false;
                GetComponentInChildren<Collider2D>().enabled = false;
                StartCoroutine(Fade());
            }
        }
    }

    private void HideHealthBar()
    {
        healthbar.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Collection_Area"))
        {
            enteredZone = false;
        }
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(2);
        SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
        while (renderer.color.a > 0)
        {
            yield return new WaitForSeconds(0.1f);
            float a = renderer.color.a - 0.1f;
            float r = renderer.color.r;
            float g = renderer.color.g;
            float b = renderer.color.b;
            renderer.color = new Color(r, g, b, a);
        }
        Destroy(gameObject);
    }
}
