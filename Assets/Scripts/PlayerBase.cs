using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private Vector2 FinalVector2;
    [SerializeField] private HorizontalMove horizontalMove;
    [SerializeField] private PlayerCollision playerCollision;
    [SerializeField] private PlayerJump playerJump;
    [SerializeField] private PlayerWallClimb playerWallClimb;

    private float _wallJumpLerpValue = 0f;

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
                _wallJumpLerpValue = horizontalMove.GetLerpMoveValue(_wallJumpLerpValue);
                Debug.Log(_wallJumpLerpValue);
                playerJump.SetHasWallJumped(horizontalMove.CheckEndLerp(_wallJumpLerpValue));
                FinalVector2 += horizontalMove.GetLerpMoveVector(_wallJumpLerpValue);
            }
            else
                FinalVector2 += horizontalMove.GetMoveVector();

            FinalVector2 += playerJump.GetGravityVector();
            FinalVector2 += playerJump.GetJumpVector();
        }

        rb.velocity = FinalVector2;
    }
}
