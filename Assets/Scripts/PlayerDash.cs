using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    [Header("Dash Stats")]
    public int MaxDashes = 1;
    public float Dashes = 1.0f;
    public float RechargeSpeed = 1f;

    public bool IsDashing { get { return isDashing; } }

    public bool IsUpDash { get { return isUpDash; } }

    public Vector2 DashVector { get { return dashVector; } }

    private bool isDashing;
    private bool canDash;
    private bool isUpDash;
    private bool dashButtonPressed = false;
    private Vector2 dashVector;

    private Vector2 inputVector;
    private HorizontalMove hm;

    private void Awake()
    {
        isDashing = false;
        canDash = true;
        isUpDash = false;
        dashButtonPressed = false;
        inputVector = Vector2.zero;
        hm = GetComponent<HorizontalMove>();
        dashVector = Vector2.zero;
    }

    private void FixedUpdate()
    {
        Dashes = Mathf.Clamp(Dashes + RechargeSpeed * Time.deltaTime, 0, MaxDashes);
    }

    private IEnumerator DoDash(Vector2 Direction)
    {
        Camera.main.GetComponent<ScreenShake>().Shake();
        isDashing = true;
        canDash = false;
        dashVector = Direction * 32f;

        yield return new WaitForSeconds(0.17f);

        isDashing = false;
        if (dashVector.y > 0) isUpDash = true;
        dashVector = Vector2.zero;
        
        StartCoroutine(DashDelay());
    }

    private IEnumerator DashDelay()
    {
        yield return new WaitForSeconds(0.15f);

        isUpDash = false;
        canDash = true;
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!isDashing && canDash && !dashButtonPressed)
        {
            dashButtonPressed = true;
            inputVector = hm.inputVec;

            // YJK, Dash가 1 이상일 때 Dash 1 줄이고 대쉬 진행
            if (inputVector == Vector2.zero || Dashes < 1f) return;
            Dashes -= 1f;
            if (GameObject.Find("GameManager") != null)
                GameObject.Find("GameManager").GetComponent<GameManager>().RumblePulse(0.75f, 0.75f, 0.2f);
            StartCoroutine(DoDash(inputVector));
        }

        if (context.canceled)
        {
            dashButtonPressed = false;
        }
    }
    
    // YJK, 대쉬 횟수 증가
    public void AddMaxDash()
    {
        MaxDashes++;
    }
}
