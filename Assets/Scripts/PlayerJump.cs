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
/// 플레이어 점프 및 중력 처리 클래스
/// </summary>
public class PlayerJump : MonoBehaviour
{
    #region 인스텍터 변수 선언

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerBase playerBase;

    [Space]

    [Header("Jump Stats")]
    [Range(0.5f, 50.0f)]
    [SerializeField] private float jumpPower;
    [SerializeField] private Vector2 wallJumpPowerVector;
    [Range(0.01f, 50.0f)]
    [SerializeField] private float wallJumpLerpFactor;

    [Space]

    [Header("Gravity Multiplier Information")]
    [Range(0.01f, 10.0f)]
    [SerializeField] private float fallMultiplier = 2.5f;
    [Range(0.01f, 10.0f)]
    [SerializeField] private float lowJumpMultiplier = 2f;

    [Space]

    [Header("Timer")]
    [Range(0f, 1.0f)]
    [SerializeField] private float jumpEndTime;
    [Range(0f, 1.0f)]
    [SerializeField] private float wallJumpAbleTime = 0f;
    [Range(0f, 1.0f)]
    [SerializeField] private float bufferResetTime = 0f;
    [Range(0f, 1.0f)]
    [SerializeField] private float coyoteTime = 0f;

    [Space]

    [Header("Result Vectors")]
    [SerializeField] private Vector2 gravityVector;
    [SerializeField] private Vector2 jumpVector;
    [SerializeField] private Vector2 wallJumpVector;

    #endregion

    #region 로컬 변수 선언

    private bool mustApplyJumpVector = false;   // 점프 벡터 확정 적용 제어 bool
    private bool isHoldJump = false;            // 점프키 입력 체크 변수(누르고 있으면 true)
    private float jumpCheckTime;                // 최소 점프 높이 설정 타이머용 변수
    private float wallJumpCheckTime;            // 벽점프 코요테 타임 타이머용 변수
    private bool isBufferFull;                  // 선입력 버퍼 적용 bool
    private float bufferCheckTime;              // 버퍼 초기화 타이머용 변수
    private int recentWallDir;                  // 최근에 닿은 벽 방향 저장 변수
    [SerializeField]private float coyoteCheckTime;              // 코요테 타임 타이머용 변수
    private bool hasJumped;                     // 점프를 했었는지 판단용 bool / 점프 시 True, 땅이나 벽에 닿으면 False

    #endregion

    #region MonoBehavior 이벤트 함수

