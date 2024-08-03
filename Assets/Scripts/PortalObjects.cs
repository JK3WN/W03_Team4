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

// YJK, 닿는 오브젝트들을 Exit 오브젝트로 날려버림

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
        // YJK, Direction 값 동일한 오브젝트 닿으면 Exit 쪽에 이 오브젝트 생성
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
        // YJK, Direction 값 동일한 오브젝트와의 충돌 끝나면 그 오브젝트 파괴
        if (collision.gameObject.CompareTag("Platform") && Direction == collision.gameObject.GetComponent<PlatformMove>().Direction)
        {
            Destroy(collision.gameObject);
        }
    }
}
