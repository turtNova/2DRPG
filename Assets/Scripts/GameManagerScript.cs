using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    // Variables
    public int player1Hp = 100;
    public int player2Hp = 100;
    private GameObject player1;
    private GameObject player2;

    void Start()
    {
        player1 = GameObject.FindWithTag("player1");
        player2 = GameObject.FindWithTag("player2");
    }

    // Update is called once per frame
    void Update()
    {
        if (player1Hp == 0)
        {
            player1.SetActive(false);
        }
        if (player2Hp == 0)
        {
            player2.SetActive(false);
        }

    }

    public void DamagePlayer(int damage, int player)
    {
        if (player == 1)
        {
            player1Hp -= damage;
        }
        else
        {
            player2Hp -= damage;
        }
    }
}
