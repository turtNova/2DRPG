using UnityEngine;

public class Player2HurtboxScript : MonoBehaviour
{
    private GameManagerScript gameManagerScript;
    public float player2IFrames = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManagerScript = GameObject.FindWithTag("gameManager").GetComponent<GameManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        player2IFrames -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7 && player2IFrames <= 0.01f)
        {
            Debug.Log("Got hit by player1");
            gameManagerScript.DamagePlayer2(20);
            player2IFrames = 0.5f;
        }
    }
}
