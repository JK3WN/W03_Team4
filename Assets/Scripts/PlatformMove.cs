using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    [Header("Movement")]
    public Vector2 MoveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //YJK, 시간에 따라 위치를 MoveSpeed만큼 이동
        Vector2 NewPosition = new Vector2(transform.position.x, transform.position.y) + MoveSpeed * Time.deltaTime;
        transform.position = NewPosition;
    }
}
