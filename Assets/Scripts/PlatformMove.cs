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
        //YJK, �ð��� ���� ��ġ�� MoveSpeed��ŭ �̵�
        rb.velocity = MoveSpeed;

        //YJK, HP�� 0 ���ϰ� �Ǹ� �� �÷��� ����
        if (HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
