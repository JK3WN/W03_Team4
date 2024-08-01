using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    [Header("Movement")]
    public Vector2 MoveSpeed;

    [Header("Stats")] public float MaxHP = 100f;
    private float HP;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        HP = MaxHP;
        rb = GetComponent<Rigidbody2D>();
        if (MoveSpeed.x == 0) rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        if (MoveSpeed.y == 0) rb.constraints = RigidbodyConstraints2D.FreezePositionY;
    }

    // Update is called once per frame
    void Update()
    {
        //YJK, 시간에 따라 위치를 MoveSpeed만큼 이동
        rb.velocity = MoveSpeed;

        //YJK, HP가 0 이하가 되면 이 플랫폼 삭제
        if (HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
