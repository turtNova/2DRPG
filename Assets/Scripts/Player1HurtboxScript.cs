using UnityEngine;

public class Player1HurtboxScript : MonoBehaviour
{
    private GameManagerScript gameManagerScript;
    public float player1IFrames = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManagerScript = GameObject.FindWithTag("gameManager").GetComponent<GameManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        player1IFrames -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6 && player1IFrames <= 0.01f)
        {
            Debug.Log("Got hit by player2");
            gameManagerScript.DamagePlayer1(20);
            player1IFrames = 0.5f;
        }
    }
}
