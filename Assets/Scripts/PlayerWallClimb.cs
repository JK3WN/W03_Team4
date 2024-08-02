using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// �ۼ��� : ���켮
/// </para>
/// <para>
/// ===========================================
/// </para>
/// �÷��̾� ��Ÿ�� ���� ���� Ŭ����
/// </summary>
public class PlayerWallClimb : MonoBehaviour
{
    #region �ν����� ���� ����

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerCollision playerCollision;
    // �÷��̾� ���̽� Ŭ���� �߰� �� ���� �ʿ�
    [SerializeField] private HorizontalMove horizontalMove;

    // �÷��̾� ���̽� Ŭ���� �߰� �� boolean �̵�
    private bool isWallClimbing => playerCollision.OnWall 
                                   && ((playerCollision.WallSide - 1.5f) * horizontalMove.inputVec.x < 0)
                                   && !playerCollision.OnGround;

    [Space]

    [Header("Wall Climb Values")]
    [Range(0.001f, 50f)]
    [SerializeField] private float wallSlowFallValue;

    #endregion

    #region �ܺ� ����

    // ���� �Ŵ޷ȴ��� ��ȯ
    public bool IsWallClimbing { get { return isWallClimbing; } }

    #endregion

    void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        if (playerCollision == null)
            playerCollision = new PlayerCollision();
        if (horizontalMove == null)
            horizontalMove = new HorizontalMove();
    }

    void FixedUpdate()
    {
        if (isWallClimbing)
            WallClimbing();
    }

    void Update()
    {
        
    }

    private void WallClimbing()
    {
        rb.velocity = new Vector2(rb.velocity.x, -wallSlowFallValue);
    }
}