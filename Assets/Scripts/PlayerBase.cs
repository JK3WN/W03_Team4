using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// 작성자 : 1.임재균, 2.조우석
/// </para>
/// <para>
/// ===========================================
/// </para>
/// 플레이어 오브젝트 관리용 클래스
/// 플레이어에 적용될 각종 컴포넌트를 참조받아 PlayerBase에서 플레이어의 모든 물리 벡터 적용
/// <para>
/// FinalVector : 플레이어에 적용할 모든 물리 벡터를 합친 최종 벡터
/// </para>
/// <para>
/// CanWallJump : 벽 점프가 가능한지에 대한 판정 True/False 반환
/// </para>
/// <para>
/// HasJumped : 플레이어가 점프 상태인지 True/False 반환
/// </para>
/// <para>
/// HasWallJumped : 플레이어가 벽점프 상태인지 True/False 반환
/// </para>
/// <para>
/// IsWallClimbing : 벽에 매달렸는지에 대한 판전 True/False 반환
/// </para>
/// <para>
/// OnGround : 플레이어가 땅에 있는지 True/False 반환
/// </para>
/// <para>
/// OnWall : 플레이어가 벽에 붙었는지 True/False 반환
/// </para>
/// <para>
/// WallSide : 어느 방향 벽에 붙었는지 int 반환
/// </para>
/// <para>
/// 0 : 벽X | 1 : 오른쪽 | 2 : 왼쪽
/// </para>
/// </summary>
public class PlayerBase : MonoBehaviour
{
    #region 인스펙터 변수 선언
    [Header("Player Base Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private HorizontalMove horizontalMove;
    [SerializeField] private PlayerCollision playerCollision;
    [SerializeField] private PlayerJump playerJump;
    [SerializeField] private PlayerWallClimb playerWallClimb;
    [SerializeField] private PlayerDash playerDash;

    [Space]
    [Header("Player Physics")]
    [SerializeField] private Vector2 FinalVector;

    [Space] 
    [Header("Booleans")] 
    [SerializeField] private bool canWallJump = false;
    [SerializeField] private bool canCoyoteJump = false;
    [SerializeField] private bool hasJumped = false;
    [SerializeField] private bool hasWallJumped = false;

    // 플레이어가 벽에 붙음 && 플레이어가 벽 방향으로 키보드 누름
    // && 플레이어가 땅에 붙어있지 않음 && 플레이어가 상승중이 아닐 때
    private bool isWallClimbing => playerCollision.OnWall 
                                   && ((playerCollision.WallSide - 1.5f) * horizontalMove.inputVec.x < 0) 
                                   && !playerCollision.OnGround 
                                   && rb.velocity.y <= 0;

    private bool endDash = false;
    
    #endregion

    #region 외부 참조

    // 현재 벽 점프가 가능한지 반환
    public bool CanWallJump 
    {
        get { return canWallJump; }
        set
        {
            if (value != canWallJump)
            {
                canWallJump = value;
            }
        }
    }

    public bool CanCoyoteJump
    {
        get { return canCoyoteJump; }
        set
        {
            if (value != canCoyoteJump)
            {
                canCoyoteJump = value;
            }
        }
    }

    // 현재 점프 상태인지 반환
    public bool HasJumped
    {
        get { return hasJumped; }
        set
        {
            if (value != hasJumped)
            {
                hasJumped = value;
            }
        }
    }

    // 현재 벽점프 상태인지 반환
    public bool HasWallJumped
    {
        get { return hasWallJumped; }
        set
        {
            if (value != hasWallJumped)
            {
                hasWallJumped = value;
            }
        }
    }

    // 현재 벽 타기 상태인지 반환
    public bool IsWallClimbing
    {
        get { return isWallClimbing; }
    }

    public bool OnGround
    {
        get { return playerCollision.OnGround; }
    }

    public bool OnWall
    {
        get { return playerCollision.OnWall; }
    }

    public int WallSide
    {
        get { return playerCollision.WallSide; }
    }

    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        horizontalMove = GetComponent<HorizontalMove>();
        playerCollision = GetComponent<PlayerCollision>();
        playerJump = GetComponent<PlayerJump>();
        playerWallClimb = GetComponent<PlayerWallClimb>();
        playerDash = GetComponent<PlayerDash>();
    }

    void FixedUpdate()
    {
        FinalVector = Vector2.zero;

        // 대쉬 시 velocity를 고정하여 적용, 대쉬 끝나기 전까지 다른 움직임 차단
        if (playerDash.IsDashing)
        {
            endDash = true;
            rb.velocity = playerDash.DashVector;
            return;
        }
        else if (playerDash.IsUpDash && endDash)
        {
            endDash = false;
            rb.velocity = Vector2.zero;
            return;
        }

        FinalVector += playerCollision.GetGroundVector();
        // 플레이어가 벽을 잡고 있으면 매달리기와 벽점프에 대한 벡터만 적용
        if (isWallClimbing)
        {
            FinalVector += playerWallClimb.GetWallVector();
            FinalVector += playerJump.GetJumpVector();
        }
        else
        {
            // 벽점프 시 벽점프 벡터 적용
            if (hasWallJumped)
            {
                FinalVector += playerJump.LerpWallJumpVector(horizontalMove.GetMoveVector());
            }
            // 외에는 이동 벡터 적용
            else
            {
                FinalVector += horizontalMove.GetMoveVector();
            }

            FinalVector += playerJump.GetJumpVector();
            FinalVector += playerJump.GetGravityVector();
        }

        // 최종 벡터 적용
        rb.velocity = FinalVector;
    }
}