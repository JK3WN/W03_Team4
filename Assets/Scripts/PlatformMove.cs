using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemList
{
    None = 0,
    Fly = 1,
    Dash = 2,
    Range = 3,
    AttackSpeed = 4,
    Damage = 5,
    MovementSpeed = 6,
    Resurrection = 7
}

public class PlatformMove : MonoBehaviour
{
    [Header("Movement")]
    public Vector2 MoveSpeed;
    public EnterDirection Direction;

    [Header("Stats")] public float MaxHP = 100f;
    private float HP;
    public ItemList Item;
    public GameObject[] IconList;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        HP = MaxHP;
        rb = GetComponent<Rigidbody2D>();
        if (MoveSpeed.x == 0) rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        if (MoveSpeed.y == 0) rb.constraints = RigidbodyConstraints2D.FreezePositionY;

        //YJK, Direction enum�� MoveSpeed�� ���� �ڵ����� ����
        if (MoveSpeed.x > 0) Direction = EnterDirection.East;
        if (MoveSpeed.x < 0) Direction = EnterDirection.West;
        if (MoveSpeed.y > 0) Direction = EnterDirection.North;
        if (MoveSpeed.y < 0) Direction = EnterDirection.South;

        //YJK, Item ���� ������ ������ ����
        if((int)Item != 0)
        {
            IconList[(int)Item - 1].SetActive(true);
            IconList[(int)Item - 1].gameObject.transform.localScale = new Vector3(1 / transform.localScale.x, 1 / transform.localScale.y, 1);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //YJK, �ð��� ���� ��ġ�� MoveSpeed��ŭ �̵�
        rb.velocity = MoveSpeed;

        //YJK, HP�� 0 ���ϰ� �Ǹ� �� �÷��� ����
        if (HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        //YJK, ������Ʈ ������ �� �÷��̾ �ڽ����� ������ ���Ѱ� ������
        foreach(Transform child in transform)
        {
            if(child.gameObject.CompareTag("Player")) child.transform.parent = null;
        }
        Destroy(this.gameObject);
    }
}
