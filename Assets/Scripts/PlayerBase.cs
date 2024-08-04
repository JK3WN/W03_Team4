using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// �ۼ��� : 
/// </para>
/// <para>
/// ===========================================
/// </para>
/// �÷��̾� ������Ʈ ������ Ŭ����
/// �÷��̾ ����� ���� ������Ʈ�� �����޾� PlayerBase���� �÷��̾��� ��� ���� ���� ����
/// <para>
/// FinalVector: �÷��̾ ������ ��� ���� ���͸� ��ģ ���� ����
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

    // Update is called once per frame
    void FixedUpdate()
    {
        FinalVector = Vector2.zero;

        // �뽬 �� velocity�� �����Ͽ� ����, �뽬 ������ ������ �ٸ� ������ ����
        if (playerDash.IsDashing)
        {
            rb.velocity = playerDash.DashVector;
            return;
        }

        // �÷��̾ ���� ��� ������ �Ŵ޸���� �������� ���� ���͸� ����
        if (playerWallClimb.IsWallClimbing)
        {
            FinalVector += playerWallClimb.GetWallVector();
            FinalVector += playerJump.GetJumpVector();
        }
        else
        {
            // �پ��ִ� �÷����� �ӷ� �߰�
            if (playerCollision.GroundObject != null)
            {
                FinalVector += new Vector2(playerCollision.GroundObject.GetComponent<Rigidbody2D>().velocity.x, 0);
            }

            // ������ �� ������ ���� ����
            if (playerJump.HasWallJumped)
            {
                FinalVector += playerJump.LerpWallJumpVector(horizontalMove.GetMoveVector());
            }
            // �ܿ��� �̵� ���� ����
            else
            {
                FinalVector += horizontalMove.GetMoveVector();
            }

            FinalVector += playerJump.GetJumpVector();
            FinalVector += playerJump.GetGravityVector();
        }

        // ���� ���� ����
        rb.velocity = FinalVector;
    }
}