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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FinalVector2 = Vector2.zero;
        /*if (GetComponent<PlayerWallClimb>().IsWallClimbing)
        {
            FinalVector2 += GetComponent<PlayerWallClimb>().wallVector;
            // TODO: Wall Velocity
        }
        else
        {
            FinalVector2 += GetComponent<HorizontalMove>().nextVec;
            if (GetComponent<PlayerCollision>().GroundObject != null)
            {
                FinalVector2 += new Vector2(GetComponent<PlayerCollision>().GroundObject.GetComponent<Rigidbody2D>().velocity.x, 0);
            }
            FinalVector2 += GetComponent<PlayerJump>().jumpVector;
        rb.velocity = FinalVector2;*/

        FinalVector2 += playerJump.GetGravityVector();
        FinalVector2 += horizontalMove.GetMoveVector();
        FinalVector2 += playerJump.GetJumpVector();

        rb.velocity = FinalVector2;
    }
}
