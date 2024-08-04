using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// 작성자 : 
/// </para>
/// <para>
/// ===========================================
/// </para>
/// 플레이어 오브젝트 관리용 클래스
/// 플레이어에 적용될 각종 컴포넌트를 참조받아 PlayerBase에서 플레이어의 모든 물리 벡터 적용
/// <para>
/// FinalVector: 플레이어에 적용할 모든 물리 벡터를 합친 최종 벡터
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

        // 대쉬 시 velocity를 고정하여 적용, 대쉬 끝나기 전까지 다른 움직임 차단
        if (playerDash.IsDashing)
        {
            rb.velocity = playerDash.DashVector;
            return;
        }

        // 플레이어가 벽을 잡고 있으면 매달리기와 벽점프에 대한 벡터만 적용
        if (playerWallClimb.IsWallClimbing)
        {
            FinalVector += playerWallClimb.GetWallVector();
            FinalVector += playerJump.GetJumpVector();
        }
        else
        {
            // 붙어있는 플랫폼의 속력 추가
            if (playerCollision.GroundObject != null)
            {
                FinalVector += new Vector2(playerCollision.GroundObject.GetComponent<Rigidbody2D>().velocity.x, 0);
            }

            // 벽점프 시 벽점프 벡터 적용
            if (playerJump.HasWallJumped)
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