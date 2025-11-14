using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player2ActionScript : MonoBehaviour
{
    public Rigidbody2D playerRb;
    public PlayerInputActions playerControls;
    private GameObject attackAction;
    private InputAction move, jump, attack;
    public float timer;
    public float attackCooldown = 0.5f;

    public Vector2 moveDirection = Vector2.zero;
    private float moveSpeed = 5f;
    private float jumpForce = 10f;

    public float castDistance = 0.2f;
    private float castBuffer = 0.2f; // Raycast distance after accounting for rigidbody height/radius
    public bool onGround = false;
    private LayerMask groundLayer; // Could be set to public and chosen in the editor instead, but I prefer this method



    private void OnEnable()
    {
        move = playerControls.Player2.Move;
        jump = playerControls.Player2.Jump;
        attack = playerControls.Player2.Attack;
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
        castDistance = GetComponent<CircleCollider2D>().radius + castBuffer;
        attackAction = transform.GetChild(0).gameObject;
        timer = attackCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        moveDirection = move.ReadValue<Vector2>();
        onGround = IsGrounded();
        if (jump.triggered && IsGrounded())
        {
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            //onGround = false;
        }
        CheckAttack();
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, castDistance, groundLayer);
        if (hit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void CheckAttack()
    {
        if (attack.triggered && timer >= attackCooldown)
        {
            timer = 0f;
            attackAction.SetActive(true);
        }
        if (timer >= attackCooldown)
        {
            attackAction.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, new Vector3(0.1f, castDistance, 0));
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