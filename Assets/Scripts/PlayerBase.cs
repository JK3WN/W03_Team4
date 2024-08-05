using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// �ۼ��� : 1.�����, 2.���켮
/// </para>
/// <para>
/// ===========================================
/// </para>
/// �÷��̾� ������Ʈ ������ Ŭ����
/// �÷��̾ ����� ���� ������Ʈ�� �����޾� PlayerBase���� �÷��̾��� ��� ���� ���� ����
/// <para>
/// FinalVector : �÷��̾ ������ ��� ���� ���͸� ��ģ ���� ����
/// </para>
/// <para>
/// CanWallJump : �� ������ ���������� ���� ���� True/False ��ȯ
/// </para>
/// <para>
/// IsJumping : �÷��̾ ���� �������� True/False ��ȯ
/// </para>
/// <para>
/// IsWallJumping : �÷��̾ ������ �������� True/False ��ȯ
/// </para>
/// <para>
/// IsWallClimbing : ���� �Ŵ޷ȴ����� ���� ���� True/False ��ȯ
/// </para>
/// <para>
/// OnGround : �÷��̾ ���� �ִ��� True/False ��ȯ
/// </para>
/// <para>
/// OnWall : �÷��̾ ���� �پ����� True/False ��ȯ
/// </para>
/// <para>
/// WallSide : ��� ���� ���� �پ����� int ��ȯ
/// </para>
/// <para>
/// 0 : ��X | 1 : ������ | 2 : ����
/// </para>
/// </summary>
public class PlayerBase : MonoBehaviour
{
    #region �ν����� ���� ����
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
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isWallJumping = false;

    // �÷��̾ ���� ���� && �÷��̾ �� �������� Ű���� ����
    // && �÷��̾ ���� �پ����� ���� && �÷��̾ ������� �ƴ� ��
    private bool isWallClimbing => playerCollision.OnWall 
                                   && ((playerCollision.WallSide - 1.5f) * horizontalMove.inputVec.x < 0) 
                                   && !playerCollision.OnGround 
                                   && rb.velocity.y <= 0;

    private bool endDash = false;
    
    #endregion

    #region �ܺ� ����

    // ���� �� ������ �������� ��ȯ
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

    // ���� ���� �������� ��ȯ
    public bool IsJumping
    {
        get { return isJumping; }
        set
        {
            if (value != isJumping)
            {
                isJumping = value;
            }
        }
    }

    // ���� ������ �������� ��ȯ
    public bool IsWallJumping
    {
        get { return isWallJumping; }
        set
        {
            if (value != isWallJumping)
            {
                isWallJumping = value;
            }
        }
    }

    // ���� �� Ÿ�� �������� ��ȯ
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

        // �뽬 �� velocity�� �����Ͽ� ����, �뽬 ������ ������ �ٸ� ������ ����
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

        // �÷��̾ ���� ��� ������ �Ŵ޸���� �������� ���� ���͸� ����
        if (isWallClimbing)
        {
            FinalVector += playerWallClimb.GetWallVector();
            FinalVector += playerJump.GetJumpVector();
        }
        else
        {
            // ������ �� ������ ���� ����
            if (isWallJumping)
            {
                FinalVector += playerJump.LerpWallJumpVector(horizontalMove.GetMoveVector());
            }
            // �ܿ��� �̵� ���� ����
            else
            {
                FinalVector += horizontalMove.GetMoveVector();
            }

            FinalVector += playerCollision.GetGroundVector();
            FinalVector += playerJump.GetJumpVector();
            FinalVector += playerJump.GetGravityVector();
        }

        // ���� ���� ����
        rb.velocity = FinalVector;
    }
}