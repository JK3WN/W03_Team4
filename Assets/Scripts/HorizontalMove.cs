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

    Rigidbody2D rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        //nextVec = new Vector2(inputVec.x * speed, rigid.velocity.y);
        //rigid.MovePosition((rigid.position + nextVec));

        //rigid.velocity = nextVec;

        // YJK, �θ�(�� ������Ʈ)�� velocity�� �÷��̾ �߰�
        /*
        if(transform.parent != null)
        {
            rigid.velocity += new Vector2(transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity.x, 0);
        }
        */
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


    /*void OnCollisionEnter2D(Collision2D collision)
    {
        rigid.velocity += collision.gameObject.GetComponent<Rigidbody2D>().velocity;

        Debug.Log(rigid.velocity);
    }*/
}
