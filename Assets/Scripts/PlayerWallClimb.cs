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

    [Space]

    [Header("Wall Climb Values")]
    [Range(0.001f, 50f)]
    [SerializeField] private float wallSlowFallValue;

    [Space]
    [Header("Result Vector")]
    [SerializeField] private Vector2 wallVector;

    #endregion

    void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
    }
    
    public Vector2 GetWallVector()
    {
        wallVector = new Vector2(rb.velocity.x, -wallSlowFallValue);
        return wallVector;
    }
}
