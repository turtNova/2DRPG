using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D playerRb;

    float moveSpeed = 5;
    float moveX, moveY;

    void Start()
    {
        GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveY = Input.GetAxisRaw("Vertical");
        moveX = Input.GetAxisRaw("Horizontal");

        playerRb.linearVelocity = new Vector2(moveX, moveY).normalized * moveSpeed;
    }
}
