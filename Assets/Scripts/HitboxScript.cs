using UnityEngine;

public class PunchHitboxScript : MonoBehaviour
{
    private GameObject playerTwo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTwo = GameObject.FindWithTag("player2");
    }

    private void OnEnable()
    {
        Debug.Log("This works");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.layer == 7)
    //    {
    //        Debug.Log("Hit player2");
    //    }
    //}
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.layer == 7)
    //    {
    //        Debug.Log("Hit player2");
    //    }
    //}
}
