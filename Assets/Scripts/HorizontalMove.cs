using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class HorizontalMove : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;

    Rigidbody2D rigid;
    private PlayerControl pc;
    private PlayerCollision playerCollision;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerCollision = GetComponent<PlayerCollision>();
    }

    void FixedUpdate()
    {
        rigid.velocity += new Vector2(inputVec.x, rigid.velocity.y) * speed * Time.fixedDeltaTime;
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        rigid.velocity += collision.gameObject.GetComponent<Rigidbody2D>().velocity;

        Debug.Log(rigid.velocity);
    }
}
