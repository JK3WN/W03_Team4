using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerP : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;

    Rigidbody2D rigid;
    private PlayerControl pc;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 nextVec = new Vector2(inputVec.x, rigid.velocity.y) * speed * Time.fixedDeltaTime;
        rigid.MovePosition((rigid.position + nextVec));
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
}
