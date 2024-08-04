using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemList
{
    None = 0,
    Fly = 1
}

public class PlatformMove : MonoBehaviour
{
    [Header("Movement")]
    public Vector2 MoveSpeed;
    public EnterDirection Direction;

    [Header("Stats")] public int MaxHP = 1;
    public GameObject[] HPList;
    public ItemList Item;
    public int Exp = 1, x = 1, y = 1, start;

    [Header("Current Status")]
    public int HP;
    public bool isTouched = false;

    private Rigidbody2D rb;

    private void Awake()
    {
        HP = MaxHP;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (MoveSpeed.x == 0) rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        if (MoveSpeed.y == 0) rb.constraints = RigidbodyConstraints2D.FreezePositionY;

        // YJK, Direction enum을 MoveSpeed에 따라 자동으로 변경
        if (MoveSpeed.x > 0) Direction = EnterDirection.East;
        if (MoveSpeed.x < 0) Direction = EnterDirection.West;
        if (MoveSpeed.y > 0) Direction = EnterDirection.North;
        if (MoveSpeed.y < 0) Direction = EnterDirection.South;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // YJK, 현 HP 따라 채워져야 하는 HP 원 채움
        for(int i = 0; i < HP && i < HPList.Length; i++)
        {
            HPList[i].SetActive(true);
        }

        // YJK, 현 HP 따라 비어있는 HP 원 비움
        for(int i = HP; i < HPList.Length; i++)
        {
            HPList[i].SetActive(false);
        }

        // YJK, isTouched가 참이면 이 스프라이트를 반투명하게 만듦
        if (isTouched)
        {
            Color color = GetComponent<SpriteRenderer>().color;
            color.a = 0.5f;
            GetComponent<SpriteRenderer>().color = color;
        }

        // YJK, 시간에 따라 위치를 MoveSpeed만큼 이동
        rb.velocity = MoveSpeed;

        // YJK, HP가 0 이하가 되면 이 플랫폼 삭제
        if (HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // YJK, 블록이 플레이어에 닿으면 isTouched 참으로
        if (collision.gameObject.CompareTag("Player") && !isTouched)
        {
            isTouched = true;
            GameObject.Find("GameManager").GetComponent<GameManager>().CurrentExp += Exp;
        }
    }

    private void OnDestroy()
    {
        switch (Direction)
        {
            case EnterDirection.East:
                GameObject.Find("SpawnManager").GetComponent<BrickSpawner>().DeleteBrick(this.gameObject, start, 0);
                break;
            case EnterDirection.West:
                GameObject.Find("SpawnManager").GetComponent<BrickSpawner>().DeleteBrick(this.gameObject, start, 1);
                break;
            case EnterDirection.North:
                GameObject.Find("SpawnManager").GetComponent<BrickSpawner>().DeleteBrick(this.gameObject, start, 2);
                break;
            default:
                GameObject.Find("SpawnManager").GetComponent<BrickSpawner>().DeleteBrick(this.gameObject, start, 3);
                break;
        }
        Destroy(this.gameObject);
    }
}
