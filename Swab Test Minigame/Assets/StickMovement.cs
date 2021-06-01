using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickMovement : MonoBehaviour
{

    [SerializeField]
    private float flySpeed = 10f;

    private int flyDir = 0;
    private float boundary = 20f;

    void Update()
    {
        if (Mathf.Abs(transform.position.x) > boundary)
        {
            Destroy(gameObject);
        }
        transform.Translate(Vector2.right * flySpeed * Time.deltaTime * flyDir, Space.World);
    }

    public void Shoot(int direction)
    {
        flyDir = direction;
    }
}
