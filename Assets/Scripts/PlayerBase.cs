using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector2 FinalVector2;
    [SerializeField] private HorizontalMove horizontalMove;
    [SerializeField] private PlayerCollision playerCollision;
    [SerializeField] private PlayerJump playerJump;
    [SerializeField] private PlayerWallClimb playerWallClimb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FinalVector2 = Vector2.zero;
        
        if (playerWallClimb.IsWallClimbing)
        {
            FinalVector2 += playerWallClimb.GetWallVector();
            FinalVector2 += playerJump.GetJumpVector();
        }
        else
        {
            if (GetComponent<PlayerCollision>().GroundObject != null)
            {
                FinalVector2 += new Vector2(GetComponent<PlayerCollision>().GroundObject.GetComponent<Rigidbody2D>().velocity.x, 0);
            }

            if (playerJump.HasWallJumped)
            {
                FinalVector2 += playerJump.LerpWallJumpVector(horizontalMove.GetMoveVector());
            }
            else
            {
                FinalVector2 += horizontalMove.GetMoveVector();
            }

            FinalVector2 += playerJump.GetJumpVector();
            FinalVector2 += playerJump.GetGravityVector();
        }

        rb.velocity = FinalVector2;
    }
}
