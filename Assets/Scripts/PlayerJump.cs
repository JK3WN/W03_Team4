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
