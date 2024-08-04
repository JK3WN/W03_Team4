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
