using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// 작성자 : 조우석
/// </para>
/// <para>
/// ===========================================
/// </para>
/// 플레이어 벽타기 관련 동작 클래스
/// </summary>
public class PlayerWallClimb : MonoBehaviour
{
    #region 인스텍터 변수 선언

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerCollision playerCollision;
    // 플레이어 베이스 클래스 추가 시 수정 필요
    [SerializeField] private HorizontalMove horizontalMove;

    // 플레이어 베이스 클래스 추가 시 boolean 이동
    private bool isWallClimbing => playerCollision.OnWall 
                                   && ((playerCollision.WallSide - 1.5f) * horizontalMove.inputVec.x < 0)
                                   && !playerCollision.OnGround;

    [Space]

    [Header("Wall Climb Values")]
    [Range(0.001f, 50f)]
    [SerializeField] private float wallSlowFallValue;

    [Space]
    public Vector2 wallVector;

    #endregion

    #region 외부 참조

    // 벽에 매달렸는지 반환
    public bool IsWallClimbing { get { return isWallClimbing; } }

    #endregion

    void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        if (playerCollision == null)
            playerCollision = GetComponent<PlayerCollision>();
        if (horizontalMove == null)
            horizontalMove = GetComponent<HorizontalMove>();
    }

    void FixedUpdate()
    {
        wallVector = Vector2.zero;
        if (isWallClimbing)
            WallClimbing();
    }

    void Update()
    {
        
    }

    private void WallClimbing()
    {
        //rb.velocity = new Vector2(rb.velocity.x, -wallSlowFallValue);
        wallVector = new Vector2(rb.velocity.x, -wallSlowFallValue);
    }
}
