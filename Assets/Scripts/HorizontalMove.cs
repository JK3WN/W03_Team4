using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// <para>
/// 작성자 : 이승철
/// </para>
/// <para>
/// ===========================================
/// </para>
/// 플레이어의 좌우 이동 적용 클래스
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

        // YJK, 부모(땅 오브젝트)의 velocity를 플레이어에 추가
        if(transform.parent != null)
        {
            rigid.velocity += transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity;
        }
    }

    /// <summary>
    /// <para>
    /// 작성자 : 1. 이승철, 2. 조우석
    /// </para>
    /// <para>
    /// ===========================================
    /// </para>
    /// input system에서 'Move' event invoke 시 호출 함수
    /// Move 이벤트 호출 시 인풋 액션의 move vector2를 받아와 inputVector에 적용
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
