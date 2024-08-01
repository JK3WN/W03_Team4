using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    [Header("Movement")]
    public Vector2 MoveSpeed;

    [Header("Stats")] public float MaxHP;
    private float HP;

    // Start is called before the first frame update
    void Start()
    {
        HP = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 NewPosition = new Vector2(transform.position.x, transform.position.y) + MoveSpeed * Time.deltaTime;
        transform.position = NewPosition;
    }
}
