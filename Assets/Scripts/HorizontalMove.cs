using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// <para>
/// �ۼ��� : �̽�ö
/// </para>
/// <para>
/// ===========================================
/// </para>
/// �÷��̾��� �¿� �̵� ���� Ŭ����
/// </summary>
public class HorizontalMove : MonoBehaviour
{
    public Vector2 inputVec, nextVec;
    public float speed;

    [Range(0, 1)] public float deadZone = 0.05f;

    Rigidbody2D rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (inputVec.x < deadZone && inputVec.x > -deadZone)
        {
            inputVec.x = 0;
        }
        if (inputVec.y < deadZone && inputVec.y > -deadZone)
        {
            inputVec.y = 0;
        }
    }

    public Vector2 GetMoveVector()
    {
        return new Vector2(inputVec.x * speed, rigid.velocity.y);
    }

    /// <summary>
    /// <para>
    /// �ۼ��� : �̽�ö
    /// </para>
    /// <para>
    /// ===========================================
    /// </para>
    /// input system���� 'Move' event invoke �� ȣ�� �Լ�
    /// CWS, Move �̺�Ʈ ȣ�� �� ��ǲ �׼��� move vector2�� �޾ƿ� inputVector�� ����
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        inputVec = context.ReadValue<Vector2>();
    }
}
