using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// 작성자 : 신효진
/// <para>
/// ===========================================
/// </para>
/// <para>
/// 플레이어의 충돌 처리 클래스
/// </para>
/// <para>
/// OnGround : 플레이어가 땅에 있는지 True/False 반환
/// </para>
/// <para>
/// OnWall : 플레이어가 벽에 붙었는지 True/False 반환
/// </para>
/// <para>
/// WallSide : 어느 방향 벽에 붙었는지 반환
/// </para>
/// <para>
/// 0 : 벽X | 1 : 오른쪽 | 2 : 왼쪽
/// </para>
/// </summary>
public class PlayerCollision : MonoBehaviour
{
    #region 인스펙터 변수 선언
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

    #region 로컬 변수 선언

    private Color debugCollsiionColor = Color.green;

    private int _centerCollisionCount = 0;

    #endregion

    #region 외부 참조

    // 땅을 밟았는지 반환
    public bool OnGround { get { return onGround; } }

    // 벽에 붙었는지 붙지 않았는지 반환
    public bool OnWall { get { return onWall; } }

    // 어느 벽에 붙었는지 반환
    // 0 : 벽에 붙지 않음 | 1 : 오른쪽 벽 | 2 : 왼쪽 벽
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

        // CWS, 붙어있던 땅의 속력을 받아와 벡터에 적용
        if (onGround)
        {
            GroundVector = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer)
                .gameObject.GetComponent<Rigidbody2D>().velocity;
        }

        //Debug.Log($"Ground: {onGround}, RightWall: {onRightWall}, LeftWall: {onLeftWall}");
    }

    /// <summary>
    /// <para>
    /// 작성자 : 조우석
    /// </para>
    /// <para>
    /// ===========================================
    /// </para>
    /// 붙어있던 땅의 벡터의 x 성분만 따와 반환
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
    /// 작성자 : 신효진
    /// </para>
    /// <para>
    /// ===========================================
    /// </para>
    /// 좌, 우, 땅 충돌 감지 메서드
    /// 감지된 Boolean 값은 인스펙터 변수에 저장
    /// </summary>
    private void CheckCollision()
    {
        // 땅, 우, 좌 방향 Collision check
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, wallLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, wallLayer);
        
        onWall = onRightWall || onLeftWall;

        _centerCollisionCount = Physics2D.OverlapCircleAll(transform.position, collisionRadius / 2, groundLayer | wallLayer).Length;
    }
}