    void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        if (playerBase == null)
            playerBase = GetComponent<PlayerBase>();
    }

    // 점프에 대한 각종 타이머 계산
    void Update()
    {
        if (playerBase.IsWallJumping)
            CheckWallJumpTime();

        if (playerBase.IsJumping)
            CheckJumpTime();

        if (isBufferFull)
            CheckBufferTime();

        if (!hasJumped 
            && !playerBase.OnGround 
            && !playerBase.IsWallClimbing
            && coyoteCheckTime <= coyoteTime)
        {
            if (!playerBase.CanCoyoteJump)
            {
                playerBase.CanCoyoteJump = true;
                coyoteCheckTime = 0;
            }
            CheckCoyoteTime();
        }
        else
        {
            playerBase.CanCoyoteJump = false;
        }
            

        if (hasJumped && 
            ((playerBase.OnGround && !playerBase.IsJumping) 
             || (playerBase.IsWallClimbing && !playerBase.IsWallJumping)))
        {
            hasJumped = false;
            coyoteCheckTime = 0;
        }

        CheckCanWallJump();
        CheckRecentWall();
    }

    void FixedUpdate()
    {
        

        // 점프 시 벡터 확정 적용 후 초기화를 위한 구문
        if (mustApplyJumpVector)
        {
            mustApplyJumpVector = false;
            return;
        }
        jumpVector = Vector2.zero;

        // input buffer에 선입력된 점프가 있는 상태에서 땅에 닿으면 점프
        if (isBufferFull && playerBase.OnGround && !hasJumped)
        {
            SetJumpVector();
            // 다음 구문에서 벡터 확정 후 초기화를 막기 위해서 return
        }

        MultiplyOnPlayerFall();
    }

    #endregion

    #region 중력 함수

    /// <summary>
    /// <para>
    /// 작성자 : 조우석
    /// </para>
    /// <para>
    /// ===========================================
    /// </para>
    /// 플레이어의 중력 계산 후 중력에 대한 벡터를 반환
    /// </summary>
    public Vector2 GetGravityVector()
    {
        MultiplyOnPlayerFall();
        return gravityVector;
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

    #endregion

    #region 점프 함수

    /// <summary>
    /// <para>
    /// 작성자 : 조우석
    /// </para>
    /// <para>
    /// ===========================================
    /// </para>
    /// 점프 벡터를 반환
    /// </summary>
    public Vector2 GetJumpVector()
    {
        return jumpVector;
    }

    /// <summary>
    /// <para>
    /// 작성자 : 조우석
    /// </para>
    /// <para>
    /// ===========================================
    /// </para>
    /// 점프 시 적용해줘야 하는 값들 세팅
    /// </summary>
    private void SetJumpVector()
    {
        jumpVector = new Vector2(0, jumpPower);
        mustApplyJumpVector = true;
        playerBase.IsJumping = true;
        isBufferFull = false;
        playerBase.CanCoyoteJump = false;
        hasJumped = true;
    }

    /// <summary>
    /// <para>
    /// 작성자 : 조우석
    /// </para>
    /// <para>
    /// ===========================================
    /// </para>
    /// 타이머를 통해 점프 최소 지속시간 계산
    /// </summary>
    private void CheckJumpTime()
    {
        jumpCheckTime += Time.deltaTime;

        if (jumpCheckTime > jumpEndTime)
        {
            if (!isHoldJump)
                rb.velocity = new Vector2(rb.velocity.x, 0);
            playerBase.IsJumping = false;
            jumpCheckTime = 0f;
        }
    }

    /// <summary>
    /// <para>
    /// 작성자 : 조우석
    /// </para>
    /// <para>
    /// ===========================================
    /// </para>
    /// 코요테 타임 타이머를 통해서 플레이어의 점프 가능 시간을 계산
    /// </summary>
    private void CheckCoyoteTime()
    {
        coyoteCheckTime += Time.deltaTime;

        if (coyoteCheckTime > coyoteTime)
        {
            playerBase.CanCoyoteJump = false;
        }
    }

    #endregion

    #region 선입력 시스템 함수

    /// <summary>
    /// <para>
    /// 작성자 : 조우석
    /// </para>
    /// <para>
    /// ===========================================
    /// </para>
    /// input buffer에 점프 선입력 정보 추가
    /// </summary>
    private void AddInputBuffer()
    {
        isBufferFull = true;
        bufferCheckTime = 0;
    }

    /// <summary>
    /// <para>
    /// 작성자 : 조우석
    /// </para>
    /// <para>
    /// ===========================================
    /// </para>
    /// 버퍼 지속시간 체크
    /// input buffer에 점프 선입력이 들어있으면 일정 시간이 지나면 버퍼를 초기화한다.
    /// </summary>
    private void CheckBufferTime()
    {
        bufferCheckTime += Time.deltaTime;

        if (bufferCheckTime > bufferResetTime)
        {
            isBufferFull = false;
            bufferCheckTime = 0;
        }
    }

    #endregion

    #region 벽점프 함수

    /// <summary>
    /// <para>
    /// 작성자 : 조우석
    /// </para>
    /// <para>
    /// ===========================================
    /// </para>
    /// 벽점프 벡터 계산 후 반환
    /// 벽점프 벡터는 이동 벡터와 선형 보간을 통해서 자연스러운 벽점프 방향 전환 적용
    /// </summary>
    public Vector2 LerpWallJumpVector(Vector2 moveVector)
    {
        wallJumpVector = Vector2.Lerp(wallJumpVector, moveVector, Time.fixedDeltaTime * wallJumpLerpFactor);

        return wallJumpVector;
    }

    /// <summary>
    /// <para>
    /// 작성자 : 조우석
    /// </para>
    /// <para>
    /// ===========================================
    /// </para>
    /// 널널한 벽점프 판정(코요테 타임)을 위해서 타이머를 통해서 플레이어의 벽점프 가능 시간을 계산
    /// </summary>
    private void CheckCanWallJump()
    {
        if (playerBase.IsWallClimbing)
        {
            playerBase.CanWallJump = true;
            wallJumpCheckTime = 0;
        }
        else if (!playerBase.CanWallJump)
            return;
        else
        {
            wallJumpCheckTime += Time.deltaTime;
            if (wallJumpCheckTime > wallJumpAbleTime)
            {
                playerBase.CanWallJump = false;
                wallJumpCheckTime = 0;
            }
        }
    }

    /// <summary>
    /// <para>
    /// 작성자 : 조우석
    /// </para>
    /// <para>
    /// ===========================================
    /// </para>
    /// 타이머를 통해 벽점프 최소 지속시간 계산
    /// </summary>
    private void CheckWallJumpTime()
    {
        jumpCheckTime += Time.deltaTime;

        if (jumpCheckTime > jumpEndTime)
        {
            if (!isHoldJump)
                rb.velocity = new Vector2(rb.velocity.x, 0);
            playerBase.IsWallJumping = false;
            jumpCheckTime = 0f;
        }
    }

    private void CheckRecentWall()
    {
        if (playerBase.WallSide == 0)
            return;
        recentWallDir = playerBase.WallSide;
    }

    #endregion

    #region Input System 이벤트 함수

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
        // 점프 키를 눌렀을 때
        if (context.performed)
        {
            isHoldJump = true;
            if (!hasJumped && (playerBase.OnGround || playerBase.CanCoyoteJump))
            {
                SetJumpVector();
            }
            else
            {
                AddInputBuffer();
            }

            if (playerBase.CanWallJump && !hasJumped)
            {
                jumpVector = new Vector2(wallJumpPowerVector.x * (recentWallDir == 1 ? -1 : 1), wallJumpPowerVector.y);
                wallJumpVector = jumpVector;
                jumpCheckTime = 0;
                mustApplyJumpVector = true;
                playerBase.IsWallJumping = true;
                isHoldJump = true;
                hasJumped = true;
            }
        }

        // 점프 키를 뗐을 때
        if (context.canceled)
        {
            isHoldJump = false;
            if (rb.velocity.y > 0f)
            {
                jumpVector = new Vector2(0, 0);
                if (!playerBase.IsJumping && !playerBase.IsWallJumping)
                    rb.velocity = new Vector2(rb.velocity.x, 0);
            }
        }
    }

    #endregion
}
