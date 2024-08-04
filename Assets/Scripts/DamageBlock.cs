using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBlock : MonoBehaviour
{
    private Collider2D collider2d;
    public GameObject player;
    public float knockback = 50f;

    // Start is called before the first frame update
    void Start()
    {
        collider2d = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            if (collision.gameObject.GetComponent<PlatformMove>() != null)
            {
                collision.gameObject.GetComponent<PlatformMove>().HP--;
                GameObject.Find("GameManager").GetComponent<GameManager>().CurrentExp++;
                switch (transform.rotation.eulerAngles.z)
                {
                    case 0f:
                        //player.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.down * knockback, ForceMode2D.Impulse);
                        player.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.down * knockback;
                        break;
                    case 90f:
                        //player.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * knockback, ForceMode2D.Impulse);
                        player.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.right * knockback;
                        break;
                    case 180f:
                        //player.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * knockback, ForceMode2D.Impulse);
                        player.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.up * knockback;
                        break;
                    case 270f:
                        //player.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * knockback, ForceMode2D.Impulse);
                        player.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.left * knockback;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
