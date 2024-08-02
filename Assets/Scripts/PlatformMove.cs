using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    [Header("Movement")]
    public Vector2 MoveSpeed;
    public EnterDirection Direction;

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

        //YJK, Direction enum을 MoveSpeed에 따라 자동으로 변경
        if (MoveSpeed.x > 0) Direction = EnterDirection.East;
        if (MoveSpeed.x < 0) Direction = EnterDirection.West;
        if (MoveSpeed.y > 0) Direction = EnterDirection.North;
        if (MoveSpeed.y < 0) Direction = EnterDirection.South;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //YJK, 시간에 따라 위치를 MoveSpeed만큼 이동
        rb.velocity = MoveSpeed;

        //YJK, HP가 0 이하가 되면 이 플랫폼 삭제
        if (HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        //YJK, 오브젝트 삭제될 때 플레이어가 자식으로 있으면 내쫓고 삭제됨
        foreach(Transform child in transform)
        {
            child.transform.parent = null;
        }
        Destroy(this.gameObject);
    }
}
