using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    public bool IsDashing { get { return isDashing; } }

    public Vector2 DashVector { get { return dashVector; } }

    private bool isDashing;
    private bool canDash;
    private bool dashButtonPressed = false;
    private Vector2 dashVector;

    private Vector2 inputVector;
    private HorizontalMove hm;

    private void Awake()
    {
        isDashing = false;
        canDash = true;
        dashButtonPressed = false;
        inputVector = Vector2.zero;
        hm = GetComponent<HorizontalMove>();
        dashVector = Vector2.zero;
    }

    private IEnumerator DoDash(Vector2 Direction)
    {
        isDashing = true;
        canDash = false;
        dashVector = Direction * 25f;
        GetComponent<Rigidbody2D>().gravityScale = 3.0f;

        yield return new WaitForSeconds(0.15f);

        isDashing = false;
        dashVector = Vector2.zero;
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
        if (!isDashing && canDash && !dashButtonPressed)
        {
            dashButtonPressed = true;
            inputVector = hm.inputVec;

            if (inputVector == Vector2.zero) return;

            StartCoroutine(DoDash(inputVector));
        }

        if (context.canceled)
        {
            dashButtonPressed = false;
        }
    }
    
}
