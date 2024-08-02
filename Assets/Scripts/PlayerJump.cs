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
    [SerializeField] private Vector2 wallJumpVector;

    [Space]

    [Header("Gravity Multiplier Information")]
    [Range(0.01f, 10.0f)]
    [SerializeField] private float fallMultiplier = 2.5f;
    [Range(0.01f, 10.0f)]
    [SerializeField] private float lowJumpMultiplier = 2f;

    [SerializeField] private Vector2 gravityVector;

    [Space]
    [SerializeField] private Vector2 jumpVector;

    private bool isJumping = false;
    [SerializeField] private bool hasWallJumped = false;

    #endregion

    #region 외부 참조

    public bool HasWallJumped
    {
        get { return hasWallJumped; }
    }

    public Vector2 WallJumpVector
    {
        get { return wallJumpVector; }
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
            //rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            //jumpVector += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            gravityVector = Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0)
        {
            //rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            //jumpVector += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            gravityVector = Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    public Vector2 GetGravityVector()
    {
        MultiplyOnPlayerFall();
        return gravityVector;
    }

    public void SetHasWallJumped(bool _hasWalJumped)
    {
        Debug.Log(_hasWalJumped);
        hasWallJumped = _hasWalJumped;
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
        }

        if (context.performed && playerWallClimb.IsWallClimbing)
        {
            jumpVector = new Vector2(wallJumpVector.x * (playerCollision.WallSide == 1 ? -1 : 1), wallJumpVector.y);
            isJumping = true;
            hasWallJumped = true;
        }

        if (context.canceled && rb.velocity.y > 0f)
        {
            jumpVector = new Vector2(0, 0);
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }

    public Vector2 GetJumpVector()
    {
        return jumpVector;
    }
}
