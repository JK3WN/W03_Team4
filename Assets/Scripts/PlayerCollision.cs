using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// �ۼ��� : ��ȿ��
/// <para>
/// ===========================================
/// </para>
/// <para>
/// �÷��̾��� �浹 ó�� Ŭ����
/// </para>
/// <para>
/// OnGround : �÷��̾ ���� �ִ��� True/False ��ȯ
/// </para>
/// <para>
/// OnWall : �÷��̾ ���� �پ����� True/False ��ȯ
/// </para>
/// <para>
/// ��� ���� ���� �پ����� ��ȯ
/// </para>
/// <para>
/// 0 : ��X | 1 : ������ | 2 : ����
/// </para>
/// </summary>
public class PlayerCollision : MonoBehaviour
{
    #region �ν����� ���� ����
    [Header("Layers")]
    public LayerMask groundLayer;
    public LayerMask wallLayer;

    [Space]

    [Header("Booleans")]
    public bool onGround;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;

    [Space]

    [Header("Collision Information")]
    public float collisionRadius = 0.25f;
    public Vector2 bottomOffset, rightOffset, leftOffset;

    [Space]
    #endregion

    #region ���� ���� ����

    private Color debugCollsiionColor = Color.green;
    private GameObject GroundObject;

    #endregion

    #region �ܺ� ����

    // ���� ��Ҵ��� ��ȯ
    public bool OnGround { get { return onGround; } }

    // ���� �پ����� ���� �ʾҴ��� ��ȯ
    public bool OnWall { get { return onWall; } }

    // ��� ���� �پ����� ��ȯ
    // 0 : ���� ���� ���� | 1 : ������ �� | 2 : ���� ��
    public int WallSide { get { return 0 + (onRightWall ? 1 : 0) + (onLeftWall ? 2 : 0); } }

    #endregion

    private void Update()
    {
        CheckCollision();


        // YJK, ���� �پ������� �� ������Ʈ�� GroundObject��
        if (onGround)
        {
            GroundObject = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer).gameObject;
        }
        else
        {
            GroundObject = null;
        }

        //Debug.Log($"Ground: {onGround}, RightWall: {onRightWall}, LeftWall: {onLeftWall}");
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { bottomOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
    }

    /// <summary>
    /// <para>
    /// �ۼ��� : ��ȿ��
    /// </para>
    /// <para>
    /// ===========================================
    /// </para>
    /// ��, ��, �� �浹 ���� �޼���
    /// ������ Boolean ���� �ν����� ������ ����
    /// </summary>
    private void CheckCollision()
    {
        // ��, ��, �� ���� Collision check
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, wallLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, wallLayer);

        // �� �浹 Ȯ��
        onWall = onRightWall || onLeftWall;
    }
}
