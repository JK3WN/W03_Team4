using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// <para>
/// �ۼ��� : ���켮
/// </para>
/// <para>
/// ===========================================
/// </para>
/// �÷��̾� ���� ó�� �� �߷� ó�� Ŭ����
/// </summary>
public class PlayerJump : MonoBehaviour
{
    #region �ν����� ���� ����

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerCollision playerCollision;
    // �÷��̾� ���̽� Ŭ���� �߰� �� ���� �ʿ�
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
        MultiplyOnPlayerFall();
    }

    /// <summary>
    /// <para>
    /// �ۼ��� : ���켮
    /// </para>
    /// <para>
    /// ===========================================
    /// </para>
    /// �÷��̾� �ϰ� �� �߷¿� ����� �����ִ� �޼���
    /// �ϰ� �� 'fallMultiplier'�� ����� �߷��� ����.
    /// ��� �� 'lowJumpMultiplier'�� ����� �߷��� ����.
    /// </summary>
    private void MultiplyOnPlayerFall()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    /// <summary>
    /// <para>
    /// �ۼ��� : ���켮
    /// </para>
    /// <para>
    /// ===========================================
    /// </para>
    /// input system���� 'Jump' event invoke �� ȣ�� �Լ�
    /// ���� Ű�� ������ ���� �پ��ִ� ������ ��, �÷��̾�� jumpPower��ŭ ����
    /// ���� Ű�� ���� �÷��̾ ��� ���� ��, �÷��̾��� y �ӷ��� 0���� ����.
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && playerCollision.OnGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }

        if (context.performed && playerWallClimb.IsWallClimbing)
        {
            rb.velocity = new Vector2(wallJumpVector.x * (playerCollision.WallSide == 1 ? -1 : 1),
                wallJumpVector.y);
            Debug.Log($"Wall Jump: {rb.velocity}");
        }

        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }
}
