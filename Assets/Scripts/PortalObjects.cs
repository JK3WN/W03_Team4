using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnterDirection
{
    North = 0,
    East = 1,
    South = 2,
    West = 3
}

// YJK, ��� ������Ʈ���� Exit ������Ʈ�� ��������

public class PortalObjects : MonoBehaviour
{
    public GameObject Exit;
    public EnterDirection Direction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // YJK, Direction �� ������ ������Ʈ ������ Exit �ʿ� �� ������Ʈ ����
        if (collision.gameObject.CompareTag("Platform") && Direction == collision.gameObject.GetComponent<PlatformMove>().Direction)
        {
            GameObject Clone = GameObject.Instantiate(collision.gameObject, collision.gameObject.transform.position + Exit.transform.position - transform.position, Quaternion.identity);
            Clone.name = collision.gameObject.name;
            Debug.Log(Clone.GetComponent<PlatformMove>().HP);
            Clone.GetComponent<PlatformMove>().HP = collision.gameObject.GetComponent<PlatformMove>().HP;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // YJK, Direction �� ������ ������Ʈ���� �浹 ������ �� ������Ʈ �ı�
        if (collision.gameObject.CompareTag("Platform") && Direction == collision.gameObject.GetComponent<PlatformMove>().Direction)
        {
            Destroy(collision.gameObject);
        }
    }
}
