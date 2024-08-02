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
    public Vector2 inputVec;
    public float speed;

    Rigidbody2D rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 nextVec = new Vector2(inputVec.x * speed, rigid.velocity.y);
        //rigid.MovePosition((rigid.position + nextVec));

        rigid.velocity = nextVec;

        // YJK, �θ�(�� ������Ʈ)�� velocity�� �÷��̾ �߰�
        if(transform.parent != null)
        {
            rigid.velocity += transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity;
        }
    }

    /// <summary>
    /// <para>
    /// �ۼ��� : 1. �̽�ö, 2. ���켮
    /// </para>
    /// <para>
    /// ===========================================
    /// </para>
    /// input system���� 'Move' event invoke �� ȣ�� �Լ�
    /// Move �̺�Ʈ ȣ�� �� ��ǲ �׼��� move vector2�� �޾ƿ� inputVector�� ����
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        inputVec = context.ReadValue<Vector2>();
        //inputVec = value.Get<Vector2>();
    }


    /*void OnCollisionEnter2D(Collision2D collision)
    {
        rigid.velocity += collision.gameObject.GetComponent<Rigidbody2D>().velocity;

        Debug.Log(rigid.velocity);
    }*/
}
