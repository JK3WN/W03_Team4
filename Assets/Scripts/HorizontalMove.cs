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
    public Vector2 inputVec, nextVec;
    public float speed;

    Rigidbody2D rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public Vector2 GetMoveVector()
    {
        return new Vector2(inputVec.x * speed, rigid.velocity.y);
    }

    /// <summary>
    /// <para>
    /// 작성자 : 이승철
    /// </para>
    /// <para>
    /// ===========================================
    /// </para>
    /// input system에서 'Move' event invoke 시 호출 함수
    /// CWS, Move 이벤트 호출 시 인풋 액션의 move vector2를 받아와 inputVector에 적용
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        inputVec = context.ReadValue<Vector2>();
    }
}
