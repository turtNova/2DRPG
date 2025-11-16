using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player1Script : MonoBehaviour
{
    // Input
    public PlayerInputActions playerControls;
    private GameObject attackAction;
    private InputAction move, jump, attack;

    // Physics
    public Rigidbody2D playerRb;
    public Vector2 moveDirection = Vector2.zero;
    public float moveSpeed = 10f;
    public float jumpForce = 20f;
    public float castDistance = 0.2f;
    public bool onGround = false;
    private LayerMask groundLayer; // Could be set to public and chosen in the editor instead, but I prefer this method

    // Damage & Health
    public float attackCooldownTimer = 0.5f;
    public float localIFrames = 0f;



    private void OnEnable()
    {
        move = playerControls.Player1.Move;
        jump = playerControls.Player1.Jump;
        attack = playerControls.Player1.Attack;
        move.Enable();
        jump.Enable();
        attack.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        attack.Disable();
    }

    private void Awake()
    {
        playerControls = new PlayerInputActions();
        groundLayer = LayerMask.GetMask("Ground");
}
    void Start()
    {
        GetComponent<Rigidbody2D>();
        attackAction = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        attackCooldownTimer -= Time.deltaTime;
        moveDirection = move.ReadValue<Vector2>();
        onGround = IsGrounded();
        if (jump.triggered && IsGrounded())
        {
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            //onGround = false;
        }
        CheckAttack(0.5f, 0.2f);
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
        // Raycast2D is an event based system
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, castDistance, groundLayer);
        if (hit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void CheckAttack(float attackCooldown, float attackDuration)
    {
        if (attack.triggered && attackCooldownTimer <= attackCooldown)
        {
            attackCooldownTimer = attackCooldown;
            attackAction.SetActive(true);
        }
        if (attackCooldownTimer <= attackCooldown - attackDuration)
        {
            attackAction.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * castDistance);
    }

    // Check if on ground with collisions
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