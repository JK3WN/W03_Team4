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
/// WallSide : ��� ���� ���� �پ����� ��ȯ
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
    public Vector2 GroundVector;
    #endregion

    #region ���� ���� ����

    private Color debugCollsiionColor = Color.green;

    private int _centerCollisionCount = 0;

    #endregion

    #region �ܺ� ����

    // ���� ��Ҵ��� ��ȯ
    public bool OnGround { get { return onGround; } }

    // ���� �پ����� ���� �ʾҴ��� ��ȯ
    public bool OnWall { get { return onWall; } }

    // ��� ���� �پ����� ��ȯ
    // 0 : ���� ���� ���� | 1 : ������ �� | 2 : ���� ��
    public int WallSide { get { return 0 + (onRightWall ? 1 : 0) + (onLeftWall ? 2 : 0); } }

    public bool IsDead
    {
        get
        {
            return _centerCollisionCount > 1;
        }
    }

    #endregion

    private void Update()
    {
        CheckCollision();

        // CWS, �پ��ִ� ���� �ӷ��� �޾ƿ� ���Ϳ� ����
        if (onGround)
        {
            GroundVector = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer)
                .gameObject.GetComponent<Rigidbody2D>().velocity;
        }

        //Debug.Log($"Ground: {onGround}, RightWall: {onRightWall}, LeftWall: {onLeftWall}");
    }

    /// <summary>
    /// <para>
    /// �ۼ��� : ���켮
    /// </para>
    /// <para>
    /// ===========================================
    /// </para>
    /// �پ��ִ� ���� ������ x ���и� ���� ��ȯ
    /// </summary>
    public Vector2 GetGroundVector()
    {
        return new Vector2(GroundVector.x, 0);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { bottomOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position, collisionRadius / 2);
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
        
        onWall = onRightWall || onLeftWall;

        _centerCollisionCount = Physics2D.OverlapCircleAll(transform.position, collisionRadius / 2, groundLayer | wallLayer).Length;
    }
}
