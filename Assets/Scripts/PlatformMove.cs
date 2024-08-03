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
    public int HP;
    public GameObject[] HPList;
    public ItemList Item;

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

        // YJK, Direction enum�� MoveSpeed�� ���� �ڵ����� ����
        if (MoveSpeed.x > 0) Direction = EnterDirection.East;
        if (MoveSpeed.x < 0) Direction = EnterDirection.West;
        if (MoveSpeed.y > 0) Direction = EnterDirection.North;
        if (MoveSpeed.y < 0) Direction = EnterDirection.South;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // YJK, �� HP ���� ä������ �ϴ� HP �� ä��
        for(int i = 0; i < HP && i < HPList.Length; i++)
        {
            HPList[i].SetActive(true);
        }

        // YJK, �� HP ���� ����ִ� HP �� ����
        for(int i = HP; i < HPList.Length; i++)
        {
            HPList[i].SetActive(false);
        }

        // YJK, �ð��� ���� ��ġ�� MoveSpeed��ŭ �̵�
        rb.velocity = MoveSpeed;

        // YJK, HP�� 0 ���ϰ� �Ǹ� �� �÷��� ����
        if (HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        // YJK, ������Ʈ ������ �� �÷��̾ �ڽ����� ������ ���Ѱ� ������
        /*
        foreach(Transform child in transform)
        {
            if(child.gameObject.CompareTag("Player")) child.transform.parent = null;
        }
        Destroy(this.gameObject);
        */
    }
}
