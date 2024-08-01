using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// <para>
/// 작성자 : 조우석
/// </para>
/// <para>
/// ===========================================
/// </para>
/// 플레이어 점프 처리 및 중력 처리 클래스
/// </summary>
public class PlayerJump : MonoBehaviour
{
    #region 인스텍터 변수 선언

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerCollision playerCollision;
    [SerializeField] private InputActionAsset inputActionAsset;
    private InputAction jumpAction;

    [Space]

    [Header("Jump Stats")]
    [Range(0.5f, 50.0f)]
    [SerializeField] private float jumpPower;
    [SerializeField] private Vector2 jumpVector;

    [Space]

    [Header("Gravity Multiplier Information")]
    [Range(0.01f, 10.0f)]
    [SerializeField] private float fallMultiplier = 2.5f;
    [Range(0.01f, 10.0f)]
    [SerializeField] private float lowJumpMultiplier = 2f;

    #endregion

    void Awake()
    {
        InputActionMap actionMap = inputActionAsset.FindActionMap("PlayerMove");
        jumpAction = actionMap.FindAction("Jump");

        jumpAction.performed += OnJumpPerformed;
    }

    void OnDestroy()
    {
        jumpAction.performed -= OnJumpPerformed;
    }

    void OnEnable()
    {
        jumpAction.Enable();
    }

    void OnDisable()
    {
        jumpAction.Disable();
    }

    void FixedUpdate()
    {
        MultiplyOnPlayerFall();
        Debug.Log(rb.velocity);
    }

    /// <summary>
    /// <para>
    /// 작성자 : 조우석
    /// </para>
    /// <para>
    /// ===========================================
    /// </para>
    /// 플레이어 하강 시 중력에 배수를 곱해주는 메서드
    /// 하강 시 'fallMultiplier'를 배수로 중력을 조절.
    /// 상승 시 'lowJumpMultiplier'를 배수로 중력을 조절.
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
    /// 작성자 : 조우석
    /// </para>
    /// <para>
    /// ===========================================
    /// </para>
    /// input system에서 'Jump' event invoke 시 호출 함수
    /// </summary>
    public void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (context.performed && playerCollision.OnGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            rb.velocity += transform.parent.GetComponent<Rigidbody2D>().velocity;
        }

        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }
}
