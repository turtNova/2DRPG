using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    public Rigidbody2D playerRb;
    public PlayerInputActions playerControls;
    private InputAction move;
    private InputAction jump;

    public Vector2 moveDirection = Vector2.zero;
    private float moveSpeed = 5f;
    private float jumpForce = 10f;

    public float castDistance;
    private float castBuffer = 0.2f; // Raycast distance after accounting for rigidbody height/radius
    public bool onGround = false;
    private LayerMask groundLayer; // Could be set to public and chosen in the editor instead, but I prefer this method



    private void OnEnable()
    {
        move = playerControls.Player.Move;
        jump = playerControls.Player.Jump;
        move.Enable();
        jump.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
    }

    private void Awake()
    {
        playerControls = new PlayerInputActions();
        groundLayer = LayerMask.GetMask("Ground");
}
    void Start()
    {
        GetComponent<Rigidbody2D>();
        castDistance = GetComponent<CircleCollider2D>().radius + castBuffer;
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();
        onGround = IsGrounded();
        if (jump.triggered && IsGrounded())
        {
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            //onGround = false;
        }
    }

    private void FixedUpdate()
    {
        // Sets the horizontal velocity to the horizontal movement axis
        playerRb.linearVelocityX = moveDirection.x * moveSpeed;
    }

    // 2D Raycast down method
    public bool IsGrounded()
    {
        // Spawns a raycast from the origin of the GameObject, which is better than offsetting the position and casting from the edge
        // This way prevents edge cases where collisions occur *inside* the gameobject
        //RaycastHit2D hit;
        if (Physics2D.Raycast(transform.position, -transform.up, castDistance, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, new Vector3(0.1f, castDistance, 0));
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.layer == 3)
    //    {
    //        Debug.Log("Hit ground");
    //        onGround = true;
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.layer == 3)
    //    {
    //        onGround = false;
    //    }
    //}
}