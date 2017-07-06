using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour {

    private bool isPinned = false;
    private float speed = 45f;
    public Rigidbody2D rb;

    private void Update()
    {
        if(!isPinned) rb.MovePosition(rb.position + Vector2.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Rotator")
        {
            transform.SetParent(collision.transform);
            Score.PinCount++;
            isPinned = true;
            if(Score.PinCount % 10 == 0)
            {
                FindObjectOfType<GameManager>().NextLevel();
            }
        }
        else if(collision.tag == "Pin")
        {
            FindObjectOfType<GameManager>().EndGame();
        }
    }

    private void DestroyAttachedPin()
    {
        Destroy(gameObject);
    }
}
