using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// <para>
/// 작성자 : 조우석
/// </para>
/// <para>
/// ===========================================
/// </para>
/// 플레이어 점프 처리 및 중력 처리 클래스
/// </summary>
public class PlayerJump : MonoBehaviour
{
    #region 인스텍터 변수 선언

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerCollision playerCollision;
    // 플레이어 베이스 클래스 추가 시 수정 필요
    [SerializeField] private PlayerWallClimb playerWallClimb;

    [Space]

    [Header("Jump Stats")]
    [Range(0.5f, 50.0f)]
    [SerializeField] private float jumpPower;
    [SerializeField] private Vector2 wallJumpPowerVector;

    [Space]

    [Header("Gravity Multiplier Information")]
    [Range(0.01f, 10.0f)]
    [SerializeField] private float fallMultiplier = 2.5f;
    [Range(0.01f, 10.0f)]
    [SerializeField] private float lowJumpMultiplier = 2f;

    [SerializeField] private Vector2 gravityVector;

    [Space]
    [SerializeField] private Vector2 jumpVector;
    [SerializeField] private Vector2 wallJumpVector;

    [SerializeField] private bool canWallJump = false;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool hasWallJumped = false;
    [SerializeField] private bool hasJumped = false;
    [SerializeField] private bool isHoldJump = false;

    [SerializeField] private float wallJumpLerpFactor;
    [SerializeField] private float lerpEndTime;
    [SerializeField] private float lerpCheckTime;

    [SerializeField] private float wallJumpAbleTime = 0f;
    [SerializeField] private float wallJumpCheckTime;

    #endregion

    #region 외부 참조

    public bool CanWallJump
    {
        get { return canWallJump; }
    }

    public bool HasWallJumped
    {
        get { return hasWallJumped; }
    }

    public Vector2 WallJumpPowerVector
    {
        get { return wallJumpPowerVector; }
    }

    #endregion

    void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        if (playerCollision  == null)
            playerCollision = GetComponent<PlayerCollision>();
        if (playerWallClimb == null)
            playerWallClimb = GetComponent<PlayerWallClimb>();
    }

    void Update()
    {
        if (hasWallJumped)
            CheckWallJumpTime();
        if (hasJumped)
            CheckJumpTime();
        CheckCanWallJump();
    }

    void FixedUpdate()
    {
        if (isJumping)
        {
            isJumping = false;
            return;
        }
        jumpVector = Vector2.zero;
        MultiplyOnPlayerFall();
    }

    /// <summary>
    /// <para>
    /// 작성자 : 조우석
    /// </para>
    /// <para>
    /// ===========================================
    /// </para>
    /// 플레이어 하강 시 중력에 배수를 곱해주는 메서드
    /// 하강 시 'fallMultiplier'를 배수로 중력을 조절.
    /// 상승 시 'lowJumpMultiplier'를 배수로 중력을 조절.
    /// </summary>
    private void MultiplyOnPlayerFall()
    {
        if (rb.velocity.y < 0)
        {
            gravityVector = Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0)
        {
            gravityVector = Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    public Vector2 GetGravityVector()
    {
        MultiplyOnPlayerFall();
        return gravityVector;
    }

    private void CheckCanWallJump()
    {
        if (playerWallClimb.IsWallClimbing)
        {
            canWallJump = true;
            wallJumpCheckTime = 0;
        }
        else if (!canWallJump)
            return;
        else
        {
            wallJumpCheckTime += Time.deltaTime;
            if (wallJumpCheckTime > wallJumpAbleTime)
            {
                canWallJump = false;
                wallJumpCheckTime = 0;
            }
        }
    }

    public Vector2 LerpWallJumpVector(Vector2 moveVector)
    {
        wallJumpVector = Vector2.Lerp(wallJumpVector, moveVector, Time.fixedDeltaTime * wallJumpLerpFactor);
        
        return wallJumpVector;
    }

    private void CheckWallJumpTime()
    {
        lerpCheckTime += Time.deltaTime;

        if (lerpCheckTime > lerpEndTime)
        {
            if (!isHoldJump)
                rb.velocity = new Vector2(rb.velocity.x, 0);
            hasWallJumped = false;
            lerpCheckTime = 0f;
        }
    }

    private void CheckJumpTime()
    {
        lerpCheckTime += Time.deltaTime;

        if (lerpCheckTime > lerpEndTime)
        {
            if (!isHoldJump)
                rb.velocity = new Vector2(rb.velocity.x, 0);
            hasJumped = false;
            lerpCheckTime = 0f;
        }
    }

    /// <summary>
    /// <para>
    /// 작성자 : 조우석
    /// </para>
    /// <para>
    /// ===========================================
    /// </para>
    /// input system에서 'Jump' event invoke 시 호출 함수
    /// 점프 키를 누르고 땅에 붙어있는 상태일 때, 플레이어는 jumpPower만큼 점프
    /// 점프 키를 떼고 플레이어가 상승 중일 때, 플레이어의 y 속력을 0으로 만듦.
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && playerCollision.OnGround)
        {
            jumpVector = new Vector2(0, jumpPower);
            isJumping = true;
            isHoldJump = true;
            hasJumped = true;
        }

        if (context.performed && canWallJump)
        {
            jumpVector = new Vector2(wallJumpPowerVector.x * (playerCollision.WallSide == 1 ? -1 : 1), wallJumpPowerVector.y);
            wallJumpVector = jumpVector;
            lerpCheckTime = 0;
            Debug.Log($"{wallJumpVector}");
            isJumping = true;
            hasWallJumped = true;
            isHoldJump = true;
        }

        if (context.canceled && rb.velocity.y > 0f)
        {
            isHoldJump = false;
            jumpVector = new Vector2(0, 0);
            if (!hasJumped || ! hasWallJumped)
                rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }

    public Vector2 GetJumpVector()
    {
        return jumpVector;
    }
}
