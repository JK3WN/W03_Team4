using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DashTest : MonoBehaviour
{
    Rigidbody2D rb;

    private Vector2 dir;
    private Vector2 moveDir;

    private bool isDashing;
    private bool canDash;

    private void Awake()
    {
        isDashing = false;
        canDash = true;
        rb = GetComponent<Rigidbody2D>();
        dir = Vector2.zero;
        moveDir = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if(isDashing)
            rb.velocity = dir * 1f;
    }

    private IEnumerator DoDash(Vector2 Direction)
    {
        isDashing = true;
        canDash = false;
        dir = Direction * 25f;
        GetComponent<Rigidbody2D>().gravityScale = 3.0f;

        yield return new WaitForSeconds(0.15f);

        isDashing = false;
        GetComponent<Rigidbody2D>().gravityScale = 1.0f;

        StartCoroutine(DashDelay());
    }

    private IEnumerator DashDelay()
    {
        yield return new WaitForSeconds(0.15f);
        canDash = true;
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!isDashing && canDash)
        {
            Debug.Log("StartDash");

            StartCoroutine(DoDash(moveDir));
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>();

        Debug.Log(moveDir);
    }
}
